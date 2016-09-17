using System;
using System.Diagnostics;
using FireworksNet.Generation;
using FireworksNet.Model;

namespace FireworksNet.Mutation
{
    /// <summary>
    /// Wrapper for <see cref="AttractRepulseSparkGenerator"/>, as described in 2013 GPU paper.
    /// </summary>
    public class AttractRepulseSparkMutator : IFireworkMutator
    {
        private readonly ISparkGenerator generator;

        /// <summary>
        /// Initializes a new instance of the <see cref="AttractRepulseSparkMutator"/> class.
        /// </summary>
        /// <param name="generator">Attract-Repulse generator to generate a spark.</param>
        /// <exception cref="System.ArgumentNullException">
        /// if <paramref name="generator"/> is <c>null</c>.</exception>
        public AttractRepulseSparkMutator(ISparkGenerator generator)
        {
            if (generator == null)
            {
                throw new ArgumentNullException(nameof(generator));
            }

            this.generator = generator;
        }

        /// <summary>
        /// Changes the <paramref name="firework"/>.
        /// </summary>
        /// <param name="firework">The <see cref="MutableFirework"/> to be changed.</param>
        /// <param name="explosion">The <see cref="FireworkExplosion"/> that
        /// contains explosion characteristics.</param>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="mutableFirework"/>
        /// or <param name="explosion"/> is <c>null</c>.</exception>
        public void MutateFirework(ref MutableFirework firework, FireworkExplosion explosion)
        {
            if (firework == null)
            {
                throw new ArgumentNullException(nameof(firework));
            }

            if (explosion == null)
            {
                throw new ArgumentNullException(nameof(explosion));
            }

            Debug.Assert(this.generator != null, "Generator is null");

            Firework newState = this.generator.CreateSpark(explosion);
            Debug.Assert(newState != null, "New state is null");

            firework.Update(newState);
        }
    }
}