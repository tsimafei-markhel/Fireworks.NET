using System;
using Xunit;
using FireworksNet.Differentiation;

namespace FireworksNet.Tests.Differentiation
{
    public class DifferentiationTests
    {
        private readonly Differentiator differentiator;
        private readonly Func<double, double> targetFunc;
        private readonly double inputValue;

        public DifferentiationTests()
        {
            this.differentiator = new Differentiator();
            this.targetFunc = x => x * x;
            this.inputValue = 3;
        }

        [Fact]
        public void Differentiate_PresentAllParam_ReturnsEqualResult()
        {
            double expectedResult = 6;

            Func<double, double> resultingFunc = this.differentiator.Differentiate(this.targetFunc);
            double actualResult = resultingFunc(this.inputValue);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void Differentiate_PresentAllParam_ReturnsNonEquaResult()
        {
            double expectedResult = 5;

            Func<double, double> resultingFunc = this.differentiator.Differentiate(this.targetFunc);
            double actualResult = resultingFunc(this.inputValue);

            Assert.NotEqual(expectedResult, actualResult);
        }

        [Fact]
        public void Differentiate_NullAs1stParam_ExceptionThrown()
        {
            string expectedParamName = "func";
            Func<double, double> func = null;

            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => this.differentiator.Differentiate(func));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }
    }
}
