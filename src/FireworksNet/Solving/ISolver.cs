using System;
using FireworksNet.Model;

namespace FireworksNet.Solving
{
    /// <summary>
    /// Function root finder.
    /// </summary>
    public interface ISolver
    {
        /// <summary>
        /// Solves <paramref name="func"/> on a <paramref name="variationRange"/>.
        /// </summary>
        /// <param name="func">The function to find a root of.</param>
        /// <param name="variationRange">Represents an interval to find root on.</param>
        /// <returns>Root of the <paramref name="func"/> on <paramref name="variationRange"/>.</returns>
        double Solve(Func<double, double> func, Range variationRange);
    }
}