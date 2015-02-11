using System.Collections.Generic;
using FireworksNet.Model;

namespace FireworksNet.Explode
{
    // TODO: Naming convention for implementations of this interface:
    // - conventional (i.e. per 2010 paper) implementations are named w/o
    //   any prefixes (e.g. ExplosionSparkGenerator)
    // - enhanced/alternative implementations are named with corresponding
    //   prefix (e.g. TODO)

    /// <summary>
    /// A spark generator.
    /// </summary>
    public interface ISparkGenerator
    {
        /// <summary>
        /// Creates the sparks from the explosion.
        /// </summary>
        /// <param name="explosion">The explosion that is the source of sparks.</param>
        /// <returns>A collection of sparks for the specified explosion.</returns>
        IEnumerable<Firework> CreateSparks(Explosion explosion);

        /// <summary>
        /// Creates the spark from the explosion.
        /// </summary>
        /// <param name="explosion">The explosion that is the source of sparks.</param>
        /// <returns>A spark for the specified explosion.</returns>
        Firework CreateSpark(Explosion explosion);
    }
}