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
            if (mutableFirework == null)
            {
                throw new ArgumentNullException("mutableFirework");
            }

            if (explosion == null)
            {
                throw new ArgumentNullException("explosion");
            }

            Firework newState = this.generator.CreateSpark(explosion);
            mutableFirework.Update(newState);
        }
    }
}
