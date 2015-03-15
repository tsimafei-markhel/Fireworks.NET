using System.Collections.Generic;
using FireworksNet.Explode;
using FireworksNet.Model;

namespace FireworksGpu.GpuExplode
{
    public class GpuExploder : IExploder
    {
        /// <summary>
        /// Settings for GpuExploder
        /// </summary>
        private readonly GpuExplodeSettings settings;

        public ExplosionBase Explode(Firework epicenter, IEnumerable<double> currentFireworkQualities, int currentStepNumber)
        {
            
            // TODO
            
            IEnumerable<FireworkType, int> sparks = new IEnumerable<FireworkType,int>()
            {

            }

            return null;
        }
    }
}
