using System;
using System.Collections.Generic;
using FireworksNet.Explode;
using FireworksNet.Model;
using Xunit;

namespace FireworksNet.Tests.Explode
{
    public class ExploderTests
    {
        private readonly Exploder exploder;

        public ExploderTests()
        {
            this.exploder = new Exploder(new ExploderSettings());
        }

        [Fact]
        public void Exploder_NullAs1stParam_ExceptionThrown()
        {
            ExploderSettings settings = null;

            string expectedParamName = "settings";

            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => new Exploder(settings));
            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Fact]
        public void Explode_NullAs1stParam_ExceptionThrown()
        {
            Firework focus = null;
            IEnumerable<double> currentFireworkQualities = new List<double>();
            int currentStepNumber = 0;

            string expectedParamName = "focus";

            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => this.exploder.Explode(focus, currentFireworkQualities, currentStepNumber));
            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Fact]
        public void Explode_NullAs2ndParam_ExceptionThrown()
        {
            Firework focus = new Firework(FireworkType.Initial, 1);
            IEnumerable<double> currentFireworkQualities = null;
            int currentStepNumber = 0;

            string expectedParamName = "currentFireworkQualities";

            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => this.exploder.Explode(focus, currentFireworkQualities, currentStepNumber));
            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Fact]
        public void Explode_NegativeAs3rdParam_ExceptionThrown()
        {
            Firework focus = new Firework(FireworkType.Initial, 1);
            IEnumerable<double> currentFireworkQualities = new List<double>();
            int currentStepNumber = -1;

            string expectedParamName = "currentStepNumber";

            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => this.exploder.Explode(focus, currentFireworkQualities, currentStepNumber));
            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Fact]
        public void Explode_StepNumberLessThanFocusBirthStepNumber_ExceptionThrown()
        {
            Firework focus = new Firework(FireworkType.Initial, 1);
            IEnumerable<double> currentFireworkQualities = new List<double>();
            int currentStepNumber = 0;

            string expectedParamName = "currentStepNumber";

            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => this.exploder.Explode(focus, currentFireworkQualities, currentStepNumber));
            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }
    }
}