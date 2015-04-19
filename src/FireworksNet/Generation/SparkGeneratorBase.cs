using System;
using System.Collections.Generic;
using FireworksNet.Model;

namespace FireworksNet.Generation
{
    /// <summary>
    /// Base class for spark generators.
    /// </summary>
    /// <typeparam name="T">Type of the spark that can be generated.</typeparam>
    public abstract class SparkGeneratorBase<T> : ISparkGenerator where T : ExplosionBase
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
        /// <exception cref="System.InvalidOperationException"> if <paramref name="explosion"/>
        /// does not match <typeparamref name="T"/>.</exception>
        public virtual IEnumerable<Firework> CreateSparks(ExplosionBase explosion)
        {
            if (explosion == null)
            {
                throw new ArgumentNullException("explosion");
            }

            int desiredNumberOfSparks;
            if (!explosion.SparkCounts.TryGetValue(this.GeneratedSparkType, out desiredNumberOfSparks))
            {
                return new List<Firework>();
            }

            T typedExplosion = explosion as T;
            if (typedExplosion == null)
            {
                throw new InvalidOperationException();
            }

            List<Firework> sparks = new List<Firework>(desiredNumberOfSparks);
            for (int i = 0; i < desiredNumberOfSparks; i++)
            {
                sparks.Add(this.CreateSparkTyped(typedExplosion));
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
        /// <exception cref="System.InvalidOperationException"> if <paramref name="explosion"/>
        /// does not match <typeparamref name="T"/>.</exception>
        public virtual Firework CreateSpark(ExplosionBase explosion)
        {
            if (explosion == null)
            {
                throw new ArgumentNullException("explosion");
            }

            T typedExplosion = explosion as T;
            if (typedExplosion == null)
            {
                throw new InvalidOperationException();
            }

            return this.CreateSparkTyped(typedExplosion);
        }

        /// <summary>
        /// Creates the typed spark.
        /// </summary>
        /// <param name="explosion">The explosion that gives birth to the spark.</param>
        /// <returns>The new typed spark.</returns>
        protected abstract Firework CreateSparkTyped(T explosion);
    }
}