using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using FireworksNet.Model;
using FireworksNet.Extensions;

namespace FireworksNet.Selection
{
    public class RandomSelector : SelectorBase
    {
        private readonly System.Random randomizer;
        private readonly int samplingNumber;

        public RandomSelector(System.Random randomizer, int samplingNumber)
        {
            if (randomizer == null)
            {
                throw new ArgumentNullException("randomzier");
            }

            if (samplingNumber < 0)
            {
                throw new ArgumentOutOfRangeException("samplingNumber");
            }

            this.randomizer = randomizer;
            this.samplingNumber = samplingNumber;            
        }

        public RandomSelector(System.Random randomizer)
            : this(randomizer, 0)
        {
        }

        public override IEnumerable<Firework> Select(IEnumerable<Firework> from)
        {
            return this.Select(from, this.samplingNumber);
        }

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
                // 1. Generate number of fireworks using random
                IEnumerable<int> selectedFireworkIndices = this.randomizer.NextInt32s(numberToSelect, 0, from.Count());
                Debug.Assert(selectedFireworkIndices != null, "Generated numbers collection is null");

                // 2. Find fireworks by generated numbers
                int currentFirework = 0;
                foreach(Firework firework in from)
                {
                    Debug.Assert(firework != null, "Firework in null");

                    if (selectedFireworkIndices.Contains(currentFirework))
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