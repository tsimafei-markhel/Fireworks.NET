using System;
using System.Collections.Generic;
using FireworksNet.Fit;
using FireworksNet.Generation;
using FireworksNet.Model;
using Xunit;

namespace FireworksNet.Tests.Generation
{
    public class EliteStrategyGeneratorTests
    {
        #region TestDataSource
        public class TestEliteStrategyGenerator : EliteStrategyGenerator
        {
            public TestEliteStrategyGenerator(IEnumerable<Dimension> dimensions, IFit polynomialFit)
                : base(dimensions, polynomialFit)
            {

            }
            protected override double CalculateElitePoint(Func<double, double> func, Range variationRange)
            {
                return 0;
            }
            public IDictionary<Dimension, Func<double, double>> TestApproximateFitnessLandscapes(IEnumerable<Firework> fireworks)
            {
                return base.ApproximateFitnessLandscapes(fireworks);
            }

        }

        public static IEnumerable<object[]> ProblemData
        {
            get
            {
                PolynomialFit polynomialFit = new PolynomialFit(0);
                List<Dimension> dimensions = new List<Dimension>();

                return new[] {
                    new object[] { null,      polynomialFit, "dimensions"},
                    new object[] {dimensions, null,          "polynomialFit" }

                };
            }
        }

        public static TestEliteStrategyGenerator getTestEliteStrategyGenerator()
        {
            return new TestEliteStrategyGenerator(new List<Dimension>(), new PolynomialFit(0));
        }
        #endregion

        [Theory, MemberData("ProblemData")]
        public void Problem_NegativeParams_ArgumentNullExceptionThrown(IEnumerable<Dimension> dimensions,
            IFit polynomialFit,
            string expectedParamName)
        {
            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => new TestEliteStrategyGenerator(dimensions, polynomialFit));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Theory]
        [InlineData(null, "fireworks")]
        public void ApproximateFitnessLandscapes_NegativeParams_ArgumentNullExceptionThrown(IEnumerable<Firework> fireworks, string expectedParamName)
        {
            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => getTestEliteStrategyGenerator().TestApproximateFitnessLandscapes(fireworks));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }
    }
}