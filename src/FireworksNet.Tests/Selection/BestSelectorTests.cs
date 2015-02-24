using Xunit;
using NSubstitute;
using FireworksNet.Selection;
using System.Collections.Generic;
using FireworksNet.Model;
using System.Collections;
using System.Linq;

namespace FireworksNet.Tests.Selection
{
    public class BestSelectorTests
    {
        [Fact]
        public void Select_Equal()
        {
            BestSelector bestSelector = new BestSelector(this.GetBest, DataTestSelector.SamplingNumber);
            Assert.Equal(DataTestSelector.BestFireworks, bestSelector.Select(DataTestSelector.Fireworks, DataTestSelector.SamplingNumber));
        }

        [Fact]
        public void Select_NonEqual()
        {
            BestSelector bestSelector = new BestSelector(this.GetBest, DataTestSelector.SamplingNumber);
            Assert.NotEqual(DataTestSelector.NonBestFireworks, bestSelector.Select(DataTestSelector.Fireworks, DataTestSelector.SamplingNumber));
        }

        private Firework GetBest(IEnumerable<Firework> fireworks)
        {
            return fireworks.First<Firework>();
        }
    }
}
