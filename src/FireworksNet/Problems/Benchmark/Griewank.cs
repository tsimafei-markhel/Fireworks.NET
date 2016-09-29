using System;
using System.Collections.Generic;
using FireworksNet.Model;

namespace FireworksNet.Problems.Benchmark
{
    /// <summary>
    /// Represents Griewank test function, as used in 2010 paper.
    /// </summary>
    /// <remarks>http://mathworld.wolfram.com/GriewankFunction.html</remarks>
    public sealed class Griewank : BenchmarkProblem
    {
        private const int dimensionality = 30;
        private const double minDimensionValue = -100.0;
        private const double maxDimensionValue = 100.0;
        private const double minInitialDimensionValue = 30.0;
        private const double maxInitialDimensionValue = 50.0;
        private const double knownBestQuality = 0.0;
        private const ProblemTarget problemTarget = ProblemTarget.Minimum;

        /// <summary>
        /// Initializes a new instance of the <see cref="Griewank"/> class.
        /// </summary>
        /// <param name="dimensions">Dimensions of the problem.</param>
        /// <param name="initialDimensionRanges">Initial dimension ranges, to be used to 
        /// create initial fireworks.</param>
        /// <param name="targetFunction">Quality function.</param>
        /// <param name="knownSolution">Known solution.</param>
        /// <param name="target">Problem target.</param>
        private Griewank(IList<Dimension> dimensions, IDictionary<Dimension, Range> initialDimensionRanges, Func<IDictionary<Dimension, double>, double> targetFunction, Solution knownSolution, ProblemTarget target)
            : base(dimensions, initialDimensionRanges, targetFunction, knownSolution, target)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Griewank"/> class.
        /// </summary>
        /// <returns><see cref="Griewank"/> instance that represents
        /// Griewank test function, as used in 2010 paper.</returns>
        public static Griewank Create()
        {
            Dimension[] dimensions = new Dimension[Griewank.dimensionality];
            IDictionary<Dimension, Range> initialDimensionRanges = new Dictionary<Dimension, Range>(Griewank.dimensionality);
            IDictionary<Dimension, double> knownBestCoordinates = new Dictionary<Dimension, double>(Griewank.dimensionality);
            for (int i = 0; i < Griewank.dimensionality; i++)
            {
                dimensions[i] = new Dimension(new Range(Griewank.minDimensionValue, Griewank.maxDimensionValue));
                initialDimensionRanges.Add(dimensions[i], new Range(Griewank.minInitialDimensionValue, Griewank.maxInitialDimensionValue));
                knownBestCoordinates.Add(dimensions[i], 0.0);
            }

            Func<IDictionary<Dimension, double>, double> func = new Func<IDictionary<Dimension, double>, double>(
                (c) =>
                {
                    double sum = 0.0;
                    double product = 1.0;
                    for (int i = 0; i < Griewank.dimensionality; i++)
                    {
                        double value = c[dimensions[i]];
                        sum += Math.Pow(value, 2.0);
                        product *= Math.Cos(value) / Math.Sqrt(i + 1);
                    }

                    return 1.0 + sum / 4000.0 - product;
                }
            );

            return new Griewank(dimensions, initialDimensionRanges, func, new Solution(knownBestCoordinates, Griewank.knownBestQuality), Griewank.problemTarget);
        }
    }
}