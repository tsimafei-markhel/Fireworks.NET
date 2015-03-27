using Xunit;
using FireworksNet.Extensions;

namespace FireworksNet.Tests.Extensions
{
    public class DoubleExtensionsTests
    {
        #region IsEqual

        [Fact]
        public static void IsEqual_PassedEqualArgs_ReturnsTrue()
        {
            Assert.True((10.0).IsEqual(10.0));
        }

        [Fact]
        public static void IsEqual_PassedNonEqualArgs_ReturnsFalse()
        {
            Assert.False((10.0).IsEqual(-56.9));
        }

        #endregion

        #region IsLess

        [Fact]
        public static void IsLess_Passed1stArgLessThan2nd_ReturnsTrue()
        {
            Assert.True((10.0).IsLess(11.0));
        }

        [Fact]
        public static void IsLess_Passed1stArgEqualTo2nd_ReturnsFalse()
        {
            Assert.False((10.0).IsLess(10.0));
        }

        [Fact]
        public static void IsLess_Passed1stArgGreaterThan2nd_ReturnsFalse()
        {
            Assert.False((11.0).IsLess(10.0));
        }

        #endregion

        #region IsLessOrEqual

        [Fact]
        public static void IsLessOrEqual_Passed1stArgLessThan2nd_ReturnsTrue()
        {
            Assert.True((10.0).IsLessOrEqual(11.0));
        }

        [Fact]
        public static void IsLessOrEqual_Passed1stArgEqualTo2nd_ReturnsTrue()
        {
            Assert.True((10.0).IsLessOrEqual(10.0));
        }

        [Fact]
        public static void IsLessOrEqual_Passed1stArgGreaterThan2nd_ReturnsFalse()
        {
            Assert.False((11.0).IsLessOrEqual(10.0));
        }

        #endregion

        #region IsGreater

        [Fact]
        public static void IsGreater_Passed1stArgLessThan2nd_ReturnsFalse()
        {
            Assert.False((10.0).IsGreater(11.0));
        }

        [Fact]
        public static void IsGreater_Passed1stArgEqualTo2nd_ReturnsFalse()
        {
            Assert.False((10.0).IsGreater(10.0));
        }

        [Fact]
        public static void IsGreater_Passed1stArgGreaterThan2nd_ReturnsTrue()
        {
            Assert.True((11.0).IsGreater(10.0));
        }

        #endregion

        #region IsGreaterOrEqual

        [Fact]
        public static void IsGreaterOrEqual_Passed1stArgLessThan2nd_ReturnsFalse()
        {
            Assert.False((10.0).IsGreaterOrEqual(11.0));
        }

        [Fact]
        public static void IsGreaterOrEqual_Passed1stArgEqualTo2nd_ReturnsTrue()
        {
            Assert.True((10.0).IsGreaterOrEqual(10.0));
        }

        [Fact]
        public static void IsGreaterOrEqual_Passed1stArgGreaterThan2nd_ReturnsTrue()
        {
            Assert.True((11.0).IsGreaterOrEqual(10.0));
        }

        #endregion

        #region ToStringInvariant

        [Fact]
        public static void ToStringInvariant_PassedDouble_ReturnsValidString()
        {
            double value = 10.05;
            string expectedValueString = "10.05";

            string valueString = value.ToStringInvariant();

            Assert.Equal(expectedValueString, valueString);
        }

        #endregion
    }
}