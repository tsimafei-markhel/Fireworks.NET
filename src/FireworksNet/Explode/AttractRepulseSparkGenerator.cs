using System;
using System.Collections.Generic;
using System.Diagnostics;

using FireworksNet.Distributions;
using FireworksNet.Extensions;
using FireworksNet.Model;
using FireworksNet.Explode;

namespace FireworksNet.Explode
{
    /// <summary>
    /// Implementation of Attract-Repulse mutation algorithm, as described in 2013 paper.
    /// </summary>
    public class AttractRepulseSparkGenerator : SparkGeneratorBase<FireworkExplosion>
    {
        /// <summary>
        /// Present current best solution in global scope.
        /// </summary>
        private readonly Solution bestSolution;               
        private readonly IEnumerable<Dimension> dimensions;
        private readonly IContinuousDistribution distribution;
        private readonly System.Random randomizer;

        public override FireworkType GeneratedSparkType { get { return FireworkType.SpecificSpark; } }     

        /// <summary>
        /// Create instance of AttractRepulseSparkGenerator
        /// </summary>
        /// <param name="dimensions">Dimension of research space.</param>
        /// <param name="state">Represent current state of algorithm.</param>
        /// <param name="distribution">Uniform distribution.</param>
        /// <param name="randomizer">Generator for coin flip.</param>
        public AttractRepulseSparkGenerator(Solution bestSolution, IEnumerable<Dimension> dimensions, IContinuousDistribution distribution, System.Random randomizer)
        {
            if (bestSolution == null) 
            { 
                throw new System.ArgumentNullException("bestSolution"); 
            }               
            
            if (dimensions == null) 
            { 
                throw new System.ArgumentNullException("dimensions"); 
            }

            if (distribution == null)
            {
                throw new System.ArgumentNullException("distribution");
            }

            if (randomizer == null)
            {
                throw new System.ArgumentNullException("randomizer");
            }

            this.bestSolution = bestSolution;
            this.dimensions = dimensions;
            this.distribution = distribution;
            this.randomizer = randomizer;
        }

        protected override Firework CreateSparkTyped(FireworkExplosion explosion)
        {
            Debug.Assert(explosion != null, "Explosion is null");
            Debug.Assert(explosion.ParentFirework != null, "Explosion parent firework is null");
            Debug.Assert(explosion.ParentFirework.Coordinates != null, "Explosion parent firework coordinate is null");

            Firework spark = new Firework(GeneratedSparkType, explosion.StepNumber, explosion.ParentFirework.Coordinates);

            // attract-repulse scaling factor. (1-δ, 1+δ)
            double scalingFactor = this.distribution.Sample();

            foreach (Dimension dimension in this.dimensions)
            {
                Debug.Assert(dimension != null, "Dimension is null");
                Debug.Assert(dimension.VariationRange != null, "Dimension variation range is null");
                Debug.Assert(!dimension.VariationRange.Length.IsEqual(0.0), "Dimension variation range length is 0");

                if ((int)Math.Round(this.randomizer.NextDouble(0.0, 1.0), MidpointRounding.AwayFromZero) == 1) // Coin flip
                {
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
