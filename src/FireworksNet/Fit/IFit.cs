using System;

namespace FireworksNet.Fit
{
    /// <summary>
    /// Approximation by function.
    /// </summary>
    public interface IFit
    {
        /// <summary>
        /// Approximates fitness landscape using <param name="fireworkCoordinates">
        /// and <param name="fireworkQualities">.
        /// </summary>
        /// <param name="fireworkCoordinates">The coordinates of <see cref="Firework"/>s
        ///  in the current one dimensional search space.</param>
        /// <param name="fireworkQualities">The qualities of <see cref="Firework"/>s.</param>
        /// <returns>Approximated polynomial function.</returns>
        Func<double, double> Approximate(double[] fireworkCoordinates, double[] fireworkQualities);
    }
}
