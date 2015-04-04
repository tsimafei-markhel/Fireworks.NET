using System;
using System.Collections.Generic;
using System.Diagnostics;
using FireworksNet.Distributions;
using FireworksNet.Extensions;
using FireworksNet.Model;

namespace FireworksNet.Explode
{
    /// <summary>
    /// Conventional Gaussian spark generator, as described in 2010 paper.
    /// </summary>
    public class GaussianSparkGenerator : SparkGeneratorBase<FireworkExplosion>
    {
        private readonly IEnumerable<Dimension> dimensions;
        private readonly IContinuousDistribution distribution;
        private readonly System.Random randomizer;

        /// <summary>
        /// Gets the type of the generated spark.
        /// </summary>
        public override FireworkType GeneratedSparkType { get { return FireworkType.SpecificSpark; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="GaussianSparkGenerator"/> class.
        /// </summary>
        /// <param name="dimensions">The dimensions to fit generated sparks into.</param>
        /// <param name="distribution">The distribution.</param>
        /// <param name="randomizer">The randomizer.</param>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="dimensions"/>
        /// or <paramref name="distribution"/> or <paramref name="randomizer"/> is
        /// <c>null</c>.
        /// </exception>
        public GaussianSparkGenerator(IEnumerable<Dimension> dimensions, IContinuousDistribution distribution, System.Random randomizer)
        {
            if (dimensions == null)
            {
                throw new ArgumentNullException("dimensions");
            }

            if (distribution == null)
            {
                throw new ArgumentNullException("distribution");
            }

            if (randomizer == null)
            {
                throw new ArgumentNullException("randomizer");
            }

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
            Debug.Assert(this.distribution != null, "Distribution is null");
            Debug.Assert(this.dimensions != null, "Dimension collection is null");
            Debug.Assert(this.randomizer != null, "Randomizer is null");

            Firework spark = new Firework(this.GeneratedSparkType, explosion.StepNumber, explosion.ParentFirework.Coordinates);

            Debug.Assert(spark.Coordinates != null, "Spark coordinate collection is null");

            double offsetDisplacement = this.distribution.Sample(); // Coefficient of Gaussian explosion
            foreach (Dimension dimension in this.dimensions)
            {
                Debug.Assert(dimension != null, "Dimension is null");
                Debug.Assert(dimension.VariationRange != null, "Dimension variation range is null");
                Debug.Assert(!dimension.VariationRange.Length.IsEqual(0.0), "Dimension variation range length is 0");

                if (this.randomizer.NextBoolean()) // Coin flip
                {
                    spark.Coordinates[dimension] *= offsetDisplacement;
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