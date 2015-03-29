using FireworksNet.Algorithm.Implementation;
using FireworksNet.Explode;
using FireworksNet.Model;
using FireworksNet.Random;
using NSubstitute;
using System;
using System.Collections.Generic;
using Xunit;

namespace FireworksNet.Tests.Explode
{
    public class ParallelExploderTests
    {
        [Fact]
        public void CreateInstanceOfParallelExploder_PassNullAsParameter_ArgumentNullExceptionThrown()
        {
            //Arrange
            const string expectedParamName = "settings";           

            //Act
            ArgumentNullException exeption = Assert.Throws<ArgumentNullException>(() => new ParallelExploder(null));

            //Assert
            Assert.NotNull(exeption);
            Assert.Equal(expectedParamName, exeption.ParamName);            
        }

        [Fact]
        public void CreateInstanceOfParallelExploder_PassValidSettings_ShouldReturnNotNullExploder()
        {
            //Arrange             
            var exploderSettings = Substitute.For<ParallelExploderSettings>();

            //Act
            var exploder = new ParallelExploder(exploderSettings);

            //Assert
            Assert.NotNull(exploder);
        }

        [Fact]
        public void Explode_PassValidParameters_ShouldReturnExplosionBase()
        {
            //Arrange
            const int expectedBirthStepNumber = 1;
            const FireworkType expectedFireworkType = FireworkType.SpecificSpark;
            var exploderSettings = Substitute.For<ParallelExploderSettings>();
            var epicenter = Substitute.For<Firework>(expectedFireworkType, expectedBirthStepNumber - 1);
            var qualities = Substitute.For<IEnumerable<double>>();
            var exploder = new ParallelExploder(exploderSettings);

            //Act
            var explosion = exploder.Explode(epicenter, qualities, expectedBirthStepNumber);          
             
            //Assert
            Assert.NotNull(explosion);
            Assert.True(explosion is FireworkExplosion);
            Assert.Equal(exploderSettings.Amplitude, (explosion as FireworkExplosion).Amplitude);
            Assert.Equal(epicenter, (explosion as FireworkExplosion).ParentFirework);
            Assert.Equal(expectedBirthStepNumber, explosion.StepNumber);            
        }

        [Fact]
        public void Explode_Pass1stParameterAsNull_ArgumentNullExceptionThrown()
        {
            //Arrange
            const string expectedParamName = "epicenter";

            int birthStepNumber = 1;            
            var exploderSettings = Substitute.For<ParallelExploderSettings>();             
            var qualities = Substitute.For<IEnumerable<double>>();
            var exploder = new ParallelExploder(exploderSettings);

            //Act
            ArgumentNullException exeption = Assert.Throws<ArgumentNullException>(() => exploder.Explode(null, qualities, birthStepNumber));

            //Assert
            Assert.NotNull(exeption);
            Assert.Equal(expectedParamName, exeption.ParamName);
        }

        [Fact]
        public void Explode_Pass2ndParameterAsNull_ArgumentNullExceptionThrown()
        {
            //Arrange
            const string expectedParamName = "currentFireworkQualities";

            int birthStepNumber = 1;
            FireworkType expectedFireworkType = FireworkType.SpecificSpark;
            var exploderSettings = Substitute.For<ParallelExploderSettings>();
            var epicenter = Substitute.For<Firework>(expectedFireworkType, birthStepNumber - 1);
            var exploder = new ParallelExploder(exploderSettings);

            //Act
            ArgumentNullException exeption = Assert.Throws<ArgumentNullException>(() => exploder.Explode(epicenter, null, birthStepNumber));

            //Assert
            Assert.NotNull(exeption);
            Assert.Equal(expectedParamName, exeption.ParamName);
        }

        [Fact]
        public void Explode_Pass3rdParameterAsNegativeNumber_ArgumentExceptionThrown()
        {
            //Arrange
            const string expectedParamName = "currentStepNumber";

            int birthStepNumber = 1;
            FireworkType expectedFireworkType = FireworkType.SpecificSpark;
            var exploderSettings = Substitute.For<ParallelExploderSettings>();
            var epicenter = Substitute.For<Firework>(expectedFireworkType, birthStepNumber - 1);
            var qualities = Substitute.For<IEnumerable<double>>();
            var exploder = new ParallelExploder(exploderSettings);

            //Act
            ArgumentOutOfRangeException exeption = Assert.Throws<ArgumentOutOfRangeException>(() => exploder.Explode(epicenter, qualities, -1));

            //Assert
            Assert.NotNull(exeption);
            Assert.Equal(expectedParamName, exeption.ParamName);
        }
    }
}
