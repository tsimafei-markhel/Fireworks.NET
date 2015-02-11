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
        private Sphere2010(IList<Dimension> dimensions, IDictionary<Dimension, Range> initialDimensionRanges, Func<IDictionary<Dimension, double>, double> targetFunction, Solution knownSolution, ProblemTarget target)
            : base(dimensions, initialDimensionRanges, targetFunction, knownSolution, target)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Sphere2010"/> class.
        /// </summary>
        /// <returns><see cref="Sphere2010"/> instance that represents
        /// Sphere test function, as used in 2010 paper.</returns>
        public static Sphere2010 Create()
        {
            Dimension[] dimensions = new Dimension[Sphere2010.dimensionality];
            IDictionary<Dimension, Range> initialDimensionRanges = new Dictionary<Dimension, Range>(Sphere2010.dimensionality);
            IDictionary<Dimension, double> knownBestCoordinates = new Dictionary<Dimension, double>(Sphere2010.dimensionality);
            for (int i = 0; i < Sphere2010.dimensionality; i++)
            {
                dimensions[i] = new Dimension(new Range(Sphere2010.minDimensionValue, Sphere2010.maxDimensionValue));
                initialDimensionRanges.Add(dimensions[i], new Range(Sphere2010.minInitialDimensionValue, Sphere2010.maxInitialDimensionValue));
                knownBestCoordinates.Add(dimensions[i], 0.0);
            }

            Func<IDictionary<Dimension, double>, double> func = new Func<IDictionary<Dimension, double>, double>(
                (c) =>
                {
                    return c.Values.Sum(v => Math.Pow(v, 2.0));
                }
            );

            return new Sphere2010(dimensions, initialDimensionRanges, func, new Solution(knownBestCoordinates, Sphere2010.knownBestQuality), Sphere2010.problemTarget);
        }
    }
}