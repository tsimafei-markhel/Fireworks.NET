using System;
using FireworksNet.Distances;
using FireworksNet.Extensions;
using FireworksNet.Model;

namespace FireworksNet.StopConditions
{
    public class MinErrorStopCondition : IStopCondition
    {
        private readonly Solution knownSolution;
		private readonly IDistance distanceCalculator;
        private readonly double minError;

        public MinErrorStopCondition(Solution knownSolution, IDistance distanceCalculator, double minError)
		{
			if (knownSolution == null)
			{
				throw new ArgumentNullException("knownSolution");
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
			this.distanceCalculator = distanceCalculator;
			this.minError = minError;
		}

		public MinErrorStopCondition(Solution knownSolution, IDistance distanceCalculator)
			: this(knownSolution, distanceCalculator, double.Epsilon)
		{
        }

        public virtual bool ShouldStop(AlgorithmState state)
        {
			if (state == null)
			{
				throw new ArgumentNullException("state");
			}

            double error = distanceCalculator.Calculate(state.BestSolution, knownSolution);
            return error.IsLessOrEqual(minError);
        }
    }
}