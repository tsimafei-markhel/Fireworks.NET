using System;
using System.Collections.Generic;
using Fireworks.Distributions;
using Fireworks.Model;
using Fireworks.Randomization;

namespace Fireworks.Explode
{
    /// <summary>
    /// Conventional Gaussian spark generator, as described in 2010 paper
    /// </summary>
    public class GaussianSparkGenerator : SparkGenerator
    {
        private readonly IEnumerable<Dimension> dimensions;
        private readonly IContinuousDistribution distribution;
        private readonly IRandom randomizer;

        public override FireworkType GeneratedSparkType { get { return FireworkType.SpecificSpark; } }

        public GaussianSparkGenerator(IEnumerable<Dimension> dimensions, IContinuousDistribution distribution, IRandom randomizer)
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

        public override Firework CreateSpark(Explosion explosion)
        {
            // TODO: Think over explosion.ParentFirework.BirthStepNumber + 1. Is that correct?
            Firework spark = new Firework(FireworkType.SpecificSpark, explosion.ParentFirework.BirthStepNumber + 1, explosion.ParentFirework.Coordinates);

            double offsetDisplacement = distribution.Sample(); // Coefficient of Gaussian explosion
            foreach (Dimension dimension in dimensions)
            {
                if ((int)Math.Round(randomizer.GetNext(0.0, 1.0), MidpointRounding.AwayFromZero) == 1) // Coin flip
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