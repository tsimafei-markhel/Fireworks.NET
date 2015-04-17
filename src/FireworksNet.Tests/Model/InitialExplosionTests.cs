using System;
using FireworksNet.Model;
using Xunit;

namespace FireworksNet.Tests.Model
{
    public class InitialExplosionTests
    {
        [Fact]
        public void InitialExplosion_NegativeAs1stParam_ExceptionThrown()
        {
            int initialSparksNumber = -1;
            string expectedParamName = "initialSparksNumber";

            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => new InitialExplosion(initialSparksNumber));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }
    }
}