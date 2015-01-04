using System;
using System.Collections.Generic;

namespace FireworksNet.Model
{
	public class InitialExplosion : Explosion
	{
        public InitialExplosion(Int32 stepNumber, Int32 initialSparksCount)
            : base(stepNumber, new Dictionary<FireworkType, Int32>() { { FireworkType.Initial, initialSparksCount } })
		{
            if (initialSparksCount < 0)
            {
                throw new ArgumentOutOfRangeException("initialSparksCount");
            }
		}

        public InitialExplosion(Int32 initialSparksCount)
            : this(0, initialSparksCount)
        {
        }
	}
}