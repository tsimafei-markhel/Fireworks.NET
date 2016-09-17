using System;
using System.Collections.Generic;
using FireworksNet.Distributions;
using FireworksNet.Generation;
using FireworksNet.Model;
using NSubstitute;
using Xunit;

namespace FireworksNet.Tests.Generation
{
    public class AttractRepulseSparkGeneratorTests : AbstractSourceData
    {
        [Fact]
        public void CreateSpark_MustReturnNotNullFirework()
        {
            const int expectedBirthStepNumber = 1;
            const FireworkType expectedFireworkType = FireworkType.SpecificSpark;

            Solution bestSolution = Substitute.For<Solution>(0);
            IList<Dimension> dimensions = Substitute.For<IList<Dimension>>();
            System.Random randomizer = Substitute.For<System.Random>();
            ContinuousUniformDistribution distribution = Substitute.For<ContinuousUniformDistribution>(AbstractSourceData.Amplitude - AbstractSourceData.Delta, AbstractSourceData.Amplitude + AbstractSourceData.Delta);
            Firework epicenter = Substitute.For<Firework>(expectedFireworkType, expectedBirthStepNumber - 1);
            IEnumerable<double> qualities = Substitute.For<IEnumerable<double>>();
            Dictionary<FireworkType, int> sparks = Substitute.For<Dictionary<FireworkType, int>>();
            FireworkExplosion explosion = Substitute.For<FireworkExplosion>(epicenter, expectedBirthStepNumber, AbstractSourceData.Amplitude, sparks);

            AttractRepulseSparkGenerator sparkGenerator = new AttractRepulseSparkGenerator(bestSolution, dimensions, distribution, randomizer);

            Firework spark = sparkGenerator.CreateSpark(explosion);

            Assert.NotNull(spark);
            Assert.Equal(expectedFireworkType, spark.FireworkType);
            Assert.Equal(expectedBirthStepNumber, spark.BirthStepNumber);
        }

        [Theory]
        [MemberData("DataForTestCreationInstanceOfAttractRepulseGenerator")]
        public void CreateIntaceOfAttractRepulseGenerator_PassEachParameterAsNullAndOtherIsCorrect_ArgumentNullExceptionThrown(
            Solution bestSolution, IEnumerable<Dimension> dimensions, ContinuousUniformDistribution distribution, System.Random randomizer, string expectedParamName)
        {
            ArgumentNullException exeption = Assert.Throws<ArgumentNullException>(
                () => new AttractRepulseSparkGenerator(bestSolution, dimensions, distribution, randomizer));

            Assert.Equal(expectedParamName, exeption.ParamName);
        }
    }
}
