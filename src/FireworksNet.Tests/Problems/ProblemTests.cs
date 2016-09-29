using System;
using System.Collections.Generic;
using FireworksNet.Model;
using FireworksNet.Problems;
using Xunit;

namespace FireworksNet.Tests.Problems
{
    public class ProblemTests
    {
        #region test data

        public static TestProblem getTestProblem()
        {
            IList<Dimension> dimensions = new List<Dimension>();
            IDictionary<Dimension, Range> initialRanges = new Dictionary<Dimension, Range>();
            Func<IDictionary<Dimension, double>, double> targetFunction =
                new Func<IDictionary<Dimension, double>, double>(
                    (c) =>
                    {
                        return 29;
                    }
                );
            ProblemTarget target = ProblemTarget.Minimum;

            Range range = new Range(0, 1);
            dimensions.Add(new Dimension(range));
            initialRanges.Add(new KeyValuePair<Dimension, Range>(dimensions[0], range));

            TestProblem testProblem = new TestProblem(dimensions, initialRanges, targetFunction, target);

            return testProblem;

        }

        public static IEnumerable<object[]> ProblemDataOfArgumentNullExceptionTrown
        {
            get
            {
                IList<Dimension> dimensions = new List<Dimension>();
                IDictionary<Dimension, Range> initialRanges = new Dictionary<Dimension, Range>();
                Func<IDictionary<Dimension, double>, double> targetFunction =
                    new Func<IDictionary<Dimension, double>, double>(
                        (c) =>
                        {
                            return 29;
                        }
                    );
                ProblemTarget target = ProblemTarget.Minimum;

                Range range = new Range(0, 1);
                dimensions.Add(new Dimension(range));
                initialRanges.Add(new KeyValuePair<Dimension, Range>(dimensions[0], range));

                return new[] {
                    new object[] { null,       initialRanges, targetFunction, target, "dimensions"},
                    new object[] { dimensions, null,          targetFunction, target, "initialRanges"},
                    new object[] { dimensions, initialRanges, null,           target, "targetFunction"}
                };
            }
        }
        public static IEnumerable<object[]> ProblemDataOfArgumentExceptionTrown
        {
            get
            {
                IList<Dimension> dimensions = new List<Dimension>();
                IDictionary<Dimension, Range> initialRanges = new Dictionary<Dimension, Range>();
                Func<IDictionary<Dimension, double>, double> targetFunction =
                    new Func<IDictionary<Dimension, double>, double>(
                        (c) =>
                        {
                            return 29;
                        }
                    );
                ProblemTarget target = ProblemTarget.Minimum;

                Range range = new Range(0, 1);
                dimensions.Add(new Dimension(range));
                initialRanges.Add(new KeyValuePair<Dimension, Range>(dimensions[0], range));

                IDictionary<Dimension, Range> badRanges = new Dictionary<Dimension, Range>();

                Range range2 = new Range(2, 3);
                badRanges.Add(new KeyValuePair<Dimension, Range>(new Dimension(range2), range2));

                return new[] {
                    new object[] { new List<Dimension>(), initialRanges,                      targetFunction, target, "dimensions"},
                    new object[] { dimensions,            new Dictionary<Dimension, Range>(), targetFunction, target, "initialRanges"},
                    new object[] { dimensions,            badRanges,                          targetFunction, target, "initialRanges"}
                };
            }
        }
        #endregion

        public class TestProblem : Problem
        {
            public TestProblem(IList<Dimension> dimensions, IDictionary<Dimension, Range> initialRanges, Func<IDictionary<Dimension, double>, double> targetFunction, ProblemTarget target)
                : base(dimensions, initialRanges, targetFunction, target)
            {

            }
        }

        [Theory, MemberData("ProblemDataOfArgumentNullExceptionTrown")]
        public void Problem_NegativeParams_ArgumentNullExceptionThrown(IList<Dimension> dimensions,
            IDictionary<Dimension,
            Range> initialRanges,
            Func<IDictionary<Dimension, double>, double> targetFunction,
            ProblemTarget target,
            string expectedParamName)
        {
            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => new TestProblem(dimensions, initialRanges, targetFunction, target));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Theory]
        [InlineData(null, "coordinateValues")]
        public void CalculateQuality_NegativeParams_ArgumentNullExceptionTrown(IDictionary<Dimension, double> coordinateValues, string expectedParamName)
        {
            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => getTestProblem().CalculateQuality(coordinateValues));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Theory]
        [InlineData(null, "dimensions")]
        public void CreateDefaultInitialRanges_NegativeParams_ArgumentNullExceptionTrown(IList<Dimension> dimensions, string expectedParamName)
        {
            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => Problem.CreateDefaultInitialRanges(dimensions));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }
    }
}