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
        private int samplingNumber;
        private IDistance distanceCalculator;
        private Func<IEnumerable<Firework>, Firework> getBest;
        private Firework bestFirework;
        private List<Firework> allFireworks;

        public NearBestSelectorTests()
        {
            this.samplingNumber = SelectorTestsHelper.SamplingNumber;
            this.getBest = SelectorTestsHelper.GetBest;
            this.bestFirework = SelectorTestsHelper.FirstBestFirework;            
            this.allFireworks = new List<Firework>(SelectorTestsHelper.Fireworks);
            this.distanceCalculator = Substitute.For<IDistance>();
            for (int i = 1; i < 10; i++)
            {
                this.distanceCalculator.Calculate(this.bestFirework, this.allFireworks[i]).Returns(i);
            }
        }

        [Fact]
        public void Select_Equal()
        {                     
            NearBestSelector selector = new NearBestSelector(this.distanceCalculator, this.getBest, this.samplingNumber);
            IEnumerable<Firework> expectedFireworks = SelectorTestsHelper.NearBestFireworks;

            IEnumerable<Firework> resultingFireworks = selector.Select(allFireworks);

            Assert.NotSame(expectedFireworks, resultingFireworks);
            Assert.Equal(expectedFireworks, resultingFireworks);
        }

        [Fact]
        public void Select_NonEqual()
        {
            NearBestSelector selector = new NearBestSelector(this.distanceCalculator, this.getBest, this.samplingNumber);
            IEnumerable<Firework> expectedFireworks = SelectorTestsHelper.NonNearBestFirework;

            IEnumerable<Firework> resultingFireworks = selector.Select(allFireworks);

            Assert.NotSame(expectedFireworks, resultingFireworks);
            Assert.NotEqual(expectedFireworks, resultingFireworks);
        }
    }
}