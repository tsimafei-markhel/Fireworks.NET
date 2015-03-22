using FireworksNet.Distributions;
using FireworksNet.Explode;
using FireworksNet.Model;
using System.Collections.Generic;
namespace FireworksNet.ParallelExplode
{
    /// <summary>
    /// Wrapper for AttractRepulseSparkGenerator
    /// </summary>
    public class AttractRepulseSparkMutator : IFireworkMutator
    {
        private ISparkGenerator attractRepulseSparkGenerator;

        public AttractRepulseSparkMutator(ref Solution bestSolution, IEnumerable<Dimension> dimensions, IContinuousDistribution distribution, System.Random randomizer)
        {
            this.attractRepulseSparkGenerator = new AttractRepulseSparkGenerator(ref bestSolution, dimensions, distribution, randomizer);
        } 

        public void MutateFirework(ref Firework fireWorkToMutate, FireworkExplosion explosion)
        {
            // ??????????????
            attractRepulseSparkGenerator.CreateSparks(explosion);
        }
    }
}
