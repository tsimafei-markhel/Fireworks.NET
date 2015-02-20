using System;
using System.Collections.Generic;

namespace FireworksNet.Model
{
    /// <summary>
    /// Represents an initial explosion - fake explosion that is used to
    /// generate initial fireworks.
    /// </summary>
    public class InitialExplosion : ExplosionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InitialExplosion"/> class.
        /// </summary>
        /// <param name="stepNumber">The number of step this explosion took place at.</param>
        /// <param name="initialSparksCount">The number of sparks (i.e. initial fireworks)
        /// to be generated.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"> if <paramref name="initialSparksCount"/>
        /// is less than zero.</exception>
        public InitialExplosion(int stepNumber, int initialSparksCount)
            : base(stepNumber, new Dictionary<FireworkType, int>() { { FireworkType.Initial, initialSparksCount } })
        {
            if (initialSparksCount < 0)
            {
                throw new ArgumentOutOfRangeException("initialSparksCount");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InitialExplosion"/> class.
        /// </summary>
        /// <param name="initialSparksCount">The number of sparks (i.e. initial fireworks)
        /// to be generated.</param>
        public InitialExplosion(int initialSparksCount)
            : this(0, initialSparksCount)
        {
        }
    }
}