using System;
using Xunit;
using FireworksNet.Model;
using FireworksNet.Solving;

namespace FireworksNet.Tests.Solving
{
    public class SolverTests
    {
        private readonly Solver solver;
        private readonly Func<double, double> targetFunc;
        private readonly Range range;

        public SolverTests()
        {
            this.solver = new Solver();
            this.targetFunc = x => x + x;
            this.range = new Range(-5, 5);
        }

        [Fact]
        public void Solve_PresentAllParam_ReturnsEqualRoot()
        {
            double expectedRoot = 0;

            double actualRoot = this.solver.Solve(this.targetFunc, range);

            Assert.Equal(expectedRoot, actualRoot);
        }

        [Fact]
        public void Solve_PresentAllParam_ReturnsNonEqualRoot()
        {
            double expectedRoot = 1;

            double actualRoot = this.solver.Solve(this.targetFunc, range);

            Assert.NotEqual(expectedRoot, actualRoot);
        }

        [Fact]
        public void Solve_NullAs1stParam_ExceptionThrown()
        {
            string expectedParamName = "polynomialFunc";
            Func<double, double> func = null;

            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => this.solver.Solve(func, this.range));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }

        [Fact]
        public void Solve_NullAs2ndParam_ExceptionThrown()
        {
            string expectedParamName = "variationRange";
            Range testRange = null;

            ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => this.solver.Solve(this.targetFunc, testRange));

            Assert.NotNull(actualException);
            Assert.Equal(expectedParamName, actualException.ParamName);
        }
    }
}
