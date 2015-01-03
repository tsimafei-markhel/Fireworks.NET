using System;
using System.Collections.Generic;
using FireworksNet.Distances;
using FireworksNet.Extensions;
using FireworksNet.Model;

namespace FireworksNet.Problems
{
    public class MinErrorStopCondition : IStopCondition
    {
        private readonly Double[] knownBest;
        private readonly Double minError;
        private readonly IDistance distanceCalculator;

        public MinErrorStopCondition(Double[] knownBest, Double minError, IDistance distanceCalculator)
        {
            if (knownBest == null)
            {
                throw new ArgumentNullException("knownBest");
            }

            if (Double.IsNaN(minError) || Double.IsInfinity(minError))
            {
                throw new ArgumentOutOfRangeException("knownBest");
            }

            if (distanceCalculator == null)
            {
                throw new ArgumentNullException("distanceCalculator");
            }

            this.knownBest = knownBest;
            this.minError = minError;
            this.distanceCalculator = distanceCalculator;
        }

        public Boolean ShouldStop(IEnumerable<Firework> currentFireworks)
        {
            throw new NotImplementedException();

            // TODO: Looking for best existing firework on each call may be inefficient...

            Firework bestFirework = null; // TODO: call something to find best existing solution
            Double error = distanceCalculator.Calculate(bestFirework, knownBest);

            return error.IsLessOrEqual(minError);
        }
    }
}