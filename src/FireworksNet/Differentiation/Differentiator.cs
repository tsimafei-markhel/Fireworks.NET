using System;

namespace FireworksNet.Differentiation
{
    /// <summary>
    /// A function differentiator.
    /// </summary>
    public class Differentiator : IDifferentiator
    {
        /// <summary>
        /// Differentiates <paramref name="func"/>.
        /// </summary>
        /// <param name="func">Function to find first derivative for.</param>
        /// <returns>First derivative of the <paramref name="func"/>.</returns>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="func"/>
        /// is <c>null</c>.</exception>
        public virtual Func<double, double> Differentiate(Func<double, double> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return MathNet.Numerics.Differentiate.FirstDerivativeFunc(func);
        }
    }
}