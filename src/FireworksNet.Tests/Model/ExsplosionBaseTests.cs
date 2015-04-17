using System;
using System.Collections.Generic;
using FireworksNet.Model;
using Xunit;

namespace FireworksNet.Tests.Model
{
    public class ExplosionBaseTests
    {
        private class TestExplosionBase : ExplosionBase
        {
            public TestExplosionBase(int stepNumber, IDictionary<FireworkType, int> sparkCounts)
                : base(stepNumber, sparkCounts)
            {
            }
        }

        [Fact]
        public void ExplosionBase_NegativeAs1stParam_ExceptionThrown()
        {
            int stepNumber = -1;
            IDictionary<FireworkType, int> sparkCounts = new Dictionary<FireworkType, int>();
            string expectedParamName = "stepNumber";

            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => new TestExplosionBase(stepNumber, sparkCounts));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Fact]
        public void ExplosionBase_NegativeAs2ndParam_ExceptionThrown()
        {
            int stepNumber = 1;
            IDictionary<FireworkType, int> sparkCounts = null;
            string expectedParamName = "sparkCounts";

            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => new TestExplosionBase(stepNumber, sparkCounts));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }
    }
}