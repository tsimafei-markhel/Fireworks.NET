using System;
using System.Collections.Generic;
using Xunit;
using FireworksNet.Selection;
using FireworksNet.Model;

namespace FireworksNet.Tests.Selection
{
    public class BestSelectorTests
    {
        private readonly int samplingNumber;
        private readonly int countFireworks;
        private readonly Func<IEnumerable<Firework>, Firework> getBest;
        private readonly IEnumerable<Firework> allFireworks;
        private readonly BestFireworkSelector bestSelector;

        public BestSelectorTests()
        {
            this.samplingNumber = SelectorTestsHelper.SamplingNumber;
            this.countFireworks = SelectorTestsHelper.CountFireworks;
            this.getBest = SelectorTestsHelper.GetBest;
            this.allFireworks = SelectorTestsHelper.Fireworks;
            this.bestSelector = new BestFireworkSelector(this.getBest, this.samplingNumber);
        }

        [Fact]
        public void SelectFireworks_PresentAllParam_ReturnsEqualFireworks()
        {
            IEnumerable<Firework> expectedFireworks = SelectorTestsHelper.BestFireworks;

            IEnumerable<Firework> resultingFireworks = this.bestSelector.SelectFireworks(this.allFireworks, this.samplingNumber);

            Assert.NotSame(expectedFireworks, resultingFireworks);
            Assert.Equal(expectedFireworks, resultingFireworks);
        }

        [Fact]
        public void SelectFireworks_PresentAllParam_ReturnsNonEqualFireworks()
        {
            IEnumerable<Firework> expectedFireworks = SelectorTestsHelper.NonBestFireworks;

            IEnumerable<Firework> resultingFireworks = this.bestSelector.SelectFireworks(this.allFireworks, this.samplingNumber);

            Assert.NotSame(expectedFireworks, resultingFireworks);
            Assert.NotEqual(expectedFireworks, resultingFireworks);
        }

        [Fact]
        public void SelectFireworks_NullAs1stParam_ExceptionThrown()
        {
            string expectedParamName = "from";
            IEnumerable<Firework> currentFireworks = null;

            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => this.bestSelector.SelectFireworks(currentFireworks));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Fact]
        public void SelectFireworks_NegativeNumberAs2ndParam_ExceptionThrown()
        {
            string expectedParamName = "numberToSelect";
            int samplingNumber = -1;

            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => this.bestSelector.SelectFireworks(this.allFireworks, samplingNumber));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Fact]
        public void SelectFireworks_GreatNumberAs2ndParam_ExceptionThrown()
        {
            string expectedParamName = "numberToSelect";
            int samplingNumber = this.countFireworks + 1;

            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => this.bestSelector.SelectFireworks(this.allFireworks, samplingNumber));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Fact]
        public void SelectFireworks_CountFireworksEqual2ndParam_ReturnsEqualFireworks()
        {
            IEnumerable<Firework> expectedFireworks = this.allFireworks;
            int samplingNumber = this.countFireworks;

            IEnumerable<Firework> resultingFireworks = this.bestSelector.SelectFireworks(this.allFireworks, samplingNumber);

            Assert.NotSame(expectedFireworks, resultingFireworks);
            Assert.Equal(expectedFireworks, resultingFireworks);
        }

        [Fact]
        public void SelectFireworks_NullAs2ndParam_ReturnsEmptyCollectionFireworks()
        {
            IEnumerable<Firework> expectedFireworks = new List<Firework>();
            int samplingNumber = 0;

            IEnumerable<Firework> resultingFireworks = this.bestSelector.SelectFireworks(this.allFireworks, samplingNumber);

            Assert.NotSame(expectedFireworks, resultingFireworks);
            Assert.Equal(expectedFireworks, resultingFireworks);
        }
    }
}