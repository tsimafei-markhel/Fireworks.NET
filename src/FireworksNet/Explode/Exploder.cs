using System;
using System.Collections.Generic;
using System.Linq;
using FireworksNet.Extensions;
using FireworksNet.Model;

namespace FireworksNet.Explode
{
    public class Exploder : IExploder
    {
        private readonly ExploderSettings settings;

        /// <summary>
        /// Minimum allowed explosion sparks number (not rounded).
        /// </summary>
        private readonly Double minAllowedExplosionSparksNumberExact;

        /// <summary>
        /// Maximum allowed explosion sparks number (not rounded).
        /// </summary>
        private readonly Double maxAllowedExplosionSparksNumberExact;

        /// <summary>
        /// Minimum allowed explosion sparks number (rounded).
        /// </summary>
        private readonly Int32 minAllowedExplosionSparksNumber;

        /// <summary>
        /// Maximum allowed explosion sparks number (rounded).
        /// </summary>
        private readonly Int32 maxAllowedExplosionSparksNumber;

        public Exploder(ExploderSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            this.settings = settings;

            this.minAllowedExplosionSparksNumberExact = settings.ExplosionSparksNumberLowerBound * settings.ExplosionSparksNumberModifier;
            this.maxAllowedExplosionSparksNumberExact = settings.ExplosionSparksNumberUpperBound * settings.ExplosionSparksNumberModifier;
            this.minAllowedExplosionSparksNumber = (int)Math.Round(this.minAllowedExplosionSparksNumberExact, MidpointRounding.AwayFromZero);
            this.maxAllowedExplosionSparksNumber = (int)Math.Round(this.maxAllowedExplosionSparksNumberExact, MidpointRounding.AwayFromZero);
        }

        public virtual Explosion Explode(Firework epicenter, IEnumerable<Double> currentFireworkQualities, Int32 currentStepNumber)
        {
            if (epicenter == null)
            {
                throw new ArgumentNullException("epicenter");
            }

            if (currentFireworkQualities == null)
            {
                throw new ArgumentNullException("currentFireworkQualities");
            }

            if (currentStepNumber < 0)
            {
                throw new ArgumentOutOfRangeException("currentStepNumber");
            }

            if (currentStepNumber < epicenter.BirthStepNumber) // Not '<=' here because that would limit possible algorithm implementations
            {
                throw new ArgumentOutOfRangeException("currentStepNumber");
            }

            double amplitude = CalculateAmplitude(epicenter, currentFireworkQualities);
            IDictionary<FireworkType, int> sparkCounts = new Dictionary<FireworkType, int>()
            {
                // TODO: Need further decomposition (e.g. ExplosionSparksNumberCalculator)?..
                { FireworkType.ExplosionSpark, CalculateExplosionSparksNumber(epicenter, currentFireworkQualities) },
                { FireworkType.SpecificSpark, CalculateSpecificSparksNumber(epicenter, currentFireworkQualities) }
            };

            return new FireworkExplosion(epicenter, currentStepNumber, amplitude, sparkCounts);
        }

        protected virtual Double CalculateAmplitude(Firework epicenter, IEnumerable<Double> currentFireworkQualities)
        {
            // Using Aggregate() here because Min() won't use my Double extensions
            double minFireworkQuality = currentFireworkQualities.Aggregate((agg, next) => next.IsLess(agg) ? next : agg);
            return settings.ExplosionSparksMaximumAmplitude * (epicenter.Quality - minFireworkQuality + double.Epsilon) / (currentFireworkQualities.Sum(fq => fq - minFireworkQuality) + double.Epsilon);
        }

        protected virtual Int32 CalculateExplosionSparksNumber(Firework epicenter, IEnumerable<Double> currentFireworkQualities)
        {
            double explosionSparksNumberExact = CalculateExplosionSparksNumberExact(epicenter, currentFireworkQualities);

            if (explosionSparksNumberExact.IsLess(minAllowedExplosionSparksNumberExact))
            {
                return minAllowedExplosionSparksNumber;
            }
            else if (explosionSparksNumberExact.IsGreater(maxAllowedExplosionSparksNumberExact))
            {
                return maxAllowedExplosionSparksNumber;
            }
            else
            {
                return (int)Math.Round(explosionSparksNumberExact, MidpointRounding.AwayFromZero);
            }
        }

        protected virtual Int32 CalculateSpecificSparksNumber(Firework epicenter, IEnumerable<Double> currentFireworkQualities)
        {
            return settings.SpecificSparksPerExplosionNumber;
        }

        private Double CalculateExplosionSparksNumberExact(Firework epicenter, IEnumerable<Double> currentFireworkQualities)
        {
            // Using Aggregate() here because Max() won't use my Double extensions
            double maxFireworkQuality = currentFireworkQualities.Aggregate((agg, next) => next.IsGreater(agg) ? next : agg);
            return settings.ExplosionSparksNumberModifier * (maxFireworkQuality - epicenter.Quality + double.Epsilon) / (currentFireworkQualities.Sum(fq => maxFireworkQuality - fq) + double.Epsilon);
        }
    }
}