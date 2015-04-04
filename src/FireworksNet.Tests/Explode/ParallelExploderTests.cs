using System;
using System.Collections.Generic;
using FireworksNet.Algorithm.Implementation;
using FireworksNet.Explode;
using FireworksNet.Model;
using FireworksNet.Random;
using NSubstitute;
using Xunit;
using Xunit.Extensions;


namespace FireworksNet.Tests.Explode
{
    public class ParallelExploderTests : AbstractSourceData
    {
        [Fact]
        public void CreateInstanceOfParallelExploder_PassNullAsParameter_ArgumentNullExceptionThrown()
        {
            //Arrange
            const string expectedParamName = "settings";

            //Act
            ArgumentNullException exeption = Assert.Throws<ArgumentNullException>(() => new ParallelExploder(null));

            //Assert
            Assert.Equal(expectedParamName, exeption.ParamName);
        }

        [Fact]
        public void CreateInstanceOfParallelExploder_PassValidSettings_ShouldReturnNotNullExploder()
        {
            //Arrange             
            var exploderSettings = Substitute.For<FireworksNet.Explode.ParallelExploderSettings>();

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

        [Theory, MemberData("DataForTestMethodExplodeOfParallelExploder")]
        public void Explode_PassEachParameterAsNullAndOtherIsCorrect_ArgumentExceptionThrown(
            Firework epicenter, IEnumerable<double> qualities, int currentStepNumber, Type exceptionType,  string expectedParamName)
        {
            //Arrange
            var exploderSettings = Substitute.For<ParallelExploderSettings>();           
            var exploder = new ParallelExploder(exploderSettings);
                       
            //Act
            string actualParamName = null;

            if (typeof(ArgumentNullException) == exceptionType)
            {
                ArgumentNullException exeption = Assert.Throws<ArgumentNullException>(
                    () => exploder.Explode(epicenter, qualities, currentStepNumber));
                actualParamName = exeption.ParamName;
            }
            else if(typeof(ArgumentOutOfRangeException) == exceptionType)
            {
                ArgumentOutOfRangeException exeption = Assert.Throws<ArgumentOutOfRangeException>(
                    () => exploder.Explode(epicenter, qualities, currentStepNumber));
                actualParamName = exeption.ParamName;
            }            

            //Assert             
            Assert.Equal(expectedParamName, actualParamName);
        }        
    }
}
