using System;
using MathNet.Numerics.Random;

namespace FireworksNet.Random
{
    /// <summary>
    /// Represents a pseudo-random number generator.
    /// </summary>
    /// <remarks>Uses <see cref="SystemRandomSource"/> thread-safe wrapper
    /// around <see cref="Random"/> that comes with Math.NET Numerics.</remarks>
    public class DefaultRandom : System.Random
    {
        private readonly SystemRandomSource randomizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultRandom"/> class.
        /// </summary>
        public DefaultRandom()
        {
            this.randomizer = new SystemRandomSource();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultRandom"/> class.
        /// </summary>
        /// <param name="seed">The seed.</param>
        public DefaultRandom(int seed)
            : this(seed, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultRandom"/> class.
        /// </summary>
        /// <param name="seed">The seed.</param>
        /// <param name="threadSafe">If set to <c>true</c>, a thread safe version
        /// is used. Default value is <c>true</c>.</param>
        public DefaultRandom(int seed, bool threadSafe)
        {
            this.randomizer = new SystemRandomSource(seed, threadSafe);
        }

        #region System.Random overrides

        /// <summary>
        /// Returns a nonnegative random number.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer greater than or equal to zero and less than <see cref="Int32.MaxValue"/>.
        /// </returns>
        public override int Next()
        {
            return this.randomizer.Next();
        }

        /// <summary>
        /// Returns a nonnegative random number less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated.
        /// <paramref name="maxValue"/> must be greater than or equal to zero.</param>
        /// <returns>
        /// A 32-bit signed integer greater than or equal to zero, and less than <paramref name="maxValue"/>;
        /// that is, the range of return values ordinarily includes zero but not <paramref name="maxValue"/>.
        /// However, if <paramref name="maxValue"/> equals zero, <paramref name="maxValue"/> is returned.
        /// </returns>
        public override int Next(int maxValue)
        {
            return this.randomizer.Next(maxValue);
        }

        /// <summary>
        /// Returns a random number within a specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number returned.
        /// <paramref name="maxValue"/> must be greater than or equal to <paramref name="minValue"/>.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer greater than or equal to <paramref name="minValue"/> and less than
        /// <paramref name="maxValue"/>; that is, the range of return values includes <paramref name="minValue"/>
        /// but not <paramref name="maxValue"/>. If <paramref name="minValue"/> equals <paramref name="maxValue"/>,
        /// <paramref name="minValue"/> is returned.
        /// </returns>
        public override int Next(int minValue, int maxValue)
        {
            return this.randomizer.Next(minValue, maxValue);
        }

        /// <summary>
        /// Fills the elements of a specified array of bytes with random numbers.
        /// </summary>
        /// <param name="buffer">An array of bytes to contain random numbers.</param>
        public override void NextBytes(Byte[] buffer)
        {
            this.randomizer.NextBytes(buffer);
        }

        /// <summary>
        /// Returns a random number between 0.0 and 1.0.
        /// </summary>
        /// <returns>
        /// A double-precision floating point number greater than or equal to 0.0, and less than 1.0.
        /// </returns>
        public override double NextDouble()
        {
            return this.randomizer.NextDouble();
        }

        #endregion
    }
}