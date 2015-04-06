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
        [Fact]
        public void IsValueInRange_Calculation_PositiveExpected()
        {
            Assert.True(dimension.IsValueInRange(10));
            Assert.False(dimension.IsValueInRange(16));
            Assert.False(dimension.IsValueInRange(4.5));
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
