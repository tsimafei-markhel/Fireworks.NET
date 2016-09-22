using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FireworksNet.Extensions;
using FireworksNet.Model;
using FireworksNet.Selection.Extremum;

namespace FireworksNet.Explode
{
    /// <summary>
    /// Explosion generator, per 2010 paper.
    /// </summary>
    public class Exploder : IExploder
    {
        /// <summary>
        /// The exploder settings.
        /// </summary>
        private readonly ExploderSettings settings;

        /// <summary>
        /// The extremum firework selector.
        /// </summary>
        private readonly IExtremumFireworkSelector extremumFireworkSelector;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="Exploder"/> class.
        /// </summary>
        /// <param name="settings">The exploder settings.</param>
        /// <param name="extremumFireworkSelector">The extremum firework selector.</param>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="settings"/>
        /// or <paramref name="extremumFireworkSelector"/> is <c>null</c>.</exception>
        public Exploder(ExploderSettings settings, IExtremumFireworkSelector extremumFireworkSelector)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (extremumFireworkSelector == null)
            {
                throw new ArgumentNullException(nameof(extremumFireworkSelector));
            }

            this.settings = settings;
            this.extremumFireworkSelector = extremumFireworkSelector;

            this.minAllowedExplosionSparksNumberExact = settings.ExplosionSparksNumberLowerBound * settings.ExplosionSparksNumberModifier;
            this.maxAllowedExplosionSparksNumberExact = settings.ExplosionSparksNumberUpperBound * settings.ExplosionSparksNumberModifier;
            this.minAllowedExplosionSparksNumber = (int)Math.Round(this.minAllowedExplosionSparksNumberExact, MidpointRounding.AwayFromZero);
            this.maxAllowedExplosionSparksNumber = (int)Math.Round(this.maxAllowedExplosionSparksNumberExact, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Creates an explosion.
        /// </summary>
        /// <param name="focus">The explosion focus (center).</param>
        /// <param name="currentFireworks">The collection of fireworks that exist 
        /// at the moment of explosion.</param>
        /// <param name="currentStepNumber">The current step number.</param>
        /// <returns>New explosion.</returns>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="focus"/>
        /// or <paramref name="currentFireworks"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException"> if 
        /// <paramref name="currentStepNumber"/> is less than zero or
        /// less than birth step number of the <paramref name="focus"/>.
        /// </exception>
        public virtual ExplosionBase Explode(Firework focus, IEnumerable<Firework> currentFireworks, int currentStepNumber)
        {
            if (focus == null)
            {
                throw new ArgumentNullException(nameof(focus));
            }

            if (currentFireworks == null)
            {
                throw new ArgumentNullException(nameof(currentFireworks));
            }

            if (currentStepNumber < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(currentStepNumber));
            }

            if (currentStepNumber < focus.BirthStepNumber) // Not '<=' here because that would limit possible algorithm implementations
            {
                throw new ArgumentOutOfRangeException(nameof(currentStepNumber));
            }

            // TODO: Add caching based on the step number (and maybe on the current fireworks) - to avoid selecting same qualities multiple times.
            IEnumerable<double> currentFireworkQualities = currentFireworks.Select(fw => fw.Quality);

            Debug.Assert(currentFireworkQualities != null, "Current firework qualities is null");

            double amplitude = this.CalculateAmplitude(focus, currentFireworks, currentFireworkQualities);

            Debug.Assert(!double.IsNaN(amplitude), "Amplitude is NaN");
            Debug.Assert(!double.IsInfinity(amplitude), "Amplitude is Infinity");

            IDictionary<FireworkType, int> sparkCounts = new Dictionary<FireworkType, int>
            {
                { FireworkType.ExplosionSpark, this.CountExplosionSparks(focus, currentFireworkQualities) },
                { FireworkType.SpecificSpark, this.CountSpecificSparks(focus, currentFireworkQualities) }
            };

            return new FireworkExplosion(focus, currentStepNumber, amplitude, sparkCounts);
        }

