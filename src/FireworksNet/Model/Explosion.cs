using System;
using System.Collections.Generic;

namespace FireworksNet.Model
{
	public abstract class Explosion
	{
        /// <summary>
        /// Gets a unique identifier of this <see cref="Explosion"/>.
        /// </summary>
        public TId Id { get; private set; }

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

            this.Id = new TId();
			this.StepNumber = stepNumber;
            this.SparkCounts = sparkCounts;
		}
	}
}