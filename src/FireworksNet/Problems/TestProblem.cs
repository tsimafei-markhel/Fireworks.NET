using System;
using System.Collections.Generic;
using FireworksNet.Model;

namespace FireworksNet.Problems
{
    public class TestProblem : Problem
    {
        public Double KnownSolution { get; private set; }

        public TestProblem(IEnumerable<Dimension> dimensions, IDictionary<Dimension, Range> initialDimensionRanges, Func<IDictionary<Dimension, Double>, Double> targetFunction, Double knownSolution, IStopCondition stopCondition)
            : base(dimensions, initialDimensionRanges, targetFunction, stopCondition)
        {
            if (double.IsNaN(knownSolution) || double.IsInfinity(knownSolution))
            {
                throw new ArgumentOutOfRangeException("knownSolution");
            }

            this.KnownSolution = knownSolution;
        }

        public TestProblem(IEnumerable<Dimension> dimensions, Func<IDictionary<Dimension, Double>, Double> targetFunction, Double knownSolution, IStopCondition stopCondition)
            : this(dimensions, null, targetFunction, knownSolution, stopCondition)
        {
        }
    }
}