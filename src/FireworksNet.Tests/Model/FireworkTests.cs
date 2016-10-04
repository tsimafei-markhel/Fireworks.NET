using System;
using System.Collections.Generic;
using FireworksNet.Model;
using Xunit;

namespace FireworksNet.Tests.Model
{
    public class FireworkTests
    {
        #region Constructor tests

        [Fact]
        public void Constructor_NegativeBirthStepNumber_ExceptionThrown()
        {
            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => new Firework(FireworkType.Initial, -1, 0, new Dictionary<Dimension, double>()));

            Assert.NotNull(actualException);
            Assert.Equal("birthStepNumber", actualException.ParamName);
        }

        [Fact]
        public void Constructor_NegativeBirthOrder_ExceptionThrown()
        {
            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => new Firework(FireworkType.Initial, 0, -1, new Dictionary<Dimension, double>()));

            Assert.NotNull(actualException);
            Assert.Equal("birthOrder", actualException.ParamName);
        }

        [Fact]
        public void Constructor_NullCoordinates_ExceptionThrown()
        {
            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => new Firework(FireworkType.Initial, 0, 0, null));

            Assert.NotNull(actualException);
            Assert.Equal("coordinates", actualException.ParamName);
        }

        [Fact]
        public void Constructor_ValidFireworkType_SetsPassedFireworkType()
        {
            FireworkType expectedType = FireworkType.Initial;

            Firework result = new Firework(expectedType, 0, 0, new Dictionary<Dimension, double>());

            Assert.Equal(expectedType, result.FireworkType);
        }

        [Fact]
        public void Constructor_ValidBirthStepNumber_SetsPassedBirthStepNumber()
        {
            int expectedBirthStepNumber = 10;

            Firework result = new Firework(FireworkType.Initial, expectedBirthStepNumber, 0, new Dictionary<Dimension, double>());

            Assert.Equal(expectedBirthStepNumber, result.BirthStepNumber);
        }

        [Fact]
        public void Constructor_ValidBirthOrder_SetsPassedBirthOrder()
        {
            int expectedBirthOrder = 2;

            Firework result = new Firework(FireworkType.Initial, 0, expectedBirthOrder, new Dictionary<Dimension, double>());

            Assert.Equal(expectedBirthOrder, result.BirthOrder);
        }

        [Fact]
        public void Constructor_ValidCoordinates_SetsPassedCoordinates()
        {
            Dictionary<Dimension, double> expectedCoordinates = new Dictionary<Dimension, double>
            {
                { new Dimension(new Range(-10.0, 20.0)), 10.0 },
                { new Dimension(new Range(50.0, 120.0)), 67.85 }
            };

            Firework result = new Firework(FireworkType.Initial, 0, 0, expectedCoordinates);

            Assert.Equal(expectedCoordinates, result.Coordinates);
        }

        #endregion

        #region Constructor2 tests

        [Fact]
        public void Constructor2_NegativeBirthStepNumber_ExceptionThrown()
        {
            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => new Firework(FireworkType.Initial, -1, 0));

            Assert.NotNull(actualException);
            Assert.Equal("birthStepNumber", actualException.ParamName);
        }

        [Fact]
        public void Constructor2_NegativeBirthOrder_ExceptionThrown()
        {
            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => new Firework(FireworkType.Initial, 0, -1));

            Assert.NotNull(actualException);
            Assert.Equal("birthOrder", actualException.ParamName);
        }

        [Fact]
        public void Constructor2_ValidFireworkType_SetsPassedFireworkType()
        {
            FireworkType expectedType = FireworkType.Initial;

            Firework result = new Firework(expectedType, 0, 0);

            Assert.Equal(expectedType, result.FireworkType);
        }

        [Fact]
        public void Constructor2_ValidBirthStepNumber_SetsPassedBirthStepNumber()
        {
            int expectedBirthStepNumber = 10;

            Firework result = new Firework(FireworkType.Initial, expectedBirthStepNumber, 0);

            Assert.Equal(expectedBirthStepNumber, result.BirthStepNumber);
        }

        [Fact]
        public void Constructor2_ValidBirthOrder_SetsPassedBirthOrder()
        {
            int expectedBirthOrder = 2;

            Firework result = new Firework(FireworkType.Initial, 0, expectedBirthOrder);

            Assert.Equal(expectedBirthOrder, result.BirthOrder);
        }

        [Fact]
        public void Constructor2_ValidArguments_SetsEmptyCoordinates()
        {
            Firework result = new Firework(FireworkType.Initial, 0, 0);

            Assert.NotNull(result.Coordinates);
            Assert.Equal(0, result.Coordinates.Count);
        }

        #endregion
    }
}