using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FireworksNet.Model;

namespace FireworksNet.Generation
{
    /// <summary>
    /// Base class for spark generators.
    /// </summary>
    /// <typeparam name="TExplosion">Type of the explosion that produces sparks.</typeparam>
    public abstract class SparkGeneratorBase<TExplosion> : ISparkGenerator<TExplosion> where TExplosion : ExplosionBase
    {
        /// <summary>
        /// Gets the type of the generated spark.
        /// </summary>
        public abstract FireworkType GeneratedSparkType { get; }

        /// <summary>
        /// Creates the sparks from the explosion.
        /// </summary>
        /// <param name="explosion">The explosion that is the source of sparks.</param>
        /// <returns>
        /// A collection of sparks for the specified explosion.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="explosion"/>
        /// is <c>null</c>.</exception>
        public virtual IEnumerable<Firework> CreateSparks(TExplosion explosion)
        {
            if (explosion == null)
            {
                throw new ArgumentNullException(nameof(explosion));
            }

            Debug.Assert(explosion.SparkCounts != null, "Explosion spark counts collection is null");

            int desiredNumberOfSparks;
            if (!explosion.SparkCounts.TryGetValue(this.GeneratedSparkType, out desiredNumberOfSparks))
            {
                return Enumerable.Empty<Firework>();
            }

            IList<Firework> sparks = new List<Firework>(desiredNumberOfSparks);
            for (int i = 0; i < desiredNumberOfSparks; i++)
            {
                sparks.Add(this.CreateSpark(explosion, i));
            }

            return sparks;
        }

        /// <summary>
        /// Creates the spark from the explosion.
        /// </summary>
        /// <param name="explosion">The explosion that is the source of sparks.</param>
        /// <returns>A spark for the specified explosion.</returns>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="explosion"/>
        /// is <c>null</c>.</exception>
        public virtual Firework CreateSpark(TExplosion explosion)
        {
            return this.CreateSpark(explosion, 0);
        }

        /// <summary>
        /// Creates the spark from the explosion.
        /// </summary>
        /// <param name="explosion">The explosion that is the source of sparks.</param>
        /// <param name="birthOrder">The number of spark in the collection of sparks born by
        /// this generator within one step.</param>
        /// <returns>A spark for the specified explosion.</returns>
        public abstract Firework CreateSpark(TExplosion explosion, int birthOrder);
    }
}