using System;
using System.Collections.Generic;
using FireworksNet.Distances;
using Xunit;
using NSubstitute;
using FireworksNet.Model;
using FireworksNet.Selection;

namespace FireworksNet.Tests.Selection
{
    public class NearBestSelectorTests
    {
        private IDistance distanceCalculator;

        public NearBestSelectorTests()
        {
            IEnumerable<Dimension> dimensions = new List<Dimension>();
            this.distanceCalculator = new EuclideanDistance(dimensions);
        }
       
        [Fact]
        public void Select_Equal()
        {
            NearBestSelector selector = new NearBestSelector(this.distanceCalculator, this.GetBest, DataTestSelector.SamplingNumber);
            Assert.Equal(DataTestSelector.NearBestFireworks, selector.Select(DataTestSelector.Fireworks));
        }

        [Fact]
        public void Select_NonEqual()
        {
            NearBestSelector selector = new NearBestSelector(this.distanceCalculator, this.GetBest, DataTestSelector.SamplingNumber);
            Assert.NotEqual(DataTestSelector.NonNearBestFirework, selector.Select(DataTestSelector.Fireworks));
        }

        private Firework GetBest(IEnumerable<Firework> fireworks)
        {
            return DataTestSelector.OneBestFirework;
        }
    }
}
