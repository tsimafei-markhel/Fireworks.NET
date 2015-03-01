using System.Collections.Generic;
using Xunit;
using NSubstitute;
using FireworksNet.Selection;
using FireworksNet.Model;

namespace FireworksNet.Tests.Selection
{
    public class RandomSelectorTests
    {
        private int samplingNumber;

        public RandomSelectorTests()
        {
            this.samplingNumber = SelectorTestsHelper.SamplingNumber;
        }

        [Fact]
        public void Select_Equal()
        {
            System.Random randomizer = Substitute.For<System.Random>();
            randomizer.Next(0, 10).Returns(0, 1, 2);
            RandomSelector selector = new RandomSelector(randomizer, samplingNumber);
            IEnumerable<Firework> expectedFireworks = SelectorTestsHelper.RandomFireworks;

            IEnumerable<Firework> resultingFireworks = selector.Select(SelectorTestsHelper.Fireworks);

            Assert.NotSame(expectedFireworks, resultingFireworks);
            Assert.Equal(expectedFireworks, resultingFireworks);
        }

        [Fact]
        public void Select_NonEqual()
        {
            System.Random randomizer = Substitute.For<System.Random>();
            randomizer.Next(0, 10).Returns(0, 1, 3);
            RandomSelector selector = new RandomSelector(randomizer, samplingNumber);
            IEnumerable<Firework> expectedFireworks = SelectorTestsHelper.RandomFireworks;

            IEnumerable<Firework> resultingFireworks = selector.Select(SelectorTestsHelper.Fireworks);

            Assert.NotSame(expectedFireworks, resultingFireworks);
            Assert.NotEqual(expectedFireworks, resultingFireworks);
        }

        [Fact]
        public void Select_NullFireworksCollection_ExceptionThrown()
        {

        }
    }
}