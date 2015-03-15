using System.Collections.Generic;
using Xunit;
using NSubstitute;
using FireworksNet.Selection;
using FireworksNet.Model;
using System;

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
            string expectedParamName = "numberToSelect";
            int samplingNumber = -1;

            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => this.randomSelector.SelectFireworks(this.allFireworks, samplingNumber));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Fact]
        public void SelectFireworks_GreatNumberAs2ndParam_ExceptionThrown()
        {
            string expectedParamName = "numberToSelect";
            int samplingNumber = this.countFireworks + 1;

            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => this.randomSelector.SelectFireworks(this.allFireworks, samplingNumber));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Fact]
        public void SelectFireworks_CountFireworksEqual2ndParam_ReturnsEqualFireworks()
        {
            IEnumerable<Firework> expectedFireworks = this.allFireworks;
            int samplingNumber = this.countFireworks;

            IEnumerable<Firework> resultingFireworks = this.randomSelector.SelectFireworks(this.allFireworks, samplingNumber);

            Assert.NotSame(expectedFireworks, resultingFireworks);
            Assert.Equal(expectedFireworks, resultingFireworks);
        }

        [Fact]
        public void SelectFireworks_NullAs2ndParam_ReturnsEmptyCollectionFireworks()
        {
            IEnumerable<Firework> expectedFireworks = new List<Firework>();
            int samplingNumber = 0;

            IEnumerable<Firework> resultingFireworks = this.randomSelector.SelectFireworks(this.allFireworks, samplingNumber);

            Assert.NotSame(expectedFireworks, resultingFireworks);
            Assert.Equal(expectedFireworks, resultingFireworks);
        }
    }
}