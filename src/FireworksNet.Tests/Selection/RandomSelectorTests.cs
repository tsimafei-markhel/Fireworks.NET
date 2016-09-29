using System;
using System.Collections.Generic;
using System.Linq;
using FireworksNet.Model;
using FireworksNet.Selection;
using Xunit;

namespace FireworksNet.Tests.Selection
{
    public class RandomSelectorTests
    {
        private readonly int samplingNumber;
        private readonly int countFireworks;
        private readonly System.Random randomizer;
        private readonly IEnumerable<Firework> allFireworks;
        private readonly RandomFireworkSelector randomSelector;

        public RandomSelectorTests()
        {
            this.samplingNumber = SelectorTestsHelper.SamplingNumber;
            this.countFireworks = SelectorTestsHelper.CountFireworks;
            this.randomizer = new System.Random();
            this.allFireworks = SelectorTestsHelper.Fireworks;
            this.randomSelector = new RandomFireworkSelector(this.randomizer, this.samplingNumber);
        }

        [Fact]
        public void SelectFireworks_PresentAllParam_ReturnsExistsFireworks()
        {
            IEnumerable<Firework> resultingFireworks = randomSelector.SelectFireworks(this.allFireworks, this.samplingNumber);

            foreach(Firework firework in resultingFireworks)
            {
                Assert.Contains(firework, this.allFireworks);
            }
        }

        [Fact]
        public void SelectFireworks_PresentAllParam_ReturnsNonEqualCollections()
        {
            IEnumerable<Firework> firstResultingFireworks = randomSelector.SelectFireworks(this.allFireworks, this.samplingNumber);
            IEnumerable<Firework> secondResultingFireworks = randomSelector.SelectFireworks(this.allFireworks, this.samplingNumber);

            Assert.NotSame(firstResultingFireworks, secondResultingFireworks);
            Assert.NotEqual(firstResultingFireworks, secondResultingFireworks);
        }

        [Fact]
        public void SelectFireworks_NullAs1stParam_ExceptionThrown()
        {
            string expectedParamName = "from";
            IEnumerable<Firework> currentFireworks = null;

            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => this.randomSelector.SelectFireworks(currentFireworks, this.samplingNumber));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Fact]
        public void SelectFireworks_NegativeNumberAs2ndParam_ExceptionThrown()
        {
            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => this.randomSelector.SelectFireworks(this.allFireworks, -1));

            Assert.NotNull(actualException);
            Assert.Equal("numberToSelect", actualException.ParamName);
        }

        [Fact]
        public void SelectFireworks_GreatNumberAs2ndParam_ExceptionThrown()
        {
            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => this.randomSelector.SelectFireworks(this.allFireworks, this.countFireworks + 1));

            Assert.NotNull(actualException);
            Assert.Equal("numberToSelect", actualException.ParamName);
        }

        [Fact]
        public void SelectFireworks_CountFireworksEqual2ndParam_ReturnsEqualFireworks()
        {
            IEnumerable<Firework> resultingFireworks = this.randomSelector.SelectFireworks(this.allFireworks, this.countFireworks);

            Assert.NotSame(this.allFireworks, resultingFireworks);
            Assert.Equal(this.allFireworks, resultingFireworks);
        }

        [Fact]
        public void SelectFireworks_ZeroAs2ndParam_ReturnsEmptyCollectionFireworks()
        {
            IEnumerable<Firework> resultingFireworks = this.randomSelector.SelectFireworks(this.allFireworks, 0);

            Assert.NotSame(Enumerable.Empty<Firework>(), resultingFireworks);
            Assert.Equal(Enumerable.Empty<Firework>(), resultingFireworks);
        }
    }
}