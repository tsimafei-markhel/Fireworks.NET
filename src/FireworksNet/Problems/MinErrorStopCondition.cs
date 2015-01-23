using System;
using System.Collections.Generic;
using FireworksNet.Distances;
using FireworksNet.Extensions;
using FireworksNet.Model;

namespace FireworksNet.Problems
{
    public class MinErrorStopCondition : IStopCondition
    {
        private readonly Solution knownSolution;
		private readonly Func<IEnumerable<Firework>, Firework> bestFireworkSelector;
		private readonly IDistance distanceCalculator;
        private readonly double minError;

        public MinErrorStopCondition(Solution knownSolution, Func<IEnumerable<Firework>, Firework> bestFireworkSelector, IDistance distanceCalculator, double minError)
		{
			if (knownSolution == null)
			{
				throw new ArgumentNullException("knownSolution");
			}

			if (bestFireworkSelector == null)
			{
				throw new ArgumentNullException("bestFireworkSelector");
			}

			if (distanceCalculator == null)
			{
				throw new ArgumentNullException("distanceCalculator");
			}

            if (double.IsNaN(minError) || double.IsInfinity(minError))
			{
				throw new ArgumentOutOfRangeException("knownBest");
			}

			this.knownSolution = knownSolution;
			this.bestFireworkSelector = bestFireworkSelector;
			this.distanceCalculator = distanceCalculator;
			this.minError = minError;
		}

		public MinErrorStopCondition(Solution knownSolution, Func<IEnumerable<Firework>, Firework> bestFireworkSelector, IDistance distanceCalculator)
			: this(knownSolution, bestFireworkSelector, distanceCalculator, double.Epsilon)
		{
        }

        public virtual bool ShouldStop(IEnumerable<Firework> currentFireworks)
        {
			if (currentFireworks == null)
			{
				throw new ArgumentNullException("currentFireworks");
			}

			Firework bestFirework = bestFireworkSelector(currentFireworks);
            double error = distanceCalculator.Calculate(bestFirework, knownSolution);

            return error.IsLessOrEqual(minError);
        }
    }
}