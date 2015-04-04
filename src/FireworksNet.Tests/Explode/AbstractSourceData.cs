using System;
using System.Collections.Generic;
using FireworksNet.Model;
using NSubstitute;
using FireworksNet.Algorithm.Implementation;
using FireworksNet.Distributions;

namespace FireworksNet.Tests.Explode
{
    public abstract class AbstractSourceData
    {
        public static double Amplitude;
        public static double Delta;

        public static IEnumerable<object[]> DataForTestMethodExplodeOfParallelExploder
        {
            get
            {
                var epicenter = Substitute.For<Firework>(FireworkType.SpecificSpark, 0);
                var qualities = Substitute.For<IEnumerable<double>>();

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
                var bestSolution = Substitute.For<Solution>(0);
                var dimensions = Substitute.For<IEnumerable<Dimension>>();                 

                var distribution = Substitute.For<ContinuousUniformDistribution>(Amplitude - Delta, Amplitude + Delta);
                var randomizer = Substitute.For<System.Random>();

                return new[]
                {
                    new object[]{null,         dimensions, distribution, randomizer, "bestSolution"},
                    new object[]{bestSolution, null,       distribution, randomizer, "dimensions"},
                    new object[]{bestSolution, dimensions, null,         randomizer, "distribution"},
                    new object[]{bestSolution, dimensions, distribution, null,       "randomizer"},
                };
            }
        }

        public AbstractSourceData()
        {
            Amplitude = 1;
            Delta = 0.1;
        } 
    }
}
