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
            randomizer = new SystemRandomSource();
        }

        public DefaultRandom(int seed)
            : this(seed, true)
        {
        }

        public DefaultRandom(int seed, bool threadSafe)
        {
            randomizer = new SystemRandomSource(seed, threadSafe);
        }

        #region System.Random overrides

        public override int Next()
        {
            return randomizer.Next();
        }

        public override int Next(int maxValue)
        {
            return randomizer.Next(maxValue);
        }

        public override int Next(int minValue, int maxValue)
        {
            return randomizer.Next(minValue, maxValue);
        }

        public override void NextBytes(Byte[] buffer)
        {
            randomizer.NextBytes(buffer);
        }

        public override double NextDouble()
        {
            return randomizer.NextDouble();
        }

        #endregion
    }
}