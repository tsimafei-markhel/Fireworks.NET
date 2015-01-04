using System;
using System.Collections.Generic;
using FireworksNet.Model;

namespace FireworksNet.Explode
{
    public abstract class SparkGenerator<T> : ISparkGenerator where T : Explosion
    {
        public abstract FireworkType GeneratedSparkType { get; }

        public virtual IEnumerable<Firework> CreateSparks(Explosion explosion)
        {
            int desiredNumberOfSparks;
            if (!explosion.SparkCounts.TryGetValue(GeneratedSparkType, out desiredNumberOfSparks))
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
                sparks.Add(CreateSparkTyped(typedExplosion));
            }

            return sparks;
        }

        public virtual Firework CreateSpark(Explosion explosion)
        {
            T typedExplosion = explosion as T;
            if (typedExplosion == null)
            {
                throw new InvalidOperationException();
            }

            return CreateSparkTyped(typedExplosion);
        }

        protected abstract Firework CreateSparkTyped(T explosion);
    }
}