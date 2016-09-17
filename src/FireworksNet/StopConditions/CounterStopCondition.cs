using System;
using System.Threading;
using FireworksNet.Model;

namespace FireworksNet.StopConditions
{
    /// <summary>
    /// Counts something; stops when count exceeds some threshold.
    /// </summary>
    public class CounterStopCondition : IStopCondition
    {
        private int count;

        /// <summary>
        /// Gets the threshold. Exceeding this threshold is a
        /// stop condition for <see cref="CounterStopCondition"/>
        /// user.
        /// </summary>
        public int Threshold { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CounterStopCondition"/> class.
        /// </summary>
        /// <param name="threshold">The threshold.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"> if <paramref name="threshold"/>
        /// is less than zero.</exception>
        public CounterStopCondition(int threshold)
        {
            if (threshold < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(threshold));
            }

            this.count = 0;
            this.Threshold = threshold;
        }

        /// <summary>
        /// Tells if an algorithm that is currently in <paramref name="state"/> state
        /// should stop (and don't make further steps) or not.
        /// </summary>
        /// <param name="state">The current algorithm state.</param>
        /// <returns>
        /// <c>true</c> if an algorithm that is currently in <paramref name="state"/>
        /// state should stop (and don't make further steps). Otherwise <c>false</c>.
        /// </returns>
        public virtual bool ShouldStop(AlgorithmState state)
        {
            return this.count >= this.Threshold;
        }

        /// <summary>
        /// Increments the internal counter.
        /// </summary>
        public virtual void IncrementCounter()
        {
            Interlocked.Increment(ref this.count);
        }

        /// <summary>
        /// Increments the internal counter. Can be used as an event handler.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="eventArgs">The event arguments.</param>
        public virtual void IncrementCounter(object sender, object eventArgs)
        {
            this.IncrementCounter();
        }
    }
}