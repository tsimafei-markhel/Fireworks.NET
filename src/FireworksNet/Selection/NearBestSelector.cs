using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FireworksNet.Distances;
using FireworksNet.Extensions;
using FireworksNet.Model;

namespace FireworksNet.Selection
{
    public class NearBestSelector : ISelector
    {
        private readonly IDistance distanceCalculator;
        private readonly Func<IEnumerable<Firework>, Firework> bestFireworkSelector;
        private readonly int samplingNumber;

        public NearBestSelector(IDistance distanceCalculator, Func<IEnumerable<Firework>, Firework> bestFireworkSelector, int samplingNumber)
        {
            if (distanceCalculator == null)
            {
                throw new ArgumentNullException("distanceCalculator");
            }

            if (bestFireworkSelector == null)
            {
                throw new ArgumentNullException("bestFireworkSelector");
            }

            if (samplingNumber < 0)
            {
                throw new ArgumentOutOfRangeException("samplingNumber");
            }

            this.distanceCalculator = distanceCalculator;
            this.bestFireworkSelector = bestFireworkSelector;
            this.samplingNumber = samplingNumber;
        }

        public NearBestSelector(IDistance distanceCalculator, Func<IEnumerable<Firework>, Firework> bestFireworkSelector)
            : this(distanceCalculator, bestFireworkSelector, 0)
        {
        }

        public virtual IEnumerable<Firework> Select(IEnumerable<Firework> from)
        {
            return this.Select(from, this.samplingNumber);
        }

        public virtual IEnumerable<Firework> Select(IEnumerable<Firework> from, int numberToSelect)
        {
            if (from == null)
            {
                throw new ArgumentNullException("from");
            }

            if (numberToSelect < 0)
            {
                throw new ArgumentOutOfRangeException("numberToSelect");
            }

            if (numberToSelect > from.Count())
            {
                throw new ArgumentOutOfRangeException("numberToSelect");
            }

            if (numberToSelect == from.Count())
            {
                return new List<Firework>(from);
            }

            List<Firework> selectedLocations = new List<Firework>(numberToSelect);
            if (numberToSelect == 0)
            {
                return selectedLocations;
            }         

            if (numberToSelect >= 1)
            {
                Debug.Assert(this.bestFireworkSelector != null, "Best firework selector is null");

                // 1. Find a firework with best quality
                Firework bestFirework = this.bestFireworkSelector(from);

                // 2. Calculate distances near best firework
                IDictionary<Firework, double> distances = this.CalculateDistances(from, bestFirework);
                Debug.Assert(distances != null, "Distance collection is null");

                // 3. Select nearest individuals
                IOrderedEnumerable<KeyValuePair<Firework, double>> sortedDistances = distances.OrderBy(p => p.Value, new DoubleExtensions.DoubleExtensionComparer());
                Debug.Assert(sortedDistances != null, "Sorted distances collection is null");

                IEnumerable<Firework> nearestLocations = sortedDistances.Take(numberToSelect).Select(sp => sp.Key);
                Debug.Assert(nearestLocations != null, "Nearest locations collection is null");

                selectedLocations.AddRange(nearestLocations);
            }

            return selectedLocations;
        }

        protected virtual IDictionary<Firework, double> CalculateDistances(IEnumerable<Firework> allCurrentFireworks, Firework bestFirework)
        {
            if (allCurrentFireworks == null)
            {
                throw new ArgumentNullException("allCurrentFireworks");
            }

            if (bestFirework == null)
            {
                throw new ArgumentNullException("bestFirework");
            }

            Debug.Assert(this.distanceCalculator != null, "Distance calculator is null");

            Dictionary<Firework, double> distances = new Dictionary<Firework, double>(allCurrentFireworks.Count() - 1);
            foreach (Firework firework in allCurrentFireworks)
            {
                Debug.Assert(firework != null, "Firework is null");

                if (firework != bestFirework)
                {
                    double distance = this.distanceCalculator.Calculate(bestFirework, firework);
                    distances.Add(firework, distance);
                }                               
            }

            return distances;
        }
    }
}