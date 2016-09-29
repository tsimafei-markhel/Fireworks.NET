using System;

namespace FireworksNet.Fit
{
    /// <summary>
    /// Approximation of a function.
    /// </summary>
    public interface IFit
    {
        /// <summary>
        /// Approximates a function using <paramref name="argumentValues"/>
        /// and <paramref name="functionValues"/>.
        /// </summary>
        /// <param name="argumentValues">An array of function argument values.</param>
        /// <param name="functionValues">An array of function values that correspond
        /// to the <paramref name="argumentValues"/>.</param>
        /// <returns>Target function approximation.</returns>
        Func<double, double> Approximate(double[] argumentValues, double[] functionValues);
    }
}