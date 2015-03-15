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

        public GpuExploder(GpuExplodeSettings settings)
        {
            if (settings == null) { throw new System.ArgumentNullException("gpu explode settings"); }
        }

        public ExplosionBase Explode(Firework epicenter, IEnumerable<double> currentFireworkQualities, int currentStepNumber)
        {
            // TODO

            return null;
        }
    }
}
