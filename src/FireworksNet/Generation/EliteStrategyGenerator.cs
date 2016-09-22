using System;
using System.Collections.Generic;
using System.Diagnostics;
using FireworksNet.Fit;
using FireworksNet.Model;

namespace FireworksNet.Generation
{
    /// <summary>
    /// Elite strategy spark generator, as described in 2012 paper.
    /// </summary>
    public abstract class EliteStrategyGenerator : SparkGeneratorBase<EliteExplosion>
    {
        private readonly IEnumerable<Dimension> dimensions;
        private readonly IFit polynomialFit;

        /// <summary>
        /// Gets the type of the generated spark.
        /// </summary>
        public override FireworkType GeneratedSparkType { get { return FireworkType.EliteFirework; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="EliteStrategyGenerator"/> class.
        /// </summary>
        /// <param name="dimensions">The dimensions to fit generated sparks into.</param>
        /// <param name="polynomialFit">The polynomial fit.</param>
        protected EliteStrategyGenerator(IEnumerable<Dimension> dimensions, IFit polynomialFit)
        {
            if (dimensions == null)
            {
                throw new ArgumentNullException(nameof(dimensions));
            }

            if (polynomialFit == null)
            {
                throw new ArgumentNullException(nameof(polynomialFit));
            }

            this.dimensions = dimensions;
            this.polynomialFit = polynomialFit;
        }

        /// <summary>
        /// Calculates elite point.
        /// </summary>
        /// <param name="func">The function to calculate elite point.</param>
        /// <param name="variationRange">Represents an interval to calculate 
        /// elite point.</param>
        /// <returns>The coordinate of elite point.</returns>
        protected abstract double CalculateElitePoint(Func<double, double> func, Range variationRange);

        /// <summary>
        /// Creates the typed spark by Elite Strategy.
        /// </summary>
        /// <param name="explosion">The explosion that contains the collection 
        /// of sparks.</param>
        /// <returns>The new typed spark.</returns>
        protected override Firework CreateSparkTyped(EliteExplosion explosion)
        {
            Debug.Assert(explosion != null, "Explosion is null");
            Debug.Assert(explosion.Fireworks != null, "Fireworks collection is null");
            Debug.Assert(this.dimensions != null, "Dimension collection is null");
            Debug.Assert(this.polynomialFit != null, "Polynomial fit is null");

            // 12. Obtain a spark from approximated curves by Elite Strategy
            IDictionary<Dimension, Func<double, double>> fitnessLandscapes = this.ApproximateFitnessLandscapes(explosion.Fireworks);
            IDictionary<Dimension, double> coordinatesElitePoint = new Dictionary<Dimension, double>();

            foreach (KeyValuePair<Dimension, Func<double, double>> data in fitnessLandscapes)
            {
                double elitePoint = this.CalculateElitePoint(data.Value, data.Key.VariationRange);
                coordinatesElitePoint[data.Key] = elitePoint;
            }

            return new Firework(this.GeneratedSparkType, explosion.StepNumber, coordinatesElitePoint);
        }

        /// <summary>
        /// Approximates fitness landscape in each one dimensional search space.
        /// </summary>
        /// <param name="fireworks">The collection of <see cref="Firework"/>s to obtain
        /// coordinates in each one dimensional search space.</param>
        /// <returns>A map. Key is a <see cref="Dimension"/>. Value is a approximated
        /// curve by coordinates and qualities of <see cref="Firework"/>s.</returns>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="fireworks"/>
        /// is <c>null</c>.</exception>
        protected virtual IDictionary<Dimension, Func<double, double>> ApproximateFitnessLandscapes(IEnumerable<Firework> fireworks)
        {
            if (fireworks == null)
            {
                throw new ArgumentNullException(nameof(fireworks));
            }

            IList<Firework> currentFireworks = new List<Firework>(fireworks);

            double[] qualities = new double[currentFireworks.Count];

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

                current = 0;
                foreach (Firework firework in currentFireworks)
                {
                    coordinates[current] = firework.Coordinates[dimension];
                    current++;
                }

                fitnessLandscapes[dimension] = this.polynomialFit.Approximate(coordinates, qualities);
            }

            return fitnessLandscapes;
        }
    }
}