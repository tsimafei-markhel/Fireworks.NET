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
        [Fact]
        public void Select_Equal()
        {
            int samplingNumber = SelectorTestsHelper.SamplingNumber;
            IDistance distanceCalculator = Substitute.For<IDistance>();
            Func<IEnumerable<Firework>, Firework> getBest = SelectorTestsHelper.GetBest;
            Firework bestFirework = SelectorTestsHelper.FirstBestFirework;
            NearBestSelector selector = new NearBestSelector(distanceCalculator, getBest, samplingNumber);
            List<Firework> fireworks = new List<Firework>(SelectorTestsHelper.Fireworks);
            IEnumerable<Firework> expectedFireworks = SelectorTestsHelper.NearBestFireworks;

            for (int i = 1; i < 10; i++)
            {
                distanceCalculator.Calculate(bestFirework, fireworks[i]).Returns(i);
            }

            IEnumerable<Firework> resultingFireworks = selector.Select(fireworks);

            Assert.Equal(expectedFireworks, resultingFireworks);
        }

        [Fact]
        public void Select_NonEqual()
        {
            int samplingNumber = SelectorTestsHelper.SamplingNumber;
            IDistance distanceCalculator = Substitute.For<IDistance>();
            Func<IEnumerable<Firework>, Firework> getBest = SelectorTestsHelper.GetBest;
            Firework bestFirework = SelectorTestsHelper.FirstBestFirework;
            NearBestSelector selector = new NearBestSelector(distanceCalculator, getBest, samplingNumber);
            List<Firework> fireworks = new List<Firework>(SelectorTestsHelper.Fireworks);
            IEnumerable<Firework> expectedFireworks = SelectorTestsHelper.NonNearBestFirework;

            for (int i = 1; i < 10; i++)
            {
                distanceCalculator.Calculate(bestFirework, fireworks[i]).Returns(i);
            }

            IEnumerable<Firework> resultingFireworks = selector.Select(fireworks);

            Assert.NotEqual(expectedFireworks, resultingFireworks);
        }
    }
}