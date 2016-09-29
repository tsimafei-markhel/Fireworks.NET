using System;
using System.Collections.Generic;
using FireworksNet.Model;

namespace FireworksNet.Problems.Benchmark
{
    /// <summary>
    /// Represents Rosenbrock test function, as used in 2010 paper.
    /// </summary>
    /// <remarks>http://en.wikipedia.org/wiki/Test_functions_for_optimization</remarks>
    public sealed class Rosenbrock : BenchmarkProblem
    {
        private const int dimensionality = 30;
        private const double minDimensionValue = -100.0;
        private const double maxDimensionValue = 100.0;
        private const double minInitialDimensionValue = 30.0;
        private const double maxInitialDimensionValue = 50.0;
        private const double knownBestQuality = 0.0;
        private const ProblemTarget problemTarget = ProblemTarget.Minimum;

        /// <summary>
        /// Initializes a new instance of the <see cref="Rosenbrock"/> class.
        /// </summary>
        /// <param name="dimensions">Dimensions of the problem.</param>
        /// <param name="initialDimensionRanges">Initial dimension ranges, to be used to 
        /// create initial fireworks.</param>
        /// <param name="targetFunction">Quality function.</param>
        /// <param name="knownSolution">Known solution.</param>
        /// <param name="target">Problem target.</param>
        private Rosenbrock(IList<Dimension> dimensions, IDictionary<Dimension, Range> initialDimensionRanges, Func<IDictionary<Dimension, double>, double> targetFunction, Solution knownSolution, ProblemTarget target)
            : base(dimensions, initialDimensionRanges, targetFunction, knownSolution, target)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Rosenbrock"/> class.
        /// </summary>
        /// <returns><see cref="Rosenbrock"/> instance that represents
        /// Rosenbrock test function, as used in 2010 paper.</returns>
        public static Rosenbrock Create()
        {
            Dimension[] dimensions = new Dimension[Rosenbrock.dimensionality];
            IDictionary<Dimension, Range> initialDimensionRanges = new Dictionary<Dimension, Range>(Rosenbrock.dimensionality);
            IDictionary<Dimension, double> knownBestCoordinates = new Dictionary<Dimension, double>(Rosenbrock.dimensionality);
            for (int i = 0; i < Rosenbrock.dimensionality; i++)
            {
                dimensions[i] = new Dimension(new Range(Rosenbrock.minDimensionValue, Rosenbrock.maxDimensionValue));
                initialDimensionRanges.Add(dimensions[i], new Range(Rosenbrock.minInitialDimensionValue, Rosenbrock.maxInitialDimensionValue));
                knownBestCoordinates.Add(dimensions[i], 0.0);
            }

            Func<IDictionary<Dimension, double>, double> func = new Func<IDictionary<Dimension, double>, double>(
                (c) =>
                {
                    double value = 0.0;
                    for (int i = 0; i < Rosenbrock.dimensionality - 1; i++)
                    {
                        double dimensionCoordinate = c[dimensions[i]];
                        double nextDimensionCoordinate = c[dimensions[i + 1]];
                        value += 100 * Math.Pow(nextDimensionCoordinate - Math.Pow(dimensionCoordinate, 2.0), 2.0) + Math.Pow(dimensionCoordinate - 1.0, 2.0);
                    }

                    return value;
                }
            );

            return new Rosenbrock(dimensions, initialDimensionRanges, func, new Solution(knownBestCoordinates, Rosenbrock.knownBestQuality), Rosenbrock.problemTarget);
        }
    }
}