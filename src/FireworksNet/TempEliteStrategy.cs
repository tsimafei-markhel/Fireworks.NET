using System;
using System.Collections.Generic;
using FireworksNet.Model;
using FireworksNet.Fit;
using FireworksNet.Solving;
using FireworksNet.Differentiation;
using FireworksNet.Generation;

namespace FireworksNet
{
    /// <summary>
    /// Elite strategy spark generator using first order functions, per 2012 paper.
    /// </summary>
    public class FirstOrderEliteStrategy : EliteStrategyGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FirstOrderEliteStrategy"/> class.
        /// </summary>
        /// <param name="dimensions">The dimensions to fit generated sparks into.</param>
        /// <param name="polynomialFit">The polynomial fit.</param>
        public FirstOrderEliteStrategy(IEnumerable<Dimension> dimensions, IFit polynomialFit)
            : base(dimensions, polynomialFit)
        {
        }

        /// <summary>
        /// Calculates elite point by searching midpoint.
        /// </summary>
        /// <param name="func">The function to calculate elite point.</param>
        /// <param name="variationRange">Represents an interval to calculate 
        /// elite point.</param>
        /// <returns>The coordinate of elite point.</returns>
        protected override double CalculateElitePoint(Func<double, double> func, Range variationRange)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }

            if (variationRange == null)
            {
                throw new ArgumentNullException("variationRange");
            }

            // TODO: Review of this logic.
            return (variationRange.Maximum - variationRange.Minimum) / 2 + variationRange.Minimum;
        }
    }

    /// <summary>
    /// Elite strategy spark generator using first order functions, per 2012 paper.
    /// </summary>
    public class SecondOrderEliteStrategy : EliteStrategyGenerator
    {
        private readonly IDifferentiator differentiation;
        private readonly ISolver solver;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecondOrderEliteStrategy"/> class.
        /// </summary>
        /// <param name="dimensions">The dimensions to fit generated sparks into.</param>
        /// <param name="polynomialFit">The polynomial fit.</param>
        /// <param name="differentiation">A function differentiator.</param>
        /// <param name="solver">A polynomial function solver.</param>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="differentiation"/>
        /// or <paramref name="solver"/> is <c>null</c>.
        /// </exception>
        public SecondOrderEliteStrategy(IEnumerable<Dimension> dimensions, IFit polynomialFit, IDifferentiator differentiation, ISolver solver)
            : base(dimensions, polynomialFit)
        {
            if (differentiation == null)
            {
                throw new ArgumentNullException("differentiation");
            }

            if (this.solver == null)
            {
                throw new ArgumentNullException("solver");
            }

            this.differentiation = differentiation;
            this.solver = solver;
        }

        /// <summary>
        /// Calculates elite point with help differentiating <paramref name="func"/> 
        /// and solving the resulting function.
        /// </summary>
        /// <param name="func">The function to calculate elite point.</param>
        /// <param name="variationRange">Represents an interval to calculate 
        /// elite point.</param>
        /// <returns>The coordinate of elite point.</returns>
        protected override double CalculateElitePoint(Func<double, double> func, Range variationRange)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }

            if (variationRange == null)
            {
                throw new ArgumentNullException("variationRange");
            }

            Func<double, double> derivative = this.differentiation.Differentiate(func);

            return solver.Solve(derivative, variationRange);
        }
    }
}
