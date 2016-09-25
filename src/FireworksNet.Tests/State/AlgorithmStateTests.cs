using System;
using System.Linq;
using FireworksNet.Model;
using FireworksNet.State;
using Xunit;

namespace FireworksNet.Tests.State
{
    public class AlgorithmStateTests
    {
        [Fact]
        public void Constructor_NullFireworks_ExceptionThrown()
        {
            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => new AlgorithmState(null, 0, new Solution(0.0)));
            Assert.Equal("fireworks", actualException.ParamName);
        }

        [Fact]
        public void Constructor_NegativeValue_ExceptionThrown()
        {
            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => new AlgorithmState(Enumerable.Empty<Firework>(), -1, new Solution(0.0)));
            Assert.Equal("stepNumber", actualException.ParamName);
        }

        [Fact]
        public void Constructor_NullValue_ExceptionThrown()
        {
            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => new AlgorithmState(Enumerable.Empty<Firework>(), 0, null));
            Assert.Equal("bestSolution", actualException.ParamName);
        }
    }
}