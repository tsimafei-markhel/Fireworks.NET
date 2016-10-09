using System;
using System.Collections.Generic;
using System.Diagnostics;
using FireworksNet.Extensions;
using FireworksNet.Model;

namespace FireworksNet.Generation
{
    /// <summary>
    /// Enhanced Explosion spark generator, as described in 2013 paper.
    /// </summary>
    public class EnhancedExplosionSparkGenerator : SparkGeneratorBase<EnhancedFireworkExplosion>
    {
        private readonly IEnumerable<Dimension> dimensions;
        private readonly System.Random randomizer;

        private const double offsetDisplacementRandomMin = -1.0;
        private const double offsetDisplacementRandomMax = 1.0;

        /// <summary>
        /// Gets the type of the generated spark.
        /// </summary>
        public override FireworkType GeneratedSparkType { get { return FireworkType.ExplosionSpark; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnhancedExplosionSparkGenerator"/> class.
        /// </summary>
        /// <param name="dimensions">The dimensions to fit generated sparks into.</param>
        /// <param name="randomizer">The randomizer.</param>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="dimensions"/>
        /// or <paramref name="randomizer"/> is <c>null</c>.
        /// </exception>
        public EnhancedExplosionSparkGenerator(IEnumerable<Dimension> dimensions, System.Random randomizer)
        {
            if (dimensions == null)
            {
                throw new ArgumentNullException(nameof(dimensions));
            }

            if (randomizer == null)
            {
                throw new ArgumentNullException(nameof(randomizer));
            }

            this.dimensions = dimensions;
            this.randomizer = randomizer;
        }

        /// <summary>
        /// Creates the spark from the explosion.
        /// </summary>
        /// <param name="explosion">The explosion that gives birth to the spark.</param>
        /// <param name="birthOrder">The number of spark in the collection of sparks born by
        /// this generator within one step.</param>
        /// <returns>A spark for the specified explosion.</returns>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="explosion"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException"> if <paramref name="birthOrder"/>
        /// is less than zero.</exception>
        public override Firework CreateSpark(EnhancedFireworkExplosion explosion, int birthOrder)
        {
            if (explosion == null)
            {
                throw new ArgumentNullException(nameof(explosion));
            }

            if (birthOrder < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(birthOrder));
            }

            Debug.Assert(explosion.ParentFirework != null, "Explosion parent firework is null");
            Debug.Assert(explosion.ParentFirework.Coordinates != null, "Explosion parent firework coordinate collection is null");
            Debug.Assert(explosion.Amplitudes != null, "Explosion amplitude collection is null");
            Debug.Assert(this.randomizer != null, "Randomizer is null");
            Debug.Assert(this.dimensions != null, "Dimension collection is null");

            Firework spark = new Firework(this.GeneratedSparkType, explosion.StepNumber, birthOrder, explosion.ParentFirework.Coordinates);

            Debug.Assert(spark.Coordinates != null, "Spark coordinate collection is null");

            foreach (Dimension dimension in this.dimensions)
            {
                if (this.randomizer.NextBoolean()) // Coin flip
                {
                    Debug.Assert(dimension != null, "Dimension is null");
                    Debug.Assert(dimension.VariationRange != null, "Dimension variation range is null");
                    Debug.Assert(!dimension.VariationRange.Length.IsEqual(0.0), "Dimension variation range length is 0");
                    Debug.Assert(explosion.Amplitudes.ContainsKey(dimension), "Amplitude for the dimension is not in the explosion amplitude collection");
                    Debug.Assert(!double.IsNaN(explosion.Amplitudes[dimension]), "Amplitude for the dimension is Nan");
                    Debug.Assert(!double.IsInfinity(explosion.Amplitudes[dimension]), "Amplitude for the dimension is Infinity");

                    double offsetDisplacement = explosion.Amplitudes[dimension] * this.randomizer.NextDouble(EnhancedExplosionSparkGenerator.offsetDisplacementRandomMin, EnhancedExplosionSparkGenerator.offsetDisplacementRandomMax);

                    spark.Coordinates[dimension] += offsetDisplacement;
                    if (!dimension.IsValueInRange(spark.Coordinates[dimension]))
                    {
                        spark.Coordinates[dimension] = dimension.VariationRange.Minimum + this.randomizer.NextDouble() * dimension.VariationRange.Length;
                    }
                }
            }

            return spark;
        }
    }
}