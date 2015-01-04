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

            List<Firework> sparks = new List<Firework>(desiredNumberOfSparks);
            for (int i = 0; i < desiredNumberOfSparks; i++)
            {
                sparks.Add(CreateSpark(explosion));
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

            return CreateSpark(typedExplosion);
        }

        protected abstract Firework CreateSpark(T explosion);
    }
}