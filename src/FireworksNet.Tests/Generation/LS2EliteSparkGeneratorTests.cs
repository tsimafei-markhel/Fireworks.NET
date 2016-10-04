using System;
using System.Collections.Generic;
using FireworksNet.Differentiation;
using FireworksNet.Fit;
using FireworksNet.Generation;
using FireworksNet.Model;
using FireworksNet.Solving;
using Xunit;

namespace FireworksNet.Tests.Generation
{
    public class LS2EliteSparkGeneratorTests
    {
        #region TestDataSource
        public static IEnumerable<object[]> ProblemData
        {
            get
            {
                IFit polynomialFit = new PolynomialFit(0);
                List<Dimension> dimensions = new List<Dimension>();
                IDifferentiator differentiation = new Differentiator();
                ISolver solver = new Solver();

                return new[] {
                    new object[] { null,      polynomialFit, differentiation, solver, "dimensions"},
                    new object[] {dimensions, null,          differentiation, solver, "polynomialFit" },
                    new object[] {dimensions, polynomialFit, null,            solver, "differentiation" },
                    new object[] {dimensions, polynomialFit, differentiation, null,   "solver" }


                };
            }
        }
        #endregion

        [Theory, MemberData("ProblemData")]
        public void LS2EliteStrategyGenerator_NegativeParams_ArgumentNullExceptionThrown(IEnumerable<Dimension> dimensions,
            IFit polynomialFit,
            IDifferentiator differentiation,
            ISolver solver,
            string expectedParamName)
        {
            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => new LS2EliteSparkGenerator(dimensions, polynomialFit, differentiation, solver));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }
    }
}