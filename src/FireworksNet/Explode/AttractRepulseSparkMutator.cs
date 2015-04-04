using System.Collections.Generic;
using FireworksNet.Distributions;
using FireworksNet.Explode;
using FireworksNet.Model;

namespace FireworksNet.Explode
{
    /// <summary>
    /// Wrapper for AttractRepulseSparkGenerator, as described in 2013 GPU paper.
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
