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
        Func<double, double> Differentiate(Func<double, double> func);
    }

    public class Differentiation : IDifferentiation
    {
        public virtual Func<double, double> Differentiate(Func<double, double> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("polynomialFunc");
            }

            return MathNet.Numerics.Differentiate.FirstDerivativeFunc(func);
        }
    }

    public interface ISolver
    {
        double Solve(Func<double, double> polynomialFunc, Range variationRange);
    }

    public class BrentSolver : ISolver
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

            return Brent.FindRoot(polynomialFunc, variationRange.Minimum, variationRange.Maximum);
        }
    }

    public class FirstOrderEliteStrategy : TempEliteStrategy
    {
        public FirstOrderEliteStrategy(IEnumerable<Dimension> dimensions, IFit polynomialFit)
            : base(dimensions, polynomialFit)
        {
        }

        public override double SelectElitePoint(Func<double, double> polynomialFunc, Range variationRange)
        {
            if (polynomialFunc == null)
            {
                throw new ArgumentNullException("polynomialFunc");
            }

            if (variationRange == null)
            {
                throw new ArgumentNullException("variationRange");
            }

            // TODO: Review of this logic.
            return (variationRange.Maximum - variationRange.Minimum) / 2 + variationRange.Minimum;
        }
    }

    public class SecondOrderEliteStrategy : TempEliteStrategy
    {
        private readonly IDifferentiation differentiation;
        private readonly ISolver solver;

        public SecondOrderEliteStrategy(IEnumerable<Dimension> dimensions, IFit polynomialFit, IDifferentiation differentiation, ISolver solver)
            : base(dimensions, polynomialFit)
        {
            if (differentiation == null)
            {
                throw new ArgumentNullException("differentiation");
            }

            if (this.solver == null)
            {
                throw new ArgumentNullException("solver");
            }

            this.differentiation = differentiation;
            this.solver = solver;
        }

        public override double SelectElitePoint(Func<double, double> func, Range variationRange)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }

            if (variationRange == null)
            {
                throw new ArgumentNullException("variationRange");
            }

            Func<double, double> derivative = this.differentiation.Differentiate(func);

            return solver.Solve(derivative, variationRange);
        }
    }

    public abstract class TempEliteStrategy
    {
        private readonly IEnumerable<Dimension> dimensions;
        private readonly IFit polynomialFit;

        public FireworkType ElitePointType { get { return FireworkType.EliteFirework; } }

        protected TempEliteStrategy(IEnumerable<Dimension> dimensions, IFit polynomialFit)
        {
            if (dimensions == null)
            {
                throw new ArgumentNullException("dimensions");
            }

            if (polynomialFit == null)
            {
                throw new ArgumentNullException("polynomialFit");
            }

            this.dimensions = dimensions;
            this.polynomialFit = polynomialFit;
        }

        public abstract double SelectElitePoint(Func<double, double> func, Range variationRange);

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
                double elitePoint = this.SelectElitePoint(data.Value, data.Key.VariationRange);
                coordinatesElitePoint[data.Key] = elitePoint;
            }

            return new Firework(this.ElitePointType, birthStepNumber, coordinatesElitePoint);
        }
    }
}
