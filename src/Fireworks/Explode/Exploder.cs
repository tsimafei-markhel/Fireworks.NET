using System;
using System.Collections.Generic;
using System.Linq;
using Fireworks.Extensions;
using Fireworks.Model;

namespace Fireworks.Explode
{
    public class Exploder : IExploder
    {
        private readonly IEnumerable<Double> fireworkQualities;
        private readonly ExploderSettings settings;
        private readonly Double minFireworkQuality;
        private readonly Double maxFireworkQuality;

        public Exploder(IEnumerable<Double> fireworkQualities, ExploderSettings settings)
        {
            if (fireworkQualities == null)
            {
                throw new ArgumentNullException("fireworkQualities");
            }

            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            this.fireworkQualities = fireworkQualities;
            this.settings = settings;

            // Using Aggregate() here because Min() and Max() won't use my Double extensions
            this.minFireworkQuality = fireworkQualities.Aggregate((agg, next) => next.IsLess(agg) ? next : agg);
            this.maxFireworkQuality = fireworkQualities.Aggregate((agg, next) => next.IsGreater(agg) ? next : agg);
        }

        public virtual Explosion Explode(Firework epicenter)
        {
            if (epicenter == null)
            {
                throw new ArgumentNullException("epicenter");
            }

            double amplitude = CalculateAmplitude(epicenter);
            IDictionary<FireworkType, int> sparkCounts = new Dictionary<FireworkType, int>()
            {
                { FireworkType.ExplosionSpark, CalculateExplosionSparksNumber(epicenter) },
                { FireworkType.SpecificSpark, CalculateSpecificSparksNumber(epicenter) }
            };

            return new Explosion(epicenter, amplitude, sparkCounts);
        }

        protected virtual Double CalculateAmplitude(Firework epicenter)
        {
            return settings.ExplosionSparksMaximumAmplitude * (epicenter.Quality - minFireworkQuality + double.Epsilon) / (fireworkQualities.Sum(fq => fq - minFireworkQuality) + double.Epsilon);
        }

        protected virtual Int32 CalculateExplosionSparksNumber(Firework epicenter)
        {
            double explosionSparksNumberExact = CalculateExplosionSparksNumberExact(epicenter.Quality);

            if (explosionSparksNumberExact.IsLess(settings.ExplosionSparksNumberLowerBound * settings.ExplosionSparksNumberModifier))
            {
                return (int)Math.Round(settings.ExplosionSparksNumberLowerBound * settings.ExplosionSparksNumberModifier, MidpointRounding.AwayFromZero);
            }
            else if (explosionSparksNumberExact.IsGreater(settings.ExplosionSparksNumberUpperBound * settings.ExplosionSparksNumberModifier))
            {
                return (int)Math.Round(settings.ExplosionSparksNumberUpperBound * settings.ExplosionSparksNumberModifier, MidpointRounding.AwayFromZero);
            }
            else
            {
                return (int)Math.Round(explosionSparksNumberExact, MidpointRounding.AwayFromZero);
            }
        }

        protected virtual Int32 CalculateSpecificSparksNumber(Firework epicenter)
        {
            return settings.SpecificSparksNumber;
        }

        private Double CalculateExplosionSparksNumberExact(Double explodedFireworkQuality)
        {
            return settings.ExplosionSparksNumberModifier * (maxFireworkQuality - explodedFireworkQuality + double.Epsilon) / (fireworkQualities.Sum(fq => maxFireworkQuality - fq) + double.Epsilon);
        }
    }
}