using System;
using System.Collections.Generic;
using System.Linq;
using FireworksNet.Model;

namespace FireworksNet.Problems.Benchmark
{
	// TODO: Need to rework this to get rid of Lazy<T>. Each problem - separate class. Allow to pass IStopCondition to ctor.

	/// <summary>
	/// Stores various benchmark problems that can be used to test algorithm and its efficiency.
	/// </summary>
	/// <remarks>http://en.wikipedia.org/wiki/Test_functions_for_optimization</remarks>
	public static class BenchmarkProblems
	{
		/// <summary>
		/// Sphere test function, as used in 2010 paper
		/// </summary>
		public static Lazy<BenchmarkProblem> Sphere2010 = new Lazy<BenchmarkProblem>(InitializeSphere2010);

		/// <summary>
		/// Initializes the Sphere test function, as used in 2010 paper.
		/// </summary>
		/// <returns><see cref="BenchmarkProblem"/> instance that represents
		/// Sphere test function, as used in 2010 paper.</returns>
		private static BenchmarkProblem InitializeSphere2010()
		{
			Dimension[] dimensions = new Dimension[30];
			IDictionary<Dimension, Range> initialDimensionRanges = new Dictionary<Dimension, Range>(30);
			for (int i = 0; i < 30; i++)
			{
				dimensions[i] = new Dimension(new Range(-100.0, 100.0));
				initialDimensionRanges.Add(dimensions[i], new Range(30.0, 50.0));
			}

			Func<IDictionary<Dimension, double>, double> func = new Func<IDictionary<Dimension, double>, double>(
				(c) =>
				{
					return c.Values.Sum(v => Math.Pow(v, 2.0));
				}
			);

			// TODO: In 2010 paper, stop condition was not number of steps
			return new BenchmarkProblem(dimensions, initialDimensionRanges, func, new Solution(0.0), new StepCounterStopCondition(10), ProblemTarget.Minimum);
		}
	}
}