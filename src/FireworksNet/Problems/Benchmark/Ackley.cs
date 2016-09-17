using System;
using System.Collections.Generic;
using FireworksNet.Model;

namespace FireworksNet.Problems.Benchmark
{
    /// <summary>
    /// Represents Ackley test function, as used in 2010 paper.
    /// </summary>
    /// <remarks>http://en.wikipedia.org/wiki/Test_functions_for_optimization</remarks>
    public sealed class Ackley : BenchmarkProblem
    {
        private const int dimensionality = 30;
        private const double minDimensionValue = -100.0;
        private const double maxDimensionValue = 100.0;
        private const double minInitialDimensionValue = 30.0;
        private const double maxInitialDimensionValue = 50.0;
        private const double knownBestQuality = 0.0;
        private const ProblemTarget problemTarget = ProblemTarget.Minimum;

        /// <summary>
        /// Initializes a new instance of the <see cref="Ackley"/> class.
        /// </summary>
        /// <param name="dimensions">Dimensions of the problem.</param>
        /// <param name="initialDimensionRanges">Initial dimension ranges, to be used to 
        /// create initial fireworks.</param>
        /// <param name="targetFunction">Quality function.</param>
        /// <param name="knownSolution">Known solution.</param>
        /// <param name="target">Problem target.</param>
        private Ackley(IList<Dimension> dimensions, IDictionary<Dimension, Range> initialDimensionRanges, Func<IDictionary<Dimension, double>, double> targetFunction, Solution knownSolution, ProblemTarget target)
            : base(dimensions, initialDimensionRanges, targetFunction, knownSolution, target)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Ackley"/> class.
        /// </summary>
        /// <returns><see cref="Ackley"/> instance that represents
        /// Ackley test function, as used in 2010 paper.</returns>
        public static Ackley Create()
        {
            Dimension[] dimensions = new Dimension[Ackley.dimensionality];
            IDictionary<Dimension, Range> initialDimensionRanges = new Dictionary<Dimension, Range>(Ackley.dimensionality);
            IDictionary<Dimension, double> knownBestCoordinates = new Dictionary<Dimension, double>(Ackley.dimensionality);
            for (int i = 0; i < Ackley.dimensionality; i++)
            {
                dimensions[i] = new Dimension(new Range(Ackley.minDimensionValue, Ackley.maxDimensionValue));
                initialDimensionRanges.Add(dimensions[i], new Range(Ackley.minInitialDimensionValue, Ackley.maxInitialDimensionValue));
                knownBestCoordinates.Add(dimensions[i], 0.0);
            }

            Func<IDictionary<Dimension, double>, double> func = new Func<IDictionary<Dimension, double>, double>(
                (c) =>
                {
                    double firstSum = 0.0;
                    double secondSum = 0.0;
                    foreach(double value in c.Values)
                    {
                        firstSum += Math.Pow(value, 2.0);
                        secondSum += Math.Cos(2.0 * Math.PI * Math.Pow(value, 2.0));
                    }

                    return 20.0 + Math.E - 20.0 * Math.Exp(-0.2 * Math.Sqrt((1 / Ackley.dimensionality) * firstSum)) - Math.Exp((1 / Ackley.dimensionality) * secondSum);
                }
            );

            return new Ackley(dimensions, initialDimensionRanges, func, new Solution(knownBestCoordinates, Ackley.knownBestQuality), Ackley.problemTarget);
        }
    }
}