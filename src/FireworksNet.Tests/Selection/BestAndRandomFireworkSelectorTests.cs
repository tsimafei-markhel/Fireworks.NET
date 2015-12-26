using System;
using System.Collections.Generic;
using FireworksNet.Distances;
using FireworksNet.Model;
using FireworksNet.Selection;
using NSubstitute;
using Xunit;

namespace FireworksNet.Tests.Selection
{
    public class BestAndRandomFireworkSelectorTests
    {
        #region TestData
        private static readonly Func<IEnumerable<Firework>, Firework> getBest = SelectorTestsHelper.GetBest;
        private static readonly IDistance distanceCalculator = Substitute.For<IDistance>();
        private static readonly int samplingNumber = SelectorTestsHelper.SamplingNumber;
        private static readonly System.Random randomizer = new System.Random();

        public static IEnumerable<object[]> ProblemData
        {
            get
            {

                Func<IEnumerable<Firework>, Firework> best = getBest;
                int samplingNumberParam = samplingNumber;
                System.Random randomizerParam = randomizer;

                return new[] {
                    new object[] { null,           best,  samplingNumberParam , "randomizer"},
                    new object[] {randomizerParam, null,  samplingNumberParam , "bestFireworkSelector"}
                };
            }
        }
        public static IEnumerable<object[]> ProblemData2
        {
            get
            {
                Func<IEnumerable<Firework>, Firework> best = getBest;
                int samplingNumberParam = -1;
                System.Random randomizerParam = randomizer;

                return new[] {
                    new object[] { randomizerParam,best,  samplingNumberParam , "locationsNumber"}
                };
            }
        }
        #endregion

        [Theory, MemberData("ProblemData")]
        public void BestAndRandomFireworkSelector_NegativeParams_ArgumentNullExceptionThrown(System.Random randomizer,
            Func<IEnumerable<Firework>, Firework> bestFireworkSelector,
            int locationsNumber,
            string expectedParamName)
        {
            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => new BestAndRandomFireworkSelector(randomizer, bestFireworkSelector, locationsNumber));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Theory, MemberData("ProblemData2")]
        public void BestAndRandomFireworkSelector_Negative3rdParams_ArgumentOutOfRangeExceptionThrown(System.Random randomizer,
            Func<IEnumerable<Firework>, Firework> bestFireworkSelector,
            int locationsNumber,
            string expectedParamName)
        {
            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => new BestAndRandomFireworkSelector(randomizer, bestFireworkSelector, locationsNumber));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }
    }
}