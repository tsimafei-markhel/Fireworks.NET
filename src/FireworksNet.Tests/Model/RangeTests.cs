using System;
using System.Collections.Generic;
using FireworksNet.Model;
using Xunit;

namespace FireworksNet.Tests.Model
{
    public class RangeTests
    {
        private readonly Range range;

        public RangeTests()
        {
            this.range = new Range(-1.0, false, 5.5, true);
        }

        public static IEnumerable<object[]> RangeData
        {
            get
            {
                Range range1 = Range.Create(100, 50, true, false);
                Range range2 = Range.CreateWithRestrictions(100, 50, 40, 160, true, false);
                Range range3 = new Range(40, false, 60, true);
                Range range4 = new Range(40, false, 60, false);

                return new[] {
                    new object[] { range1, range2,      true  },
                    new object[] { range1, range3,      false },
                    new object[] { range4, range3,      false },
                    new object[] { range1, "badObject", false }
                };
            }
        }

        public static IEnumerable<object[]> RangeDataOfCreateWithRestrictionsMethod
        {
            get
            {
                int deviationPercent = 1;
                double mean = 0.0;
                double deviationValue = 1.0;
                double minRestriction = 0;
                double maxRestriction = 2;

                return new[] {
                    new object[] { mean,                     deviationPercent,        double.NaN,     maxRestriction, "minimum" },
                    new object[] { mean,                     deviationPercent,        minRestriction, double.NaN,     "maximum" },
                    new object[] { mean,                     deviationPercent,        2,              -2,             "minimum" },
                    new object[] { double.NaN,               deviationPercent,        minRestriction, maxRestriction, "mean" },
                    new object[] { double.PositiveInfinity,  deviationPercent,        minRestriction, maxRestriction, "mean" },
                    new object[] { double.NegativeInfinity,  deviationPercent,        minRestriction, maxRestriction, "mean" },
                    new object[] { double.NaN,               deviationValue,          minRestriction, maxRestriction, "mean" },
                    new object[] { double.PositiveInfinity,  deviationValue,          minRestriction, maxRestriction, "mean" },
                    new object[] { double.NegativeInfinity,  deviationValue,          minRestriction, maxRestriction, "mean" },
                    new object[] { mean,                     -1,                      minRestriction, maxRestriction, "deviationPercent" },
                    new object[] { mean,                     double.NaN,              minRestriction, maxRestriction, "deviationValue" },
                    new object[] { mean,                     double.PositiveInfinity, minRestriction, maxRestriction, "deviationValue" },
                    new object[] { mean,                     double.NegativeInfinity, minRestriction, maxRestriction, "deviationValue" }
                };
            }
        }

        public static IEnumerable<object[]> RangeDataOfCreateMethod
        {
            get
            {
                int deviationPercent = 1;
                double mean = 0.0;
                double deviationValue = 1.0;

                return new[] {
                    new object[] { double.NaN,               deviationPercent,        "mean" },
                    new object[] { double.PositiveInfinity,  deviationPercent,        "mean" },
                    new object[] { double.NegativeInfinity,  deviationPercent,        "mean" },
                    new object[] { double.NaN,               deviationValue,          "mean" },
                    new object[] { double.PositiveInfinity,  deviationValue,          "mean" },
                    new object[] { double.NegativeInfinity,  deviationValue,          "mean" },
                    new object[] { mean,                     -1,                      "deviationPercent" },
                    new object[] { mean,                     double.NaN,              "deviationValue" },
                    new object[] { mean,                     double.PositiveInfinity, "deviationValue" },
                    new object[] { mean,                     double.NegativeInfinity, "deviationValue" }
                };
            }
        }

        [Theory]
        [InlineData(10, false)]
        [InlineData(16, false)]
        [InlineData(-4.5, false)]
        [InlineData(5.5, false)]
        [InlineData(0.0, true)]
        [InlineData(3.8, true)]
        [InlineData(-1.0, true)]
        public void IsValueInRange_Calculation_PositiveExpected(double value, bool expected)
        {
            bool actual = this.range.IsInRange(value);
            Assert.Equal(expected, actual);
        }

        [Theory, MemberData("RangeData")]
        public void GetHashCode_RangesVariations_PositiveExpected(object range1, object obj, bool expected)
        {
            int hash1 = range1.GetHashCode();
            int hash2 = obj.GetHashCode();

            bool actual = (hash1 == hash2);

            Assert.Equal(expected, actual);
        }

        [Theory, MemberData("RangeData")]
        public void Equals_RangesVariations_PositiveExpected(object range1, object obj, bool expected)
        {
            bool actual = range1.Equals(obj);

            Assert.Equal(expected, actual);
        }

        [Theory, MemberData("RangeData")]
        public void ComparingOperator_RangesVariations_PositiveExpected(Range range1, object obj, bool expected)
        {
            bool actual = (range1 == (obj as Range));
            bool actual2 = !(range1 != (obj as Range));

            Assert.Equal(expected, actual);
            Assert.Equal(expected, actual2);
        }

        [Fact]
        public void Range_NaNAs1tsParam_ArgumentOutOfRangeExceptionThrown()
        {
            double min = double.NaN;
            double max = 0.0D;
            string expectedParamName = "minimum";

            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => new Range(min, max));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Fact]
        public void Range_NaNAs2ndParam_ArgumentOutOfRangeExceptionThrown()
        {
            double min = 0.0D;
            double max = double.NaN;
            string expectedParamName = "maximum";

            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => new Range(min, max));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Fact]
        public void Range_СonfusedAs1stAND2ndParam_ArgumentOutOfRangeExceptionThrown()
        {
            double min = 1.0D;
            double max = -1.0D;
            string expectedParamName = "minimum";

            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => new Range(min, max));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Theory]
        [MemberData("RangeDataOfCreateMethod")]
        public void Create_NegativeParams_ArgumentOutOfRangeExceptionThrown(double mean, object deviation, string expectedParamName)
        {
            ArgumentOutOfRangeException actualException;
            if (deviation is double)
            {
                actualException = Assert.Throws<ArgumentOutOfRangeException>(() => Range.Create(mean, (double)deviation));
            }
            else
            {
                actualException = Assert.Throws<ArgumentOutOfRangeException>(() => Range.Create(mean, (int)deviation));
            }

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Theory]
        [MemberData("RangeDataOfCreateWithRestrictionsMethod")]
        public void CreateWithRestrictions_NegativeParams_ArgumentOutOfRangeExceptionThrown(double mean, object deviation, double minRestriction, double maxRestriction, string expectedParamName)
        {
            ArgumentOutOfRangeException actualException;
            if (deviation is double)
            {
                actualException = Assert.Throws<ArgumentOutOfRangeException>(() => Range.CreateWithRestrictions(mean, (double)deviation, minRestriction, maxRestriction));
            }
            else
            {
                actualException = Assert.Throws<ArgumentOutOfRangeException>(() => Range.CreateWithRestrictions(mean, (int)deviation, minRestriction, maxRestriction));
            }

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }
    }
}