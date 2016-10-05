using System;
using System.Collections.Generic;
using FireworksNet.Generation;
using FireworksNet.Model;
using FireworksNet.Mutation;
using NSubstitute;
using Xunit;

namespace FireworksNet.Tests.Mutation
{
    public class AttractRepulseSparkMutatorTests : FireworksNet.Tests.Generation.AbstractSourceData
    {
        [Fact]
        public void CreateInstanceOfAttractRepulseSparkMutator_PassValidParameter()
        {
            ISparkGenerator<FireworkExplosion> generator = CreateAttractRepulseSparkGenerator();

            AttractRepulseSparkMutator mutator = new AttractRepulseSparkMutator(generator);

            Assert.NotNull(mutator);
        }

        [Fact]
        public void CreateInstanceOfAttractRepulseSparkMutator_PassParameterAsNull_ArgumentNullExceptionThrown()
        {
            const string expectedParamName = "generator";

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new AttractRepulseSparkMutator(null));

            Assert.Equal(expectedParamName, exception.ParamName);
        }

        [Theory, MemberData("DataForTestMethodMutateFireworkOfAttractRepulseSparkMutator")]
        public void MutateFirework_PassEachParameterAsNullAndOtherIsCorrect_ArgumentNullExceptionThrown(
            MutableFirework mutableFirework, FireworkExplosion explosion, String expectedParamName)
        {
            ISparkGenerator<FireworkExplosion> generator = CreateAttractRepulseSparkGenerator();
            AttractRepulseSparkMutator mutator = new AttractRepulseSparkMutator(generator);

            ArgumentException exception = Assert.Throws<ArgumentNullException>(() => mutator.MutateFirework(ref mutableFirework, explosion));

            Assert.Equal(expectedParamName, exception.ParamName);
        }

        [Fact]
        public void MutateFirework_PassValidParameters_ShouldChangeFireworkState()
        {
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
                coordinatesAfter.Add(dimension, 1);
            }

            MutableFirework mutableFirework = new MutableFirework(FireworkType.SpecificSpark, 0, 0, coordinatesBefore);
            MutableFirework mutateFirework = new MutableFirework(FireworkType.SpecificSpark, 1, 0, coordinatesAfter); // Present state mutable firework after mutate

            FireworkExplosion explosion = CreateFireworkExplosion(mutableFirework);
            ISparkGenerator<FireworkExplosion> generator = CreateAttractRepulseSparkGenerator();
            generator.CreateSpark(explosion).Returns(mutateFirework);
            AttractRepulseSparkMutator mutator = Substitute.For<AttractRepulseSparkMutator>(generator);

            mutator.MutateFirework(ref mutableFirework, explosion);

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