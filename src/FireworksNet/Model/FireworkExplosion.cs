using System;
using System.Collections.Generic;

namespace FireworksNet.Model
{
    /// <summary>
    /// Represents an explosion produced by the firework.
    /// </summary>
    public class FireworkExplosion : ExplosionBase
    {
        /// <summary>
        /// Gets the firework that has exploded (center of an explosion).
        /// </summary>
        public Firework ParentFirework { get; private set; }

        /// <summary>
        /// Gets the amplitude of an explosion.
        /// </summary>
        public double Amplitude { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FireworkExplosion"/> class.
        /// </summary>
        /// <param name="parentFirework">The firework that has exploded (focus of an
        /// explosion).</param>
        /// <param name="stepNumber">The number of step this explosion took place at.</param>
        /// <param name="amplitude">The amplitude of an explosion.</param>
        /// <param name="sparkCounts">The collection that stores numbers of sparks generated 
        /// during the explosion, per spark (firework) type.</param>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="parentFirework"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException"> if <paramref name="amplitude"/>
        /// is <see cref="Double.NaN"/> or infinity.</exception>
        public FireworkExplosion(Firework parentFirework, int stepNumber, double amplitude, IDictionary<FireworkType, int> sparkCounts)
            : base(stepNumber, sparkCounts)
        {
            if (parentFirework == null)
            {
                throw new ArgumentNullException("parentFirework");
            }

            if (double.IsNaN(amplitude) || double.IsInfinity(amplitude))
            {
                throw new ArgumentOutOfRangeException("amplitude");
            }

            this.ParentFirework = parentFirework;
            this.Amplitude = amplitude;
        }
    }
}