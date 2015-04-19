using System;
using MathNet.Numerics;
using FireworksNet.Model;

namespace FireworksNet.Solver
{
    /// <summary>
    /// A polynomial function solver.
    /// </summary>
    public class Solver : ISolver
    {
        /// <summary>
        /// Solves <param name="polynomialFunc"> on <param name="variationRange">.
        /// </summary>
        /// <param name="polynomialFunc">A polynomial function to solve.</param>
        /// <param name="variationRange">Represents an interval of finding root.</param>
        /// <returns>Root of <param name="polynomialFunc"> on 
        /// <param name="variationRange">.</returns>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="polynomialFunc"/>
        /// and <paramref name="variationRange"/> is <c>null</c>.</exception>
        public virtual double Solve(Func<double, double> polynomialFunc, Range variationRange)
        {
            if (polynomialFunc == null)
            {
                throw new ArgumentNullException("polynomialFunc");
            }

            if (variationRange == null)
            {
                throw new ArgumentNullException("variationRange");
            }

            return FindRoots.OfFunction(polynomialFunc, variationRange.Minimum, variationRange.Maximum);         
        }
    }
}
