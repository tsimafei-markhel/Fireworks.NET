using System.Collections.Generic;
using FireworksNet.Model;

namespace FireworksNet.Explode
{
    /// <summary>
    /// An explosion generator.
    /// </summary>
    public interface IExploder
    {
        /// <summary>
        /// Creates an explosion.
        /// </summary>
        /// <param name="epicenter">The explosion epicenter.</param>
        /// <param name="currentFireworkQualities">The qualities of fireworks that exist 
        /// at the moment of explosion.</param>
        /// <param name="currentStepNumber">The current step number.</param>
        /// <returns>New explosion.</returns>
        Explosion Explode(Firework epicenter, IEnumerable<double> currentFireworkQualities, int currentStepNumber);
    }
}