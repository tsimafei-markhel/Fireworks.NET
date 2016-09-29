using System;
using System.Collections.Generic;

namespace FireworksNet.Model
{
    /// <summary>
    /// Base class used to represent a firework explosion.
    /// </summary>
    public abstract class ExplosionBase
    {
        /// <summary>
        /// Gets a unique identifier of this <see cref="ExplosionBase"/>.
        /// </summary>
        public TId Id { get; private set; }

        /// <summary>
        /// Gets the number of step this explosion took place at.
        /// </summary>
        public int StepNumber { get; private set; }

        /// <summary>
        /// Gets the collection that stores numbers of sparks generated 
        /// during the explosion, per spark (firework) type.
        /// </summary>
        public IDictionary<FireworkType, int> SparkCounts { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExplosionBase"/> class.
        /// </summary>
        /// <param name="stepNumber">The number of step this explosion took place at.</param>
        /// <param name="sparkCounts">The collection that stores numbers of sparks generated 
        /// during the explosion, per spark (firework) type.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"> if <paramref name="stepNumber"/>
        /// is less than zero.</exception>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="sparkCounts"/>
        /// is <c>null</c>.</exception>
        protected ExplosionBase(int stepNumber, IDictionary<FireworkType, int> sparkCounts)
        {
            if (stepNumber < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(stepNumber));
            }
            
            if (sparkCounts == null)
            {
                throw new ArgumentNullException(nameof(sparkCounts));
            }

            this.Id = new TId();
            this.StepNumber = stepNumber;
            this.SparkCounts = sparkCounts;
        }
    }
}