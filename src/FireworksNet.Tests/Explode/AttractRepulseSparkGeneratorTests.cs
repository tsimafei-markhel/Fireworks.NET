using System;

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
    public class AttractRepulseSparkGeneratorTests
    {
        [Fact]
        public void CreateSparkTyped_MustReturnNotNullFirework()
        {
            const int expectedBirthStepNumber = 0;
            const FireworkType expectedFireworkType = FireworkType.SpecificSpark;

            //Arrange
            var exploderSettings = Substitute.For<ParallelExploderSettings>();
            var algorithmSettings = Substitute.For<ParallelFireworksAlgorithmSettings>();
            
            var bestSolution = Substitute.For<Solution>(0);
            var problem = Sphere.Create();                
            var randomizer = Substitute.For<System.Random>();
            var distribution = Substitute.For<ContinuousUniformDistribution>(
                    algorithmSettings.Amplitude - algorithmSettings.Delta, algorithmSettings.Amplitude + algorithmSettings.Delta);

            var exploder = Substitute.For<ParallelExploder>(exploderSettings);
            var explosion = exploder.Explode(ExploderTestHelper.Epicenter, ExploderTestHelper.Qualities, 1);
            var returnedSpark = Substitute.For<Firework>(expectedFireworkType, expectedBirthStepNumber);

            var sparkGenerator = Substitute.For<AttractRepulseSparkGenerator>(bestSolution, problem.Dimensions, distribution, randomizer);                                            
            sparkGenerator 
                .CreateSpark(explosion) 
                .Returns(returnedSpark);                                 
            
            //Act
            var spark = sparkGenerator.CreateSpark(explosion);

            //Assert
            Assert.NotNull(spark);
            Assert.Equal(expectedFireworkType, spark.FireworkType);
            Assert.Equal(expectedBirthStepNumber, spark.BirthStepNumber);
        }

        [Fact]
        public void CreateIntaceOfAttractRepulseGenerator_Pass1stParameterAsNull_ArgumentNullExceptionThrown()
        {
            const string expectedParamName = "bestSolution";

            //Arrange
            var problem = Sphere.Create();
            var algorithmSettings = Substitute.For<ParallelFireworksAlgorithmSettings>();
            var distribution = Substitute.For<ContinuousUniformDistribution>(
                    algorithmSettings.Amplitude - algorithmSettings.Delta, algorithmSettings.Amplitude + algorithmSettings.Delta);
            var randomizer = Substitute.For<System.Random>();

            //Act
            ArgumentNullException exeption = Assert.Throws<ArgumentNullException>(
                () => new AttractRepulseSparkGenerator(null, problem.Dimensions, distribution, randomizer));

            //Assert
            Assert.Equal(expectedParamName, exeption.ParamName);
        }

        [Fact]
        public void CreateIntaceOfAttractRepulseGenerator_Pass2thParameterAsNull_ArgumentNullExceptionThrown()
        {
            const string expectedParamName = "dimentions";

            //Arrange
            var bestSolution = Substitute.For<Solution>(0);
            var algorithmSettings = Substitute.For<ParallelFireworksAlgorithmSettings>();
            var distribution = Substitute.For<ContinuousUniformDistribution>(
                    algorithmSettings.Amplitude - algorithmSettings.Delta, algorithmSettings.Amplitude + algorithmSettings.Delta);
            var randomizer = Substitute.For<System.Random>();

            //Act
            ArgumentNullException exeption = Assert.Throws<ArgumentNullException>(
                () => new AttractRepulseSparkGenerator(bestSolution, null, distribution, randomizer));

            //Assert
            Assert.Equal(expectedParamName, exeption.ParamName);
        }

        [Fact]
        public void CreateIntaceOfAttractRepulseGenerator_Pass3rdParameterAsNull_ArgumentNullExceptionThrown()
        {
            const string expectedParamName = "distribution";

            //Arrange
            var bestSolution = Substitute.For<Solution>(0);
            var problem = Sphere.Create();
            var randomizer = Substitute.For<System.Random>();

            //Act
            ArgumentNullException exeption = Assert.Throws<ArgumentNullException>(
                () => new AttractRepulseSparkGenerator(bestSolution, problem.Dimensions, null, randomizer));

            //Assert
            Assert.Equal(expectedParamName, exeption.ParamName);
        }

        [Fact]
        public void CreateIntaceOfAttractRepulseGenerator_Pass4thParameterAsNull_ArgumentNullExceptionThrown()
        {
            const string expectedParamName = "randomizer";

            //Arrange
            var bestSolution = Substitute.For<Solution>(0);
            var problem = Sphere.Create();
            var algorithmSettings = Substitute.For<ParallelFireworksAlgorithmSettings>();
            var distribution = Substitute.For<ContinuousUniformDistribution>(
                    algorithmSettings.Amplitude - algorithmSettings.Delta, algorithmSettings.Amplitude + algorithmSettings.Delta);
            
            //Act
            ArgumentNullException exeption = Assert.Throws<ArgumentNullException>(
                () => new AttractRepulseSparkGenerator(bestSolution, problem.Dimensions, distribution, null));

            //Assert
            Assert.Equal(expectedParamName, exeption.ParamName);
        }
    }
}
