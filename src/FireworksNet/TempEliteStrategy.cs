using System;
using System.Collections.Generic;
using MathNet.Numerics;
using MathNet.Numerics.LinearRegression;
using FireworksNet.Model;
using FireworksNet.Problems;
using MathNet.Numerics.RootFinding;
using System.Diagnostics;

namespace FireworksNet
{
    /// <summary>
    /// Approximation by polynomial function.
    /// </summary>
    public interface IFit
    {
        /// <summary>
        /// Approximates fitness landscape.
        /// </summary>
        /// <param name="fireworkCoordinates">The coordinates of fireworks in 
        /// the current one dimensional search space.</param>
        /// <param name="fireworkQualities">The qualities of fireworks.</param>
        /// <returns>Approximated polynomial.</returns>
        Func<double, double> Approximate(double[] fireworkCoordinates, double[] fireworkQualities);
    }

    public class PolynomialFit : IFit
    {
        private readonly int order;

        public PolynomialFit(int order)
        {
            this.order = order;
        }

        public virtual Func<double, double> Approximate(double[] fireworkCoordinates, double[] fireworkQualities)
        {
            if (fireworkQualities == null)
            {
                throw new ArgumentNullException("fireworkQualities");
            }

            if (fireworkCoordinates == null)
            {
                throw new ArgumentNullException("fireworkCoordinates");
            }

            return Fit.PolynomialFunc(fireworkCoordinates, fireworkQualities, this.order, DirectRegressionMethod.QR);
        }
    }

    public interface IDifferentiation
    {
        Func<double, double> Differentiate(Func<double, double> polynomialFunc);
    }

    public class PolynomialDifferentiation : IDifferentiation
    {
        public virtual Func<double, double> Differentiate(Func<double, double> polynomialFunc)
        {
            if (polynomialFunc == null)
            {
                throw new ArgumentNullException("polynomialFunc");
            }

            return MathNet.Numerics.Differentiate.FirstDerivativeFunc(polynomialFunc);
        }
    }

    public interface IPolynomialSolver
    {
        double Solve(Func<double, double> polynomialFunc, double lowerBound, double upperBound);
    }

    public class SecondOrderPolynomialSolver : IPolynomialSolver
    {
        public virtual double Solve(Func<double, double> polynomialFunc, double lowerBound, double upperBound)
        {
            if (polynomialFunc == null)
            {
                throw new ArgumentNullException("polynomialFunc");
            }

            return Brent.FindRoot(polynomialFunc, lowerBound, upperBound);
        }
    }

    public class FirstOrderEliteStrategy : TempEliteStrategy
    {
        public FirstOrderEliteStrategy(IFit polynomialFit, IEnumerable<Dimension> dimensions)
            : base(polynomialFit, dimensions)
        {
        }

        public override double SelectElitePoint(Func<double, double> polynomialFunc, double lowerBound, double upperBound)
        {
            if (polynomialFunc == null)
            {
                throw new ArgumentNullException("polynomialFunc");
            }

            if (lowerBound == upperBound)
            {
                return lowerBound;
            }

            Debug.Assert(lowerBound > upperBound, "Lower bound more than upper bound.");

            // TODO: Review of this logic.
            return (upperBound - lowerBound) / 2 + lowerBound;
        }
    }

    public class SecondOrderEliteStrategy : TempEliteStrategy
    {
        private readonly IDifferentiation polynomialDifferentiation;
        private readonly IPolynomialSolver polynomialSolver;

        public SecondOrderEliteStrategy(IFit polynomialFit, IEnumerable<Dimension> dimensions, IDifferentiation polynomialDifferentiation, IPolynomialSolver polynomialSolver)
            : base(polynomialFit, dimensions)
        {
            if (polynomialDifferentiation == null)
            {
                throw new ArgumentNullException("polynomialDifferentiation");
            }

            if (this.polynomialSolver == null)
            {
                throw new ArgumentNullException("polynomialSolver");
            }

            this.polynomialSolver = polynomialSolver;
            this.polynomialDifferentiation = polynomialDifferentiation;
        }

        public override double SelectElitePoint(Func<double, double> polynomialFunc, double lowerBound, double upperBound)
        {
            if (polynomialFunc == null)
            {
                throw new ArgumentNullException("polynomialFunc");
            }

            if (lowerBound == upperBound)
            {
                return lowerBound;
            }

            Debug.Assert(lowerBound > upperBound, "Lower bound more than upper bound.");

            Func<double, double> derivative = this.polynomialDifferentiation.Differentiate(polynomialFunc);

            return polynomialSolver.Solve(derivative, lowerBound, upperBound);
        }
    }

    public abstract class TempEliteStrategy
    {
        private readonly IEnumerable<Dimension> dimensions;
        private readonly IFit polynomialFit;

        public FireworkType ElitePointType { get { return FireworkType.SpecificSpark; } }

        protected TempEliteStrategy(IFit polynomialFit, IEnumerable<Dimension> dimensions)
        {
            if (polynomialFit == null)
            {
                throw new ArgumentNullException("polynomialFit");
            }

            if (dimensions == null)
            {
                throw new ArgumentNullException("dimensions");
            }

            this.polynomialFit = polynomialFit;
            this.dimensions = dimensions;
        }

        public abstract double SelectElitePoint(Func<double, double> polynomialFunc, double lowerBound, double upperBound);

        public Firework GetFirework(IEnumerable<Firework> from, int birthStepNumber)
        {
            if (from == null)
            {
                throw new ArgumentNullException("from");
            }

            if (birthStepNumber < 0)
            {
                throw new ArgumentOutOfRangeException("birthStepNumber");
            }

            List<Firework> currentFireworks = new List<Firework>(from);

            //11. Approximate fitness landscape in each projected one dimensional search space
            double[] qualities = new double[currentFireworks.Count];

            //Get qualities of the fireworks
            int current = 0;
            foreach (Firework firework in currentFireworks)
            {
                qualities[current] = firework.Quality;
                current++;
            }

            Dictionary<Dimension, Func<double, double>> fitnessLandscapes = new Dictionary<Dimension, Func<double, double>>();

            foreach (Dimension dimension in this.dimensions)
            {
                double[] coordinates = new double[currentFireworks.Count];

                //Get coordinates of the fireworks in current dimension
                current = 0;
                foreach (Firework firework in currentFireworks)
                {
                    coordinates[current] = firework.Coordinates[dimension];
                    current++;
                }

                Func<double, double> polynomial = this.polynomialFit.Approximate(coordinates, qualities);
                fitnessLandscapes[dimension] = polynomial;
            }

            //12. Obtain a spark from approximated curves by Elite Strategy

            Dictionary<Dimension, double> coordinatesElitePoint = new Dictionary<Dimension, double>();

            foreach (KeyValuePair<Dimension, Func<double, double>> data in fitnessLandscapes)
            {
                double lowerBound = data.Key.VariationRange.Minimum;
                double upperBound = data.Key.VariationRange.Maximum;
                double elitePoint = this.SelectElitePoint(data.Value, lowerBound, upperBound);
                coordinatesElitePoint[data.Key] = elitePoint;
            }

            return new Firework(this.ElitePointType, birthStepNumber, coordinatesElitePoint);
        }
    }
}
