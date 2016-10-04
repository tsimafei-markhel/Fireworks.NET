using System.Collections.Generic;
using FireworksNet.Model;

namespace FireworksNet.Generation
{
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
        IEnumerable<Firework> CreateSparks(ExplosionBase explosion);

        /// <summary>
        /// Creates the spark from the explosion.
        /// </summary>
        /// <param name="explosion">The explosion that is the source of sparks.</param>
        /// <returns>A spark for the specified explosion.</returns>
        Firework CreateSpark(ExplosionBase explosion);

        /// <summary>
        /// Creates the spark from the explosion.
        /// </summary>
        /// <param name="explosion">The explosion that is the source of sparks.</param>
        /// <param name="birthOrder">The number of spark in the collection of sparks born by
        /// this generator within one step.</param>
        /// <returns>A spark for the specified explosion.</returns>
        Firework CreateSpark(ExplosionBase explosion, int birthOrder);
    }
}