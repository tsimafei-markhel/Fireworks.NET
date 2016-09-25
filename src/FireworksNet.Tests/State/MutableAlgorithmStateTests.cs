using System;
using System.Collections.Generic;
using System.Linq;
using FireworksNet.Model;
using FireworksNet.State;
using Xunit;

namespace FireworksNet.Tests.State
{
    public class MutableAlgorithmStateTests
    {
        [Fact]
        public void Constructor_NullFireworks_ExceptionThrown()
        {
            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => new MutableAlgorithmState(null, 0, new Solution(0.0)));
            Assert.Equal("fireworks", actualException.ParamName);
        }

        [Fact]
        public void Constructor_NegativeStepNumber_ExceptionThrown()
        {
            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => new MutableAlgorithmState(Enumerable.Empty<Firework>(), -1, new Solution(0.0)));
            Assert.Equal("stepNumber", actualException.ParamName);
        }

        [Fact]
        public void Constructor_NullBestSolution_ExceptionThrown()
        {
            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => new MutableAlgorithmState(Enumerable.Empty<Firework>(), 0, null));
            Assert.Equal("bestSolution", actualException.ParamName);
        }

        [Fact]
        public void UpdateState_NullFireworks_ExceptionThrown()
        {
            MutableAlgorithmState testState = new MutableAlgorithmState(Enumerable.Empty<Firework>(), 0, new Solution(0.0));

            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => testState.UpdateState(null, 0, new Solution(0.0)));
            Assert.Equal("fireworks", actualException.ParamName);
        }

        [Fact]
        public void UpdateState_NegativeStepNumber_ExceptionThrown()
        {
            MutableAlgorithmState testState = new MutableAlgorithmState(Enumerable.Empty<Firework>(), 0, new Solution(0.0));

            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => testState.UpdateState(Enumerable.Empty<Firework>(), -1, new Solution(0.0)));
            Assert.Equal("stepNumber", actualException.ParamName);
        }

        [Fact]
        public void UpdateState_NullBestSolution_ExceptionThrown()
        {
            MutableAlgorithmState testState = new MutableAlgorithmState(Enumerable.Empty<Firework>(), 0, new Solution(0.0));

            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => testState.UpdateState(Enumerable.Empty<Firework>(), 0, null));
            Assert.Equal("bestSolution", actualException.ParamName);
        }

        [Fact]
        public void UpdateState_ValidFireworks_FireworksUpdated()
        {
            IEnumerable<Firework> newFireworks = new List<Firework> { new Firework(FireworkType.Initial, 0) };
            MutableAlgorithmState testState = new MutableAlgorithmState(Enumerable.Empty<Firework>(), 0, new Solution(0.0));

            testState.UpdateState(newFireworks, 0, new Solution(0.0));

            Assert.Equal(newFireworks, testState.Fireworks);
        }

        [Fact]
        public void UpdateState_ValidStepNumber_StepNumberUpdated()
        {
            int newStepNumber = 2;
            MutableAlgorithmState testState = new MutableAlgorithmState(Enumerable.Empty<Firework>(), 0, new Solution(0.0));

            testState.UpdateState(Enumerable.Empty<Firework>(), newStepNumber, new Solution(0.0));

            Assert.Equal(newStepNumber, testState.StepNumber);
        }

        [Fact]
        public void UpdateState_ValidBestSolution_BestSolutionUpdated()
        {
            Solution newBestSolution = new Solution(1.0);
            MutableAlgorithmState testState = new MutableAlgorithmState(Enumerable.Empty<Firework>(), 0, new Solution(0.0));

            testState.UpdateState(Enumerable.Empty<Firework>(), 0, newBestSolution);

            Assert.Equal(newBestSolution, testState.BestSolution);
        }
    }
}