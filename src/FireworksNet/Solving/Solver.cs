using System;
using FireworksNet.Model;

namespace FireworksNet.Solving
{
    /// <summary>
    /// Function root finder.
    /// </summary>
    public class Solver : ISolver
    {
        /// <summary>
        /// Solves <paramref name="func"/> on a <paramref name="variationRange"/>.
        /// </summary>
        /// <param name="func">The function to find a root of.</param>
        /// <param name="variationRange">Represents an interval to find root on.</param>
        /// <returns>Root of the <paramref name="func"/> on <paramref name="variationRange"/>.</returns>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="func"/>
        /// or <paramref name="variationRange"/> is <c>null</c>.</exception>
        public double Solve(Func<double, double> func, Range variationRange)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            if (variationRange == null)
            {
                throw new ArgumentNullException(nameof(variationRange));
            }

            return MathNet.Numerics.FindRoots.OfFunction(func, variationRange.Minimum, variationRange.Maximum);
        }
    }
}