using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FireworksNet.Distances;
using FireworksNet.Extensions;
using FireworksNet.Model;

namespace FireworksNet.Selection
{
    /// <summary>
    /// Selects <see cref="Firework"/>s that will stay around for the next step
    /// based on the distance between the best <see cref="Firework"/> and each other 
    /// <see cref="Firework"/>s, per 2012 paper.
    /// </summary>
    public class NearBestFireworkSelector : FireworkSelectorBase
    {
        private readonly IDistance distanceCalculator;
        private readonly Func<IEnumerable<Firework>, Firework> bestFireworkSelector;

        /// <summary>
        /// Initializes a new instance of the <see cref="NearBestFireworkSelector"/> class.
        /// </summary>
        /// <param name="distanceCalculator">The distance calculator.</param>
        /// <param name="bestFireworkSelector">The function that can be used to select 
        /// best <see cref="Firework"/>.</param>
        /// <param name="locationsNumber">The number of <see cref="Firework"/>s to be selected.</param>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="distanceCalculator"/>
        /// or <paramref name="bestFireworkSelector"/> is <c>null</c>.
        /// </exception>
        public NearBestFireworkSelector(IDistance distanceCalculator, Func<IEnumerable<Firework>, Firework> bestFireworkSelector, int locationsNumber)
            : base(locationsNumber)
        {
            if (distanceCalculator == null)
            {
                throw new ArgumentNullException("distanceCalculator");
            }

            if (bestFireworkSelector == null)
            {
                throw new ArgumentNullException("bestFireworkSelector");
            }

            this.distanceCalculator = distanceCalculator;
            this.bestFireworkSelector = bestFireworkSelector;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NearBestFireworkSelector"/> class.
        /// </summary>
        /// <param name="distanceCalculator">The distance calculator.</param>
        /// <param name="bestFireworkSelector">The function that can be used to select 
        /// best <see cref="Firework"/>.</param>
        /// <remarks>It is assumed that number of <see cref="Firework"/>s to be selected
        /// differs from step to step and hence is passed to the <c>Select</c> method.</remarks>
        public NearBestFireworkSelector(IDistance distanceCalculator, Func<IEnumerable<Firework>, Firework> bestFireworkSelector)
            : this(distanceCalculator, bestFireworkSelector, 0)
        {
        }

        /// <summary>
        /// Selects <paramref name="numberToSelect"/> <see cref="Firework"/>s from
        /// the <paramref name="from"/> collection. Selected <see cref="Firework"/>s
        /// are stored in the new collection, <paramref name="from"/> is not modified.
        /// </summary>
        /// <param name="from">A collection to select <see cref="Firework"/>s
        /// from.</param>
        /// <param name="numberToSelect">The number of <see cref="Firework"/>s
        /// to select.</param>
        /// <returns>
        /// A subset of <see cref="Firework"/>s.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="from"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException"> if <paramref name="numberToSelect"/>
        /// is less than zero or greater than the number of elements in <paramref name="from"/>.
        /// </exception>
        public override IEnumerable<Firework> SelectFireworks(IEnumerable<Firework> from, int numberToSelect)
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
                IOrderedEnumerable<KeyValuePair<Firework, double>> sortedDistances = distances.OrderBy(p => p.Value, new DoubleExtensionComparer());
                Debug.Assert(sortedDistances != null, "Sorted distances collection is null");

                IEnumerable<Firework> nearestLocations = sortedDistances.Take(numberToSelect).Select(sp => sp.Key);
                Debug.Assert(nearestLocations != null, "Nearest locations collection is null");

                selectedLocations.AddRange(nearestLocations);
            }

            return selectedLocations;
        }

        /// <summary>
        /// Calculates the distances between each <see cref="Firework"/> and best 
        /// <see cref="Firework"/>.
        /// </summary>
        /// <param name="allCurrentFireworks">The collection of <see cref="Firework"/>s to calculate
        /// distances between.</param>
        /// <param name="bestFirework">The best <see cref="Firework"/> to calculate
        /// distances between.</param>
        /// <returns>A map. Key is a <see cref="Firework"/>. Value is a distance
        /// between that <see cref="Firework"/> and the best <see cref="Firework"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="allCurrentFireworks"/> and 
        /// <paramref name="bestFirework"/> is <c>null</c>.
        /// </exception>
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