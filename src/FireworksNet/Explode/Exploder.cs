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

        public virtual ExplosionBase Explode(Firework focus, IEnumerable<double> currentFireworkQualities, int currentStepNumber)
        {
            if (focus == null)
            {
                throw new ArgumentNullException("focus");
            }

            if (currentFireworkQualities == null)
            {
                throw new ArgumentNullException("currentFireworkQualities");
            }

            if (currentStepNumber < 0)
            {
                throw new ArgumentOutOfRangeException("currentStepNumber");
            }

            if (currentStepNumber < focus.BirthStepNumber) // Not '<=' here because that would limit possible algorithm implementations
            {
                throw new ArgumentOutOfRangeException("currentStepNumber");
            }

            double amplitude = this.CalculateAmplitude(focus, currentFireworkQualities);
            IDictionary<FireworkType, int> sparkCounts = new Dictionary<FireworkType, int>()
            {
                { FireworkType.ExplosionSpark, this.CountExplosionSparks(focus, currentFireworkQualities) },
                { FireworkType.SpecificSpark, this.CountSpecificSparks(focus, currentFireworkQualities) }
            };

            return new FireworkExplosion(focus, currentStepNumber, amplitude, sparkCounts);
        }

        protected virtual double CalculateAmplitude(Firework focus, IEnumerable<double> currentFireworkQualities)
        {
            // Using Aggregate() here because Min() won't use my double extensions
            double minFireworkQuality = currentFireworkQualities.Aggregate((agg, next) => next.IsLess(agg) ? next : agg);
            return this.settings.ExplosionSparksMaximumAmplitude * (focus.Quality - minFireworkQuality + double.Epsilon) / (currentFireworkQualities.Sum(fq => fq - minFireworkQuality) + double.Epsilon);
        }

        protected virtual int CountExplosionSparks(Firework focus, IEnumerable<double> currentFireworkQualities)
        {
            double explosionSparksNumberExact = this.CountExplosionSparksExact(focus, currentFireworkQualities);
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

        protected virtual int CountSpecificSparks(Firework focus, IEnumerable<double> currentFireworkQualities)
        {
            return this.settings.SpecificSparksPerExplosionNumber;
        }

        private double CountExplosionSparksExact(Firework focus, IEnumerable<double> currentFireworkQualities)
        {
            // Using Aggregate() here because Max() won't use my double extensions
            double maxFireworkQuality = currentFireworkQualities.Aggregate((agg, next) => next.IsGreater(agg) ? next : agg);
            return this.settings.ExplosionSparksNumberModifier * (maxFireworkQuality - focus.Quality + double.Epsilon) / (currentFireworkQualities.Sum(fq => maxFireworkQuality - fq) + double.Epsilon);
        }
    }
}