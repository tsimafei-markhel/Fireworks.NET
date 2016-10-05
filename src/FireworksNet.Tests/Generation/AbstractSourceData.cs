using System;
using System.Collections.Generic;
using FireworksNet.Distributions;
using FireworksNet.Generation;
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

        public static IEnumerable<object[]> DataForTestMethodMutateFireworkOfAttractRepulseSparkMutator
        {
            get
            {
                IDictionary<Dimension, double> coordinates = Substitute.For<IDictionary<Dimension, double>>();
                MutableFirework mutableFirework = Substitute.For<MutableFirework>(FireworkType.SpecificSpark, 0, 0, coordinates);

                MutableFirework epicenter = mutableFirework;
                Dictionary<FireworkType, int> sparks = Substitute.For<Dictionary<FireworkType, int>>();
                FireworkExplosion explosion = Substitute.For<FireworkExplosion>(epicenter, 1, Amplitude, sparks);

                return new[]
                {
                    new object[] { mutableFirework, null,      "explosion"},
                    new object[] { null,            explosion, "firework"}
                };
            }
        }

        public static ISparkGenerator<FireworkExplosion> CreateAttractRepulseSparkGenerator()
        {
            Solution bestSolution = Substitute.For<Solution>(0);
            IList<Dimension> dimensions = Substitute.For<IList<Dimension>>();
            ContinuousUniformDistribution distribution = Substitute.For<ContinuousUniformDistribution>(Amplitude - Delta, Amplitude + Delta);
            System.Random randomizer = Substitute.For<System.Random>();
            AttractRepulseSparkGenerator generator = Substitute.For<AttractRepulseSparkGenerator>(bestSolution, dimensions, distribution, randomizer);

            return generator;
        }

        public static FireworkExplosion CreateFireworkExplosion(Firework epicenter)
        {
            Dictionary<FireworkType, int> sparks = Substitute.For<Dictionary<FireworkType, int>>();
            FireworkExplosion explosion = Substitute.For<FireworkExplosion>(epicenter, 1, Amplitude, sparks);

            return explosion;
        }
    }
}