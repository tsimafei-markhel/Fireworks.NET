using System.Collections.Generic;
using FireworksNet.Distributions;
using FireworksNet.Explode;
using FireworksNet.Model;
using System;
using System.Diagnostics;

namespace FireworksNet.Explode
{
    /// <summary>
    /// Wrapper for AttractRepulseSparkGenerator, as described in 2013 GPU paper.
    /// </summary>
    public class AttractRepulseSparkMutator : IFireworkMutator
    {     
        /// <summary>
        /// Executes Attract-Repulse mutation of firework, as described in 2013 GPU paper.
        /// </summary>
        private ISparkGenerator generator;

        public AttractRepulseSparkMutator(ISparkGenerator generator)
        {
            if (generator == null)
            {
                throw new ArgumentNullException("generator");
            }

            this.generator = generator;
        }

        public void MutateFirework(ref Firework mutableFirework, FireworkExplosion explosion)
        {
            Debug.Assert(mutableFirework != null, "Mutable firework is null");
            Debug.Assert(explosion != null, "Explosion is null");

            //TODO: ar-generator must obtain two parameters: mutable firework and explosion
            Firework updatable = generator.CreateSpark(/*mutableFirework, */ explosion);//???
            
            UpdateFirework(updatable, ref mutableFirework);
        }

        /// <summary>
        /// Updates mutable firework. He simply copy fields.
        /// </summary>
        /// <param name="source">Source firework - from where to copy.</param>
        /// <param name="target">Target firework - where to copy.</param>
        /// <returns></returns>
        private static Firework UpdateFirework(Firework source, ref Firework target)
        {
            source.Coordinates = target.Coordinates;
            source.Quality = target.Quality;
            source.BirthStepNumber = target.BirthStepNumber;//TODO: maybe this field unnecessary     
            source.FireworkType = target.FireworkType; //TODO: maybe this field unnecessary     

            return source;
        }
    }
}
