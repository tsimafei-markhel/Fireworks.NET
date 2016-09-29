using System;
using System.Collections.Generic;
using System.Linq;
using FireworksNet.Model;

namespace FireworksNet.Problems.Benchmark
{
    /// <summary>
    /// Represents Rastrigrin test function, as used in 2010 paper.
    /// </summary>
    /// <remarks>http://en.wikipedia.org/wiki/Rastrigin_function</remarks>
    public sealed class Rastrigrin : BenchmarkProblem
    {
        private const int dimensionality = 30;
        private const double minDimensionValue = -100.0;
        private const double maxDimensionValue = 100.0;
        private const double minInitialDimensionValue = 30.0;
        private const double maxInitialDimensionValue = 50.0;
        private const double knownBestQuality = 0.0;
        private const ProblemTarget problemTarget = ProblemTarget.Minimum;

        /// <summary>
        /// Initializes a new instance of the <see cref="Rastrigrin"/> class.
        /// </summary>
        /// <param name="dimensions">Dimensions of the problem.</param>
        /// <param name="initialDimensionRanges">Initial dimension ranges, to be used to 
        /// create initial fireworks.</param>
        /// <param name="targetFunction">Quality function.</param>
        /// <param name="knownSolution">Known solution.</param>
        /// <param name="target">Problem target.</param>
        private Rastrigrin(IList<Dimension> dimensions, IDictionary<Dimension, Range> initialDimensionRanges, Func<IDictionary<Dimension, double>, double> targetFunction, Solution knownSolution, ProblemTarget target)
            : base(dimensions, initialDimensionRanges, targetFunction, knownSolution, target)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Rastrigrin"/> class.
        /// </summary>
        /// <returns><see cref="Rastrigrin"/> instance that represents
        /// Rastrigrin test function, as used in 2010 paper.</returns>
        public static Rastrigrin Create()
        {
            Dimension[] dimensions = new Dimension[Rastrigrin.dimensionality];
            IDictionary<Dimension, Range> initialDimensionRanges = new Dictionary<Dimension, Range>(Rastrigrin.dimensionality);
            IDictionary<Dimension, double> knownBestCoordinates = new Dictionary<Dimension, double>(Rastrigrin.dimensionality);
            for (int i = 0; i < Rastrigrin.dimensionality; i++)
            {
                dimensions[i] = new Dimension(new Range(Rastrigrin.minDimensionValue, Rastrigrin.maxDimensionValue));
                initialDimensionRanges.Add(dimensions[i], new Range(Rastrigrin.minInitialDimensionValue, Rastrigrin.maxInitialDimensionValue));
                knownBestCoordinates.Add(dimensions[i], 0.0);
            }

            Func<IDictionary<Dimension, double>, double> func = new Func<IDictionary<Dimension, double>, double>(
                (c) =>
                {
                    return c.Values.Sum(v => Math.Pow(v, 2.0) - 10.0 * Math.Cos(2.0 * Math.PI * v) + 10.0);
                }
            );

            return new Rastrigrin(dimensions, initialDimensionRanges, func, new Solution(knownBestCoordinates, Rastrigrin.knownBestQuality), Rastrigrin.problemTarget);
        }
    }
}