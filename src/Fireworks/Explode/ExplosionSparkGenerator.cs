using System;
using System.Collections.Generic;
using Fireworks.Model;
using Fireworks.Randomization;

namespace Fireworks.Explode
{
	/// <summary>
	/// Conventional spark generator, as described in 2010 paper
	/// </summary>
	public class ExplosionSparkGenerator : ISparkGenerator
	{
		private readonly IEnumerable<Dimension> dimensions;
		private readonly IRandom randomizer;

		public const FireworkType GeneratedSparkType = FireworkType.ExplosionSpark;

		public ExplosionSparkGenerator(IEnumerable<Dimension> dimensions, IRandom randomizer)
		{
			if (dimensions == null)
			{
				throw new ArgumentNullException("dimensions");
			}

			if (randomizer == null)
			{
				throw new ArgumentNullException("random");
			}

			this.dimensions = dimensions;
			this.randomizer = randomizer;
		}

		public IEnumerable<Firework> CreateSparks(Explosion explosion)
		{
            int desiredNumberOfSparks;
            if (!explosion.SparkCounts.TryGetValue(GeneratedSparkType, out desiredNumberOfSparks))
            {
                return new List<Firework>();
            }

            List<Firework> sparks = new List<Firework>(desiredNumberOfSparks);
            for (int i = 0; i < desiredNumberOfSparks; i++)
            {
                sparks.Add(CreateSpark(explosion));
            }

            return sparks;
		}

		public Firework CreateSpark(Explosion explosion)
		{
			// TODO: Think over explosion.ParentFirework.BirthStepNumber + 1. Is that correct?
			Firework spark = new Firework(GeneratedSparkType, explosion.ParentFirework.BirthStepNumber + 1, explosion.ParentFirework.Coordinates);

			double offsetDisplacement = explosion.Amplitude * randomizer.GetNext(-1.0, 1.0);
			foreach (Dimension dimension in dimensions)
			{
				if ((int)Math.Round(randomizer.GetNext(0.0, 1.0), MidpointRounding.AwayFromZero) == 1) // Coin flip
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