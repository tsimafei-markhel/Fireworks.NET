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

        public DefaultRandom()
        {
            this.randomizer = new SystemRandomSource();
        }

        public DefaultRandom(int seed)
            : this(seed, true)
        {
        }

        public DefaultRandom(int seed, bool threadSafe)
        {
            this.randomizer = new SystemRandomSource(seed, threadSafe);
        }

        #region System.Random overrides

        public override int Next()
        {
            return this.randomizer.Next();
        }

        public override int Next(int maxValue)
        {
            return this.randomizer.Next(maxValue);
        }

        public override int Next(int minValue, int maxValue)
        {
            return this.randomizer.Next(minValue, maxValue);
        }

        public override void NextBytes(Byte[] buffer)
        {
            this.randomizer.NextBytes(buffer);
        }

        public override double NextDouble()
        {
            return this.randomizer.NextDouble();
        }

        #endregion
    }
}