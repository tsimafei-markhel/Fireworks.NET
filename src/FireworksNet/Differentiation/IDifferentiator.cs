using System;

namespace FireworksNet.Differentiation
{
    /// <summary>
    /// A function differentiator.
    /// </summary>
    public interface IDifferentiator
    {
        /// <summary>
        /// Differentiates <paramref name="func"/>.
        /// </summary>
        /// <param name="func">Function to find first derivative for.</param>
        /// <returns>First derivative of the <paramref name="func"/>.</returns>
        Func<double, double> Differentiate(Func<double, double> func);
    }
}