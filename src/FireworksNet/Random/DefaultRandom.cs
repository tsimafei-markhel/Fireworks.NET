using System;
using MathNet.Numerics.Random;

namespace FireworksNet.Random
{
    /// <summary>
    /// Represents a pseudo-random number generator
    /// </summary>
    /// <remarks>Uses <see cref="SystemRandomSource"/> thread-safe wrapper
    /// around <see cref="Random"/> that comes with Math.NET Numerics</remarks>
    public class DefaultRandom : System.Random
    {
        private readonly SystemRandomSource randomizer;

        public DefaultRandom()
        {
            randomizer = new SystemRandomSource();
        }

        public DefaultRandom(Int32 seed)
            : this(seed, true)
        {
        }

        public DefaultRandom(Int32 seed, Boolean threadSafe)
        {
            randomizer = new SystemRandomSource(seed, threadSafe);
        }

        #region System.Random overrides

        public override Int32 Next()
        {
            return randomizer.Next();
        }

        public override Int32 Next(Int32 maxValue)
        {
            return randomizer.Next(maxValue);
        }

        public override Int32 Next(Int32 minValue, Int32 maxValue)
        {
            return randomizer.Next(minValue, maxValue);
        }

        public override void NextBytes(Byte[] buffer)
        {
            randomizer.NextBytes(buffer);
        }

        public override Double NextDouble()
        {
            return randomizer.NextDouble();
        }

        #endregion
    }
}