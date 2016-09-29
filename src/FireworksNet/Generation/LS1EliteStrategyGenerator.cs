using System;
using System.Collections.Generic;
using FireworksNet.Fit;
using FireworksNet.Model;

namespace FireworksNet.Generation
{
    /// <summary>
    /// Elite strategy spark generator using first order functions, per 2012 paper.
    /// </summary>
    public class LS1EliteStrategyGenerator : EliteStrategyGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LS1EliteStrategyGenerator"/> class.
        /// </summary>
        /// <param name="dimensions">The dimensions to fit generated sparks into.</param>
        /// <param name="polynomialFit">The polynomial fit.</param>
        public LS1EliteStrategyGenerator(IEnumerable<Dimension> dimensions, IFit polynomialFit)
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
                throw new ArgumentNullException(nameof(func));
            }

            if (variationRange == null)
            {
                throw new ArgumentNullException(nameof(variationRange));
            }

            // TODO: Review of this logic.
            return (variationRange.Maximum - variationRange.Minimum) / 2 + variationRange.Minimum;
        }
    }
}
