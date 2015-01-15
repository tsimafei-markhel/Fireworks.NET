using System;
using System.Collections.Generic;
using FireworksNet.Model;

namespace FireworksNet.Problems.Benchmark
{
    public class BenchmarkProblem : Problem
    {
        public Double KnownSolution { get; private set; }

        public BenchmarkProblem(IEnumerable<Dimension> dimensions, IDictionary<Dimension, Range> initialDimensionRanges, Func<IDictionary<Dimension, Double>, Double> targetFunction, Double knownSolution, IStopCondition stopCondition, ProblemTarget target)
            : base(dimensions, initialDimensionRanges, targetFunction, stopCondition, target)
        {
            if (double.IsNaN(knownSolution) || double.IsInfinity(knownSolution))
            {
                throw new ArgumentOutOfRangeException("knownSolution");
            }

            this.KnownSolution = knownSolution;
        }

        public BenchmarkProblem(IEnumerable<Dimension> dimensions, Func<IDictionary<Dimension, Double>, Double> targetFunction, Double knownSolution, IStopCondition stopCondition, ProblemTarget target)
            : this(dimensions, null, targetFunction, knownSolution, stopCondition, target)
        {
        }

        public BenchmarkProblem(IEnumerable<Dimension> dimensions, Func<IDictionary<Dimension, Double>, Double> targetFunction, Double knownSolution, IStopCondition stopCondition)
            : this(dimensions, null, targetFunction, knownSolution, stopCondition, ProblemTarget.Minimum)
        {
        }
    }
}