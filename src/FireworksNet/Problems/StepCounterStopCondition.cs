using System;
using System.Collections.Generic;
using System.Linq;
using FireworksNet.Model;

namespace FireworksNet.Problems
{
    public class StepCounterStopCondition : CounterStopCondition
    {
        public StepCounterStopCondition(int maxStepCount)
			: base(maxStepCount)
        {
        }

        public override bool ShouldStop(IEnumerable<Firework> currentFireworks)
        {
            if (currentFireworks == null)
            {
                throw new ArgumentNullException("currentFireworks");
            }

            int maxCurrentStep = currentFireworks.Max(fw => fw.BirthStepNumber);
            return base.ShouldStop(currentFireworks) || (maxCurrentStep >= threshold);
        }
    }
}