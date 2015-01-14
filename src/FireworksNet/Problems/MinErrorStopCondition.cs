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
		private readonly Func<IEnumerable<Firework>, Firework> bestFireworkSelector;
		private readonly IDistance distanceCalculator;
		private readonly Double minError;

		public MinErrorStopCondition(Double[] knownBest, Func<IEnumerable<Firework>, Firework> bestFireworkSelector, IDistance distanceCalculator, Double minError)
		{
			if (knownBest == null)
			{
				throw new ArgumentNullException("knownBest");
			}

			if (bestFireworkSelector == null)
			{
				throw new ArgumentNullException("bestFireworkSelector");
			}

			if (distanceCalculator == null)
			{
				throw new ArgumentNullException("distanceCalculator");
			}

			if (Double.IsNaN(minError) || Double.IsInfinity(minError))
			{
				throw new ArgumentOutOfRangeException("knownBest");
			}

			this.knownBest = knownBest;
			this.bestFireworkSelector = bestFireworkSelector;
			this.distanceCalculator = distanceCalculator;
			this.minError = minError;
		}

		public MinErrorStopCondition(Double[] knownBest, Func<IEnumerable<Firework>, Firework> bestFireworkSelector, IDistance distanceCalculator)
			: this(knownBest, bestFireworkSelector, distanceCalculator, double.Epsilon)
		{
        }

        public Boolean ShouldStop(IEnumerable<Firework> currentFireworks)
        {
			if (currentFireworks == null)
			{
				throw new ArgumentNullException("currentFireworks");
			}

			Firework bestFirework = bestFireworkSelector(currentFireworks); // TODO: Looking for best existing firework on each call may be inefficient...
            Double error = distanceCalculator.Calculate(bestFirework, knownBest);

            return error.IsLessOrEqual(minError);
        }
    }
}