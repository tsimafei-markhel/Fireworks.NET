using FireworksNet.Distances;
using FireworksNet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireworksNet.Selection.SamplingMethods
{
    public class NearBestSelector : ISelector
    {
        private readonly IDistance distanceCalculator;
        private readonly Func<IEnumerable<Firework>, Firework> bestFireworkSelector;
        private readonly int samplingNumber;

        public NearBestSelector(IDistance distanceCalculator, Func<IEnumerable<Firework>, Firework> bestFireworkSelector, int samplingNumber)
        {
            if (distanceCalculator == null)
            {
                throw new ArgumentNullException("distanceCalculator");
            }

            if (bestFireworkSelector == null)
            {
                throw new ArgumentNullException("bestFireworkSelector");
            }

            if (samplingNumber < 0)
            {
                throw new ArgumentOutOfRangeException("samplingNumber");
            }

            this.samplingNumber = samplingNumber;
            this.distanceCalculator = distanceCalculator;
            this.bestFireworkSelector = bestFireworkSelector;
        }

        public NearBestSelector(IDistance distanceCalculator, Func<IEnumerable<Firework>, Firework> bestFireworkSelector)
            : this(distanceCalculator, bestFireworkSelector, 0)
        {
        }

        public virtual IEnumerable<Firework> Select(IEnumerable<Firework> from)
        {
            return this.Select(from, this.samplingNumber);
        }

        public virtual IEnumerable<Firework> Select(IEnumerable<Firework> from, int numberToSelect)
        {
            List<Firework> selectedLocations = new List<Firework>(numberToSelect);
            return selectedLocations;
        }

    }
}
