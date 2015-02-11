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
            Debug.Assert(this.dimensions != null, "Dimension collection is null");
            Debug.Assert(this.initialRanges != null, "Initial ranges collection is null");
            Debug.Assert(this.randomizer != null, "Randomizer is null");

            Firework spark = new Firework(this.GeneratedSparkType, 0);

            Debug.Assert(spark.Coordinates != null, "Spark coordinate collection is null");

            foreach (Dimension dimension in this.dimensions)
            {
                Debug.Assert(dimension != null, "Dimension is null");
                Debug.Assert(this.initialRanges.ContainsKey(dimension), "Dimension collection does not contain corresponding initial range");

                Range dimensionRange = this.initialRanges[dimension];

                Debug.Assert(dimensionRange != null, "Initial dimension range is null");

                spark.Coordinates[dimension] = this.randomizer.NextDouble(dimensionRange);
            }

            return spark;
        }
    }
}