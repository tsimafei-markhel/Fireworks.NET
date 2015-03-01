using System;
using System.Collections.Generic;
using Xunit;
using FireworksNet.Selection;
using FireworksNet.Model;

namespace FireworksNet.Tests.Selection
{
    public class BestSelectorTests
    {
        private int samplingNumber;
        private int countFireworks;
        private Func<IEnumerable<Firework>, Firework> getBest;
        private IEnumerable<Firework> allFireworks;
        private BestSelector bestSelector;

        public BestSelectorTests()
        {
            this.samplingNumber = SelectorTestsHelper.SamplingNumber;
            this.countFireworks = SelectorTestsHelper.CountFireworks;
            this.getBest = SelectorTestsHelper.GetBest;
            this.allFireworks = SelectorTestsHelper.Fireworks;
            this.bestSelector = new BestSelector(this.getBest, this.samplingNumber);
        }

        [Fact]
        public void Select_Missing2ndParam_ReturnsEqualFireworks()
        {
            IEnumerable<Firework> expectedFireworks = SelectorTestsHelper.BestFireworks;

            IEnumerable<Firework> resultingFireworks = bestSelector.Select(this.allFireworks);

            Assert.NotSame(expectedFireworks, resultingFireworks);
            Assert.Equal(expectedFireworks, resultingFireworks);
        }

        [Fact]
        public void Select_Missing2ndParam_ReturnsNonEqualFireworks()
        {
            IEnumerable<Firework> expectedFireworks = SelectorTestsHelper.NonBestFireworks;

            IEnumerable<Firework> resultingFireworks = bestSelector.Select(this.allFireworks);

            Assert.NotSame(expectedFireworks, resultingFireworks);
            Assert.NotEqual(expectedFireworks, resultingFireworks);
        }

        [Fact]
        public void Select_PresentAllParam_ReturnsEqualFireworks()
        {
            IEnumerable<Firework> expectedFireworks = SelectorTestsHelper.BestFireworks;
            this.samplingNumber++;

            IEnumerable<Firework> resultingFireworks = bestSelector.Select(this.allFireworks, this.samplingNumber);

            Assert.NotSame(expectedFireworks, resultingFireworks);
            Assert.Equal(expectedFireworks, resultingFireworks);
        }

        [Fact]
        public void Select_PresentAllParam_ReturnsNonEqualFireworks()
        {
            IEnumerable<Firework> expectedFireworks = SelectorTestsHelper.NonBestFireworks;
            this.samplingNumber++;

            IEnumerable<Firework> resultingFireworks = bestSelector.Select(this.allFireworks, this.samplingNumber);

            Assert.NotSame(expectedFireworks, resultingFireworks);
            Assert.NotEqual(expectedFireworks, resultingFireworks);
        }

        [Fact]
        public void Select_NullAs1stParam_ExceptionThrown()
        {
            IEnumerable<Firework> currentFireworks = null;
            string exceptionMessage = "Value cannot be null.\r\nParameter name: from";

            Exception actualException = Assert.Throws<ArgumentNullException>(() => this.bestSelector.Select(currentFireworks));

            Assert.NotNull(actualException);
            Assert.Equal(exceptionMessage, actualException.Message);
        }

        [Fact]
        public void Select_NegativeNumberAs2ndParam_ExceptionThrown()
        {
            string exceptionMessage = "Specified argument was out of the range of valid values.\r\nParameter name: numberToSelect";
            this.samplingNumber = -1;

            Exception actualException = Assert.Throws<ArgumentOutOfRangeException>(() => this.bestSelector.Select(this.allFireworks, this.samplingNumber));
          
            Assert.NotNull(actualException);
            Assert.Equal(exceptionMessage, actualException.Message);
        }

        [Fact]
        public void Select_GreatNumberAs2ndParam_ExceptionThrown()
        {
            string exceptionMessage = "Specified argument was out of the range of valid values.\r\nParameter name: numberToSelect";
            this.samplingNumber = this.countFireworks + 1;

            Exception actualException = Assert.Throws<ArgumentOutOfRangeException>(() => this.bestSelector.Select(this.allFireworks, this.samplingNumber));

            Assert.NotNull(actualException);
            Assert.Equal(exceptionMessage, actualException.Message);
        }

        [Fact]
        public void Select_CountFireworksEqual2ndParam_ReturnsEqualFireworks()
        {
            IEnumerable<Firework> expectedFireworks = this.allFireworks;
            this.samplingNumber = this.countFireworks;

            IEnumerable<Firework> resultingFireworks = bestSelector.Select(this.allFireworks, this.samplingNumber);

            Assert.NotSame(expectedFireworks, resultingFireworks);
            Assert.Equal(expectedFireworks, resultingFireworks);
        }

        [Fact]
        public void Select_NullAs2ndParam_ReturnsEmptyCollectionFireworks()
        {
            IEnumerable<Firework> expectedFireworks = new List<Firework>();
            this.samplingNumber = 0;

            IEnumerable<Firework> resultingFireworks = bestSelector.Select(this.allFireworks, this.samplingNumber);

            Assert.NotSame(expectedFireworks, resultingFireworks);
            Assert.Equal(expectedFireworks, resultingFireworks);
        }
    }
}