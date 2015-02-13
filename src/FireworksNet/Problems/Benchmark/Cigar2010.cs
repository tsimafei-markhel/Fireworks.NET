using System;
using System.Collections.Generic;
using FireworksNet.Model;

namespace FireworksNet.Problems.Benchmark
{
    /// <summary>
    /// Represents Cigar test function, as used in 2010 paper.
    /// </summary>
    public sealed class Cigar2010 : BenchmarkProblem
    {
        private const int dimensionality = 30;
        private const double minDimensionValue = -100.0;
        private const double maxDimensionValue = 100.0;
        private const double minInitialDimensionValue = 15.0;
        private const double maxInitialDimensionValue = 30.0;
        private const double knownBestQuality = 0.0;
        private const ProblemTarget problemTarget = ProblemTarget.Minimum;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cigar2010"/> class.
        /// </summary>
        /// <param name="dimensions">Dimensions of the problem.</param>
        /// <param name="initialDimensionRanges">Initial dimension ranges, to be used to 
        /// create initial fireworks.</param>
        /// <param name="targetFunction">Quality function.</param>
        /// <param name="knownSolution">Known solution.</param>
        /// <param name="stopCondition">Algorithm stop condition.</param>
        /// <param name="target">Problem target.</param>
        private Cigar2010(IList<Dimension> dimensions, IDictionary<Dimension, Range> initialDimensionRanges, Func<IDictionary<Dimension, double>, double> targetFunction, Solution knownSolution, ProblemTarget target)
            : base(dimensions, initialDimensionRanges, targetFunction, knownSolution, target)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Cigar2010"/> class.
        /// </summary>
        /// <returns><see cref="Cigar2010"/> instance that represents
        /// Cigar test function, as used in 2010 paper.</returns>
        public static Cigar2010 Create()
        {
            Dimension[] dimensions = new Dimension[Cigar2010.dimensionality];
            IDictionary<Dimension, Range> initialDimensionRanges = new Dictionary<Dimension, Range>(Cigar2010.dimensionality);
            IDictionary<Dimension, double> knownBestCoordinates = new Dictionary<Dimension, double>(Cigar2010.dimensionality);
            for (int i = 0; i < Cigar2010.dimensionality; i++)
            {
                dimensions[i] = new Dimension(new Range(Cigar2010.minDimensionValue, Cigar2010.maxDimensionValue));
                initialDimensionRanges.Add(dimensions[i], new Range(Cigar2010.minInitialDimensionValue, Cigar2010.maxInitialDimensionValue));
                knownBestCoordinates.Add(dimensions[i], 0.0);
            }

            Func<IDictionary<Dimension, double>, double> func = new Func<IDictionary<Dimension, double>, double>(
                (c) =>
                {
                    double sum = Math.Pow(c[dimensions[0]], 2.0);
                    for (int i = 1; i < Cigar2010.dimensionality; i++)
                    {
                        double value = c[dimensions[i]];
                        sum += Math.Pow(10.0, 4.0) * Math.Pow(value, 2.0);
                    }

                    return sum;
                }
            );

            return new Cigar2010(dimensions, initialDimensionRanges, func, new Solution(knownBestCoordinates, Cigar2010.knownBestQuality), Cigar2010.problemTarget);
        }
    }
}