using FireworksNet.Distributions;
using FireworksNet.Model;
using System.Collections.Generic;
namespace FireworksNet.ParallelExplode
{
    /// <summary>
    /// Wrapper for AttractRepulseSparkGenerator
    /// </summary>
    public class AttractRepulseSparkMutator : AttractRepulseSparkGenerator, IFireworkMutator
    {
        public AttractRepulseSparkMutator(IEnumerable<Dimension> dimensions, IContinuousDistribution distribution, System.Random randomizer)
          : base(dimensions, distribution, randomizer)
        {            
        } 

        public void MutateFirework(ref Model.Firework bestFirework, FireworkExplosion explosion)
        {
            BestSolution = bestFirework;
            CreateSparks(explosion);
        }
    }
}
