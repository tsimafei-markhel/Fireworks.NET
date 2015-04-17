using System;
using System.Collections.Generic;
using FireworksNet.Distances;
using FireworksNet.Model;
using Xunit;

namespace FireworksNet.Tests.Distances
{
    public class EuclideanDistanceTests
    {
        private readonly EuclideanDistance euclideanDistance1;
        private readonly IEnumerable<Dimension> dimensionList;
        private const int dimensionsCount = 4;
        private const double lowerLimit = 0;
        private const double upperLimit = 20.0;

        public EuclideanDistanceTests()
        {
            Range range = new Range(lowerLimit, upperLimit);
            this.dimensionList = new List<Dimension>(EuclideanDistanceTests.dimensionsCount);
            for (int i = 0; i < EuclideanDistanceTests.dimensionsCount; i++)
            {
                ((List<Dimension>)this.dimensionList).Add(new Dimension(range));
            }

            this.euclideanDistance1 = new EuclideanDistance(this.dimensionList);
        }

        [Fact]
        public void Calculate_CorrectValues_Calculated()
        {
            double[] first = { 0, 0, 0 };
            double[] second = { 0, 0, 0 };
            double[] first2 = { 5, 7, 8 };
            double[] second2 = { 5, 7, 8 };
            double[] first3 = { 6, 6, 8 };

            Assert.Equal(0, this.euclideanDistance1.Calculate(first, second), 5);
            Assert.Equal(0, this.euclideanDistance1.Calculate(first2, second2), 5);
            Assert.NotEqual(0, this.euclideanDistance1.Calculate(first3, second2), 2);
        }

        [Fact]
        public void Calculate_NegaviteFirstParam_ExceptionThrown()
        {
            double[] first = null;
            double[] second = { 0, 0, 0 };
            string expectedParamName = "first";

            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => this.euclideanDistance1.Calculate(first, second));
            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Fact]
        public void Calculate_NegaviteSecondParam_ExceptionThrown()
        {
            double[] first = { 0, 0, 0 };
            double[] second = null;
            string expectedParamName = "second";

            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => this.euclideanDistance1.Calculate(first, second));
            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Fact]
        public void Calculate_NegaviteSecondParamElementsCount_ExceptionThrown()
        {
            double[] first = { 5, 7, 8, 8 };
            double[] second = { 5, 7, 8 };
            string expectedParamName = "second";

            ArgumentException actualException = Assert.Throws<ArgumentException>(() => this.euclideanDistance1.Calculate(first, second));
            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }
    }
}