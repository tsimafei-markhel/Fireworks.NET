using System;
using System.Collections.Generic;
using System.Linq;
using FireworksNet.Model;

namespace FireworksNet.Problems
{
    public class StepCountStopCondition : IStopCondition
    {
        private readonly int maxStepCount;

        public StepCountStopCondition(Int32 maxStepCount)
        {
            if (maxStepCount < 0)
            {
                throw new ArgumentOutOfRangeException("maxStepCount");
            }

            this.maxStepCount = maxStepCount;
        }

        public Boolean ShouldStop(IEnumerable<Firework> currentFireworks)
        {
            if (currentFireworks == null)
            {
                throw new ArgumentNullException("currentFireworks");
            }

            int maxCurrentStep = currentFireworks.Max(fw => fw.BirthStepNumber); // TODO: This seems to be quite inefficient...
            return maxCurrentStep >= maxStepCount;
        }
    }
}