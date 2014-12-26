using System;
using System.Collections.Generic;
using Fireworks.Model;

namespace Fireworks.Problems
{
    public class TestProblem : Problem
    {
        public Double KnownSolution { get; private set; }

        public TestProblem(IEnumerable<Dimension> dimensions, Func<IDictionary<Dimension, Double>, Double> targetFunction, Double knownSolution)
            : base(dimensions, targetFunction)
        {
            if (double.IsNaN(knownSolution) || double.IsInfinity(knownSolution))
            {
                throw new ArgumentOutOfRangeException("knownSolution");
            }

            this.KnownSolution = knownSolution;
        }

        // TODO: Need to calculate distance between knownSolution and given solution somehow. Maybe not here...
    }
}