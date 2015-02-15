using System;
using System.Collections.Generic;

namespace FireworksNet.Model
{
    public class InitialExplosion : ExplosionBase
    {
        public InitialExplosion(int stepNumber, int initialSparksCount)
            : base(stepNumber, new Dictionary<FireworkType, int>() { { FireworkType.Initial, initialSparksCount } })
        {
            if (initialSparksCount < 0)
            {
                throw new ArgumentOutOfRangeException("initialSparksCount");
            }
        }

        public InitialExplosion(int initialSparksCount)
            : this(0, initialSparksCount)
        {
        }
    }
}