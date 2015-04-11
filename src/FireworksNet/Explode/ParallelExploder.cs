using System.Collections.Generic;
using FireworksNet.Explode;
using FireworksNet.Model;

namespace FireworksNet.Explode
{
    /// <summary>
    /// Represent exploder for gpu based implementation of Fireworks algorithm, as described in 2013 GPU paper.
    /// </summary>
    public class ParallelExploder : IExploder
    {
        private readonly ParallelExploderSettings settings;

        /// <summary>
        /// Create instance of ParallelExploder.
        /// </summary>
        /// <param name="settings">settings for ParallelExploder</param>
        public ParallelExploder(ParallelExploderSettings settings)
        {
            if (settings == null) 
            {
                throw new System.ArgumentNullException("settings"); 
            }

            this.settings = settings;
        }

        public ExplosionBase Explode(Firework epicenter, IEnumerable<double> currentFireworkQualities, int currentStepNumber)
        {
            if (epicenter == null) 
            { 
                throw new System.ArgumentNullException("epicenter"); 
            }
            
            if (currentFireworkQualities == null) 
            { 
                throw new System.ArgumentNullException("currentFireworkQualities"); 
            }
            
            if (currentStepNumber < 0) 
            {
                throw new System.ArgumentOutOfRangeException("currentStepNumber");
            }

            IDictionary<FireworkType, int> sparks = new Dictionary<FireworkType, int>()
            {
                {FireworkType.ExplosionSpark, this.settings.FixedQuantitySparks},
                {FireworkType.SpecificSpark, this.settings.FixedQuantitySparks}
            };

            return new FireworkExplosion(epicenter, currentStepNumber, this.settings.Amplitude, sparks);
        }
    }
}