        /// <summary>
        /// Calculates the explosion amplitude.
        /// </summary>
        /// <param name="focus">The explosion focus.</param>
        /// <param name="currentFireworks">The collection of fireworks that exist at the moment of explosion.</param>
        /// <param name="currentFireworkQualities">The current firework qualities.</param>
        /// <returns>The explosion amplitude.</returns>
        protected virtual double CalculateAmplitude(Firework focus, IEnumerable<Firework> currentFireworks, IEnumerable<double> currentFireworkQualities)
        {
            Debug.Assert(focus != null, "Focus is null");
            Debug.Assert(currentFireworks != null, "Current fireworks is null");
            Debug.Assert(currentFireworkQualities != null, "Current firework qualities is null");
            Debug.Assert(this.extremumFireworkSelector != null, "Extremum firework selector is null");
            Debug.Assert(this.settings != null, "Settings is null");

            double bestFireworkQuality = this.extremumFireworkSelector.SelectBest(currentFireworks).Quality;

            Debug.Assert(!double.IsNaN(bestFireworkQuality), "Best firework quality is NaN");
            Debug.Assert(!double.IsInfinity(bestFireworkQuality), "Best firework quality is Infinity");

            return this.settings.ExplosionSparksMaximumAmplitude * (focus.Quality - bestFireworkQuality + double.Epsilon) / (currentFireworkQualities.Sum(fq => fq - bestFireworkQuality) + double.Epsilon);
        }

        /// <summary>
        /// Defines the count of the explosion sparks.
        /// </summary>
        /// <param name="focus">The explosion focus.</param>
        /// <param name="currentFireworkQualities">The current firework qualities.</param>
        /// <returns>The number of explosion sparks created by that explosion.</returns>
        protected virtual int CountExplosionSparks(Firework focus, IEnumerable<double> currentFireworkQualities)
        {
            Debug.Assert(focus != null, "Focus is null");
            Debug.Assert(currentFireworkQualities != null, "Current firework qualities is null");

            double explosionSparksNumberExact = this.CountExplosionSparksExact(focus, currentFireworkQualities);

            Debug.Assert(!double.IsNaN(explosionSparksNumberExact), "Explosion sparks number exact is NaN");
            Debug.Assert(!double.IsInfinity(explosionSparksNumberExact), "Explosion sparks number exact is Infinity");

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

        /// <summary>
        /// Defines the count of the specific sparks.
        /// </summary>
        /// <param name="focus">The explosion focus.</param>
        /// <param name="currentFireworkQualities">The current firework qualities.</param>
        /// <returns>The number of specific sparks created by that explosion.</returns>
        protected virtual int CountSpecificSparks(Firework focus, IEnumerable<double> currentFireworkQualities)
        {
            Debug.Assert(this.settings != null, "Settings is null");

            return this.settings.SpecificSparksPerExplosionNumber;
        }

        /// <summary>
        /// Defines the exact (not rounded) count of the explosion sparks.
        /// </summary>
        /// <param name="focus">The explosion focus.</param>
        /// <param name="currentFireworkQualities">The current firework qualities.</param>
        /// <returns>The exact (not rounded) number of explosion sparks created by that explosion.</returns>
        private double CountExplosionSparksExact(Firework focus, IEnumerable<double> currentFireworkQualities)
        {
            Debug.Assert(focus != null, "Focus is null");
            Debug.Assert(currentFireworkQualities != null, "Current firework qualities is null");
            Debug.Assert(this.settings != null, "Settings is null");

            // Using Aggregate() here because Max() won't use my double extensions
            double maxFireworkQuality = currentFireworkQualities.Aggregate((agg, next) => next.IsGreater(agg) ? next : agg);

            Debug.Assert(!double.IsNaN(maxFireworkQuality), "Max firework quality is NaN");
            Debug.Assert(!double.IsInfinity(maxFireworkQuality), "Max firework quality is Infinity");

            return this.settings.ExplosionSparksNumberModifier * (maxFireworkQuality - focus.Quality + double.Epsilon) / (currentFireworkQualities.Sum(fq => maxFireworkQuality - fq) + double.Epsilon);
        }
    }
}