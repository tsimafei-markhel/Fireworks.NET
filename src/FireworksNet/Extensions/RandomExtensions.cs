using System.Collections.Generic;
using FireworksNet.Model;

namespace FireworksNet.Extensions
{
    /// <summary>
    /// Contains helper extension methods for <see cref="Random"/>.
    /// </summary>
    public static class RandomExtensions
    {
        /// <summary>
        /// Returns random <see cref="double"/> within specified range.
        /// </summary>
        /// <param name="random">The random number generator.</param>
        /// <param name="minInclusive">Lower bound, inclusive.</param>
        /// <param name="maxExclusive">Upper bound, exclusive.</param>
        public static double NextDouble(this System.Random random, double minInclusive, double maxExclusive)
        {
            return NextDoubleInternal(random, minInclusive, maxExclusive - minInclusive);
        }

        /// <summary>
        /// Returns random <see cref="double"/> within specified range.
        /// </summary>
        /// <param name="random">The random number generator.</param>
        /// <param name="allowedRange">A range that will contain generated number.</param>
        /// <remarks>
        /// <see cref="Range.Maximum"/> is excluded even if <see cref="allowedRange.IsMaximumOpen"/>
		/// is set to <c>false</c> (i.e. upper bound is exclusive).
        /// </remarks>
        public static double NextDouble(this System.Random random, Range allowedRange)
        {
            bool gotCorrectValue = false;
            double correctValue;
            do
            {
                correctValue = NextDoubleInternal(random, allowedRange.Minimum, allowedRange.Length);

                // 1. 'Is generated value within a range' check is missed intentionally.
                // 2. Even though upper bound is always exclusive, second check should stay here.
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
        public static IEnumerable<int> NextInt32s(this System.Random random, int neededValuesNumber, int minInclusive, int maxExclusive)
        {
            List<int> result = new List<int>(neededValuesNumber);
            for (int i = 0; i < neededValuesNumber; i++)
            {
                result.Add(random.Next(minInclusive, maxExclusive));
            }

            return result;
        }

        /// <summary>
        /// Returns random <see cref="double"/> within specified range.
        /// </summary>
        /// <param name="random">The random number generator.</param>
        /// <param name="minInclusive">Lower bound, inclusive.</param>
        /// <param name="intervalLength">The length of the range that has to contain generated number.</param>
        private static double NextDoubleInternal(System.Random random, double minInclusive, double intervalLength)
        {
            return minInclusive + random.NextDouble() * intervalLength;
        }
    }
}