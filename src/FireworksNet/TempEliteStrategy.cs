using System;
using System.Collections.Generic;
using FireworksNet.Model;
using FireworksNet.Fit;
using FireworksNet.Solver;
using FireworksNet.Differentiation;

namespace FireworksNet
{
    public class FirstOrderEliteStrategy : TempEliteStrategy
    {
        public FirstOrderEliteStrategy(IEnumerable<Dimension> dimensions, IFit polynomialFit)
            : base(dimensions, polynomialFit)
        {
        }

        public override double SelectElitePoint(Func<double, double> func, Range variationRange)
        {
            if (func == null)
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
        private readonly IDifferentiator differentiation;
        private readonly ISolver solver;

        public SecondOrderEliteStrategy(IEnumerable<Dimension> dimensions, IFit polynomialFit, IDifferentiator differentiation, ISolver solver)
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

        public FireworkType ElitePointType { get; set; }

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
            this.ElitePointType = FireworkType.EliteFirework;
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
