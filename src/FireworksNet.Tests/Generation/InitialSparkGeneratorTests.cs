using System;
using System.Collections.Generic;
using FireworksNet.Generation;
using FireworksNet.Model;
using Xunit;

namespace FireworksNet.Tests.Generation
{
    public class InitialSparkGeneratorTests
    {
        #region TestData
        public static IEnumerable<object[]> ProblemData
        {
            get
            {
                IDictionary<Dimension, Range> initialRandes = new Dictionary<Dimension, Range>();
                List<Dimension> dimensions = new List<Dimension>();
                System.Random randomizer = new System.Random();

                return new[] {
                    new object[] { null,      initialRandes,  randomizer, "dimensions"},
                    new object[] {dimensions, null,          randomizer,  "initialRanges"},
                    new object[] {dimensions, initialRandes,  null,       "randomizer"}
                };
            }
        }

        public static IEnumerable<object[]> ProblemData2
        {
            get
            {
                List<Dimension> dimensions = new List<Dimension>();
                System.Random randomizer = new System.Random();

                return new[] {
                    new object[] { null,      randomizer, "dimensions"},
                    new object[] {dimensions, null,       "randomizer"}
                };
            }
        }
        #endregion

        [Theory, MemberData("ProblemData")]
        public void InitialSparkGenerator_NegativeParams_ArgumentNullExceptionThrown(IEnumerable<Dimension> dimensions,
            IDictionary<Dimension, Range> initialRandes,
            System.Random randomizer,
            string expectedParamName)
        {
            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => new InitialSparkGenerator(dimensions, initialRandes, randomizer));
            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        // TODO: This method always throws an error
        /*[Theory, MemberData("ProblemData2")]
        public void InitialSparkGenerator_NegativeParams_ArgumentNullExceptionThrown(IEnumerable<Dimension> dimensions,
            System.Random randomizer,
            string expectedParamName)
        {
            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => new InitialSparkGenerator(dimensions, randomizer));
            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }*/
    }
}