using System;
using System.Collections.Generic;
using FireworksNet.Model;
using FireworksNet.Selection;
using FireworksNet.Distances;
using Xunit;
using NSubstitute;

namespace FireworksNet.Tests.Selection
{
    public class NearBestSelectorTests
    {
        private readonly int samplingNumber;
        private readonly int countFireworks;
        private readonly Func<IEnumerable<Firework>, Firework> getBest;
        private readonly Firework bestFirework;
        private readonly List<Firework> allFireworks;
        private readonly IDistance distanceCalculator;
        private readonly NearBestFireworkSelector nearBestSelector;

        public NearBestSelectorTests()
        {
            this.samplingNumber = SelectorTestsHelper.SamplingNumber;
            this.countFireworks = SelectorTestsHelper.CountFireworks;
            this.getBest = SelectorTestsHelper.GetBest;
            this.bestFirework = SelectorTestsHelper.FirstBestFirework;            
            this.allFireworks = new List<Firework>(SelectorTestsHelper.Fireworks);
            this.distanceCalculator = Substitute.For<IDistance>();
            for (int i = 1; i < 10; i++)
            {
                this.distanceCalculator.Calculate(this.bestFirework, this.allFireworks[i]).Returns(i);
            }
            this.nearBestSelector = new NearBestFireworkSelector(this.distanceCalculator, this.getBest, this.samplingNumber);
        }

        [Fact]
        public void SelectFireworks_PresentAllParam_ReturnsEqualFireworks()
        {                 
            IEnumerable<Firework> expectedFireworks = SelectorTestsHelper.NearBestFireworks;

            IEnumerable<Firework> resultingFireworks = this.nearBestSelector.SelectFireworks(this.allFireworks, this.samplingNumber);

            Assert.NotSame(expectedFireworks, resultingFireworks);
            Assert.Equal(expectedFireworks, resultingFireworks);
        }

        [Fact]
        public void SelectFireworks_PresentAllParam_ReturnsNonEqualFireworks()
        {          
            IEnumerable<Firework> expectedFireworks = SelectorTestsHelper.NonNearBestFirework;

            IEnumerable<Firework> resultingFireworks = this.nearBestSelector.SelectFireworks(this.allFireworks, this.samplingNumber);

            Assert.NotSame(expectedFireworks, resultingFireworks);
            Assert.NotEqual(expectedFireworks, resultingFireworks);
        }

        [Fact]
        public void SelectFireworks_NullAs1stParam_ExceptionThrown()
        {
            string expectedParamName = "from";
            IEnumerable<Firework> currentFireworks = null;

            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => this.nearBestSelector.SelectFireworks(currentFireworks, this.samplingNumber));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Fact]
        public void SelectFireworks_NegativeNumberAs2ndParam_ExceptionThrown()
        {
            string expectedParamName = "numberToSelect";
            int samplingNumber = -1;

            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => this.nearBestSelector.SelectFireworks(this.allFireworks, samplingNumber));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Fact]
        public void SelectFireworks_GreatNumberAs2ndParam_ExceptionThrown()
        {
            string expectedParamName = "numberToSelect";
            int samplingNumber = this.countFireworks + 1;

            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => this.nearBestSelector.SelectFireworks(this.allFireworks, samplingNumber));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Fact]
        public void SelectFireworks_CountFireworksEqual2ndParam_ReturnsEqualFireworks()
        {
            IEnumerable<Firework> expectedFireworks = this.allFireworks;
            int samplingNumber = this.countFireworks;

            IEnumerable<Firework> resultingFireworks = this.nearBestSelector.SelectFireworks(this.allFireworks, samplingNumber);

            Assert.NotSame(expectedFireworks, resultingFireworks);
            Assert.Equal(expectedFireworks, resultingFireworks);
        }

        [Fact]
        public void SelectFireworks_NullAs2ndParam_ReturnsEmptyCollectionFireworks()
        {
            IEnumerable<Firework> expectedFireworks = new List<Firework>();
            int samplingNumber = 0;

            IEnumerable<Firework> resultingFireworks = this.nearBestSelector.SelectFireworks(this.allFireworks, samplingNumber);

            Assert.NotSame(expectedFireworks, resultingFireworks);
            Assert.Equal(expectedFireworks, resultingFireworks);
        }
    }
}