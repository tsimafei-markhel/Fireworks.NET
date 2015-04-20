using System;

namespace FireworksNet.Differentiation
{
    /// <summary>
    /// A Function differentiator.
    /// </summary>
    public interface IDifferentiator
    {
        /// <summary>
        /// Differentiates <param name="func">.
        /// </summary>
        /// <param name="func">Function for differentiation.</param>
        /// <returns>Differentiated function of <param name="func">.</returns>
        Func<double, double> Differentiate(Func<double, double> func);
    }
}
