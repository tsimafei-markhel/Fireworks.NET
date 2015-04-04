using Xunit;
using FireworksNet.Extensions;

namespace FireworksNet.Tests.Extensions
{
    public class DoubleExtensionsTests
    {
        private const double lesserValue = 10.05D;
        private const double greaterValue = 11.984D;

        [Theory]
        [InlineData(lesserValue, greaterValue, false)]
        [InlineData(lesserValue, lesserValue, true)]
        public void IsEqual_PassedDifferentArgs_ReturnsExpected(double firstValue, double secondValue, bool expected)
        {
            Assert.Equal(expected, firstValue.IsEqual(secondValue));
        }

        [Theory]
        [InlineData(lesserValue, greaterValue, true)]
        [InlineData(lesserValue, lesserValue, false)]
        [InlineData(greaterValue, lesserValue, false)]
        public void IsLess_PassedDifferentArgs_ReturnsExpected(double firstValue, double secondValue, bool expected)
        {
            Assert.Equal(expected, firstValue.IsLess(secondValue));
        }

        [Theory]
        [InlineData(lesserValue, greaterValue, true)]
        [InlineData(lesserValue, lesserValue, true)]
        [InlineData(greaterValue, lesserValue, false)]
        public void IsLessOrEqual_PassedDifferentArgs_ReturnsExpected(double firstValue, double secondValue, bool expected)
        {
            Assert.Equal(expected, firstValue.IsLessOrEqual(secondValue));
        }

        [Theory]
        [InlineData(lesserValue, greaterValue, false)]
        [InlineData(lesserValue, lesserValue, false)]
        [InlineData(greaterValue, lesserValue, true)]
        public void IsGreater_PassedDifferentArgs_ReturnsExpected(double firstValue, double secondValue, bool expected)
        {
            Assert.Equal(expected, firstValue.IsGreater(secondValue));
        }

        [Theory]
        [InlineData(lesserValue, greaterValue, false)]
        [InlineData(lesserValue, lesserValue, true)]
        [InlineData(greaterValue, lesserValue, true)]
        public void IsGreaterOrEqual_PassedDifferentArgs_ReturnsExpected(double firstValue, double secondValue, bool expected)
        {
            Assert.Equal(expected, firstValue.IsGreaterOrEqual(secondValue));
        }

        [Fact]
        public void ToStringInvariant_PassedDouble_ReturnsValidString()
        {
            string expectedValueString = "10.05";

            string valueString = lesserValue.ToStringInvariant();

            Assert.Equal(expectedValueString, valueString);
        }
    }

    public class DoubleExtensionComparerTests
    {
        private readonly DoubleExtensionComparer comparer;
        private const double lesserValue = 10.05D;
        private const double greaterValue = 11.984D;

        public DoubleExtensionComparerTests()
        {
            this.comparer = new DoubleExtensionComparer();
        }

        [Theory]
        [InlineData(lesserValue, greaterValue, -1)]
        [InlineData(lesserValue, lesserValue, 0)]
        [InlineData(greaterValue, lesserValue, 1)]
        public void Compare_PassedDifferentArgs_ReturnsExpected(double firstValue, double secondValue, int expected)
        {
            Assert.Equal(expected, this.comparer.Compare(firstValue, secondValue));
        }
    }
}