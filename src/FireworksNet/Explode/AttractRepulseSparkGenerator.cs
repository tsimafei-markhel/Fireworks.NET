using System;
using System.Collections.Generic;
using System.Diagnostics;
using FireworksNet.Distributions;
using FireworksNet.Extensions;
using FireworksNet.Model;
using FireworksNet.Explode;

namespace FireworksNet.ParallelExplode
{
    /// <summary>
    /// Implementation of Attract-Repulse mutation algorithm
    /// </summary>
    public class AttractRepulseSparkGenerator : SparkGeneratorBase<FireworkExplosion>
    {
        private readonly System.Random randomizer;
        private readonly IEnumerable<Dimension> dimensions;
        private readonly IContinuousDistribution distribution;

        public override FireworkType GeneratedSparkType { get { return FireworkType.SpecificSpark; } }

        protected Firework BestSolution { get; set; }

        /// <summary>
        /// Create instance of AttractRepulseSparkGenerator
        /// </summary>
        /// <param name="dimensions">Dimenstion of research space</param>
        /// <param name="state">Represent current state of algorithm</param>
        /// <param name="distribution">Uniform distribution</param>
        /// <param name="randomizer">Generator for coin flip</param>
        public AttractRepulseSparkGenerator(IEnumerable<Dimension> dimensions, IContinuousDistribution distribution, System.Random randomizer)
        {            
            if (dimensions == null) { throw new System.ArgumentNullException("dimentions"); }
            if (distribution == null) { throw new System.ArgumentNullException("distribution"); }
            if (randomizer == null) { throw new System.ArgumentNullException("generator"); }

            this.dimensions = dimensions;
            this.distribution = distribution;
            this.randomizer = randomizer;
        }

        protected override Firework CreateSparkTyped(FireworkExplosion explosion)
        {
            Debug.Assert(explosion != null, "explosion is null");
            Debug.Assert(explosion.ParentFirework != null, "explosion parent firework is null");
            Debug.Assert(explosion.ParentFirework.Coordinates != null, "explosion parent firework coordinate is null");

            if (BestSolution == null) { throw new System.ArgumentNullException("best solution"); }

            Firework spark = new Firework(GeneratedSparkType, explosion.StepNumber, explosion.ParentFirework.Coordinates);

            // attract-repulse scaling factor. (1-δ, 1+δ)
            double scalingFactor = distribution.Sample();

            foreach (Dimension dimension in dimensions)
            {
                Debug.Assert(dimension != null, "dimension is null");
                Debug.Assert(dimension.VariationRange != null, "dimension variation range is null");
                Debug.Assert(!dimension.VariationRange.Length.IsEqual(0.0), "dimension variation range length is 0");

                if ((int)Math.Round(this.randomizer.NextDouble(0.0, 1.0), MidpointRounding.AwayFromZero) == 1) // Coin flip
                {
                    spark.Coordinates[dimension] += (spark.Coordinates[dimension] - BestSolution.Coordinates[dimension]) * scalingFactor;
                    if (!dimension.IsValueInRange(spark.Coordinates[dimension]))
                    {
                        spark.Coordinates[dimension] = dimension.VariationRange.Minimum + Math.Abs(spark.Coordinates[dimension]) % dimension.VariationRange.Length;
                    }
                }
            }

            return spark;
        }
    }
}
