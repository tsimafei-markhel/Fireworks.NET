using System;
using System.Linq;
using FireworksNet.Explode;
using FireworksNet.Model;
using FireworksNet.Problems;
using FireworksNet.Selection.Extremum;
using Xunit;

namespace FireworksNet.Tests.Explode
{
    public class ExploderTests
    {
        private readonly Exploder exploder;
        private readonly IExtremumFireworkSelector extremumFireworkSelector;

        public ExploderTests()
        {
            this.extremumFireworkSelector = new ExtremumFireworkSelector(ProblemTarget.Minimum);
            this.exploder = new Exploder(new ExploderSettings(), this.extremumFireworkSelector);
        }

        [Fact]
        public void Constructor_NullExploderSettings_ExceptionThrown()
        {
            string expectedParamName = "settings";

            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => new Exploder(null, this.extremumFireworkSelector));
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Fact]
        public void Constructor_NullBestFireworkSelector_ExceptionThrown()
        {
            string expectedParamName = "extremumFireworkSelector";

            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => new Exploder(new ExploderSettings(), null));
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Fact]
        public void Explode_NullFocus_ExceptionThrown()
        {
            string expectedParamName = "focus";

            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => this.exploder.Explode(null, Enumerable.Empty<Firework>(), 0));
            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Fact]
        public void Explode_NullCurrentFireworks_ExceptionThrown()
        {
            string expectedParamName = "currentFireworks";

            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => this.exploder.Explode(new Firework(FireworkType.Initial, 1, 0), null, 0));
            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Fact]
        public void Explode_NegativeCurrentStepNumber_ExceptionThrown()
        {
            string expectedParamName = "currentStepNumber";

            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => this.exploder.Explode(new Firework(FireworkType.Initial, 1, 0), Enumerable.Empty<Firework>(), -1));
            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Fact]
        public void Explode_StepNumberLessThanFocusBirthStepNumber_ExceptionThrown()
        {
            string expectedParamName = "currentStepNumber";

            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => this.exploder.Explode(new Firework(FireworkType.Initial, 1, 0), Enumerable.Empty<Firework>(), 0));
            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }
    }
}