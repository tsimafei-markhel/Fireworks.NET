using System.Collections.Generic;
using Xunit;
using System;
using FireworksNet.Model;

namespace FireworksNet.Tests.Model
{
    public class DimensionTests
    {
        private readonly Dimension dimension;

        public DimensionTests()
        {
            this.dimension = new Dimension(new Range(5, 15.5));
        }

        [Theory]
        [InlineData(10, true)]
        [InlineData(16, false)]
        [InlineData(4.5, false)]
        public void IsValueInRange_Calculation_PositiveExpected(double value, bool expected)
        {
            bool actual = this.dimension.IsValueInRange(value);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Dimension_NegaviteAs1stParam_ExceptionThrown()
        {
            Range nullVariationRange = null;
            string expectedParamName = "variationRange";

            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => new Dimension(nullVariationRange));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }
    }
}