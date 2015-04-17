using System.Collections.Generic;
using FireworksNet.Model;
using Xunit;

namespace FireworksNet.Tests.Model
{
    public class SolutionTests
    {
        private const double quality1 = 1.0D;
        private const double quality2 = 2.0D;

        public static IEnumerable<object[]> SolutionsData
        {
            get
            {
                Solution first = new Solution(quality1);
                return new[] {
                    new object[] { first, new Solution(null, quality2), false },
                    new object[] { first, new Solution(new Dictionary<Dimension, double>(), quality1), false },
                    new object[] { first, "badObject", false }
                };
            }
        }

        [Theory]
        [MemberData("SolutionsData")]
        public void Equals_SolutionsVariations_PositiveExpected(object sol1, object obj, bool expected)
        {
            bool actual = sol1.Equals(obj);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData("SolutionsData")]
        public void ComparingOperators_SolutionsVariations_PositiveExpected(object sol1, object obj, bool expected)
        {
            bool actual = (sol1 == obj);
            bool actual2 = !(sol1 != obj);

            Assert.Equal(expected, actual);
            Assert.Equal(expected, actual2);
        }

        [Theory]
        [MemberData("SolutionsData")]
        public void GetHashCode_SolutionsVariations_PositiveExpected(object sol1, object obj, bool expected)
        {
            bool actual = (sol1.GetHashCode() == obj.GetHashCode());

            Assert.Equal(expected, actual);
        }
    }
}