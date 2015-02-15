using System;
using System.Collections.Generic;
using FireworksNet.Model;

namespace FireworksNet.Problems.Benchmark
{
    /// <summary>
    /// Represents Ellipse test function, as used in 2010 paper.
    /// </summary>
    public sealed class Ellipse : BenchmarkProblem
    {
        private const int dimensionality = 30;
        private const double minDimensionValue = -100.0;
        private const double maxDimensionValue = 100.0;
        private const double minInitialDimensionValue = 15.0;
        private const double maxInitialDimensionValue = 30.0;
        private const double knownBestQuality = 0.0;
        private const ProblemTarget problemTarget = ProblemTarget.Minimum;

        /// <summary>
        /// Initializes a new instance of the <see cref="Ellipse"/> class.
        /// </summary>
        /// <param name="dimensions">Dimensions of the problem.</param>
        /// <param name="initialDimensionRanges">Initial dimension ranges, to be used to 
        /// create initial fireworks.</param>
        /// <param name="targetFunction">Quality function.</param>
        /// <param name="knownSolution">Known solution.</param>
        /// <param name="stopCondition">Algorithm stop condition.</param>
        /// <param name="target">Problem target.</param>
        private Ellipse(IList<Dimension> dimensions, IDictionary<Dimension, Range> initialDimensionRanges, Func<IDictionary<Dimension, double>, double> targetFunction, Solution knownSolution, ProblemTarget target)
            : base(dimensions, initialDimensionRanges, targetFunction, knownSolution, target)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Ellipse"/> class.
        /// </summary>
        /// <returns><see cref="Ellipse"/> instance that represents
        /// Ellipse test function, as used in 2010 paper.</returns>
        public static Ellipse Create()
        {
            Dimension[] dimensions = new Dimension[Ellipse.dimensionality];
            IDictionary<Dimension, Range> initialDimensionRanges = new Dictionary<Dimension, Range>(Ellipse.dimensionality);
            IDictionary<Dimension, double> knownBestCoordinates = new Dictionary<Dimension, double>(Ellipse.dimensionality);
            for (int i = 0; i < Ellipse.dimensionality; i++)
            {
                dimensions[i] = new Dimension(new Range(Ellipse.minDimensionValue, Ellipse.maxDimensionValue));
                initialDimensionRanges.Add(dimensions[i], new Range(Ellipse.minInitialDimensionValue, Ellipse.maxInitialDimensionValue));
                knownBestCoordinates.Add(dimensions[i], 0.0);
            }

            Func<IDictionary<Dimension, double>, double> func = new Func<IDictionary<Dimension, double>, double>(
                (c) =>
                {
                    double sum = 0.0;
                    int denominator = Ellipse.dimensionality - 1;
                    for (int i = 0; i < Ellipse.dimensionality; i++)
                    {
                        double value = c[dimensions[i]];
                        sum += Math.Pow(10.0, 4.0 * ((i) / (denominator))) * Math.Pow(value, 2.0);
                    }

                    return sum;
                }
            );

            return new Ellipse(dimensions, initialDimensionRanges, func, new Solution(knownBestCoordinates, Ellipse.knownBestQuality), Ellipse.problemTarget);
        }
    }
}