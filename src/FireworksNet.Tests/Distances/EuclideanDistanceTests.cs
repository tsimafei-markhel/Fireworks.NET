using FireworksNet.Distances;
using FireworksNet.Model;
using System.Collections.Generic;
using Xunit;
using System;

namespace FireworksNet.Tests.Distances
{
    public class EuclideanDistanceTests
    {
        private readonly EuclideanDistance euclideDistance1;
        private readonly IEnumerable<Dimension> dimensionList;
        private readonly int dimensionsCount;
        private readonly double LowerLimit = 0;
        private readonly double UpperLimit = 20.0;

        public EuclideanDistanceTests()
        {
            Range range = new Range(LowerLimit, UpperLimit);
            dimensionsCount = 4;
            dimensionList=new List<Dimension>();
            for (int i = 0; i < dimensionsCount; i++)
                ((List<Dimension>)dimensionList).Add(new Dimension(range));
            euclideDistance1 = new EuclideanDistance(dimensionList);
        }

        [Fact]
        public void Calculate_PositiveValues_Calculated()
        {
            double[] first={0,0,0};
            double[] second={0,0,0};
            double[] first2 = { 5, 7, 8 };
            double[] second2 = { 5, 7, 8 };
            double[] first3 = { 6, 6, 8 };

            Assert.Equal(0, euclideDistance1.Calculate(first, second), 5);
            Assert.Equal(0, euclideDistance1.Calculate(first2, second2), 5);
            Assert.NotEqual(0, euclideDistance1.Calculate(first3, second2), 2);
        }
      
        [Fact]
        public void Calculate_NegaviteSecondParamElementsCount_ExceptionThrown()
        {
            double[] first = { 5, 7, 8 ,8};
            double[] second = { 5, 7, 8 };
            string expectedParamName = "second";

            ArgumentException actualException = Assert.Throws<ArgumentException>(() => euclideDistance1.Calculate(first, second));
            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }
        [Fact]
        public void Calculate_NegaviteFirstParam_ExceptionThrown()
        {
            double[] first = null;
            double[] second = { 0, 0, 0};
            string expectedParamName = "first";

            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => euclideDistance1.Calculate(first, second));
            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }
        [Fact]
        public void Calculate_NegaviteSecondParam_ExceptionThrown()
        {
            double[] first = { 0, 0, 0 };
            double[] second = null;
            string expectedParamName = "second";

            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => euclideDistance1.Calculate(first, second));
            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }
    }
}
