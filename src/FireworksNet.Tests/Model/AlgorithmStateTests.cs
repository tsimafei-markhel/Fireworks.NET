using System;
using System.Collections.Generic;
using FireworksNet.Model;
using Xunit;

namespace FireworksNet.Tests.Model
{
    public class AlgorithmStateTests
    {
        private readonly AlgorithmState algoritmState;

        public AlgorithmStateTests()
        {
            this.algoritmState = new AlgorithmState();
        }

        [Fact]
        public void FireworksSetter_NullValue_ExceptionThrown()
        {
            IEnumerable<Firework> param = null;

            string expectedParamName = "value";

            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => this.algoritmState.Fireworks = param);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Fact]
        public void StepNumberSetter_NegativeValue_ExceptionThrown()
        {
            int stepNumber = -1;

            string expectedParamName = "value";

            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => this.algoritmState.StepNumber = stepNumber);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Fact]
        public void BestSolutionSetter_NullValue_ExceptionThrown()
        {
            Solution param = null;

            string expectedParamName = "value";

            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => this.algoritmState.BestSolution = param);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }
    }
}