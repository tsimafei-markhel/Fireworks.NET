using System;

namespace FireworksNet.Differentiation
{
    /// <summary>
    /// A Function differentiator.
    /// </summary>
    public class Differentiator : IDifferentiator
    {
        /// <summary>
        /// Differentiates <param name="func">.
        /// </summary>
        /// <param name="func">Function for differentiation.</param>
        /// <returns>Differentiated function of <param name="func"></returns>
        public virtual Func<double, double> Differentiate(Func<double, double> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("polynomialFunc");
            }

            return MathNet.Numerics.Differentiate.FirstDerivativeFunc(func);
        }
    }
}
