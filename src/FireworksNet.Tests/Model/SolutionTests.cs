using System.Collections.Generic;
using Xunit;
using System;
using Xunit.Extensions;
using Xunit.Sdk;
using FireworksNet.Model;

namespace FireworksNet.Tests.Model
{
    public class SolutionTests
    {

        private readonly static double quality1=1;
        private readonly static double quality2=2;
       public static IEnumerable<object[]> SolutionsData
        {
            get{
                Solution first = new Solution(quality1);
                return new[] {
                new object[] { first, new Solution(null, quality2), false  },
                new object[] { first, new Solution(new Dictionary<Dimension, double>(), quality1), true },
                new object[] { first, "badObject", false } 
                };
            }
        }



       [Theory, MemberData("SolutionsData")]
        public void Equals_SolutionsVariations_PositiveExpected(object sol1, object Obj, bool expected)
        {
            var actual = sol1.Equals(Obj);
            Assert.Equal(expected, actual);
        }

        [Theory, MemberData("SolutionsData")]
       public void ComparingOperators_SolutionsVariations_PositiveExpected(object sol1, object Obj, bool expected)
        {
            var actual = (sol1==Obj);
            Assert.Equal(expected, actual);

            var actual2 = !(sol1 != Obj);
            Assert.Equal(expected, actual);
        }


        [Theory, MemberData("SolutionsData")]
        public void GetHashCode_SolutionsVariations_PositiveExpected(object sol1, object Obj, bool expected)
        {          
            var actual=sol1.GetHashCode() == Obj.GetHashCode();
            Assert.Equal(expected, actual);
        }

    }
}
