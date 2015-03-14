using Xunit;
using FireworksNet.Extensions;

namespace FireworksNet.Tests.Extensions
{
    // TODO: Rename tests according to the programming-guidelines.md.
    public class DoubleExtensionsTests
    {
        [Fact]
        public static void IsEqualTest_Equal()
        {
            Assert.True((10.0).IsEqual(10.0));
        }

        [Fact]
        public static void IsEqualTest_NonEqual()
        {
            Assert.False((10.0).IsEqual(-56.9));
        }
    }
}