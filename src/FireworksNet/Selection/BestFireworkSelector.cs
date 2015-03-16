using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using FireworksNet.Model;

namespace FireworksNet.Selection
{
    /// <summary>
    /// Selects <see cref="Firework"/>s that will stay around for the next step:
    /// takes number of best <see cref="Firework"/>s, per 2012 paper.
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
                throw new ArgumentNullException("bestFireworkSelector");
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

            List<Firework> bestFireworks = new List<Firework>(numberToSelect);
            if (numberToSelect == 0)
            {
                return bestFireworks;
            }

            Debug.Assert(this.bestFireworkSelector != null, "Best firework selector is null");

            if (numberToSelect >= 1)
            {
                // Find number of fireworks with best quality based on sampling number
                List<Firework> currentFireworks = new List<Firework>(from);

                for (int i = 0; i < numberToSelect; i++)
                {
                    Firework bestFirework = this.bestFireworkSelector(currentFireworks);

                    bestFireworks.Add(bestFirework);
                    currentFireworks.Remove(bestFirework);
                }
            }

            return bestFireworks;
        }
    }
}