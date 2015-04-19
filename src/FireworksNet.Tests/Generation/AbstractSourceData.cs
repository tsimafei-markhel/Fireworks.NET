using System;
using System.Collections.Generic;
using FireworksNet.Distributions;
using FireworksNet.Model;
using NSubstitute;

namespace FireworksNet.Tests.Generation
{
    public abstract class AbstractSourceData
    {
        public const double Amplitude = 1.0D;
        public const double Delta = 0.1D;

        public static IEnumerable<object[]> DataForTestMethodExplodeOfParallelExploder
        {
            get
            {
                Firework epicenter = Substitute.For<Firework>(FireworkType.SpecificSpark, 0);
                IEnumerable<double> qualities = Substitute.For<IEnumerable<double>>();

                return new[]
                {
                    new object[] { null,      qualities,  0, typeof(ArgumentNullException),       "epicenter"},
                    new object[] { epicenter, null,       0, typeof(ArgumentNullException),       "currentFireworkQualities" },
                    new object[] { epicenter, qualities, -1, typeof(ArgumentOutOfRangeException), "currentStepNumber" }
                };
            }
        }

        public static IEnumerable<object[]> DataForTestCreationInstanceOfAttractRepulseGenerator
        {
            get
            {
                Solution bestSolution = Substitute.For<Solution>(0);
                IEnumerable<Dimension> dimensions = Substitute.For<IEnumerable<Dimension>>();
                ContinuousUniformDistribution distribution = Substitute.For<ContinuousUniformDistribution>(AbstractSourceData.Amplitude - AbstractSourceData.Delta, AbstractSourceData.Amplitude + AbstractSourceData.Delta);
                System.Random randomizer = Substitute.For<System.Random>();

                return new[]
                {
                    new object[] {null,         dimensions, distribution, randomizer, "bestSolution"},
                    new object[] {bestSolution, null,       distribution, randomizer, "dimensions"},
                    new object[] {bestSolution, dimensions, null,         randomizer, "distribution"},
                    new object[] {bestSolution, dimensions, distribution, null,       "randomizer"}
                };
            }
        }
    }
}