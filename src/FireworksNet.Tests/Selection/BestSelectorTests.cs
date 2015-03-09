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
        private readonly BestSelector bestSelector;

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

            IEnumerable<Firework> resultingFireworks = bestSelector.Select(this.allFireworks, this.samplingNumber);

            Assert.NotSame(expectedFireworks, resultingFireworks);
            Assert.Equal(expectedFireworks, resultingFireworks);
        }

        [Fact]
        public void Select_PresentAllParam_ReturnsNonEqualFireworks()
        {
            IEnumerable<Firework> expectedFireworks = SelectorTestsHelper.NonBestFireworks;

            IEnumerable<Firework> resultingFireworks = bestSelector.Select(this.allFireworks, this.samplingNumber);

            Assert.NotSame(expectedFireworks, resultingFireworks);
            Assert.NotEqual(expectedFireworks, resultingFireworks);
        }

        [Fact]
        public void Select_NullAs1stParam_ExceptionThrown()
        {
            IEnumerable<Firework> currentFireworks = null;
            string expectedParamName = "from";

            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => this.bestSelector.Select(currentFireworks));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Fact]
        public void Select_NegativeNumberAs2ndParam_ExceptionThrown()
        {
            string expectedParamName = "numberToSelect";

            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => this.bestSelector.Select(this.allFireworks, -1));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Fact]
        public void Select_GreatNumberAs2ndParam_ExceptionThrown()
        {
            string expectedParamName = "numberToSelect";
            int samplingNumber = this.countFireworks + 1;

            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => this.bestSelector.Select(this.allFireworks, samplingNumber));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Fact]
        public void Select_CountFireworksEqual2ndParam_ReturnsEqualFireworks()
        {
            IEnumerable<Firework> expectedFireworks = this.allFireworks;
            int samplingNumber = this.countFireworks;

            IEnumerable<Firework> resultingFireworks = bestSelector.Select(this.allFireworks, samplingNumber);

            Assert.NotSame(expectedFireworks, resultingFireworks);
            Assert.Equal(expectedFireworks, resultingFireworks);
        }

        [Fact]
        public void Select_NullAs2ndParam_ReturnsEmptyCollectionFireworks()
        {
            IEnumerable<Firework> expectedFireworks = new List<Firework>();

            IEnumerable<Firework> resultingFireworks = bestSelector.Select(this.allFireworks, 0);

            Assert.NotSame(expectedFireworks, resultingFireworks);
            Assert.Equal(expectedFireworks, resultingFireworks);
        }
    }
}