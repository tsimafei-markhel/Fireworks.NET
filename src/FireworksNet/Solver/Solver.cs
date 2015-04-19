using System;
using MathNet.Numerics.RootFinding;
using MathNet.Numerics;
using FireworksNet.Model;

namespace FireworksNet.Solver
{
    public class Solver : ISolver
    {
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
            //return Brent.FindRoot(polynomialFunc, variationRange.Minimum, variationRange.Maximum);
        }
    }
}
