using System.Collections.Generic;
using Xunit;
using NSubstitute;
using FireworksNet.Selection;
using FireworksNet.Model;

namespace FireworksNet.Tests.Selection
{
    public class RandomSelectorTests
    {
        [Theory]
        [InlineData("Equal", 2)]
        [InlineData("NonEqual", 3)]
        public void Select_Test(string typeTest, int numberLastFireworks)
        {
            int samplingNumber = SelectorTestsHelper.SamplingNumber;
            System.Random randomizer = Substitute.For<System.Random>();
            randomizer.Next(0, 10).Returns(0, 1, numberLastFireworks);
            IEnumerable<Firework> expectedFireworks = SelectorTestsHelper.RandomFireworks;
            RandomSelector selector = new RandomSelector(randomizer, samplingNumber);
                              
            IEnumerable<Firework> resultingFireworks = selector.Select(SelectorTestsHelper.Fireworks);

            if (typeTest == "Equal")
            {
                Assert.Equal(expectedFireworks, resultingFireworks);
            }
            
            if (typeTest == "NonEqual")
            {
                Assert.NotEqual(expectedFireworks, resultingFireworks);
            }            
        }
    }
}