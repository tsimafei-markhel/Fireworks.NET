using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FireworksNet.Extensions;
using FireworksNet.Model;
using System.Diagnostics;

namespace FireworksNet.Selection.SamplingMethods
{
    public class BestSelector : ISelector
    {
        private readonly Func<IEnumerable<Firework>, Firework> bestFireworkSelector;
        private readonly int samplingNumber;

        public BestSelector(Func<IEnumerable<Firework>, Firework> bestFireworkSelector, int samplingNumber)
        {
            if (samplingNumber < 0)
            {
                throw new ArgumentOutOfRangeException("samplingNumber");
            }

            if (bestFireworkSelector == null)
            {
                throw new ArgumentNullException("bestFireworkSelector");
            }

            this.bestFireworkSelector = bestFireworkSelector;
            this.samplingNumber = samplingNumber;
        }

        public BestSelector(Func<IEnumerable<Firework>, Firework> bestFireworkSelector)
            : this(bestFireworkSelector, 0)
        {
        }

        public virtual IEnumerable<Firework> Select(IEnumerable<Firework> from)
        {
            return this.Select(from, this.samplingNumber);
        }

        public virtual IEnumerable<Firework> Select(IEnumerable<Firework> from, int numberToSelect)
        {
            if (from == null)
            {
                throw new ArgumentNullException("from");
            }

            if (numberToSelect < 0)
            {
                throw new ArgumentOutOfRangeException("numberToSelect");
            }

            if (numberToSelect > from.Count())
            {
                throw new ArgumentOutOfRangeException("numberToSelect");
            }

            if (numberToSelect == from.Count())
            {
                return new List<Firework>(from);
            }

            List<Firework> bestFireworks = new List<Firework>(numberToSelect);
            if (numberToSelect == 0)
            {
                return bestFireworks;
            }

            Debug.Assert(this.bestFireworkSelector != null, "Best firework selector is null");

            if (numberToSelect >= 1)
            {
                // Find number of fireworks with best quality based on samling number
                IList<Firework> qualityLocations = this.CalculateQualities(from, numberToSelect);
                bestFireworks.AddRange(qualityLocations);
            }

            return bestFireworks;
        }

        protected virtual IList<Firework> CalculateQualities(IEnumerable<Firework> allCurrentFireworks, int numberToSelect)
        {
            if (allCurrentFireworks == null)
            {
                throw new ArgumentNullException("allCurrentFireworks");
            }

            IList<Firework> currentFireworks = this.CopyingCurrentFireworks(allCurrentFireworks);

            List<Firework> qualityLocations = new List<Firework>(numberToSelect);
            for (int i = 0; i < numberToSelect; i++)
            {
                Firework firework = this.bestFireworkSelector(currentFireworks);
                qualityLocations.Add(firework);
                currentFireworks.Remove(firework);
            }
            return qualityLocations;
        }

        // What is the correct way to do?
        protected virtual IList<Firework> CopyingCurrentFireworks(IEnumerable<Firework> currentFireworks)
        {
            List<Firework> copyCurrentFireworks = new List<Firework>(currentFireworks.Count());
            foreach (Firework firework in currentFireworks)
            {
                Debug.Assert(firework != null, "Firework is null");

                copyCurrentFireworks.Add(firework);
            }

            return copyCurrentFireworks;
        }
    }
}
