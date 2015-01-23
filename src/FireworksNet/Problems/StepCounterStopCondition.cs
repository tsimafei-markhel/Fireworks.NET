using System;
using System.Collections.Generic;
using System.Linq;
using FireworksNet.Model;

namespace FireworksNet.Problems
{
    public class StepCounterStopCondition : CounterStopCondition
    {
        public StepCounterStopCondition(Int32 maxStepCount)
			: base(maxStepCount)
        {
        }

        public override Boolean ShouldStop(IEnumerable<Firework> currentFireworks)
        {
            if (currentFireworks == null)
            {
                throw new ArgumentNullException("currentFireworks");
            }

            Int32 maxCurrentStep = currentFireworks.Max(fw => fw.BirthStepNumber);
            return base.ShouldStop(currentFireworks) || (maxCurrentStep >= threshold);
        }
    }
}