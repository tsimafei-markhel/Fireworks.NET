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
        private ISparkGenerator generator;

        /// <summary>
        /// Create instance of AttractRepulseSparkMutator.
        /// </summary>
        /// <param name="generator">Attract-Repulse generator for generate spark.</param>
        /// <exception cref="System.ArgumentNullException">
        /// if <paramref name="generator"/> is <c>null</c> </exception>        
        public AttractRepulseSparkMutator(ISparkGenerator generator)
        {
            if (generator == null)
            {
                throw new ArgumentNullException("generator");
            }

            this.generator = generator;
        }

        public void MutateFirework(ref MutableFirework mutableFirework, FireworkExplosion explosion)
        {
            Debug.Assert(mutableFirework != null, "Mutable firework is null");
            Debug.Assert(explosion != null, "Explosion is null");

            Firework newState = this.generator.CreateSpark(explosion);
            mutableFirework.Update(newState);
        }
    }
}
