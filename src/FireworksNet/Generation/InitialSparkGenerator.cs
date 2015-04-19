using System;
using System.Collections.Generic;
using System.Diagnostics;
using FireworksNet.Extensions;
using FireworksNet.Model;

namespace FireworksNet.Generation
{
    /// <summary>
    /// Conventional initial spark generator, as described in 2010 paper.
    /// </summary>
    public class InitialSparkGenerator : SparkGeneratorBase<InitialExplosion>
    {
        private readonly IEnumerable<Dimension> dimensions;
        private readonly IDictionary<Dimension, Range> initialRanges;
        private readonly System.Random randomizer;

        /// <summary>
        /// Gets the type of the generated spark.
        /// </summary>
        public override FireworkType GeneratedSparkType { get { return FireworkType.Initial; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="InitialSparkGenerator"/> class.
        /// </summary>
        /// <param name="dimensions">The dimensions to fit generated sparks into.</param>
        /// <param name="initialRanges">The initial ranges for <paramref name="dimensions"/>.</param>
        /// <param name="randomizer">The randomizer.</param>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="dimensions"/>
        /// or <paramref name="initialRanges"/> or <paramref name="randomizer"/> is
        /// <c>null</c>.
        /// </exception>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="InitialSparkGenerator"/> class.
        /// Initial ranges of the problem dimensions match those from <paramref name="dimensions"/>.
        /// </summary>
        /// <param name="dimensions">The dimensions to fit generated sparks into.</param>
        /// <param name="randomizer">The randomizer.</param>
        public InitialSparkGenerator(IEnumerable<Dimension> dimensions, System.Random randomizer)
            : this(dimensions, null, randomizer)
        {
        }

        /// <summary>
        /// Creates the typed spark.
        /// </summary>
        /// <param name="explosion">The explosion that gives birth to the spark.</param>
        /// <returns>The new typed spark.</returns>
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