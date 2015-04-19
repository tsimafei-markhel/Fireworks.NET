using System;
using System.Collections.Generic;
using FireworksNet.Model;
using Xunit;

namespace FireworksNet.Tests.Model
{
    public class FireworkExplosionTests
    {
        [Fact]
        public void FirewordExplosion_NullAs1stParam_ExceptionThrown()
        {
            Firework parent = null;
            int stepNumber = 1;
            double amplidute = 1.0;
            Dictionary<FireworkType, int> sparkCounts = new Dictionary<FireworkType, int>();
            string expectedParamName = "parentFirework";

            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => new FireworkExplosion(parent, stepNumber, amplidute, sparkCounts));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Fact]
        public void FirewordExplosion_NaNAs3tdParam_ExceptionThrown()
        {
            Firework parent = new Firework(FireworkType.Initial, 1);
            int stepNumber = 1;
            double amplidute = double.NaN;
            Dictionary<FireworkType, int> sparkCounts = new Dictionary<FireworkType, int>();
            string expectedParamName = "amplitude";

            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => new FireworkExplosion(parent, stepNumber, amplidute, sparkCounts));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Fact]
        public void FirewordExplosion_InfinityAs3tdParam_ExceptionThrown()
        {
            Firework parent = new Firework(FireworkType.Initial, 1);
            int stepNumber = 1;
            double amplidute = double.PositiveInfinity;
            Dictionary<FireworkType, int> sparkCounts = new Dictionary<FireworkType, int>();
            string expectedParamName = "amplitude";

            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => new FireworkExplosion(parent, stepNumber, amplidute, sparkCounts));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }
    }
}