using System;
using System.Collections.Generic;

namespace FireworksNet.Model
{
	public class Explosion
	{
		public Firework ParentFirework { get; private set; }

        public Int32 StepNumber { get; private set; }

		public Double Amplitude { get; private set; }

		public IDictionary<FireworkType, Int32> SparkCounts { get; private set; }

		public Explosion(Firework parentFirework, Int32 stepNumber, Double amplitude, IDictionary<FireworkType, Int32> sparkCounts)
		{
			if (parentFirework == null)
			{
				throw new ArgumentNullException("parentFirework");
			}

            if (stepNumber < 0)
            {
                throw new ArgumentOutOfRangeException("stepNumber");
            }

			if (Double.IsNaN(amplitude) || Double.IsInfinity(amplitude))
			{
				throw new ArgumentOutOfRangeException("amplitude");
			}

			if (sparkCounts == null)
			{
				throw new ArgumentNullException("sparkCounts");
			}

			ParentFirework = parentFirework;
            StepNumber = stepNumber;
			Amplitude = amplitude;
			SparkCounts = sparkCounts;
		}
	}
}