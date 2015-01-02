using System;
using System.Collections.Generic;
using FireworksNet.Model;
using FireworksNet.Randomization;

namespace FireworksNet.Explode
{
	public class InitialSparkGenerator : SparkGenerator
	{
		private readonly IEnumerable<Dimension> dimensions;
		private readonly IRandom randomizer;

		public override FireworkType GeneratedSparkType { get { return FireworkType.Initial; } }

        // TODO: Take initial dimension ranges into account
		public InitialSparkGenerator(IEnumerable<Dimension> dimensions, IRandom randomizer)
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

		public override Firework CreateSpark(Explosion explosion)
		{
			Firework spark = new Firework(GeneratedSparkType, 0);
			foreach (Dimension dimension in dimensions)
			{
				spark.Coordinates[dimension] = dimension.VariationRange.Minimum + randomizer.GetNext(0.0, 1.0) * dimension.VariationRange.Length;
			}

			return spark;
		}
	}
}