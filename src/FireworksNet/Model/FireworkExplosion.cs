using System;
using System.Collections.Generic;

namespace FireworksNet.Model
{
	public class FireworkExplosion : Explosion
	{
		public Firework ParentFirework { get; private set; }

        public Double Amplitude { get; private set; }

		public FireworkExplosion(Firework parentFirework, Int32 stepNumber, Double amplitude, IDictionary<FireworkType, Int32> sparkCounts)
            : base(stepNumber, sparkCounts)
		{
			if (parentFirework == null)
			{
				throw new ArgumentNullException("parentFirework");
			}

			if (Double.IsNaN(amplitude) || Double.IsInfinity(amplitude))
			{
				throw new ArgumentOutOfRangeException("amplitude");
			}

			ParentFirework = parentFirework;
            Amplitude = amplitude;
		}
	}
}