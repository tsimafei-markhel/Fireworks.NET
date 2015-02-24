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
        /// <param name="initialSparksNumber">The number of sparks (i.e. initial fireworks)
        /// to be generated.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"> if <paramref name="initialSparksNumber"/>
        /// is less than zero.</exception>
        public InitialExplosion(int stepNumber, int initialSparksNumber)
            : base(stepNumber, new Dictionary<FireworkType, int>() { { FireworkType.Initial, initialSparksNumber } })
        {
            if (initialSparksNumber < 0)
            {
                throw new ArgumentOutOfRangeException("initialSparksNumber");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InitialExplosion"/> class.
        /// </summary>
        /// <param name="initialSparksNumber">The number of sparks (i.e. initial fireworks)
        /// to be generated.</param>
        public InitialExplosion(int initialSparksNumber)
            : this(0, initialSparksNumber)
        {
        }
    }
}