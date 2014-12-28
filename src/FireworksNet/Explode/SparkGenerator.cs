using System.Collections.Generic;
using FireworksNet.Model;

namespace FireworksNet.Explode
{
    public abstract class SparkGenerator : ISparkGenerator
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

        public abstract Firework CreateSpark(Explosion explosion);
    }
}
