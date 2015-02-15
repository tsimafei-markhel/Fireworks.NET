using System;
using System.Collections.Generic;
using FireworksNet.Model;

namespace FireworksNet.Explode
{
    public abstract class SparkGeneratorBase<T> : ISparkGenerator where T : ExplosionBase
    {
        public abstract FireworkType GeneratedSparkType { get; }

        public virtual IEnumerable<Firework> CreateSparks(ExplosionBase explosion)
        {
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

        public virtual Firework CreateSpark(ExplosionBase explosion)
        {
            T typedExplosion = explosion as T;
            if (typedExplosion == null)
            {
                throw new InvalidOperationException();
            }

            return this.CreateSparkTyped(typedExplosion);
        }

        protected abstract Firework CreateSparkTyped(T explosion);
    }
}