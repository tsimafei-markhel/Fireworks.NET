using System;
using System.Collections.Generic;
using System.Diagnostics;
using FireworksNet.Distributions;
using FireworksNet.Extensions;
using FireworksNet.Model;
using FireworksNet.Explode;

namespace FireworksGpu.GpuExplode
{
    /// <summary>
    /// Implementation of Attract-Repulse mutation algorithm
    /// </summary>
    public class GpuAttractRepulseSparkGenerator : SparkGeneratorBase<FireworkExplosion>
    {        
        private readonly System.Random generator;
        private readonly AlgorithmState state;
        private readonly IEnumerable<Dimension> dimensions;
        private readonly IContinuousDistribution distribution;

        public override FireworkType GeneratedSparkType { get { return FireworkType.SpecificSpark; } }

        /// <summary>
        /// Create instance of GpuAttractRepulseSparkGenerator
        /// </summary>
        /// <param name="dimensions">dimenstion of research space</param>
        /// <param name="state">represent current state of algorithm</param>
        /// <param name="distribution">uniform distribution</param>
        /// <param name="generator">generator for coin flip</param>
        public GpuAttractRepulseSparkGenerator(AlgorithmState state, IEnumerable<Dimension> dimensions, IContinuousDistribution distribution, System.Random generator)
        {
            if (state == null) { throw new System.ArgumentNullException("algorithm state"); }
            if (dimensions == null) { throw new System.ArgumentNullException("dimentions"); }
            if (distribution == null) { throw new System.ArgumentNullException("distribution"); }
            if (generator == null) { throw new System.ArgumentNullException("generator"); }

            this.state = state;
            this.dimensions = dimensions;
            this.distribution = distribution;
            this.generator = generator;
        }

        protected override Firework CreateSparkTyped(FireworkExplosion explosion)
        {
            Debug.Assert(explosion != null, "explosion is null");
            Debug.Assert(explosion.ParentFirework != null, "explosion parent firework is null");
            Debug.Assert(explosion.ParentFirework.Coordinates != null, "explosion parent firework coordinate is null");
            
            Firework spark = new Firework(GeneratedSparkType, explosion.StepNumber, explosion.ParentFirework.Coordinates);

            // attract-repulse scaling factor. (1-δ, 1+δ)
            double scalingFactor = distribution.Sample();

            foreach (Dimension dimension in dimensions)
            {
                Debug.Assert(dimension != null, "dimension is null");
                Debug.Assert(dimension.VariationRange != null, "dimension variation range is null");
                Debug.Assert(!dimension.VariationRange.Length.IsEqual(0.0), "dimension variation range length is 0");

                if (generator.NextDouble(0.0, 1.0) < 0.5) // coin flip
                {
                    spark.Coordinates[dimension] += (spark.Coordinates[dimension] - state.BestSolution.Coordinates[dimension]) * scalingFactor;
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
