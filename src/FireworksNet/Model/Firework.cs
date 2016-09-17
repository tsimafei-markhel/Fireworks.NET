using System;
using System.Collections.Generic;

namespace FireworksNet.Model
{
    /// <summary>
    /// Represents a single firework.
    /// </summary>
    public class Firework : Solution
    {
        /// <summary>
        /// Gets a unique identifier of this <see cref="Firework"/>.
        /// </summary>
        public TId Id { get; private set; }

        /// <summary>
        /// Gets the type of the firework (or spark this firework
        /// has been originated from).
        /// </summary>
        public FireworkType FireworkType { get; protected set; }

        /// <summary>
        /// Gets the number of step this firework was created at.
        /// </summary>
        public int BirthStepNumber { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Firework"/> class.
        /// </summary>
        /// <param name="fireworkType">The type of the firework (or spark this firework
        /// has been originated from).</param>
        /// <param name="birthStepNumber">The number of step this firework was created at.</param>
        /// <param name="coordinates">The firework coordinates.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"> if <paramref name="birthStepNumber"/>
        /// is less than zero.</exception>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="coordinates"/>
        /// is <c>null</c>.</exception>
        public Firework(FireworkType fireworkType, int birthStepNumber, IDictionary<Dimension, double> coordinates)
            : base(coordinates, double.NaN)
        {
            if (birthStepNumber < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(birthStepNumber));
            }

            if (coordinates == null)
            {
                throw new ArgumentNullException(nameof(coordinates));
            }

            this.Id = new TId();
            this.FireworkType = fireworkType;
            this.BirthStepNumber = birthStepNumber;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Firework"/> class.
        /// </summary>
        /// <param name="fireworkType">The type of the firework (or spark this firework
        /// has been originated from).</param>
        /// <param name="birthStepNumber">The number of step this firework was created at.</param>
        public Firework(FireworkType fireworkType, int birthStepNumber)
            : this(fireworkType, birthStepNumber, new Dictionary<Dimension, double>())
        {
        }
    }
}