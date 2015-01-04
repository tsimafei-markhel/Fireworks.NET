using System;
using System.Collections.Generic;

namespace FireworksNet.Model
{
	public abstract class Explosion
	{
        public Int32 StepNumber { get; protected set; }

        public IDictionary<FireworkType, Int32> SparkCounts { get; protected set; }

		protected Explosion(Int32 stepNumber, IDictionary<FireworkType, Int32> sparkCounts)
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