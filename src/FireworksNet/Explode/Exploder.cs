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
        private readonly double minAllowedExplosionSparksNumberExact;

        /// <summary>
        /// Maximum allowed explosion sparks number (not rounded).
        /// </summary>
        private readonly double maxAllowedExplosionSparksNumberExact;

        /// <summary>
        /// Minimum allowed explosion sparks number (rounded).
        /// </summary>
        private readonly int minAllowedExplosionSparksNumber;

        /// <summary>
        /// Maximum allowed explosion sparks number (rounded).
        /// </summary>
        private readonly int maxAllowedExplosionSparksNumber;

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

        public virtual Explosion Explode(Firework epicenter, IEnumerable<double> currentFireworkQualities, int currentStepNumber)
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

            // TODO: Need further decomposition:
            //       - SparksCountCalculator
            //       - AmplitudeCalculator

            double amplitude = this.CalculateAmplitude(epicenter, currentFireworkQualities);
            IDictionary<FireworkType, int> sparkCounts = new Dictionary<FireworkType, int>()
            {
                { FireworkType.ExplosionSpark, this.CalculateExplosionSparksNumber(epicenter, currentFireworkQualities) },
                { FireworkType.SpecificSpark, this.CalculateSpecificSparksNumber(epicenter, currentFireworkQualities) }
            };

            return new FireworkExplosion(epicenter, currentStepNumber, amplitude, sparkCounts);
        }

        protected virtual double CalculateAmplitude(Firework epicenter, IEnumerable<double> currentFireworkQualities)
        {
            // Using Aggregate() here because Min() won't use my double extensions
            double minFireworkQuality = currentFireworkQualities.Aggregate((agg, next) => next.IsLess(agg) ? next : agg);
            return this.settings.ExplosionSparksMaximumAmplitude * (epicenter.Quality - minFireworkQuality + double.Epsilon) / (currentFireworkQualities.Sum(fq => fq - minFireworkQuality) + double.Epsilon);
        }

        protected virtual int CalculateExplosionSparksNumber(Firework epicenter, IEnumerable<double> currentFireworkQualities)
        {
            double explosionSparksNumberExact = this.CalculateExplosionSparksNumberExact(epicenter, currentFireworkQualities);

            if (explosionSparksNumberExact.IsLess(this.minAllowedExplosionSparksNumberExact))
            {
                return this.minAllowedExplosionSparksNumber;
            }
            else if (explosionSparksNumberExact.IsGreater(this.maxAllowedExplosionSparksNumberExact))
            {
                return this.maxAllowedExplosionSparksNumber;
            }
            else
            {
                return (int)Math.Round(explosionSparksNumberExact, MidpointRounding.AwayFromZero);
            }
        }

        protected virtual int CalculateSpecificSparksNumber(Firework epicenter, IEnumerable<double> currentFireworkQualities)
        {
            return this.settings.SpecificSparksPerExplosionNumber;
        }

        private double CalculateExplosionSparksNumberExact(Firework epicenter, IEnumerable<double> currentFireworkQualities)
        {
            // Using Aggregate() here because Max() won't use my double extensions
            double maxFireworkQuality = currentFireworkQualities.Aggregate((agg, next) => next.IsGreater(agg) ? next : agg);
            return this.settings.ExplosionSparksNumberModifier * (maxFireworkQuality - epicenter.Quality + double.Epsilon) / (currentFireworkQualities.Sum(fq => maxFireworkQuality - fq) + double.Epsilon);
        }
    }
}