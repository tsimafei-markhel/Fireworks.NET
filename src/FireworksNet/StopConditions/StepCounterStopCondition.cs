using System;
using FireworksNet.State;

namespace FireworksNet.StopConditions
{
    /// <summary>
    /// Counts algorithm steps; stops when count exceeds some threshold.
    /// </summary>
    public class StepCounterStopCondition : CounterStopCondition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StepCounterStopCondition"/> class.
        /// </summary>
        /// <param name="maxStepCount">The threshold - maximum step count.</param>
        public StepCounterStopCondition(int maxStepCount)
            : base(maxStepCount)
        {
        }

        /// <summary>
        /// Tells if an algorithm that is currently in <paramref name="state"/> state
        /// should stop (and don't make further steps) or not. Takes step count from
        /// the <paramref name="state"/>.
        /// </summary>
        /// <param name="state">The current algorithm state.</param>
        /// <returns>
        /// <c>true</c> if an algorithm that is currently in <paramref name="state"/>
        /// state should stop (and don't make further steps). Otherwise <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="state"/>
        /// is <c>null</c>.</exception>
        public override bool ShouldStop(IAlgorithmState state)
        {
            bool shouldStop = base.ShouldStop(state);
            if (!shouldStop)
            {
                if (state == null)
                {
                    throw new ArgumentNullException(nameof(state));
                }

                shouldStop = state.StepNumber >= this.Threshold;
            }

            return shouldStop;
        }
    }
}