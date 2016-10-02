using System;
using System.Collections.Generic;
using System.Diagnostics;
using FireworksNet.Distributions;
using FireworksNet.Extensions;
using FireworksNet.Model;

namespace FireworksNet.Generation
{
    /// <summary>
    /// Conventional Gaussian spark generator, as described in 2012 paper.
    /// </summary>
    /// <remarks>Modifies all coordinates of the created spark (i.e. no coin flip).</remarks>
    public class GaussianSparkGenerator2012 : SparkGeneratorBase<FireworkExplosion>
    {
        private readonly IEnumerable<Dimension> dimensions;
        private readonly IContinuousDistribution distribution;

        /// <summary>
        /// Gets the type of the generated spark.
        /// </summary>
        public override FireworkType GeneratedSparkType { get { return FireworkType.SpecificSpark; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="GaussianSparkGenerator2012"/> class.
        /// </summary>
        /// <param name="dimensions">The dimensions to fit generated sparks into.</param>
        /// <param name="distribution">The distribution.</param>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="dimensions"/>
        /// or <paramref name="distribution"/> is <c>null</c>.
        /// </exception>
        public GaussianSparkGenerator2012(IEnumerable<Dimension> dimensions, IContinuousDistribution distribution)
        {
            if (dimensions == null)
            {
                throw new ArgumentNullException(nameof(dimensions));
            }

            if (distribution == null)
            {
                throw new ArgumentNullException(nameof(distribution));
            }

            this.dimensions = dimensions;
            this.distribution = distribution;
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

            Firework spark = new Firework(this.GeneratedSparkType, explosion.StepNumber, explosion.ParentFirework.Coordinates);

            Debug.Assert(spark.Coordinates != null, "Spark coordinate collection is null");

            double offsetDisplacement = this.distribution.Sample(); // Coefficient of Gaussian explosion
            foreach (Dimension dimension in this.dimensions)
            {
                Debug.Assert(dimension != null, "Dimension is null");
                Debug.Assert(dimension.VariationRange != null, "Dimension variation range is null");
                Debug.Assert(!dimension.VariationRange.Length.IsEqual(0.0), "Dimension variation range length is 0");

                // Modify each and every coordinate (without coin flip) in order to avoid
                // fireworks with identical values of a dimension.
                spark.Coordinates[dimension] *= offsetDisplacement;
                if (!dimension.IsValueInRange(spark.Coordinates[dimension]))
                {
                    spark.Coordinates[dimension] = dimension.VariationRange.Minimum + Math.Abs(spark.Coordinates[dimension]) % dimension.VariationRange.Length;
                }
            }

            return spark;
        }
    }
}