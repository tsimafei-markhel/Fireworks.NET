using System;
using System.Collections.Generic;

namespace FireworksNet.Model
{
    /// <summary>
    /// Represents an explosion produced by the firework, per 2013 paper.
    /// </summary>
    public class EnhancedFireworkExplosion : ExplosionBase
    {
        /// <summary>
        /// Gets the firework that has exploded (center of an explosion).
        /// </summary>
        public Firework ParentFirework { get; private set; }

        /// <summary>
        /// Gets the amplitudes of an explosion for each dimension.
        /// </summary>
        public IDictionary<Dimension, double> Amplitudes { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FireworkExplosion"/> class.
        /// </summary>
        /// <param name="parentFirework">The firework that has exploded (focus of an
        /// explosion).</param>
        /// <param name="stepNumber">The number of step this explosion took place at.</param>
        /// <param name="amplitudes">The amplitudes of an explosion for each dimension.</param>
        /// <param name="sparkCounts">The collection that stores numbers of sparks generated 
        /// during the explosion, per spark (firework) type.</param>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="parentFirework"/>
        /// or <paramref name="amplitudes"/> is <c>null</c>.</exception>
        public EnhancedFireworkExplosion(Firework parentFirework, int stepNumber, IDictionary<Dimension, double> amplitudes, IDictionary<FireworkType, int> sparkCounts)
            : base(stepNumber, sparkCounts)
        {
            if (parentFirework == null)
            {
                throw new ArgumentNullException(nameof(parentFirework));
            }

            if (amplitudes == null)
            {
                throw new ArgumentOutOfRangeException(nameof(amplitudes));
            }

            this.ParentFirework = parentFirework;
            this.Amplitudes = amplitudes;
        }
    }
}