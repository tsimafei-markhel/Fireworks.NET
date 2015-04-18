using System.Collections.Generic;
using FireworksNet.Distributions;
using FireworksNet.Explode;
using FireworksNet.Model;
using NSubstitute;
using System;
using Xunit;

namespace FireworksNet.Tests.Generation
{
    public class AttractRepulseSparkMutatorTests : AbstractSourceData
    {
        [Fact]
        public void CreateInstanceOfAttractRepulseSparkMutator_PassValidParameter()
        {
            //Arrange
            var generator = CreateAttractRepulseSparkGenerator();          

            //Act
            var mutator = new AttractRepulseSparkMutator(generator);
                
            //Assert
            Assert.NotNull(mutator);
        }

        [Fact]
        public void CreateInstanceOfAttractRepulseSparkMutator_PassParameterAsNull_ArgumentNullExceptionThrown()
        {
            //Arrange
            const string expectedParamName = "generator";
            
            //Act
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new AttractRepulseSparkMutator(null));

            //Assert
            Assert.Equal(expectedParamName, exception.ParamName);
        }

        [Theory, MemberData("DataForTestMethodMutateFireworkOfAttractRepulseSparkMutator")]
        public void MutateFirework_PassEachParameterAsNullAndOtherIsCorrect_ArgumentNullExceptionThrown(
            MutableFirework mutableFirework, FireworkExplosion explosion, String expectedParamName)
        {
            //Arrange
            var generator = CreateAttractRepulseSparkGenerator();
            var mutator = new AttractRepulseSparkMutator(generator);

            //Act
            ArgumentException exception = Assert.Throws<ArgumentNullException>(() => mutator.MutateFirework(ref mutableFirework, explosion));
        
            //Assert
            Assert.Equal(expectedParamName, exception.ParamName);
        }

        [Fact]
        public void MutateFirework_PassValidParameters_ShouldChangeFireworkState()
        {
            //Arrange
            Range range = new Range(-10, 10);
            IList<Dimension> dimensions = new List<Dimension>();
            dimensions.Add(new Dimension(range));
            dimensions.Add(new Dimension(range));
            dimensions.Add(new Dimension(range));

            IDictionary<Dimension, double> coordinatesBefore = new Dictionary<Dimension, double>();
            IDictionary<Dimension, double> coordinatesAfter = new Dictionary<Dimension, double>();

            foreach (Dimension dimension in dimensions)
            {
                coordinatesBefore.Add(dimension, 0);
            }
            foreach (Dimension dimension in dimensions)
            {
                coordinatesAfter.Add(dimension, 1);
            }    

            var mutableFirework = new MutableFirework(FireworkType.SpecificSpark, 0, coordinatesBefore);
            var mutateFirework = new MutableFirework(FireworkType.SpecificSpark, 1, coordinatesAfter);//present state mutable firework after mutate

            var explosion = CreateFireworkExplosion(mutableFirework);
            var generator = CreateAttractRepulseSparkGenerator();
            generator.CreateSpark(explosion).Returns(mutateFirework);
            var mutator = Substitute.For<AttractRepulseSparkMutator>(generator);

            //Act            
            mutator.MutateFirework(ref mutableFirework, explosion);

            //Assert
            Assert.NotNull(mutableFirework);
            Assert.Equal(mutableFirework.BirthStepNumber, mutateFirework.BirthStepNumber);
            Assert.Equal(mutableFirework.Quality, mutateFirework.Quality);
            double dimensionValueBefore;
            double dimensionValueAfter;
            foreach (Dimension dimension in dimensions)
            {
                mutableFirework.Coordinates.TryGetValue(dimension, out dimensionValueBefore);
                mutateFirework.Coordinates.TryGetValue(dimension, out dimensionValueAfter);
                Assert.Equal(dimensionValueBefore, dimensionValueAfter);
            }
        }
    }
}
