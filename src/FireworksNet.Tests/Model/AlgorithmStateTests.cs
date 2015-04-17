using System.Collections.Generic;
using Xunit;
using System;
using FireworksNet.Model;

namespace FireworksNet.Tests.Model
{
    public class AlgorithmStateTests
    {
        AlgorithmState algoritmState;
        public AlgorithmStateTests()
        {
            algoritmState = new AlgorithmState();
        }
        [Fact]
        public void FireworksSetter_NullValue_ExceptionThrown()
        {
            IEnumerable<Firework> param = null;

            string expectedParamName = "value";

            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => algoritmState.Fireworks = param);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }
        [Fact]
        public void StepNumberSetter_NegativeValue_ExceptionThrown()
        {   
            int stepNumber = -1;

            string expectedParamName = "value";

            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => algoritmState.StepNumber = stepNumber);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }
        [Fact]
        public void BestSolutionSetter_NullValue_ExceptionThrown()
        {
            Solution param = null;

            string expectedParamName = "value";

            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => algoritmState.BestSolution = param);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }
    }
}
