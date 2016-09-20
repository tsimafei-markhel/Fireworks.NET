using System;
using System.Collections.Generic;
using System.Linq;
using FireworksNet.Explode;
using FireworksNet.Model;
using FireworksNet.Selection;
using Xunit;

namespace FireworksNet.Tests.Explode
{
    public class ExploderTests
    {
        private readonly Exploder exploder;
        private readonly IFireworkSelector bestFireworkSelector;

        public ExploderTests()
        {
            this.bestFireworkSelector = new BestFireworkSelector(new Func<IEnumerable<Firework>, Firework>(ExploderTests.GetBest));
            this.exploder = new Exploder(new ExploderSettings(), this.bestFireworkSelector);
        }

        public static Firework GetBest(IEnumerable<Firework> fireworks)
        {
            return fireworks.OrderBy(fr => fr.Quality).First<Firework>();
        }

        [Fact]
        public void Constructor_NullExploderSettings_ExceptionThrown()
        {
            string expectedParamName = "settings";

            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => new Exploder(null, this.bestFireworkSelector));
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Fact]
        public void Constructor_NullBestFireworkSelector_ExceptionThrown()
        {
            string expectedParamName = "bestFireworkSelector";

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

            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => this.exploder.Explode(new Firework(FireworkType.Initial, 1), null, 0));
            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Fact]
        public void Explode_NegativeCurrentStepNumber_ExceptionThrown()
        {
            string expectedParamName = "currentStepNumber";

            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => this.exploder.Explode(new Firework(FireworkType.Initial, 1), Enumerable.Empty<Firework>(), -1));
            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Fact]
        public void Explode_StepNumberLessThanFocusBirthStepNumber_ExceptionThrown()
        {
            string expectedParamName = "currentStepNumber";

            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => this.exploder.Explode(new Firework(FireworkType.Initial, 1), Enumerable.Empty<Firework>(), 0));
            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }
    }
}