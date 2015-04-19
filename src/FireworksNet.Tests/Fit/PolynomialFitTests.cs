using System;
using System.Linq;
using Xunit;
using FireworksNet.Fit;

namespace FireworksNet.Tests.Fit
{
    public class PolynomialFitTests
    {
        private readonly PolynomialFit polynomialFit;
        private readonly double[] coordinates;
        private readonly double[] qualities;

        public PolynomialFitTests()
        {
            this.polynomialFit = new FireworksNet.Fit.PolynomialFit(1);

            //Test data taken from https://github.com/mathnet/mathnet-numerics/tree/master/src/UnitTests
            this.coordinates = Enumerable.Range(1, 6).Select(Convert.ToDouble).ToArray();
            this.qualities = new[] { 4.986, 2.347, 2.061, -2.995, -2.352, -5.782 };
        }

        [Fact]
        public void Approximate_PresentAllParam_ReturnsEqualResults()
        {
            Func<double, double> expectedFunc = x => 7.01013 - 2.08551 * x;

            Func<double, double> actualFunc = this.polynomialFit.Approximate(this.coordinates, this.qualities);

            foreach (double value in Enumerable.Range(-3, 10))
            {
                Assert.Equal(expectedFunc(value), actualFunc(value), 2);
            }
        }

        [Fact]
        public void Approximate_PresentAllParam_ReturnsNonEqualResults()
        {
            Func<double, double> expectedFunc = x => 5.02435 - 2.08551 * x;

            Func<double, double> actualFunc = this.polynomialFit.Approximate(this.coordinates, this.qualities);

            foreach (double value in Enumerable.Range(-3, 10))
            {
                Assert.NotEqual(expectedFunc(value), actualFunc(value), 2);
            }
        }

        [Fact]
        public void Approximate_NullAs1stParam_ExceptionThrown()
        {
            string expectedParamName = "fireworkCoordinates";
            double[] coordinates = null;

            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => this.polynomialFit.Approximate(coordinates, this.qualities));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Fact]
        public void Approximate_NullAs2ndParam_ExceptionThrown()
        {
            string expectedParamName = "fireworkQualities";
            double[] qualities = null;

            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => this.polynomialFit.Approximate(this.coordinates, qualities));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }
    }
}
