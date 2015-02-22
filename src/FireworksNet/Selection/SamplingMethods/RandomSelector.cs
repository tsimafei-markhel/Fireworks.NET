using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using FireworksNet.Model;
using FireworksNet.Random;

namespace FireworksNet.Selection.SamplingMethods
{
    public class RandomSelector : ISelector
    {
        private readonly DefaultRandom randomizer;
        private readonly Func<IEnumerable<Firework>, Firework> bestFireworkSelector;
        private readonly int samplingNumber;

        public RandomSelector(DefaultRandom randomizer, Func<IEnumerable<Firework>, Firework> bestFireworkSelector, int samplingNumber)
        {
            if (randomizer == null)
            {
                throw new ArgumentNullException("randomzier");
            }

            if (bestFireworkSelector == null)
            {
                throw new ArgumentNullException("bestFireworkSelector");
            }

            if (samplingNumber < 0)
            {
                throw new ArgumentOutOfRangeException("locationsNumber");
            }

            this.samplingNumber = samplingNumber;
            this.randomizer = randomizer;
            this.bestFireworkSelector = bestFireworkSelector;
        }

        public RandomSelector(DefaultRandom randomizer, Func<IEnumerable<Firework>, Firework> bestFireworkSelector)
            : this(randomizer, bestFireworkSelector, 0)
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

            if (numberToSelect >= 1)
            {
                // 1. Generate number of fireworks using random
                IList<int> generatedNumber = this.GenerateNumbers(from.Count(), numberToSelect);
                Debug.Assert(generatedNumber != null, "Generated numbers collection is null");

                // 2. Find fireworks by generated numbers
                foreach (int number in generatedNumber)
                {
                    Firework location = from.ElementAt(number);
                    selectedLocations.Add(location);
                }           
            }

            return selectedLocations;
        }

        protected virtual IList<int> GenerateNumbers(int countLocations, int numberToSelect)
        {
            List<int> numberLocations= new List<int>(numberToSelect);

            for (int index = 0; index < numberToSelect; index++)
            {
                int number = this.randomizer.Next(0, countLocations - 1);
                numberLocations.Add(number);
            }

            return numberLocations;
        }
        
    }
}

