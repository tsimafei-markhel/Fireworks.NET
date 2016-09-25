using System;
using System.Collections.Generic;
using FireworksNet.Algorithm.Implementation;
using FireworksNet.Model;
using FireworksNet.Problems;
using FireworksNet.State;
using FireworksNet.StopConditions;
using Xunit;

namespace FireworksNet.Tests.Algorithm.Implementation
{
    public class FireworksAlgorithmTests
    {
        private static readonly TestProblem testProblem = InitTestProblem();
        private static readonly int positiveValue = 29;

        public class TestProblem : Problem
        {
            public TestProblem(IList<Dimension> dimensions, IDictionary<Dimension, Range> initialRanges, Func<IDictionary<Dimension, double>, double> targetFunction, ProblemTarget target)
                : base(dimensions, initialRanges, targetFunction, target)
            {
            }
        }

        public static IEnumerable<object[]> ProblemFireworksAlgorithmData
        {
            get
            {
                CounterStopCondition testStopCondition = new CounterStopCondition(1);
                FireworksAlgorithmSettings testFireworksAlgoritmSettings = new FireworksAlgorithmSettings();

                return new[] {
                    new object[] { null,        testStopCondition, testFireworksAlgoritmSettings, "problem"},
                    new object[] { testProblem, null,              testFireworksAlgoritmSettings, "stopCondition"},
                    new object[] { testProblem, testStopCondition, null,                          "settings"}

                };
            }
        }

        public static IEnumerable<object[]> FireworksAlgorithmData
        {
            get { return new[] { new object[] { positiveValue, 2 } }; }
        }

        public static TestProblem InitTestProblem()
        {
            // Init test problem
            IList<Dimension> dimensions = new List<Dimension>();
            IDictionary<Dimension, Range> initialRanges = new Dictionary<Dimension, Range>();
            Func<IDictionary<Dimension, double>, double> targetFunction =
                new Func<IDictionary<Dimension, double>, double>(
                    (c) =>
                    {
                        return positiveValue;
                    }
                );

            ProblemTarget target = ProblemTarget.Minimum;

            Range range = new Range(0, 1);
            dimensions.Add(new Dimension(range));
            initialRanges.Add(new KeyValuePair<Dimension, Range>(dimensions[0], range));

            return new TestProblem(dimensions, initialRanges, targetFunction, target);
        }

        private FireworksAlgorithm GetFireworksAlgorithm()
        {
            CounterStopCondition testStopCondition = new CounterStopCondition(1);
            testProblem.QualityCalculated += testStopCondition.IncrementCounter;

            FireworksAlgorithmSettings testFireworksAlgoritmSetting = new FireworksAlgorithmSettings
            {
                LocationsNumber = 1,
                ExplosionSparksNumberModifier = 1,
                ExplosionSparksNumberLowerBound = 0.04,
                ExplosionSparksNumberUpperBound = 0.8,
                ExplosionSparksMaximumAmplitude = 0.5,
                SpecificSparksNumber = 1,
                SpecificSparksPerExplosionNumber = 1
            };

            return new FireworksAlgorithm(testProblem, testStopCondition, testFireworksAlgoritmSetting);
        }

        [Theory]
        [MemberData("ProblemFireworksAlgorithmData")]
        public void Constructor_NegativeParams_ArgumentNullExceptionThrown(
            Problem problem,
            IStopCondition stopCondition,
            FireworksAlgorithmSettings settings,
            string expectedParamName)
        {
            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => new FireworksAlgorithm(problem, stopCondition, settings));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Theory]
        [InlineData(null, "state")]
        public void MakeStep_NegativeParams_ArgumentNullExceptionThrown(AlgorithmState state, string expectedParamName)
        {
            FireworksAlgorithm fireworksAlgorithm = this.GetFireworksAlgorithm();

            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => fireworksAlgorithm.MakeStep(state));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);

            testProblem.QualityCalculated -= ((CounterStopCondition)fireworksAlgorithm.StopCondition).IncrementCounter;
        }

        [Theory]
        [InlineData(null, "state")]
        public void GetSolution_NegativeParams_ArgumentNullExceptionThrown(AlgorithmState state, string expectedParamName)
        {
            FireworksAlgorithm fireworksAlgorithm = this.GetFireworksAlgorithm();

            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => fireworksAlgorithm.GetSolution(state));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);

            testProblem.QualityCalculated -= ((CounterStopCondition)fireworksAlgorithm.StopCondition).IncrementCounter;
        }

        [Theory]
        [InlineData(null, "state")]
        public void ShouldStop_NegativeParams_ArgumentNullExceptionThrown(AlgorithmState state, string expectedParamName)
        {
            FireworksAlgorithm fireworksAlgorithm = this.GetFireworksAlgorithm();

            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => fireworksAlgorithm.ShouldStop(state));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);

            testProblem.QualityCalculated -= ((CounterStopCondition)fireworksAlgorithm.StopCondition).IncrementCounter;
        }

        [Theory]
        [MemberData("FireworksAlgorithmData")]
        public void Solve_Calculation_PositiveExpected(double expectedValue, int precision)
        {
            FireworksAlgorithm fireworksAlgorithm = this.GetFireworksAlgorithm();
            double value = fireworksAlgorithm.Solve().Quality;

            Assert.Equal(expectedValue, value, precision);

            testProblem.QualityCalculated -= ((CounterStopCondition)fireworksAlgorithm.StopCondition).IncrementCounter;
        }
    }
}