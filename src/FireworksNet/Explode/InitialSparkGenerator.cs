using System;
using System.Collections.Generic;
using System.Linq;
using FireworksNet.Extensions;
using FireworksNet.Model;

namespace FireworksNet.Explode
{
	public class InitialSparkGenerator : SparkGenerator<InitialExplosion>
	{
		private readonly IEnumerable<Dimension> dimensions;
        private readonly IDictionary<Dimension, Range> initialDimensionRanges;
		private readonly System.Random randomizer;

		public override FireworkType GeneratedSparkType { get { return FireworkType.Initial; } }

        public InitialSparkGenerator(IEnumerable<Dimension> dimensions, IDictionary<Dimension, Range> initialDimensionRanges, System.Random randomizer)
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
            this.initialDimensionRanges = SetInitialDimensionRanges(dimensions, initialDimensionRanges);
			this.randomizer = randomizer;
		}

        public InitialSparkGenerator(IEnumerable<Dimension> dimensions, System.Random randomizer)
            : this(dimensions, null, randomizer)
        {
        }

        protected override Firework CreateSparkTyped(InitialExplosion explosion)
		{
            System.Diagnostics.Debug.Assert(randomizer != null, "Randomizer is null");

			Firework spark = new Firework(GeneratedSparkType, 0);
			foreach (Dimension dimension in dimensions)
			{
                System.Diagnostics.Debug.Assert(dimension != null, "Dimension is null");

                Range dimensionRange = initialDimensionRanges[dimension];

                System.Diagnostics.Debug.Assert(dimensionRange != null, "Initial dimension range is null");

                spark.Coordinates[dimension] = randomizer.NextDouble(dimensionRange);
			}

			return spark;
		}

        // TODO: Not sure if this method should be in this class. Maybe caller code has to decide how to setup initialDimensionRanges?
        private static IDictionary<Dimension, Range> SetInitialDimensionRanges(IEnumerable<Dimension> dimensions, IDictionary<Dimension, Range> initialDimensionRanges)
        {
            if (initialDimensionRanges != null)
            {
                // TODO: Need validation to make sure dimensions and initialDimensionRanges contain the same Dimension instances
                return initialDimensionRanges;
            }
            else
            {
                Dictionary<Dimension, Range> dimensionRanges = new Dictionary<Dimension, Range>(dimensions.Count());
                foreach (Dimension dimension in dimensions)
                {
                    System.Diagnostics.Debug.Assert(dimension != null, "Dimension is null");
                    System.Diagnostics.Debug.Assert(dimension.VariationRange != null, "Dimension variation range is null");

                    dimensionRanges.Add(dimension, dimension.VariationRange);
                }

                return dimensionRanges;
            }
        }
	}
}