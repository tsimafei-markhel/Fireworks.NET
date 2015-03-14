using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FireworksNet.Model;

namespace FireworksNet.Extensions
{
    /// <summary>
    /// Contains helper extension methods for <see cref="Random"/>.
    /// </summary>
    public static class RandomExtensions
    {
        /// <summary>
        /// Generates a random <see cref="double"/> within specified range.
        /// </summary>
        /// <param name="random">The random number generator.</param>
        /// <param name="minInclusive">Lower bound, inclusive.</param>
        /// <param name="maxExclusive">Upper bound, exclusive.</param>
        /// <returns>A random <see cref="double"/> within specified range.</returns>
        public static double NextDouble(this System.Random random, double minInclusive, double maxExclusive)
        {
            return RandomExtensions.NextDoubleInternal(random, minInclusive, maxExclusive - minInclusive);
        }

        /// <summary>
        /// Generates a random <see cref="double"/> within specified range.
        /// </summary>
        /// <param name="random">The random number generator.</param>
        /// <param name="allowedRange">A range that will contain generated number.</param>
        /// <returns>A random <see cref="double"/> within specified range.</returns>
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
                correctValue = RandomExtensions.NextDoubleInternal(random, allowedRange.Minimum, allowedRange.Length);

                // 1. 'Is generated value within a range' check is missed intentionally.
                // 2. Even though upper bound is always exclusive, second check should stay here.
                gotCorrectValue = !((allowedRange.IsMinimumOpen && allowedRange.Minimum.IsEqual(correctValue))
                                 || (allowedRange.IsMaximumOpen && allowedRange.Maximum.IsEqual(correctValue)));
            }
            while (!gotCorrectValue);

            return correctValue;
        }

        /// <summary>
        /// Generates an enumerable of random integer numbers.
        /// </summary>
        /// <param name="random">The random number generator.</param>
        /// <param name="neededValuesNumber">The amount of values to be generated.</param>
        /// <param name="minInclusive">Lower bound, inclusive.</param>
        /// <param name="maxExclusive">Upper bound, exclusive.</param>
        /// <returns>An enumerable of random integer numbers.</returns>
        /// <exception cref="System.ArgumentNullException">if <paramref name="random"/> is <c>null</c>.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException"> if <paramref name="neededValuesNumber"/>
        /// is less than zero. Or if <paramref name="neededValuesNumber"/> &gt; <paramref name="maxExclusive"/> 
        /// - <paramref name="minInclusive"/>. Or if <paramref name="maxExclusive"/> is less or equal to
        /// <paramref name="minInclusive"/>.</exception>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "s", Justification = "Lowercase 's' is used here intentionally to create plural form.")]
        public static IEnumerable<int> NextInt32s(this System.Random random, int neededValuesNumber, int minInclusive, int maxExclusive)
        {
            if (random == null)
            {
                throw new ArgumentNullException("random");
            }

            if (neededValuesNumber < 0)
            {
                throw new ArgumentOutOfRangeException("neededValuesNumber");
            }

            if (neededValuesNumber > maxExclusive - minInclusive && maxExclusive - minInclusive > 0) // maxExclusive - minInclusive > 0 is required to avoid overflow
            {
                throw new ArgumentOutOfRangeException("neededValuesNumber");
            }

            if (maxExclusive <= minInclusive)
            {
                throw new ArgumentOutOfRangeException("maxExclusive");
            }

            List<int> result = new List<int>(neededValuesNumber);
            for (int i = 0; i < neededValuesNumber; i++)
            {
                result.Add(random.Next(minInclusive, maxExclusive));
            }

            return result;
        }

        /// <summary>
        /// Generates an enumerable of random integer numbers, unique within this enumerable.
        /// </summary>
        /// <param name="random">The random number generator.</param>
        /// <param name="neededValuesNumber">The amount of values to be generated.</param>
        /// <param name="minInclusive">Lower bound, inclusive.</param>
        /// <param name="maxExclusive">Upper bound, exclusive.</param>
        /// <returns>An enumerable of random integer numbers, unique within this enumerable.</returns>
        /// <exception cref="System.ArgumentNullException">if <paramref name="random"/> is <c>null</c>.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException"> if <paramref name="neededValuesNumber"/>
        /// is less than zero. Or if <paramref name="neededValuesNumber"/> &gt; <paramref name="maxExclusive"/> 
        /// - <paramref name="minInclusive"/>. Or if <paramref name="maxExclusive"/> is less or equal to
        /// <paramref name="minInclusive"/>.</exception>
        /// <remarks>http://codereview.stackexchange.com/questions/61338/generate-random-numbers-without-repetitions</remarks>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "s", Justification = "Lowercase 's' is used here intentionally to create plural form.")]
        public static IEnumerable<int> NextUniqueInt32s(this System.Random random, int neededValuesNumber, int minInclusive, int maxExclusive)
        {
            if (random == null)
            {
                throw new ArgumentNullException("random");
            }

            if (neededValuesNumber < 0)
            {
                throw new ArgumentOutOfRangeException("neededValuesNumber");
            }

            if (neededValuesNumber > maxExclusive - minInclusive && maxExclusive - minInclusive > 0) // maxExclusive - minInclusive > 0 is required to avoid overflow
            {
                throw new ArgumentOutOfRangeException("neededValuesNumber");
            }

            if (maxExclusive <= minInclusive)
            {
                throw new ArgumentOutOfRangeException("maxExclusive");
            }

            HashSet<int> uniqueNumbers = new HashSet<int>();
            for (int top = maxExclusive - neededValuesNumber; top < maxExclusive; top++)
            {
                if (!uniqueNumbers.Add(random.Next(minInclusive, top + 1)))
                {
                    uniqueNumbers.Add(top);
                }
            }

            List<int> result = uniqueNumbers.ToList();
            int temp = 0;
            for (int i = result.Count - 1; i > 0; i--)
            {
                int k = random.Next(i + 1);
                temp = result[k];
                result[k] = result[i];
                result[i] = temp;
            }

            return result;
        }

        /// <summary>
        /// Generates a random <see cref="double"/> within specified range.
        /// </summary>
        /// <param name="random">The random number generator.</param>
        /// <param name="minInclusive">Lower bound, inclusive.</param>
        /// <param name="intervalLength">The length of the range that has to contain generated number.</param>
        /// <returns>A random <see cref="double"/> within specified range.</returns>
        /// <exception cref="System.ArgumentNullException">if <paramref name="random"/> is <c>null</c>.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">if <paramref name="minInclusive"/> or
        /// <paramref name="intervalLength"/> is NaN or Infinity.</exception>
        private static double NextDoubleInternal(System.Random random, double minInclusive, double intervalLength)
        {
            if (random == null)
            {
                throw new ArgumentNullException("random");
            }

            if (double.IsNaN(minInclusive) || double.IsInfinity(minInclusive))
            {
                throw new ArgumentOutOfRangeException("minInclusive");
            }

            if (double.IsNaN(intervalLength) || double.IsInfinity(intervalLength))
            {
                throw new ArgumentOutOfRangeException("intervalLength");
            }

            return minInclusive + random.NextDouble() * intervalLength;
        }
    }
}