using System;
using System.Collections.Generic;
using System.Threading;
using FireworksNet.Model;

namespace FireworksNet.Problems
{
	// Counts something; stops when count exceeds some threshold
	public class CounterStopCondition : IStopCondition
	{
		protected readonly Int32 threshold;
		private Int32 count;

		public CounterStopCondition(Int32 threshold)
        {
			if (threshold < 0)
            {
				throw new ArgumentOutOfRangeException("threshold");
            }

			this.threshold = threshold;
			this.count = 0;
        }

        public virtual Boolean ShouldStop(IEnumerable<Firework> currentFireworks)
        {
			return count >= threshold;
        }

		public virtual void IncreaseCounter(Object sender, Object e)
		{
			Interlocked.Increment(ref count);
		}
	}
}