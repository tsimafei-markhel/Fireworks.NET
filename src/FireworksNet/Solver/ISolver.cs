using System;
using FireworksNet.Model;

namespace FireworksNet.Solver
{
    /// <summary>
    /// Contains logic for finding root of polynomial function.
    /// </summary>
    public interface ISolver
    {
        /// <summary>
        /// Solves <param name="polynomialFunc"> on <param name="variationRange">.
        /// </summary>
        /// <param name="polynomialFunc">Polynomial function to finding root.</param>
        /// <param name="variationRange">Represents an interval of finding root.</param>
        /// <returns>Root of <param name="polynomialFunc"> on 
        /// <param name="variationRange">.</returns>
        double Solve(Func<double, double> polynomialFunc, Range variationRange);
    }
}
