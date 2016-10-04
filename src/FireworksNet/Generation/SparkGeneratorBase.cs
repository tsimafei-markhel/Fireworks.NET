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
    /// <typeparam name="TExplosion">Type of the explosion.</typeparam>
    public abstract class SparkGeneratorBase<TExplosion> : ISparkGenerator where TExplosion : ExplosionBase
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
        /// does not match <typeparamref name="TExplosion"/>.</exception>
        public virtual IEnumerable<Firework> CreateSparks(ExplosionBase explosion)
        {
            if (explosion == null)
            {
                throw new ArgumentNullException(nameof(explosion));
            }

            int desiredNumberOfSparks;
            if (!explosion.SparkCounts.TryGetValue(this.GeneratedSparkType, out desiredNumberOfSparks))
            {
                return Enumerable.Empty<Firework>();
            }

            TExplosion typedExplosion = SparkGeneratorBase<TExplosion>.GetTypedExplosion(explosion);
            IList<Firework> sparks = new List<Firework>(desiredNumberOfSparks);
            for (int i = 0; i < desiredNumberOfSparks; i++)
            {
                sparks.Add(this.CreateSparkTyped(typedExplosion, i));
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
        /// does not match <typeparamref name="TExplosion"/>.</exception>
        public virtual Firework CreateSpark(ExplosionBase explosion)
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
        /// <exception cref="System.ArgumentNullException"> if <paramref name="explosion"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException"> if <paramref name="birthOrder"/>
        /// is less than zero.</exception>
        /// <exception cref="System.InvalidOperationException"> if <paramref name="explosion"/>
        /// does not match <typeparamref name="TExplosion"/>.</exception>
        public virtual Firework CreateSpark(ExplosionBase explosion, int birthOrder)
        {
            if (explosion == null)
            {
                throw new ArgumentNullException(nameof(explosion));
            }

            if (birthOrder < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(explosion));
            }

            TExplosion typedExplosion = SparkGeneratorBase<TExplosion>.GetTypedExplosion(explosion);
            return this.CreateSparkTyped(typedExplosion, birthOrder);
        }

        /// <summary>
        /// Creates the typed spark.
        /// </summary>
        /// <param name="explosion">The explosion that gives birth to the spark.</param>
        /// <param name="birthOrder">The number of spark in the collection of sparks born by
        /// this generator within one step.</param>
        /// <returns>The new typed spark.</returns>
        protected abstract Firework CreateSparkTyped(TExplosion explosion, int birthOrder);

        /// <summary>
        /// Casts <paramref name="explosion"/> to the <typeparamref name="TExplosion"/>.
        /// </summary>
        /// <param name="explosion">The firework explosion.</param>
        /// <returns>Typed firework explosion.</returns>
        /// <exception cref="System.InvalidOperationException"> if <paramref name="explosion"/>
        /// does not match <typeparamref name="TExplosion"/>.</exception>
        private static TExplosion GetTypedExplosion(ExplosionBase explosion)
        {
            Debug.Assert(explosion != null, "Explosion is null");

            TExplosion typedExplosion = explosion as TExplosion;
            if (typedExplosion == null)
            {
                throw new InvalidOperationException();
            }

            return typedExplosion;
        }
    }
}