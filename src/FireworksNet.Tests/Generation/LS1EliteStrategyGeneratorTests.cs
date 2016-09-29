using System;
using System.Collections.Generic;
using FireworksNet.Fit;
using FireworksNet.Generation;
using FireworksNet.Model;
using Xunit;

namespace FireworksNet.Tests.Generation
{
    public class LS1EliteStrategyGeneratorTests
    {
        #region TestDataSource
        public static IEnumerable<object[]> ProblemData
        {
            get
            {
                IFit polynomialFit = new PolynomialFit(0);
                List<Dimension> dimensions = new List<Dimension>();

                return new[] {
                    new object[] { null,      polynomialFit, "dimensions"},
                    new object[] {dimensions, null,          "polynomialFit" }

                };
            }
        }
        #endregion

        [Theory, MemberData("ProblemData")]
        public void LS1EliteStrategyGenerator_NegativeParams_ArgumentNullExceptionThrown(IEnumerable<Dimension> dimensions,
            IFit polynomialFit,
            string expectedParamName)
        {
            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => new LS1EliteStrategyGenerator(dimensions, polynomialFit));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }
    }
}