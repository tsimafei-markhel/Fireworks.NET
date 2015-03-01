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
        private Func<IEnumerable<Firework>, Firework> getBest;
        private BestSelector bestSelector;

        public BestSelectorTests()
        {
            this.samplingNumber = SelectorTestsHelper.SamplingNumber;
            this.getBest = SelectorTestsHelper.GetBest;
            this.bestSelector = new BestSelector(this.getBest, this.samplingNumber);
        }

        [Fact]
        public void Select_Missing2ndParam_ReturnsEqualFreworks()
        {            
            IEnumerable<Firework> expectedFireworks = SelectorTestsHelper.BestFireworks;

            IEnumerable<Firework> resultingFireworks = bestSelector.Select(SelectorTestsHelper.Fireworks);

            Assert.NotSame(expectedFireworks, resultingFireworks);
            Assert.Equal(expectedFireworks, resultingFireworks);
        }

        [Fact]
        public void Select_NonEqual()
        {            
            IEnumerable<Firework> expectedFireworks = SelectorTestsHelper.NonBestFireworks;

            IEnumerable<Firework> resultingFireworks = bestSelector.Select(SelectorTestsHelper.Fireworks);

            Assert.NotSame(expectedFireworks, resultingFireworks);
            Assert.NotEqual(expectedFireworks, resultingFireworks);
        }
    }
}