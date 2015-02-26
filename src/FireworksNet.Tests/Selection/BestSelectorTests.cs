using System;
using System.Collections.Generic;
using Xunit;
using FireworksNet.Selection;
using FireworksNet.Model;

namespace FireworksNet.Tests.Selection
{
    public class BestSelectorTests
    {
        [Fact]
        public void Select_Equal()
        {
            int samplingNumber = SelectorTestsHelper.SamplingNumber;
            Func<IEnumerable<Firework>, Firework> getBest = SelectorTestsHelper.GetBest;
            IEnumerable<Firework> expectedFireworks = SelectorTestsHelper.BestFireworks;
            BestSelector bestSelector = new BestSelector(getBest, samplingNumber);

            IEnumerable<Firework> resultingFireworks = bestSelector.Select(SelectorTestsHelper.Fireworks);

            Assert.Equal(expectedFireworks, resultingFireworks);
        }

        [Fact]
        public void Select_NonEqual()
        {
            int samplingNumber = SelectorTestsHelper.SamplingNumber;
            Func<IEnumerable<Firework>, Firework> getBest = SelectorTestsHelper.GetBest;
            IEnumerable<Firework> expectedFireworks = SelectorTestsHelper.NonBestFireworks;
            BestSelector bestSelector = new BestSelector(getBest, samplingNumber);

            IEnumerable<Firework> resultingFireworks = bestSelector.Select(SelectorTestsHelper.Fireworks);

            Assert.NotEqual(expectedFireworks, resultingFireworks);
        }
    }
}