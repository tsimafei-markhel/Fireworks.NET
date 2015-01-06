using System;
using System.Collections.Generic;
using FireworksNet.Model;

namespace FireworksNet.Extensions
{
    public static class RandomExtensions
    {
        public static Double NextDouble(this System.Random random, Double from, Double to)
        {
            return NextDoubleInternal(random, from, to - from);
        }

        public static Double NextDouble(this System.Random random, Range allowedRange)
        {
            bool gotCorrectValue = false;
            double correctValue;
            do
            {
                correctValue = NextDoubleInternal(random, allowedRange.Minimum, allowedRange.Length);
                gotCorrectValue = !((allowedRange.IsMinimumOpen && allowedRange.Minimum.IsEqual(correctValue))
                                 || (allowedRange.IsMaximumOpen && allowedRange.Maximum.IsEqual(correctValue)));
            }
            while (!gotCorrectValue);

            return correctValue;
        }

        /// <summary>
        /// Returns an enumerable of random integer numbers.
        /// </summary>
        /// <param name="random">The random number generator.</param>
        /// <param name="neededValuesNumber">The amount of values to be generated.</param>
        /// <param name="minInclusive">Lower bound, inclusive.</param>
        /// <param name="maxExclusive">Upper bound, exclusive.</param>
        public static IEnumerable<Int32> NextInt32s(this System.Random random, Int32 neededValuesNumber, Int32 minInclusive, Int32 maxExclusive)
        {
            List<int> result = new List<int>(neededValuesNumber);
            for (int i = 0; i < neededValuesNumber; i++)
            {
                result.Add(random.Next(minInclusive, maxExclusive));
            }

            return result;
        }

        private static Double NextDoubleInternal(System.Random random, Double from, Double intervalLength)
        {
            return from + random.NextDouble() * intervalLength;
        }
    }
}