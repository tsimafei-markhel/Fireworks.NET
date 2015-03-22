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
    /// Implementation of Attract-Repulse mutation algorithm, as described in 2010 paper.
    /// </summary>
    public class AttractRepulseSparkGenerator : SparkGeneratorBase<FireworkExplosion>
    {
        private readonly System.Random randomizer;
        private readonly Solution bestSolution;
        private readonly IEnumerable<Dimension> dimensions;
        private readonly IContinuousDistribution distribution;

        public override FireworkType GeneratedSparkType { get { return FireworkType.SpecificSpark; } }     

        /// <summary>
        /// Create instance of AttractRepulseSparkGenerator
        /// </summary>
        /// <param name="dimensions">Dimenstion of research space</param>
        /// <param name="state">Represent current state of algorithm</param>
        /// <param name="distribution">Uniform distribution</param>
        /// <param name="randomizer">Generator for coin flip</param>
        public AttractRepulseSparkGenerator(ref Solution bestSolution, IEnumerable<Dimension> dimensions, IContinuousDistribution distribution, System.Random randomizer)
        {
            if (bestSolution == null) { throw new System.ArgumentNullException("bestSolution"); }            
            if (distribution == null) { throw new System.ArgumentNullException("distribution"); }
            if (randomizer == null) { throw new System.ArgumentNullException("generator"); }
            if (dimensions == null) { throw new System.ArgumentNullException("dimentions"); }

            this.bestSolution = bestSolution;
            this.dimensions = dimensions;
            this.distribution = distribution;
            this.randomizer = randomizer;
        }

        protected override Firework CreateSparkTyped(FireworkExplosion explosion)
        {
            Debug.Assert(explosion != null, "explosion is null");
            Debug.Assert(explosion.ParentFirework != null, "explosion parent firework is null");
            Debug.Assert(explosion.ParentFirework.Coordinates != null, "explosion parent firework coordinate is null");

            if (bestSolution == null) { throw new System.ArgumentNullException("best solution"); }

            Firework spark = new Firework(GeneratedSparkType, explosion.StepNumber, explosion.ParentFirework.Coordinates);

            // attract-repulse scaling factor. (1-δ, 1+δ)
            double scalingFactor = this.distribution.Sample();

            foreach (Dimension dimension in this.dimensions)
            {
                Debug.Assert(dimension != null, "dimension is null");
                Debug.Assert(dimension.VariationRange != null, "dimension variation range is null");
                Debug.Assert(!dimension.VariationRange.Length.IsEqual(0.0), "dimension variation range length is 0");

                if ((int)Math.Round(this.randomizer.NextDouble(0.0, 1.0), MidpointRounding.AwayFromZero) == 1) // Coin flip
                {
                    double d = this.bestSolution.Coordinates[dimension];

                    double d1 = spark.Coordinates[dimension];                   

                    spark.Coordinates[dimension] += (spark.Coordinates[dimension] - bestSolution.Coordinates[dimension]) * scalingFactor;
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
