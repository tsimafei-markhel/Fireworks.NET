using System;
using System.Linq;
using FireworksNet.Model;
using FireworksNet.State;
using Xunit;

namespace FireworksNet.Tests.State
{
    public class AlgorithmStateTests
    {
        private readonly AlgorithmState state;

        public AlgorithmStateTests()
        {
            this.state = new AlgorithmState(Enumerable.Empty<Firework>(), 0, new Solution(0.0));
        }

        [Fact]
        public void Constructor_NullFireworks_ExceptionThrown()
        {
            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => new AlgorithmState(null, 0, new Solution(0.0)));
            Assert.Equal("value", actualException.ParamName);
        }

        [Fact]
        public void Constructor_NegativeValue_ExceptionThrown()
        {
            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => new AlgorithmState(Enumerable.Empty<Firework>(), -1, new Solution(0.0)));
            Assert.Equal("value", actualException.ParamName);
        }

        [Fact]
        public void Constructor_NullValue_ExceptionThrown()
        {
            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => new AlgorithmState(Enumerable.Empty<Firework>(), 0, null));
            Assert.Equal("value", actualException.ParamName);
        }

        [Fact]
        public void FireworksSetter_NullFireworks_ExceptionThrown()
        {
            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => this.state.Fireworks = null);
            Assert.Equal("value", actualException.ParamName);
        }

        [Fact]
        public void StepNumberSetter_NegativeValue_ExceptionThrown()
        {
            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => this.state.StepNumber = -1);
            Assert.Equal("value", actualException.ParamName);
        }

        [Fact]
        public void BestSolutionSetter_NullValue_ExceptionThrown()
        {
            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => this.state.BestSolution = null);
            Assert.Equal("value", actualException.ParamName);
        }
    }
}