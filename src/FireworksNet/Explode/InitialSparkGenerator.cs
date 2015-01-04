using System;
using System.Collections.Generic;
using System.Linq;
using FireworksNet.Model;
using FireworksNet.Randomization;

namespace FireworksNet.Explode
{
	public class InitialSparkGenerator : SparkGenerator<InitialExplosion>
	{
		private readonly IEnumerable<Dimension> dimensions;
        private readonly IDictionary<Dimension, Range> initialDimensionRanges;
		private readonly IRandom randomizer;

		public override FireworkType GeneratedSparkType { get { return FireworkType.Initial; } }

		public InitialSparkGenerator(IEnumerable<Dimension> dimensions, IDictionary<Dimension, Range> initialDimensionRanges, IRandom randomizer)
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

        public InitialSparkGenerator(IEnumerable<Dimension> dimensions, IRandom randomizer)
            : this(dimensions, null, randomizer)
        {
        }

		protected override Firework CreateSpark(InitialExplosion explosion)
		{
			Firework spark = new Firework(GeneratedSparkType, 0);
			foreach (Dimension dimension in dimensions)
			{
                Range dimensionRange = initialDimensionRanges[dimension];
                spark.Coordinates[dimension] = dimensionRange.Minimum + randomizer.GetNext(0.0, 1.0) * dimensionRange.Length;
			}

			return spark;
		}

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
                    dimensionRanges.Add(dimension, dimension.VariationRange);
                }

                return dimensionRanges;
            }
        }
	}
}