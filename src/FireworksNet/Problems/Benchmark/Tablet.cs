using System;
using System.Collections.Generic;
using FireworksNet.Model;

namespace FireworksNet.Problems.Benchmark
{
    /// <summary>
    /// Represents Tablet test function, as used in 2010 paper.
    /// </summary>
    public sealed class Tablet : BenchmarkProblem
    {
        private const int dimensionality = 30;
        private const double minDimensionValue = -100.0;
        private const double maxDimensionValue = 100.0;
        private const double minInitialDimensionValue = 15.0;
        private const double maxInitialDimensionValue = 30.0;
        private const double knownBestQuality = 0.0;
        private const ProblemTarget problemTarget = ProblemTarget.Minimum;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tablet"/> class.
        /// </summary>
        /// <param name="dimensions">Dimensions of the problem.</param>
        /// <param name="initialDimensionRanges">Initial dimension ranges, to be used to 
        /// create initial fireworks.</param>
        /// <param name="targetFunction">Quality function.</param>
        /// <param name="knownSolution">Known solution.</param>
        /// <param name="target">Problem target.</param>
        private Tablet(IList<Dimension> dimensions, IDictionary<Dimension, Range> initialDimensionRanges, Func<IDictionary<Dimension, double>, double> targetFunction, Solution knownSolution, ProblemTarget target)
            : base(dimensions, initialDimensionRanges, targetFunction, knownSolution, target)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Tablet"/> class.
        /// </summary>
        /// <returns><see cref="Tablet"/> instance that represents
        /// Tablet test function, as used in 2010 paper.</returns>
        public static Tablet Create()
        {
            Dimension[] dimensions = new Dimension[Tablet.dimensionality];
            IDictionary<Dimension, Range> initialDimensionRanges = new Dictionary<Dimension, Range>(Tablet.dimensionality);
            IDictionary<Dimension, double> knownBestCoordinates = new Dictionary<Dimension, double>(Tablet.dimensionality);
            for (int i = 0; i < Tablet.dimensionality; i++)
            {
                dimensions[i] = new Dimension(new Range(Tablet.minDimensionValue, Tablet.maxDimensionValue));
                initialDimensionRanges.Add(dimensions[i], new Range(Tablet.minInitialDimensionValue, Tablet.maxInitialDimensionValue));
                knownBestCoordinates.Add(dimensions[i], 0.0);
            }

            Func<IDictionary<Dimension, double>, double> func = new Func<IDictionary<Dimension, double>, double>(
                (c) =>
                {
                    double sum = Math.Pow(10.0, 4.0) * Math.Pow(c[dimensions[0]], 2.0);
                    for (int i = 1; i < Tablet.dimensionality; i++)
                    {
                        double value = c[dimensions[i]];
                        sum += Math.Pow(value, 2.0);
                    }

                    return sum;
                }
            );

            return new Tablet(dimensions, initialDimensionRanges, func, new Solution(knownBestCoordinates, Tablet.knownBestQuality), Tablet.problemTarget);
        }
    }
}