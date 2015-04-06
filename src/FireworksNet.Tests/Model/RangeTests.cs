using System.Collections.Generic;
using Xunit;
using System;
using FireworksNet.Model;

namespace FireworksNet.Tests.Model
{
    public class RangeTests
    {
        
        public RangeTests(){  }
        [Fact]
        public void IsValueInRange_Calculation_PositiveExpected()
        {
            Range range = new Range(-1.0, false, 5.5, true);
            Assert.False(range.IsInRange(10));
            Assert.False(range.IsInRange(16));
            Assert.False(range.IsInRange(-4.5));
            Assert.False(range.IsInRange(5.5));

            Assert.True(range.IsInRange(0.0));
            Assert.True(range.IsInRange(3.8));
            Assert.True(range.IsInRange(-1.0));
            
        }
        [Fact]
        public void Range_NaNMinimumParam_ExceptionThrown()
        {
            double min = double.NaN;
            double max = 0;

            string expectedParamName = "minimum";

            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => new Range(min,  max));
            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }
        [Fact]
        public void Range_NaNMaximumParam_ExceptionThrown()
        {
            double min = 0;
            double max = double.NaN;

            string expectedParamName = "maximum";

            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => new Range(min, max));
            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }
        [Fact]
        public void Range_СonfusedMinimumMaximumParams_ExceptionThrown()
        {
            double min = 1;
            double max = -1;

            string expectedParamName = "minimum";

            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => new Range(min, max));
            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }
        [Fact]
        public void Create_NaNMeanParams_ExceptionThrown()
        {
            double mean = double.NaN;
            int deviationPercent = 1;

            string expectedParamName = "mean";

            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => Range.Create(mean, deviationPercent));
            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }
        [Fact]
        public void Create_InfinityMeanParams_ExceptionThrown()
        {
            double mean = double.PositiveInfinity;
            int deviationPercent = 1;

            string expectedParamName = "mean";

            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => Range.Create(mean, deviationPercent));
            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }
        [Fact]
        public void Create_NegativeDeviationPercentParams_ExceptionThrown()
        {
            double mean = 0;
            int deviationPercent = -1;

            string expectedParamName = "deviationPercent";

            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => Range.Create(mean, deviationPercent));
            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }
        [Fact]
        public void Create_NaNDeviationValueParams_ExceptionThrown()
        {
            double mean = 0;
            double deviationValue = double.NaN;

            string expectedParamName = "deviationValue";

            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => Range.Create(mean, deviationValue));
            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }
        [Fact]
        public void Create_InfinityDeviationValueParams_ExceptionThrown()
        {
            double mean = 0;
            double deviationValue = double.PositiveInfinity;

            string expectedParamName = "deviationValue";

            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => Range.Create(mean, deviationValue));
            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }
        [Fact]
        public void Create_NegativeDeviationValueParams_ExceptionThrown()
        {
            double mean = 0;
            double deviationValue = -1;

            string expectedParamName = "deviationValue";

            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => Range.Create(mean, deviationValue));
            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }
        [Fact]
        public void CreateWithRestrictions_NaNMeanParams_ExceptionThrown()
        {
            double mean = double.NaN;
            int deviationPercent = 1;
            double minRestriction = 0;
            double maxRestriction = 2;

            string expectedParamName = "mean";

            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => Range.CreateWithRestrictions(mean, deviationPercent,minRestriction,maxRestriction));
            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }
        [Fact]
        public void CreateWithRestrictions_InfinityMeanParams_ExceptionThrown()
        {
            double mean = double.PositiveInfinity;
            int deviationPercent = 1;
            double minRestriction = 0;
            double maxRestriction = 2;

            string expectedParamName = "mean";

            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => Range.CreateWithRestrictions(mean, deviationPercent, minRestriction, maxRestriction));
            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }
        [Fact]
        public void CreateWithRestrictions_NegativeDeviationPercentParams_ExceptionThrown()
        {
            double mean = 0;
            int deviationPercent = -1;
            double minRestriction = 0;
            double maxRestriction = 2;

            string expectedParamName = "deviationPercent";

            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => Range.CreateWithRestrictions(mean, deviationPercent, minRestriction, maxRestriction));
            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }
        [Fact]
        public void CreateWithRestrictions_NaNDeviationValueParams_ExceptionThrown()
        {
            double mean = 0;
            double deviationValue = double.NaN;
            double minRestriction = 0;
            double maxRestriction = 2;

            string expectedParamName = "deviationValue";

            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => Range.CreateWithRestrictions(mean, deviationValue, minRestriction, maxRestriction));
            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }
        [Fact]
        public void CreateWithRestrictions_InfinityDeviationValueParams_ExceptionThrown()
        {
            double mean = 0;
            double deviationValue = double.PositiveInfinity;
            double minRestriction = 0;
            double maxRestriction = 2;

            string expectedParamName = "deviationValue";

            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => Range.CreateWithRestrictions(mean, deviationValue, minRestriction, maxRestriction));
            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }
        [Fact]
        public void CreateWithRestrictions_NegativeDeviationValueParams_ExceptionThrown()
        {
            double mean = 0;
            double deviationValue =-1.0;
            double minRestriction = 0;
            double maxRestriction = 2;

            string expectedParamName = "deviationValue";

            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => Range.CreateWithRestrictions(mean, deviationValue, minRestriction, maxRestriction));
            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }


        [Fact]
        public void GetHashCode_RangesVariations_PositiveExpected()
        {
            Range range1 = Range.Create(100, 50, true, false);
            Range range2 = Range.CreateWithRestrictions(100, 50, 40, 160, true, false);
            Range range3 = new Range(40, false, 60, true);
            Range range4 = new Range(40, false, 60, false);
            Object badObject = "badObject";

            Assert.Equal(range1.GetHashCode(), range2.GetHashCode());
            Assert.NotEqual(range1.GetHashCode(), range3.GetHashCode());
            Assert.NotEqual(range1.GetHashCode(), badObject.GetHashCode());
            Assert.NotEqual(range3.GetHashCode(), range4.GetHashCode());
            
        }
        [Fact]
        public void Equals_RangesVariations_PositiveExpected()
        {
            Range range1 = Range.Create(100, 50, true, false);
            Range range2 = Range.CreateWithRestrictions(100, 50, 40, 160, true, false);
            Range range3 = new Range(40, false, 60, true);
            Object badObject = "badObject";

            Assert.True(range1.Equals(range2));
            Assert.False(range1.Equals(range3));
            Assert.False(range1.Equals(badObject));
            Assert.False(range1.Equals(null));
        }
        [Fact]
        public void ComparingOperator_RangesVariations_PositiveExpected()
        {
            Range range1 = Range.Create(100, 50, true, false);
            Range range2 = Range.CreateWithRestrictions(100, 50, 40, 160, true, false);
            Range range3 = new Range(40, false, 60, true);
            Range range4 = new Range(40, false, 60, false);
            Object badObject = "badObject";

            Assert.True(range1==range2);
            Assert.False(range1==range3);
            Assert.False(range4 == range3);
            Assert.False(range1==badObject);
            Assert.False(range1==null);

            Assert.False(range1 != range2);
            Assert.True(range1 != range3);
            Assert.True(range4 != range3);
            Assert.True(range1 != badObject);
            Assert.True(range1 != null);
        }
    }
}
