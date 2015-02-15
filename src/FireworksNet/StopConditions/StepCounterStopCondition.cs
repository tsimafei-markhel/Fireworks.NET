using System;
using FireworksNet.Model;

namespace FireworksNet.StopConditions
{
    public class StepCounterStopCondition : CounterStopCondition
    {
        public StepCounterStopCondition(int maxStepCount)
            : base(maxStepCount)
        {
        }

        public override bool ShouldStop(AlgorithmState state)
        {
            bool shouldStop = base.ShouldStop(state);
            if (!shouldStop)
            {
                if (state == null)
                {
                    throw new ArgumentNullException("state");
                }

                shouldStop = state.StepNumber >= this.Threshold;
            }

            return shouldStop;
        }
    }
}