using System;
using System.Collections.Generic;
using FireworksNet.Model;

namespace FireworksNet.Problems.Benchmark
{
    public class BenchmarkProblem : Problem
    {
		public Solution KnownSolution { get; private set; }

        public BenchmarkProblem(IEnumerable<Dimension> dimensions, IDictionary<Dimension, Range> initialDimensionRanges, Func<IDictionary<Dimension, double>, double> targetFunction, Solution knownSolution, IStopCondition stopCondition, ProblemTarget target)
            : base(dimensions, initialDimensionRanges, targetFunction, stopCondition, target)
        {
			if (knownSolution == null)
            {
                throw new ArgumentNullException("knownSolution");
            }

			if (knownSolution.Coordinates == null && double.IsNaN(knownSolution.Quality))
			{
				// We have neither coordinates of a known solution nor its quality.
				// Therefore, such known solution is useless.
				throw new ArgumentException(string.Empty, "knownSolution");
			}

            this.KnownSolution = knownSolution;
        }

        public BenchmarkProblem(IEnumerable<Dimension> dimensions, Func<IDictionary<Dimension, double>, double> targetFunction, Solution knownSolution, IStopCondition stopCondition, ProblemTarget target)
            : this(dimensions, null, targetFunction, knownSolution, stopCondition, target)
        {
        }

        public BenchmarkProblem(IEnumerable<Dimension> dimensions, Func<IDictionary<Dimension, double>, double> targetFunction, Solution knownSolution, IStopCondition stopCondition)
            : this(dimensions, null, targetFunction, knownSolution, stopCondition, ProblemTarget.Minimum)
        {
        }
    }
}