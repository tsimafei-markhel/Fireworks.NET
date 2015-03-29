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
    public class AttractRepulseSparkGeneratorTests
    {
        [Fact]
        public void CreateSpark_MustReturnNotNullFirework()
        {
            //Arrange
            const int expectedBirthStepNumber = 1;
            const FireworkType expectedFireworkType = FireworkType.SpecificSpark;
            
            var exploderSettings = Substitute.For<ParallelExploderSettings>();
            var algorithmSettings = Substitute.For<ParallelFireworksAlgorithmSettings>();
            
            var bestSolution = Substitute.For<Solution>(0);
            var dimensions = Substitute.For<IList<Dimension>>();                
            var randomizer = Substitute.For<System.Random>();
            var distribution = Substitute.For<ContinuousUniformDistribution>(
                    algorithmSettings.Amplitude - algorithmSettings.Delta, algorithmSettings.Amplitude + algorithmSettings.Delta);            
            var exploder = Substitute.For<ParallelExploder>(exploderSettings);
           
            var epicenter = Substitute.For<Firework>(expectedFireworkType, expectedBirthStepNumber - 1);
            var qualities = Substitute.For<IEnumerable<double>>();
            var explosion = exploder.Explode(epicenter, qualities, expectedBirthStepNumber);
            
            var sparkGenerator = new AttractRepulseSparkGenerator(bestSolution, dimensions, distribution, randomizer);                                          
            
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
            //Arrange
            const string expectedParamName = "bestSolution";
            
            var dimensions = Substitute.For<IList<Dimension>>();     
            var algorithmSettings = Substitute.For<ParallelFireworksAlgorithmSettings>();
            var distribution = Substitute.For<ContinuousUniformDistribution>(
                    algorithmSettings.Amplitude - algorithmSettings.Delta, algorithmSettings.Amplitude + algorithmSettings.Delta);
            var randomizer = Substitute.For<System.Random>();

            //Act
            ArgumentNullException exeption = Assert.Throws<ArgumentNullException>(
                () => new AttractRepulseSparkGenerator(null, dimensions, distribution, randomizer));

            //Assert
            Assert.Equal(expectedParamName, exeption.ParamName);
        }

        [Fact]
        public void CreateIntaceOfAttractRepulseGenerator_Pass2thParameterAsNull_ArgumentNullExceptionThrown()
        {
            //Arrange
            const string expectedParamName = "dimensions";

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
            //Arrange
            const string expectedParamName = "distribution";
            
            var bestSolution = Substitute.For<Solution>(0);
            var dimensions = Substitute.For<IList<Dimension>>();     
            var randomizer = Substitute.For<System.Random>();

            //Act
            ArgumentNullException exeption = Assert.Throws<ArgumentNullException>(
                () => new AttractRepulseSparkGenerator(bestSolution, dimensions, null, randomizer));

            //Assert
            Assert.Equal(expectedParamName, exeption.ParamName);
        }

        [Fact]
        public void CreateIntaceOfAttractRepulseGenerator_Pass4thParameterAsNull_ArgumentNullExceptionThrown()
        {
            //Arrange
            const string expectedParamName = "randomizer";

            var bestSolution = Substitute.For<Solution>(0);
            var dimensions = Substitute.For<IList<Dimension>>();             
            var algorithmSettings = Substitute.For<ParallelFireworksAlgorithmSettings>();
            var distribution = Substitute.For<ContinuousUniformDistribution>(
                    algorithmSettings.Amplitude - algorithmSettings.Delta, algorithmSettings.Amplitude + algorithmSettings.Delta);
            
            //Act
            ArgumentNullException exeption = Assert.Throws<ArgumentNullException>(
                () => new AttractRepulseSparkGenerator(bestSolution, dimensions, distribution, null));

            //Assert
            Assert.Equal(expectedParamName, exeption.ParamName);
        }
    }
}
