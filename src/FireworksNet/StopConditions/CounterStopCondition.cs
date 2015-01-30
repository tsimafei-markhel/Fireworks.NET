using System;
using System.Threading;
using FireworksNet.Model;

namespace FireworksNet.StopConditions
{
	// Counts something; stops when count exceeds some threshold
	public class CounterStopCondition : IStopCondition
	{
        protected readonly int threshold;
        private int count;

        public CounterStopCondition(int threshold)
        {
			if (threshold < 0)
            {
				throw new ArgumentOutOfRangeException("threshold");
            }

			this.threshold = threshold;
			this.count = 0;
        }

        public virtual bool ShouldStop(AlgorithmState state)
        {
			return count >= threshold;
        }

        public virtual void IncreaseCounter(object sender, object e)
		{
			Interlocked.Increment(ref count);
		}
	}
}