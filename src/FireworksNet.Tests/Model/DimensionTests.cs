using System.Collections.Generic;
using Xunit;
using System;
using FireworksNet.Model;

namespace FireworksNet.Tests.Model
{
    public class DimensionTests
    {
        Dimension dimension; 
        public DimensionTests()
        {
            dimension = new Dimension(new Range(5, 15.5));
        }
        [Theory, 
        InlineData(10, true),
        InlineData(16, false),
        InlineData(4.5, false)]
        public void IsValueInRange_Calculation_PositiveExpected(double value, bool expected)
        {
            var actual = dimension.IsValueInRange(value);
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void Dimension_NegaviteParam_ExceptionThrown()
        {  
            Range NullVariationRange=null;

            string expectedParamName = "variationRange";
          
            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => new Dimension(NullVariationRange));
            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }
    }
}
