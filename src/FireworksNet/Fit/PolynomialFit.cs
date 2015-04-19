using System;
using MathNet.Numerics.LinearRegression;

namespace FireworksNet.Fit
{
    /// <summary>
    /// Approximation by polynomial function.
    /// </summary>
    public class PolynomialFit : IFit
    {
        private readonly int order;

        /// <summary>
        /// Initializes a new instance of the <see cref="PolynomialFit"/> class.
        /// </summary>
        /// <param name="order">The order of polinomial function.</param>
        public PolynomialFit(int order)
        {
            this.order = order;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual Func<double, double> Approximate(double[] fireworkCoordinates, double[] fireworkQualities)
        {
            if (fireworkCoordinates == null)
            {
                throw new ArgumentNullException("fireworkCoordinates");
            }

            if (fireworkQualities == null)
            {
                throw new ArgumentNullException("fireworkQualities");
            }

            return MathNet.Numerics.Fit.PolynomialFunc(fireworkCoordinates, fireworkQualities, this.order);
        }
    }
}
