using System;
using System.Collections.Generic;
using System.Diagnostics;
using FireworksNet.Extensions;
using FireworksNet.Model;

namespace FireworksNet.Generation
{
    /// <summary>
    /// Conventional Explosion spark generator, as described in 2012 paper.
    /// </summary>
    /// <remarks>Modifies all coordinates of the created spark (i.e. no coin flip).</remarks>
    public class ExplosionSparkGenerator2012 : SparkGeneratorBase<FireworkExplosion>
    {
        private readonly IEnumerable<Dimension> dimensions;
        private readonly System.Random randomizer;

        private const double offsetDisplacementRandomMin = -1.0;
        private const double offsetDisplacementRandomMax = 1.0;

        /// <summary>
        /// Gets the type of the generated spark.
        /// </summary>
        public override FireworkType GeneratedSparkType { get { return FireworkType.ExplosionSpark; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExplosionSparkGenerator2012"/> class.
        /// </summary>
        /// <param name="dimensions">The dimensions to fit generated sparks into.</param>
        /// <param name="randomizer">The randomizer.</param>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="dimensions"/>
        /// or <paramref name="randomizer"/> is <c>null</c>.
        /// </exception>
        public ExplosionSparkGenerator2012(IEnumerable<Dimension> dimensions, System.Random randomizer)
        {
            if (dimensions == null)
            {
                throw new ArgumentNullException(nameof(dimensions));
            }

            if (randomizer == null)
            {
                throw new ArgumentNullException(nameof(randomizer));
            }

            this.dimensions = dimensions;
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
            Debug.Assert(this.randomizer != null, "Randomizer is null");
            Debug.Assert(this.dimensions != null, "Dimension collection is null");

            Firework spark = new Firework(this.GeneratedSparkType, explosion.StepNumber, explosion.ParentFirework.Coordinates);

            Debug.Assert(spark.Coordinates != null, "Spark coordinate collection is null");

            double offsetDisplacement = explosion.Amplitude * this.randomizer.NextDouble(ExplosionSparkGenerator2012.offsetDisplacementRandomMin, ExplosionSparkGenerator2012.offsetDisplacementRandomMax);
            foreach (Dimension dimension in this.dimensions)
            {
                Debug.Assert(dimension != null, "Dimension is null");
                Debug.Assert(dimension.VariationRange != null, "Dimension variation range is null");
                Debug.Assert(!dimension.VariationRange.Length.IsEqual(0.0), "Dimension variation range length is 0");

                // Modify each and every coordinate (without coin flip) in order to avoid
                // fireworks with identical values of a dimension.
                spark.Coordinates[dimension] += offsetDisplacement;
                if (!dimension.IsValueInRange(spark.Coordinates[dimension]))
                {
                    spark.Coordinates[dimension] = dimension.VariationRange.Minimum + Math.Abs(spark.Coordinates[dimension]) % dimension.VariationRange.Length;
                }
            }

            return spark;
        }
    }
}