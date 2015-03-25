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
    public interface IPolynomial
    {
        /// <summary>
        /// Approximates fitness landscape.
        /// </summary>
        /// <param name="fireworkQualities">The qualities of fireworks.</param>
        /// <param name="fireworkCoordinates">The coordinates of fireworks in 
        /// the current one dimensional search space.</param>
        /// <returns>Approximated polynomial.</returns>
        Func<double, double> Approximate(double[] fireworkQualities, double[] fireworkCoordinates);
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
        /// <param name="lowerBound">The lowet bound.</param>
        /// <param name="upperBound">The upper bound.</param>
        /// <returns>Elite point.</returns>
        double SelectElitePoint(Func<double, double> polynomialFunc, double lowerBound, double upperBound);
    }

    public class FirstOrderPolynomial : IPolynomial
    {
        private readonly int order;

        public FirstOrderPolynomial()
        {
            this.order = 1;
        }

        public virtual Func<double, double> Approximate(double[] fireworkQualities, double[] fireworkCoordinates)
        {
            if (fireworkQualities == null)
            {
                throw new ArgumentNullException("fireworkQualities");
            }

            if (fireworkCoordinates == null)
            {
                throw new ArgumentNullException("fireworkCoordinates");
            }

            return Fit.PolynomialFunc(fireworkQualities, fireworkCoordinates, this.order, DirectRegressionMethod.QR);
        }
    }

    public class SecondOrderPolynomial : IPolynomial
    {
        private readonly int order;

        public SecondOrderPolynomial()
        {
            this.order = 2;
        }

        public virtual Func<double, double> Approximate(double[] fireworkQualities, double[] fireworkCoordinates)
        {
            if (fireworkQualities == null)
            {
                throw new ArgumentNullException("fireworkQualities");
            }

            if (fireworkCoordinates == null)
            {
                throw new ArgumentNullException("fireworkCoordinates");
            }

            return Fit.PolynomialFunc(fireworkQualities, fireworkCoordinates, this.order, DirectRegressionMethod.QR);
        }
    }

    public class TempEliteStrategy
    {
        public Firework GetFirework(Problem problem, IEnumerable<Firework> from, int order)
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
                foreach(Firework firework in currentFireworks)
                {
                    coordinates[current] = firework.Coordinates[dimension];
                    current++;
                }
                
                Func<double, double> polynomial = Fit.PolynomialFunc(qualities, coordinates, order, DirectRegressionMethod.QR);
                fitnessLandscapes[dimension] = polynomial;
            }

            //12. Obtain a spark from approximated curves by Elite Strategy

            Dictionary<Dimension, double> coordinates = new Dictionary<Dimension, double>();

            if (order == 1)
            {
                foreach (KeyValuePair<Dimension, Func<double, double>> data in fitnessLandscapes)
                {
                    /*
                    double lowerBound = data.Key.VariationRange.Minimum;
                    double upperBound = data.Key.VariationRange.Maximum;
                    double midPoint = (upperBound - lowerBound) / 2 + lowerBound;
                    coordinates[data.Key] = midPoint;
                    */
                }
            }

            if (order == 2)
            {
                foreach (KeyValuePair<Dimension, Func<double, double>> data in fitnessLandscapes)
                {
                    double lowerBound = data.Key.VariationRange.Minimum;
                    double upperBound = data.Key.VariationRange.Maximum;
                    Func<double, double> derivative = Differentiate.FirstDerivativeFunc(data.Value);
                    double x = Brent.FindRoot(derivative, lowerBound, upperBound);
                    coordinates[data.Key] = x;
                }
            }

            return null;
        }
    }
}
