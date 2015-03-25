using System;
using System.Collections.Generic;
using MathNet.Numerics;
using MathNet.Numerics.LinearRegression;
using FireworksNet.Model;
using FireworksNet.Problems;
using MathNet.Numerics.RootFinding;

namespace FireworksNet
{
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
