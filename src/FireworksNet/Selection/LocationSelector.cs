using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FireworksNet.Distances;
using FireworksNet.Extensions;
using FireworksNet.Model;

namespace FireworksNet.Selection
{
    public class LocationSelector : ISelector
    {
        private readonly IDistance distanceCalculator;
        private readonly Func<IEnumerable<Firework>, Firework> bestFireworkSelector;
        private readonly int locationsNumber;

        public LocationSelector(IDistance distanceCalculator, Func<IEnumerable<Firework>, Firework> bestFireworkSelector, int locationsNumber)
        {
            if (distanceCalculator == null)
            {
                throw new ArgumentNullException("distanceCalculator");
            }

            if (bestFireworkSelector == null)
            {
                throw new ArgumentNullException("bestFireworkSelector");
            }

            if (locationsNumber < 0)
            {
                throw new ArgumentOutOfRangeException("locationsNumber");
            }

            this.locationsNumber = locationsNumber;
            this.distanceCalculator = distanceCalculator;
            this.bestFireworkSelector = bestFireworkSelector;
        }

        public LocationSelector(IDistance distanceCalculator, Func<IEnumerable<Firework>, Firework> bestFireworkSelector)
            : this(distanceCalculator, bestFireworkSelector, 0)
        {
        }

        public virtual IEnumerable<Firework> Select(IEnumerable<Firework> from)
        {
            return this.Select(from, this.locationsNumber);
        }

        // Always returns new collection
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
                // At some point, we may need to return just as much as we have
                // instead of throwing an exception.
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

            Debug.Assert(this.bestFireworkSelector != null, "Best firework selector is null");

            // 1. Find a firework with best quality - it will be kept anyways
            Firework bestFirework = this.bestFireworkSelector(from);
            selectedLocations.Add(bestFirework);

            if (numberToSelect > 1)
            {
                // 2. Calculate distances between all fireworks
                IDictionary<Firework, double> distances = this.CalculateDistances(from);
                Debug.Assert(distances != null, "Distance collection is null");

                // 3. Calculate probabilities for each firework
                IDictionary<Firework, double> probabilities = this.CalculateProbabilities(distances);
                Debug.Assert(probabilities != null, "Probability collection is null");

                // 4. Select desiredLocationsNumber - 1 of fireworks based on the probabilities
                IOrderedEnumerable<KeyValuePair<Firework, double>> sortedProbabilities = probabilities.OrderByDescending(p => p.Value, new DoubleExtensions.DoubleExtensionComparer());
                Debug.Assert(sortedProbabilities != null, "Sorted probabilities collection is null");

                IEnumerable<Firework> otherSelectedLocations = sortedProbabilities.Where(sp => sp.Key != bestFirework).Take(numberToSelect - 1).Select(sp => sp.Key);
                Debug.Assert(otherSelectedLocations != null, "Other selected locations collection is null");

                selectedLocations.AddRange(otherSelectedLocations);
            }

            return selectedLocations;
        }

        protected virtual IDictionary<Firework, double> CalculateDistances(IEnumerable<Firework> allCurrentFireworks)
        {
            // TODO: Maybe there is more efficient way to calc distance between each and each points?

            if (allCurrentFireworks == null)
            {
                throw new ArgumentNullException("allCurrentFireworks");
            }

            Debug.Assert(this.distanceCalculator != null, "Distance calculator is null");

            Dictionary<Firework, double> distances = new Dictionary<Firework, double>(allCurrentFireworks.Count());
            foreach (Firework firework in allCurrentFireworks)
            {
                Debug.Assert(firework != null, "Firework is null");

                distances.Add(firework, 0.0);
                foreach (Firework otherFirework in allCurrentFireworks)
                {
                    Debug.Assert(otherFirework != null, "Other firework is null");

                    distances[firework] += this.distanceCalculator.Calculate(firework, otherFirework);
                }
            }

            return distances;
        }

        protected virtual IDictionary<Firework, double> CalculateProbabilities(IDictionary<Firework, double> distances)
        {
            if (distances == null)
            {
                throw new ArgumentNullException("distances");
            }

            Dictionary<Firework, double> probabilities = new Dictionary<Firework, double>(distances.Count());
            double distancesSum = distances.Values.Sum();
            Debug.Assert(!distancesSum.IsEqual(0.0), "Distances sum is 0");

            foreach (KeyValuePair<Firework, double> distance in distances)
            {
                Debug.Assert(distance.Key != null, "Firework is null");

                double probability = distance.Value / distancesSum;
                probabilities.Add(distance.Key, probability);
            }

            return probabilities;
        }
    }
}