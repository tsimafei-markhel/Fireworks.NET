using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FireworksNet.Extensions;
using FireworksNet.Model;

namespace FireworksNet.Selection
{
    /// <summary>
    /// Selects <see cref="Firework"/>s that will stay around for the next step:
    /// takes the best <see cref="Firework"/> and randomly chooses others, per 2012 paper.
    /// </summary>
    public class BestAndRandomSelector : SelectorBase
    {
        private readonly System.Random randomizer;
        private readonly Func<IEnumerable<Firework>, Firework> bestFireworkSelector;

        /// <summary>
        /// Initializes a new instance of the <see cref="BestAndRandomSelector"/> class.
        /// </summary>
        /// <param name="randomizer">The random number generator.</param>
        /// <param name="bestFireworkSelector">The function that can be used to select 
        /// best <see cref="Firework"/>.</param>
        /// <param name="locationsNumber">The number of <see cref="Firework"/>s to be selected.</param>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="randomizer"/>
        /// or <paramref name="bestFireworkSelector"/> is <c>null</c>.
        /// </exception>
        public BestAndRandomSelector(System.Random randomizer, Func<IEnumerable<Firework>, Firework> bestFireworkSelector, int locationsNumber)
            : base(locationsNumber)
        {
            if (randomizer == null)
            {
                throw new ArgumentNullException("randomizer");
            }

            if (bestFireworkSelector == null)
            {
                throw new ArgumentNullException("bestFireworkSelector");
            }

            this.randomizer = randomizer;
            this.bestFireworkSelector = bestFireworkSelector;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BestAndRandomSelector"/> class.
        /// </summary>
        /// <param name="randomizer">The random number generator.</param>
        /// <param name="bestFireworkSelector">The function that can be used to select 
        /// best <see cref="Firework"/>.</param>
        /// <remarks>It is assumed that number of <see cref="Firework"/>s to be selected
        /// differs from step to step and hence is passed to the <c>Select</c> method.</remarks>
        public BestAndRandomSelector(System.Random randomizer, Func<IEnumerable<Firework>, Firework> bestFireworkSelector)
            : this(randomizer, bestFireworkSelector, 0)
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
        public override IEnumerable<Firework> Select(IEnumerable<Firework> from, int numberToSelect)
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
                Debug.Assert(this.randomizer != null, "Randomizer is null");

                // 2. Select others randomly
                IList<Firework> fromWithoutBest = new List<Firework>(from);
                fromWithoutBest.Remove(bestFirework);

                IEnumerable<int> selectedFireworksIndices = this.randomizer.NextUniqueInt32s(numberToSelect - 1, 0, fromWithoutBest.Count());
                Debug.Assert(selectedFireworksIndices != null, "Selected firework indices collection is null");

                int currentFirework = 0;
                foreach (Firework firework in fromWithoutBest)
                {
                    Debug.Assert(firework != null, "Firework is null");

                    if (selectedFireworksIndices.Contains(currentFirework))
                    {
                        selectedLocations.Add(firework);
                    }

                    currentFirework++;
                }
            }

            return selectedLocations;
        }
    }
}