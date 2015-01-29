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
    public class GaussianSparkGenerator : SparkGenerator<FireworkExplosion>
    {
		private readonly IEnumerable<Dimension> dimensions;
        private readonly IContinuousDistribution distribution;
        private readonly System.Random randomizer;

        public override FireworkType GeneratedSparkType { get { return FireworkType.SpecificSpark; } }

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

        protected override Firework CreateSparkTyped(FireworkExplosion explosion)
        {
			Debug.Assert(explosion != null, "Explosion is null");
			Debug.Assert(explosion.ParentFirework != null, "Explosion parent firework is null");
			Debug.Assert(explosion.ParentFirework.Coordinates != null, "Explosion parent firework coordinate collection is null");
			Debug.Assert(distribution != null, "Distribution is null");
			Debug.Assert(dimensions != null, "Dimension collection is null");
			Debug.Assert(randomizer != null, "Randomizer is null");

			Firework spark = new Firework(GeneratedSparkType, explosion.StepNumber, explosion.ParentFirework.Coordinates);

			Debug.Assert(spark.Coordinates != null, "Spark coordinate collection is null");

            double offsetDisplacement = distribution.Sample(); // Coefficient of Gaussian explosion
            foreach (Dimension dimension in dimensions)
            {
				Debug.Assert(dimension != null, "Dimension is null");
				Debug.Assert(dimension.VariationRange != null, "Dimension variation range is null");

                if ((int)Math.Round(randomizer.NextDouble(0.0, 1.0), MidpointRounding.AwayFromZero) == 1) // Coin flip
                {
                    spark.Coordinates[dimension] *= offsetDisplacement;
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