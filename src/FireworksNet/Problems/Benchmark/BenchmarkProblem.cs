using System;
using System.Collections.Generic;
using System.Linq;
using FireworksNet.Model;

namespace FireworksNet.Problems.Benchmark
{
    public class BenchmarkProblem : Problem
    {
		public readonly Lazy<BenchmarkProblem> Sphere2010 = new Lazy<BenchmarkProblem>(InitializeSphere2010);

		// TODO: May need coordinates instead of value...
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

		private static BenchmarkProblem InitializeSphere2010()
		{
			Dimension[] dimensions = new Dimension[30];
			IDictionary<Dimension, Range> initialDimensionRanges = new Dictionary<Dimension, Range>(30);
			for (int i = 0; i < 30; i++ )
			{
				dimensions[i] = new Dimension(new Range(-100.0, 100.0));
				initialDimensionRanges.Add(dimensions[i], new Range(30.0, 50.0));
			}

			Func<IDictionary<Dimension, double>, double> func = new Func<IDictionary<Dimension, double>, double>(
				(c) => {
					return c.Values.Sum(v => Math.Pow(v, 2.0));
				}
			);

			// TODO: In 2010 paper, stop condition was not number of steps
			// TODO: Need 'max number of quality function evaluations' stop condition
			return new BenchmarkProblem(dimensions, initialDimensionRanges, func, 0.0, new StepCountStopCondition(10), ProblemTarget.Minimum);
		}
    }
}