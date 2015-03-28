using FireworksNet.Distributions;
using FireworksNet.Explode;
using FireworksNet.Model;
using System.Collections.Generic;
namespace FireworksNet.Explode
{
    /// <summary>
    /// Wrapper for AttractRepulseSparkGenerator, as described in 2013 paper.
    /// </summary>
    public class AttractRepulseSparkMutator : IFireworkMutator
    {
        private ISparkGenerator attractRepulseSparkGenerator;

        public AttractRepulseSparkMutator(ref Solution bestSolution, IEnumerable<Dimension> dimensions, IContinuousDistribution distribution, System.Random randomizer)
        {
            this.attractRepulseSparkGenerator = new AttractRepulseSparkGenerator(bestSolution, dimensions, distribution, randomizer);
        } 

        public void MutateFirework(ref Firework fireWorkToMutate, FireworkExplosion explosion)
        {
            
        }
    }
}
