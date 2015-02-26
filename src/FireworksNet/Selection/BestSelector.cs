using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using FireworksNet.Model;

namespace FireworksNet.Selection
{
    public class BestSelector : ISelector
    {
        private readonly Func<IEnumerable<Firework>, Firework> bestFireworkSelector;
        private readonly int samplingNumber;

        public BestSelector(Func<IEnumerable<Firework>, Firework> bestFireworkSelector, int samplingNumber)
        {
            if (bestFireworkSelector == null)
            {
                throw new ArgumentNullException("bestFireworkSelector");
            }

            if (samplingNumber < 0)
            {
                throw new ArgumentOutOfRangeException("samplingNumber");
            }

            this.bestFireworkSelector = bestFireworkSelector;
            this.samplingNumber = samplingNumber;
        }

        public BestSelector(Func<IEnumerable<Firework>, Firework> bestFireworkSelector)
            : this(bestFireworkSelector, 0)
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