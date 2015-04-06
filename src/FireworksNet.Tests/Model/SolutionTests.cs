using System.Collections.Generic;
using Xunit;
using System;
using FireworksNet.Model;

namespace FireworksNet.Tests.Model
{
    public class SolutionTests
    {
        [Fact]
        public void Equals_SolutionsVariations_PositiveExpected()
        {
            double quality1 = 1;
            double quality2 = 2;

            Solution solution1 = new Solution(quality1);
            Solution solution2 = new Solution(null,quality2);
            Solution solution3 = new Solution(new Dictionary<Dimension,double>(), quality1);
            Solution solution4 = new Solution(new Dictionary<Dimension,double>(), quality1);
            Object badObject = "badObject";
            Assert.True(solution3.Equals(solution3));

            Assert.False(solution1.Equals(solution2));
            Assert.False(solution2.Equals(solution3));
            Assert.False(solution1.Equals(solution3));
            Assert.False(solution1.Equals(badObject));
            Assert.False(solution1.Equals(null));
        }
        [Fact]
        public void ComparingOperators_SolutionsVariations_PositiveExpected()
        {
            double quality1 = 1;
            double quality2 = 2;

            Solution solution1 = new Solution(quality1);
            Solution solution2 = new Solution(null, quality2);
            Solution solution3 = new Solution(new Dictionary<Dimension, double>(), quality1);
            Solution solution4 = new Solution(new Dictionary<Dimension, double>(), quality1);
            Object badObject = "badObject";

            Assert.False(solution1 == solution2);
            Assert.False(solution1 == solution3);
            Assert.True(solution4 == solution3);
            Assert.False(solution1 == badObject);
            Assert.False(solution1 == null);

            Assert.True(solution1 != solution2);
            Assert.True(solution1 != solution3);
            Assert.False(solution4 != solution3);
            Assert.True(solution1 != badObject);
            Assert.True(solution1 != null);
        }
        [Fact]
        public void GetHashCode_SolutionsVariations_PositiveExpected()
        {
            double quality1 = 1;
            double quality2 = 2;

            Solution solution1 = new Solution(quality1);
            Solution solution2 = new Solution(null, quality2);
            Solution solution3 = new Solution(new Dictionary<Dimension, double>(), quality1);
            Solution solution4 = new Solution(new Dictionary<Dimension, double>(), quality1);
            Object badObject = "badObject";

            Assert.NotEqual(solution1.GetHashCode(), solution2.GetHashCode());
            Assert.NotEqual(solution1.GetHashCode(), solution3.GetHashCode());
            Assert.NotEqual(solution1.GetHashCode(), badObject.GetHashCode());
            Assert.Equal(solution3.GetHashCode(), solution4.GetHashCode());

        }

    }
}
