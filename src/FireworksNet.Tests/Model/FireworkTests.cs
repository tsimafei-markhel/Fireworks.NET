using System.Collections.Generic;
using Xunit;
using System;
using FireworksNet.Model;

namespace FireworksNet.Tests.Model
{
   public class FireworkTests
    {
       [Fact]
       public void Firework_NullAs3tdParam_ExceptionThrown()
       {
           
           int birthSpetNumber = 1;
           FireworkType fireworkType = FireworkType.Initial;
           IDictionary<Dimension, double> coordinates = null;

           string expectedParamName = "coordinates";

           ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => new Firework(fireworkType,birthSpetNumber,coordinates));
           Assert.NotNull(actualException);
           Assert.Equal(expectedParamName, actualException.ParamName);
       }
       [Fact]
       public void Firework_NegaviteAs2ndParam_ExceptionThrown()
       {
           int birthSpetNumber = -1;
           FireworkType fireworkType = FireworkType.Initial;
           IDictionary<Dimension, double> coordinates = new Dictionary<Dimension,double>();

           string expectedParamName = "birthStepNumber";

           ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => new Firework(fireworkType, birthSpetNumber, coordinates));
           Assert.NotNull(actualException);
           Assert.Equal(expectedParamName, actualException.ParamName);
       }
    }
}
