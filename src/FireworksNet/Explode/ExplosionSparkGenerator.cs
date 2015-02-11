using System;
using System.Collections.Generic;
using System.Diagnostics;
using FireworksNet.Extensions;
using FireworksNet.Model;

namespace FireworksNet.Explode
{
    /// <summary>
    /// Conventional Explosion spark generator, as described in 2010 paper.
    /// </summary>
    public class ExplosionSparkGenerator : SparkGenerator<FireworkExplosion>
    {
        private readonly IEnumerable<Dimension> dimensions;
        private readonly System.Random randomizer;

        private const double offsetDisplacementRandomMin = -1.0;
        private const double offsetDisplacementRandomMax = 1.0;

        public override FireworkType GeneratedSparkType { get { return FireworkType.ExplosionSpark; } }

        public ExplosionSparkGenerator(IEnumerable<Dimension> dimensions, System.Random randomizer)
        {
            if (dimensions == null)
            {
                throw new ArgumentNullException("dimensions");
            }

            if (randomizer == null)
            {
                throw new ArgumentNullException("randomizer");
            }

            this.dimensions = dimensions;
            this.randomizer = randomizer;
        }

        protected override Firework CreateSparkTyped(FireworkExplosion explosion)
        {
            Debug.Assert(explosion != null, "Explosion is null");
            Debug.Assert(explosion.ParentFirework != null, "Explosion parent firework is null");
            Debug.Assert(explosion.ParentFirework.Coordinates != null, "Explosion parent firework coordinate collection is null");
            Debug.Assert(this.randomizer != null, "Randomizer is null");
            Debug.Assert(this.dimensions != null, "Dimension collection is null");

            Firework spark = new Firework(this.GeneratedSparkType, explosion.StepNumber, explosion.ParentFirework.Coordinates);

            Debug.Assert(spark.Coordinates != null, "Spark coordinate collection is null");

            double offsetDisplacement = explosion.Amplitude * this.randomizer.NextDouble(ExplosionSparkGenerator.offsetDisplacementRandomMin, ExplosionSparkGenerator.offsetDisplacementRandomMax);
            foreach (Dimension dimension in this.dimensions)
            {
                Debug.Assert(dimension != null, "Dimension is null");
                Debug.Assert(dimension.VariationRange != null, "Dimension variation range is null");

                if ((int)Math.Round(this.randomizer.NextDouble(0.0, 1.0), MidpointRounding.AwayFromZero) == 1) // Coin flip
                {
                    spark.Coordinates[dimension] += offsetDisplacement;
                    if (!dimension.IsValueInBounds(spark.Coordinates[dimension]))
                    {
                        spark.Coordinates[dimension] = dimension.VariationRange.Minimum + Math.Abs(spark.Coordinates[dimension]) % dimension.VariationRange.Length;
                    }
                }
            }

            return spark;
        }
    }
}