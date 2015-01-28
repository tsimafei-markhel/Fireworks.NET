using System;
using System.Collections.Generic;
using System.Linq;
using FireworksNet.Model;

namespace FireworksNet.Problems.Benchmark
{
	/// <summary>
	/// Represents Sphere test function, as used in 2010 paper.
	/// </summary>
	/// <remarks>http://en.wikipedia.org/wiki/Test_functions_for_optimization</remarks>
	public sealed class Sphere2010 : BenchmarkProblem
	{
		private const int dimensionality = 30;
		private const double minDimensionValue = -100.0;
		private const double maxDimensionValue = 100.0;
		private const double minInitialDimensionValue = 30.0;
		private const double maxInitialDimensionValue = 50.0;
		private const double knownBestQuality = 0.0;
		private const ProblemTarget problemTarget = ProblemTarget.Minimum;

		/// <summary>
		/// Initializes a new instance of the <see cref="Sphere2010"/> class.
		/// </summary>
		/// <param name="dimensions">Dimensions of the problem.</param>
		/// <param name="initialDimensionRanges">Initial dimension ranges, to be used to 
		/// create initial fireworks.</param>
		/// <param name="targetFunction">Quality function.</param>
		/// <param name="knownSolution">Known solution.</param>
		/// <param name="stopCondition">Algorithm stop condition.</param>
		/// <param name="target">Problem target.</param>
		private Sphere2010(IEnumerable<Dimension> dimensions, IDictionary<Dimension, Range> initialDimensionRanges, Func<IDictionary<Dimension, double>, double> targetFunction, Solution knownSolution, IStopCondition stopCondition, ProblemTarget target)
			: base(dimensions, initialDimensionRanges, targetFunction, knownSolution, stopCondition, target)
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="Sphere2010"/> class.
		/// </summary>
		/// <returns><see cref="Sphere2010"/> instance that represents
		/// Sphere test function, as used in 2010 paper.</returns>
		public static Sphere2010 Create()
		{
			// TODO: In 2010 paper, stop condition was not number of steps
			return Create(new StepCounterStopCondition(10));
		}

		/// <summary>
		/// Initializes a new instance of <see cref="Sphere2010"/> class with
		/// specified stop condition.
		/// </summary>
		/// <param name="stopCondition">Algorithm stop condition.</param>
		/// <returns><see cref="Sphere2010"/> instance that represents
		/// Sphere test function, as used in 2010 paper, with specified
		/// stop condition.</returns>
		/// <exception cref="System.ArgumentNullException"> if <paramref name="stopCondition"/>
		/// is null.</exception>
		public static Sphere2010 Create(IStopCondition stopCondition)
		{
			if (stopCondition == null)
			{
				throw new ArgumentNullException("stopCondition");
			}

			Dimension[] dimensions = new Dimension[dimensionality];
			IDictionary<Dimension, Range> initialDimensionRanges = new Dictionary<Dimension, Range>(dimensionality);
			for (int i = 0; i < dimensionality; i++)
			{
				dimensions[i] = new Dimension(new Range(minDimensionValue, maxDimensionValue));
				initialDimensionRanges.Add(dimensions[i], new Range(minInitialDimensionValue, maxInitialDimensionValue));
			}

			Func<IDictionary<Dimension, double>, double> func = new Func<IDictionary<Dimension, double>, double>(
				(c) =>
				{
					return c.Values.Sum(v => Math.Pow(v, 2.0));
				}
			);

			return new Sphere2010(dimensions, initialDimensionRanges, func, new Solution(knownBestQuality), stopCondition, problemTarget);
		}
	}
}