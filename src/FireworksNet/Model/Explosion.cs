using System;
using System.Collections.Generic;

namespace FireworksNet.Model
{
	public abstract class Explosion
	{
		// TODO: Add Id?..

        public int StepNumber { get; private set; }

        public IDictionary<FireworkType, int> SparkCounts { get; private set; }

        protected Explosion(int stepNumber, IDictionary<FireworkType, int> sparkCounts)
		{
            if (stepNumber < 0)
            {
                throw new ArgumentOutOfRangeException("stepNumber");
            }
            
			if (sparkCounts == null)
			{
				throw new ArgumentNullException("sparkCounts");
			}

			StepNumber = stepNumber;
			SparkCounts = sparkCounts;
		}
	}
}