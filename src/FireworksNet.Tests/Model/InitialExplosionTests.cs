using System.Collections.Generic;
using Xunit;
using System;
using FireworksNet.Model;

namespace FireworksNet.Tests.Model
{
    public class InitialExplosionTests
    {
        [Fact]
        public void InitialExplosion_NegativeInitialSparksNumberParam_ExceptionThrown()
        {

            int initialSparkaNumber = -1;

            string expectedParamName = "initialSparksNumber";

            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => new InitialExplosion(initialSparkaNumber));
            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }
    }
}
