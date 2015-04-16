using System.Collections.Generic;
using Xunit;
using System;
using FireworksNet.Model;

namespace FireworksNet.Tests.Model
{
    public class RangeTests
    {
        private readonly Range range;

        public RangeTests(){
            range = new Range(-1.0, false, 5.5, true);
        }

        public static IEnumerable<object[]> RangeData
        {
            get
            {
                Range range1 = Range.Create(100, 50, true, false);
                return new[] {
                //new object[] { range1, Range.CreateWithRestrictions(100, 50, 40, 160, true, false), true  },
              //  new object[] { range1, new Range(40, false, 60, true), false },
                //new object[] { new Range(40, false, 60, false), new Range(40, false, 60, true), false },
                new object[] { range1, "badObject", false } 
                };
            }
        }

        [Theory,
        InlineData(10, false),
        InlineData(16, false),
        InlineData(-4.5, false),
        InlineData(5.5, false),
        InlineData(0.0, true),
        InlineData(3.8, true),
        InlineData(-1.0, true)]
        public void IsValueInRange_Calculation_PositiveExpected(double value, bool expected)
        {
            var actual = range.IsInRange(value);
            Assert.Equal(expected, actual);          
        }

     /*  [Theory, MemberData("RangeData")]
        public void GetHashCode_RangesVariations_PositiveExpected(object range1, object Obj, bool expected)
        {

            var actual = (range1.GetHashCode() == Obj.GetHashCode());
            Assert.Equal(expected, actual);

        }
        [Theory, MemberData("RangeData")]
        public void Equals_RangesVariations_PositiveExpected(object range1, object Obj, bool expected)
        {
            var actual = range1.Equals(Obj);
            Assert.Equal(expected, actual);
        }
        [Theory, MemberData("RangeData")]
        public void ComparingOperator_RangesVariations_PositiveExpected(object range1, object Obj, bool expected)
        {
            var actual = (range1 == Obj);
            Assert.Equal(expected, actual);

            var actual2 = !(range1 != Obj);
            Assert.Equal(expected, actual);
        }
        */

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


        
    }
}
