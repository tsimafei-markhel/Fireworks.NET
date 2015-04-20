using System;

namespace FireworksNet.Fit
{
    /// <summary>
    /// Approximation a polynomial function.
    /// </summary>
    public class PolynomialFit : IFit
    {
        private readonly int order;

        /// <summary>
        /// Initializes a new instance of the <see cref="PolynomialFit"/> class.
        /// </summary>
        /// <param name="order">The order of polinomial function.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"> if <paramref name="order"/>
        /// is less than zero.</exception>
        public PolynomialFit(int order)
        {
            if (order < 0)
            {
                throw new ArgumentOutOfRangeException("order");
            }

            this.order = order;
        }

        /// <summary>
        /// Least-Squares fitting the points (x,y), where x-s represented as 
        /// <paramref name="fireworkCoordinates"/> and y-s represented as 
        /// <paramref name="fireworkQualities"/>.
        /// </summary>
        /// <param name="fireworkCoordinates">A firework coordinates.</param>
        /// <param name="fireworkQualities">A firework qualities.</param>
        /// <returns>Approximated polynomial function.</returns>
        /// <exception cref="System.ArgumentNullException"> if 
        /// <paramref name="fireworkCoordinates"/> and <paramref name="fireworkQualities"/>
        /// is <c>null</c>.</exception>
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
