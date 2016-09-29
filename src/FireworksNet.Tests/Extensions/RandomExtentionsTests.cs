using System;
using System.Collections.Generic;
using FireworksNet.Extensions;
using Xunit;

namespace FireworksNet.Tests.Extensions
{
    public class RandomExtentionsTests
    {
        private readonly System.Random rnd;

        public RandomExtentionsTests()
        {
            rnd = new System.Random();
        }

        public static IEnumerable<object[]> NextDoubleData
        {
            get
            {
                System.Random rnd = new System.Random();
                const double minInclusive = 10.05D;
                const double intervalLength = 11.984D;
                return new[] {
                    new object[] { rnd, double.NaN,              intervalLength,          "minInclusive" },
                    new object[] { rnd, double.PositiveInfinity, intervalLength,          "minInclusive" },
                    new object[] { rnd, double.NegativeInfinity, intervalLength,          "minInclusive" },
                    new object[] { rnd, minInclusive,            double.NaN,              "intervalLength" },
                    new object[] { rnd, minInclusive,            double.PositiveInfinity, "intervalLength" },
                    new object[] { rnd, minInclusive,            double.NegativeInfinity, "intervalLength" }
                };
            }
        }

        public static IEnumerable<object[]> NextInt32sData
        {
            get
            {
                System.Random rnd = new System.Random();
                const int minInclusive = 10;
                const int maxExclusive = 17;
                const int neededValuesNumber = 3;
                return new[] {
                    new object[] { rnd, -1,                 minInclusive, maxExclusive, "neededValuesNumber" },
                    new object[] { rnd, neededValuesNumber, 15,           maxExclusive, "neededValuesNumber" },
                    new object[] { rnd, neededValuesNumber, 18,           maxExclusive, "maxExclusive" }
                };
            }
        }

        #region NextDouble() tests

        [Fact]
        public void NextDouble_NullAsRandom_ExceptionThrown()
        {
            System.Random random = null;
            const double minInclusive = 10.05D;
            const double intervalLength = 11.984D;

            string expectedParamName = "random";

            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => random.NextDouble(minInclusive, intervalLength));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Theory]
        [MemberData("NextDoubleData")]
        public void NextDouble_NegativeDoubleArgs_ExceptionThrown(System.Random random, double minInclusive, double intervalLength, string expectedParamName)
        {
            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => random.NextDouble(minInclusive, intervalLength));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        // TODO : endless loop in NextDouble(this System.Random random, Range allowedRange)
        /*[Fact]
        public void NextDouble_BadRange_ExceptionThrown()
        {
            System.Random random = new System.Random();
            Range range = new Range(1.0, true, 1.0, true);
         
            //string expectedParamName = "range";
            //ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => random.NextDouble(range));
            //Assert.NotNull(actualException);
            //Assert.Equal(expectedParamName, actualException.ParamName);
          
            string expectedParamName = "1";
            string actualExeption = random.NextDouble(range).ToString();
            Assert.Equal(expectedParamName, actualExeption);
        }
        */

        #endregion

        #region NextInt32s() tests

        [Fact]
        public void NextInt32s_NullAsRandom_ExceptionThrown()
        {
            System.Random random = null;
            const int minInclusive = 5;
            const int maxExclusive = 9;
            const int neededValuesNumber = 3;

            string expectedParamName = "random";

            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => random.NextInt32s(neededValuesNumber, minInclusive, maxExclusive));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Theory]
        [MemberData("NextInt32sData")]
        public void NextInt32s_NegativeIntArgs_ExceptionThrown(System.Random random, int neededValuesNumber, int minInclusive, int maxExclusive, string expectedParamName)
        {
            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => random.NextInt32s(neededValuesNumber, minInclusive, maxExclusive));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        #endregion

        #region NextUniqueInt32s() tests

        [Fact]
        public void NextUniqueInt32s_NullAsRandom_ExceptionThrown()
        {
            System.Random random = null;
            const int minInclusive = 5;
            const int maxExclusive = 9;
            const int neededValuesNumber = 3;

            string expectedParamName = "random";

            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => random.NextUniqueInt32s(neededValuesNumber, minInclusive, maxExclusive));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Theory]
        [MemberData("NextInt32sData")]
        public void NextUniqueInt32s_NegativeIntArgs_ExceptionThrown(System.Random random, int neededValuesNumber, int minInclusive, int maxExclusive, string expectedParamName)
        {
            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => random.NextUniqueInt32s(neededValuesNumber, minInclusive, maxExclusive));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        #endregion

        #region NextBoolean() tests

        [Fact]
        public void NextBoolean_NullAsRandom_ExceptionThrown()
        {
            System.Random random = null;
            string expectedParamName = "random";

            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => random.NextBoolean());

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        #endregion
    }
}