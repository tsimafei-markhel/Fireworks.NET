using System;
using System.Collections.Generic;
using FireworksNet.Distributions;
using FireworksNet.Generation;
using FireworksNet.Model;
using Xunit;

namespace FireworksNet.Tests.Generation
{
    public class GaussianSparkGeneratorTests
    {
        #region TestData
        class TestDistribution : IContinuousDistribution
        {

            public double Sample()
            {
                return 0;
            }

            public IEnumerable<double> Samples()
            {
                return new List<double>();
            }
        }

        public static IEnumerable<object[]> ProblemData
        {
            get
            {
                IContinuousDistribution distribution = new TestDistribution();
                List<Dimension> dimensions = new List<Dimension>();
                System.Random randomizer = new System.Random();

                return new[] {
                    new object[] { null,      distribution,  randomizer, "dimensions"},
                    new object[] {dimensions, null,          randomizer, "distribution" },
                    new object[] {dimensions, distribution,  null,       "randomizer"}
                };
            }
        }

        #endregion

        [Theory, MemberData("ProblemData")]
        public void GaussianSparkGenerator_NegativeParams_ArgumentNullExceptionThrown(IEnumerable<Dimension> dimensions,
            IContinuousDistribution distribution,
            System.Random randomizer,
            string expectedParamName)
        {
            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => new GaussianSparkGenerator(dimensions, distribution, randomizer));
            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }
    }
}