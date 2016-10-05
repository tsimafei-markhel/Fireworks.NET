using System.Collections.Generic;
using FireworksNet.Model;

namespace FireworksNet.Explode
{
    /// <summary>
    /// An explosion generator.
    /// </summary>
    /// <typeparam name="TExplosion">Type of the explosion generated.</typeparam>
    public interface IExploder<TExplosion> where TExplosion : ExplosionBase
    {
        /// <summary>
        /// Creates an explosion.
        /// </summary>
        /// <param name="focus">The explosion focus (center).</param>
        /// <param name="currentFireworks">The collection of fireworks that exist 
        /// at the moment of explosion.</param>
        /// <param name="currentStepNumber">The current step number.</param>
        /// <returns>New explosion.</returns>
        TExplosion Explode(Firework focus, IEnumerable<Firework> currentFireworks, int currentStepNumber);
    }
}