using System;
using System.Collections;
using System.Collections.Generic;
using FireworksNet.Explode;
using FireworksNet.Problems;
using FireworksNet.Model;
using FireworksNet.Distributions;
using FireworksNet.Problems.Benchmark;
using FireworksNet.Tests.Extensions;
using FireworksNet.Algorithm.Implementation;
using FireworksNet.Random;
using NSubstitute;
using Xunit;

namespace FireworksNet.Tests.Explode
{
    public class AttractRepulseSparkGeneratorTests : TestDataSource
    {
        [Fact]
        public void CreateSpark_MustReturnNotNullFirework()
        {
            //Arrange
            const int expectedBirthStepNumber = 1;
            const FireworkType expectedFireworkType = FireworkType.SpecificSpark;    
        
            var bestSolution = Substitute.For<Solution>(0);
            var dimensions = Substitute.For<IList<Dimension>>();                
            var randomizer = Substitute.For<System.Random>();
            var distribution = Substitute.For<ContinuousUniformDistribution>(Amplitude - Delta, Amplitude + Delta);           
            
            var epicenter = Substitute.For<Firework>(expectedFireworkType, expectedBirthStepNumber - 1);
            var qualities = Substitute.For<IEnumerable<double>>();
            var sparks = Substitute.For<Dictionary<FireworkType, int>>();
            var explosion = Substitute.For<FireworkExplosion>(epicenter, expectedBirthStepNumber, Amplitude, sparks);
            
            var sparkGenerator = new AttractRepulseSparkGenerator(bestSolution, dimensions, distribution, randomizer);                                          
            
            //Act
            var spark = sparkGenerator.CreateSpark(explosion);

            //Assert
            Assert.NotNull(spark);
            Assert.Equal(expectedFireworkType, spark.FireworkType);
            Assert.Equal(expectedBirthStepNumber, spark.BirthStepNumber);
        }

        [Theory, MemberData("DataForTestCreationInstanceOfAttractRepulseGenerator")]
        public void CreateIntaceOfAttractRepulseGenerator_PassEachParameterAsNullAndOtherIsCorrect_ArgumentNullExceptionThrown(
            Solution bestSolution, IEnumerable<Dimension> dimensions, ContinuousUniformDistribution distribution, System.Random randomizer, string expectedParamName)
        {
            //Act
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => new AttractRepulseSparkGenerator(bestSolution, dimensions, distribution, randomizer));

            //Assert
            Assert.Equal(expectedParamName, exception.ParamName);
        }        
    }
}
