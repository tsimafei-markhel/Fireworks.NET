using System.Collections.Generic;
using FireworksNet.Explode;
using FireworksNet.Model;

namespace FireworksGpu.GpuExplode
{
    public class GpuExploder : IExploder
    {       
        private readonly GpuExplodeSettings settings;

        /// <summary>
        /// Create instance of GpuExploder
        /// </summary>
        /// <param name="settings">settings for GpuExploder</param>
        public GpuExploder(GpuExplodeSettings settings)
        {
            if (settings == null) { throw new System.ArgumentNullException("gpu explode settings"); }
        }

        public ExplosionBase Explode(Firework epicenter, IEnumerable<double> currentFireworkQualities, int currentStepNumber)
        {
            if (epicenter == null) { throw new System.ArgumentNullException("epicenter cannot be null"); }
            if (currentFireworkQualities == null) { throw new System.ArgumentNullException("current firework qualities cannot be null"); }
            if (currentStepNumber < 0) { throw new System.ArgumentNullException("current step number cannot be negative"); }

            IDictionary<FireworkType, int> sparks = new Dictionary<FireworkType, int>()
            {
                {FireworkType.ExplosionSpark, settings.FixedQuantitySparks},
                {FireworkType.SpecificSpark, settings.FixedQuantitySparks}
            };

            return new FireworkExplosion(epicenter, currentStepNumber, settings.Amplitude, sparks);
        }
    }
}
