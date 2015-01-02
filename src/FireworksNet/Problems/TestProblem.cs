using System;
using System.Collections.Generic;
using FireworksNet.Model;

namespace FireworksNet.Problems
{
    public class TestProblem : Problem
    {
        public Double KnownSolution { get; private set; }

        public TestProblem(IEnumerable<Dimension> dimensions, IDictionary<Dimension, Range> initialDimensionRanges, Func<IDictionary<Dimension, Double>, Double> targetFunction, Double knownSolution)
            : base(dimensions, initialDimensionRanges, targetFunction)
        {
            if (double.IsNaN(knownSolution) || double.IsInfinity(knownSolution))
            {
                throw new ArgumentOutOfRangeException("knownSolution");
            }

            this.KnownSolution = knownSolution;
        }

        public TestProblem(IEnumerable<Dimension> dimensions, Func<IDictionary<Dimension, Double>, Double> targetFunction, Double knownSolution)
            : this(dimensions, null, targetFunction, knownSolution)
        {
        }

        // TODO: Need to calculate distance between knownSolution and given solution somehow. Maybe not here...
    }
}