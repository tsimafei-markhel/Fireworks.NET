using System;
using System.Collections.Generic;
using System.Diagnostics;
using FireworksNet.Distributions;
using FireworksNet.Extensions;
using FireworksNet.Model;

namespace FireworksNet.Generation
{
    /// <summary>
    /// Implementation of Attract-Repulse mutation algorithm, as described in 2013 GPU paper.
    /// </summary>
    public class AttractRepulseSparkGenerator : SparkGeneratorBase<FireworkExplosion>
    {
        /// <summary>
        /// The current best solution in global scope.
        /// </summary>
        private readonly Solution bestSolution;
        private readonly IEnumerable<Dimension> dimensions;
        private readonly IContinuousDistribution distribution;
        private readonly System.Random randomizer;

        /// <summary>
        /// Gets the type of the generated spark.
        /// </summary>
        public override FireworkType GeneratedSparkType { get { return FireworkType.SpecificSpark; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="AttractRepulseSparkGenerator"/> class.
        /// </summary>
        /// <param name="bestSolution">The current best solution in global scope.</param>
        /// <param name="dimensions">The dimensions to fit generated sparks into.</param>
        /// <param name="distribution">The distribution to be used to obtain scaling factor.</param>
        /// <param name="randomizer">The randomizer.</param>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="bestSolution"/>
        /// or <paramref name="dimensions"/> or <paramref name="distribution"/> or
        /// <paramref name="randomizer"/> is <c>null</c>.
        /// </exception>
        public AttractRepulseSparkGenerator(Solution bestSolution, IEnumerable<Dimension> dimensions, IContinuousDistribution distribution, System.Random randomizer)
        {
            if (bestSolution == null)
            {
                throw new ArgumentNullException(nameof(bestSolution));
            }

            if (dimensions == null)
            {
                throw new ArgumentNullException(nameof(dimensions));
            }

            if (distribution == null)
            {
                throw new ArgumentNullException(nameof(distribution));
            }

            if (randomizer == null)
            {
                throw new ArgumentNullException(nameof(randomizer));
            }

            this.bestSolution = bestSolution;
            this.dimensions = dimensions;
            this.distribution = distribution;
            this.randomizer = randomizer;
        }

        /// <summary>
        /// Creates the typed spark.
        /// </summary>
        /// <param name="explosion">The explosion that gives birth to the spark.</param>
        /// <returns>The new typed spark.</returns>
        protected override Firework CreateSparkTyped(FireworkExplosion explosion)
        {
            Debug.Assert(explosion != null, "Explosion is null");
            Debug.Assert(explosion.ParentFirework != null, "Explosion parent firework is null");
            Debug.Assert(explosion.ParentFirework.Coordinates != null, "Explosion parent firework coordinate collection is null");

            Firework spark = new Firework(this.GeneratedSparkType, explosion.StepNumber, explosion.ParentFirework.Coordinates);

            // Attract-Repulse scaling factor. (1-δ, 1+δ)
            double scalingFactor = this.distribution.Sample();

            Solution copyOfBestSolution = null;

            lock (this.bestSolution)
            {
                copyOfBestSolution = new Solution(this.bestSolution.Coordinates, this.bestSolution.Quality);
            }

            foreach (Dimension dimension in this.dimensions)
            {
                Debug.Assert(dimension != null, "Dimension is null");
                Debug.Assert(dimension.VariationRange != null, "Dimension variation range is null");
                Debug.Assert(!dimension.VariationRange.Length.IsEqual(0.0), "Dimension variation range length is 0");

                if (this.randomizer.NextBoolean()) // Coin flip
                {
                    spark.Coordinates[dimension] += (spark.Coordinates[dimension] - copyOfBestSolution.Coordinates[dimension]) * scalingFactor;
                    if (!dimension.IsValueInRange(spark.Coordinates[dimension]))
                    {
                        spark.Coordinates[dimension] = dimension.VariationRange.Minimum + Math.Abs(spark.Coordinates[dimension]) % dimension.VariationRange.Length;
                    }
                }
            }

            return spark;
        }
    }
}
