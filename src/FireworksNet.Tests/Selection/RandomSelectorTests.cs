using Xunit;
using NSubstitute;
using FireworksNet.Extensions;
using System.Collections.Generic;

namespace FireworksNet.Tests.Selection
{
    public class RandomSelectorTests
    {
        public RandomSelectorTests()
        {
            
        }

        [Fact]
        public void Select()
        {
            System.Random rand = Substitute.For<System.Random>();
            IEnumerable<int> test = new List<int>(){  1,2,3,4,5};
            rand.Received().NextInt32s(0, 1, 2).Returns(test);
        }
    }
}
