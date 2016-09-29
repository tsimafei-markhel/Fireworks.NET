using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FireworksNet.Model;

namespace FireworksNet.Selection
{
    /// <summary>
    /// Selects <see cref="Firework"/>s that will stay around for the next step:
    /// takes number of the best <see cref="Firework"/>s, per 2012 paper.
    /// </summary>
    public class BestFireworkSelector : FireworkSelectorBase
    {
        private readonly Func<IEnumerable<Firework>, Firework> bestFireworkSelector;

        /// <summary>
        /// Initializes a new instance of the <see cref="BestFireworkSelector"/> class.
        /// </summary>
        /// <param name="bestFireworkSelector">The function that can be used to select 
        /// best <see cref="Firework"/>.</param>
        /// <param name="locationsNumber">The number of <see cref="Firework"/>s to be selected.</param>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="bestFireworkSelector"/> 
        /// is <c>null</c>.
        /// </exception>
        public BestFireworkSelector(Func<IEnumerable<Firework>, Firework> bestFireworkSelector, int locationsNumber)
            : base(locationsNumber)
        {
            if (bestFireworkSelector == null)
            {
                throw new ArgumentNullException(nameof(bestFireworkSelector));
            }

            this.bestFireworkSelector = bestFireworkSelector;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BestFireworkSelector"/> class.
        /// </summary>
        /// <param name="bestFireworkSelector">The function that can be used to select 
        /// best <see cref="Firework"/>.</param>
        /// <remarks>It is assumed that number of <see cref="Firework"/>s to be selected
        /// differs from step to step and hence is passed to the <c>Select</c> method.</remarks>
        public BestFireworkSelector(Func<IEnumerable<Firework>, Firework> bestFireworkSelector)
            : this(bestFireworkSelector, 0)
        {
        }

        /// <summary>
        /// Selects <paramref name="numberToSelect"/> the best <see cref="Firework"/>s from
        /// the <paramref name="from"/> collection. Selected <see cref="Firework"/>s
        /// are stored in the new collection, <paramref name="from"/> is not modified.
        /// </summary>
        /// <param name="from">A collection to select <see cref="Firework"/>s
        /// from.</param>
        /// <param name="numberToSelect">The number of <see cref="Firework"/>s
        /// to select.</param>
        /// <returns>
        /// A subset of the best <see cref="Firework"/>s.
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
                throw new ArgumentNullException(nameof(from));
            }

            if (numberToSelect < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(numberToSelect));
            }

            if (numberToSelect > from.Count())
            {
                throw new ArgumentOutOfRangeException(nameof(numberToSelect));
            }

            if (numberToSelect == from.Count())
            {
                return new List<Firework>(from);
            }

            Debug.Assert(this.bestFireworkSelector != null, "Best firework selector is null");

            List<Firework> bestFireworks = new List<Firework>(numberToSelect);
            if (numberToSelect == 1)
            {
                // Handle "give me one best firework" case separately
                // for performance
                bestFireworks.Add(this.bestFireworkSelector(from));
            }
            else if (numberToSelect > 1)
            {
                // Find fireworks with the best quality based on a sampling number
                List<Firework> currentFireworks = new List<Firework>(from);
                for (int i = 0; i < numberToSelect; i++)
                {
                    // TODO: It makes sense to sort the collection first, and then take
                    // the first ones.
                    Firework bestFirework = this.bestFireworkSelector(currentFireworks);
                    bestFireworks.Add(bestFirework);
                    currentFireworks.Remove(bestFirework);
                }
            }

            return bestFireworks;
        }
    }
}