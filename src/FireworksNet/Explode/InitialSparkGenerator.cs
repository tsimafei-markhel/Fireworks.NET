using System;
using System.Collections.Generic;
using System.Diagnostics;
using FireworksNet.Extensions;
using FireworksNet.Model;

namespace FireworksNet.Explode
{
	public class InitialSparkGenerator : SparkGenerator<InitialExplosion>
	{
		private readonly IEnumerable<Dimension> dimensions;
        private readonly IDictionary<Dimension, Range> initialRanges;
		private readonly System.Random randomizer;

		public override FireworkType GeneratedSparkType { get { return FireworkType.Initial; } }

        public InitialSparkGenerator(IEnumerable<Dimension> dimensions, IDictionary<Dimension, Range> initialRanges, System.Random randomizer)
		{
			if (dimensions == null)
			{
				throw new ArgumentNullException("dimensions");
			}

			if (initialRanges == null)
			{
				throw new ArgumentNullException("initialRanges");
			}

			if (randomizer == null)
			{
                throw new ArgumentNullException("randomizer");
			}

			this.dimensions = dimensions;
			this.initialRanges = initialRanges;
			this.randomizer = randomizer;
		}

        public InitialSparkGenerator(IEnumerable<Dimension> dimensions, System.Random randomizer)
            : this(dimensions, null, randomizer)
        {
        }

        protected override Firework CreateSparkTyped(InitialExplosion explosion)
		{
			Debug.Assert(dimensions != null, "Dimension collection is null");
			Debug.Assert(initialRanges != null, "Initial ranges collection is null");
            Debug.Assert(randomizer != null, "Randomizer is null");

			Firework spark = new Firework(GeneratedSparkType, 0);

			Debug.Assert(spark.Coordinates != null, "Spark coordinate collection is null");

			foreach (Dimension dimension in dimensions)
			{
                Debug.Assert(dimension != null, "Dimension is null");
				Debug.Assert(initialRanges.ContainsKey(dimension), "Dimension collection does not contain corresponding initial range");

                Range dimensionRange = initialRanges[dimension];

                Debug.Assert(dimensionRange != null, "Initial dimension range is null");

                spark.Coordinates[dimension] = randomizer.NextDouble(dimensionRange);
			}

			return spark;
		}
	}
}