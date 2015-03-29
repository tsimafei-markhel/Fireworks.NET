using System;
using System.Collections.Generic;
using MathNet.Numerics;
using MathNet.Numerics.LinearRegression;
using FireworksNet.Model;
using FireworksNet.Problems;
using MathNet.Numerics.RootFinding;

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

    /// <summary>
    /// Selects an elite point from approximated curves by Elite Strategy.
    /// </summary>
    public interface IElitePointSelector
    {
        /// <summary>
        /// Selects an elite point by using polynomial function.
        /// </summary>
        /// <param name="polynomialFunc">The polynomial of 1st or 2nd order.</param>
        /// <param name="lowerBound">The lower bound.</param>
        /// <param name="upperBound">The upper bound.</param>
        /// <returns>Elite point.</returns>
        double SelectElitePoint(Func<double, double> polynomialFunc, double lowerBound, double upperBound);
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

    public class FirstOrderSelector : IElitePointSelector
    {
        // TODO: Review of this logic.
        public virtual double SelectElitePoint(Func<double, double> polynomialFunc, double lowerBound, double upperBound)
        {
            if (polynomialFunc == null)
            {
                throw new ArgumentNullException("polynomialFunc");
            }

            if (lowerBound > upperBound)
            {
                throw new ArgumentException("lowerBound");
            }

            return (upperBound - lowerBound) / 2 + lowerBound;
        }
    }

    public class SecondOrderSelector : IElitePointSelector
    {
        public virtual double SelectElitePoint(Func<double, double> polynomialFunc, double lowerBound, double upperBound)
        {
            if (polynomialFunc == null)
            {
                throw new ArgumentNullException("polynomialFunc");
            }

            if (lowerBound > upperBound)
            {
                throw new ArgumentException("lowerBound");
            }

            Func<double, double> derivative = Differentiate.FirstDerivativeFunc(polynomialFunc);

            return Brent.FindRoot(derivative, lowerBound, upperBound);
        }
    }

    public class TempEliteStrategy
    {
        public FireworkType ElitePointType { get { return FireworkType.ExplosionSpark; } }

        public Firework GetFirework(Problem problem, IFit polynomialFit, IElitePointSelector elitePointSelector, FireworkExplosion elitePointExplosion, IEnumerable<Firework> from, int order)
        {
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

            foreach (Dimension dimension in problem.Dimensions)
            {
                double[] coordinates = new double[currentFireworks.Count];

                //Get coordinates of the fireworks in current dimension
                current = 0;
                foreach (Firework firework in currentFireworks)
                {
                    coordinates[current] = firework.Coordinates[dimension];
                    current++;
                }

                Func<double, double> polynomial = polynomialFit.Approximate(coordinates, qualities);
                fitnessLandscapes[dimension] = polynomial;
            }

            //12. Obtain a spark from approximated curves by Elite Strategy

            Dictionary<Dimension, double> coordinatesElitePoint = new Dictionary<Dimension, double>();

            foreach (KeyValuePair<Dimension, Func<double, double>> data in fitnessLandscapes)
            {
                double lowerBound = data.Key.VariationRange.Minimum;
                double upperBound = data.Key.VariationRange.Maximum;
                double elitePoint = elitePointSelector.SelectElitePoint(data.Value, lowerBound, upperBound);
                coordinatesElitePoint[data.Key] = elitePoint;
            }

            return new Firework(this.ElitePointType, elitePointExplosion.StepNumber, coordinatesElitePoint);
        }
    }
}
