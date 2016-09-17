using System;
using System.Collections.Generic;

namespace FireworksNet.Model
{
    /// <summary>
    /// Represents an explosion, which gave rise to a fireworks collection.
    /// </summary>
    public class EliteExplosion : ExplosionBase
    {
        /// <summary>
        /// Gets the fireworks collection.
        /// </summary>
        public IEnumerable<Firework> Fireworks { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EliteExplosion"/> class.
        /// </summary>
        /// <param name="stepNumber">The number of step this explosion took place at.</param>
        /// <param name="fireworksNumber">The number of fireworks.</param>
        /// <param name="fireworks">The collection that stores numbers of fireworks.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"> if <paramref name="fireworksNumber"/>
        /// is less than zero.</exception>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="fireworks"/> 
        /// is <c>null</c>.</exception>
        public EliteExplosion(int stepNumber, int fireworksNumber, IEnumerable<Firework> fireworks)
            : base(stepNumber, new Dictionary<FireworkType, int> { { FireworkType.ExplosionSpark, fireworksNumber } })
        {
            if (fireworksNumber < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(fireworksNumber));
            }

            if (fireworks == null)
            {
                throw new ArgumentNullException(nameof(fireworks));
            }

            this.Fireworks = fireworks;
        }
    }
}
