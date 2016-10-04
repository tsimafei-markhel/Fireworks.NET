using System;
using System.Collections.Generic;
using System.Globalization;

namespace FireworksNet.Model
{
    /// <summary>
    /// Represents a single firework.
    /// </summary>
    public class Firework : Solution
    {
        /// <summary>
        /// Firework label format: {BirthStepNumber}.{FireworkType}.{BirthOrder}
        /// </summary>
        protected const string LabelFormat = "{0}.{1}.{2}";

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
        /// Gets the number of firework in the collection of fireworks born by
        /// the same generator within one step.
        /// </summary>
        public int BirthOrder { get; protected set; }

        /// <summary>
        /// Gets the firework label that can be used to easily distinguish it
        /// from other fireworks: {BirthStepNumber}.{FireworkType}.{BirthOrder}
        /// </summary>
        public virtual string Label
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, Firework.LabelFormat, this.BirthStepNumber, this.FireworkType, this.BirthOrder);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Firework"/> class.
        /// </summary>
        /// <param name="fireworkType">The type of the firework (or spark this firework
        /// has been originated from).</param>
        /// <param name="birthStepNumber">The number of step this firework was created at.</param>
        /// <param name="birthOrder">The number of firework in the collection of fireworks born by
        /// the same generator within one step.</param>
        /// <param name="coordinates">The firework coordinates.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"> if <paramref name="birthStepNumber"/>
        /// or <paramref name="birthOrder"/> is less than zero.</exception>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="coordinates"/>
        /// is <c>null</c>.</exception>
        public Firework(FireworkType fireworkType, int birthStepNumber, int birthOrder, IDictionary<Dimension, double> coordinates)
            : base(coordinates, double.NaN)
        {
            if (birthStepNumber < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(birthStepNumber));
            }

            if (birthOrder < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(birthOrder));
            }

            if (coordinates == null)
            {
                throw new ArgumentNullException(nameof(coordinates));
            }

            this.Id = new TId();
            this.FireworkType = fireworkType;
            this.BirthStepNumber = birthStepNumber;
            this.BirthOrder = birthOrder;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Firework"/> class.
        /// </summary>
        /// <param name="fireworkType">The type of the firework (or spark this firework
        /// has been originated from).</param>
        /// <param name="birthStepNumber">The number of step this firework was created at.</param>
        /// <param name="birthOrder">The number of firework in the collection of fireworks born by
        /// the same generator within one step.</param>
        public Firework(FireworkType fireworkType, int birthStepNumber, int birthOrder)
            : this(fireworkType, birthStepNumber, birthOrder, new Dictionary<Dimension, double>())
        {
        }
    }
}