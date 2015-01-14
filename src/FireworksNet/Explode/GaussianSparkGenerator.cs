using System;
using System.Collections.Generic;
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
            Firework spark = new Firework(FireworkType.SpecificSpark, explosion.StepNumber, explosion.ParentFirework.Coordinates);

            double offsetDisplacement = distribution.Sample(); // Coefficient of Gaussian explosion
            foreach (Dimension dimension in dimensions)
            {
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