using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using FireworksNet.Model;
using FireworksNet.Random;
using FireworksNet.Extensions;

namespace FireworksNet.Selection
{
    public class RandomSelector : ISelector
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

            this.samplingNumber = samplingNumber;
            this.randomizer = randomizer;
        }

        public RandomSelector(System.Random randomizer)
            : this(randomizer, 0)
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
                // 1. Generate number of fireworks using random
                IEnumerable<int> generatedNumber = this.randomizer.NextInt32s(numberToSelect, 0, from.Count() - 1);
                Debug.Assert(generatedNumber != null, "Generated numbers collection is null");

                // 2. Find fireworks by generated numbers
                foreach (int number in generatedNumber)
                {
                    Firework firework = from.ElementAt(number);
                    Debug.Assert(firework != null, "Firework in null");

                    selectedLocations.Add(firework);
                }           
            }

            return selectedLocations;
        }     
    }
}

