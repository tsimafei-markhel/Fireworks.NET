using System.Collections.Generic;

namespace FireworksNet.Model
{
    /// <summary>
    /// Firework that provides an ability to change its state.
    /// </summary>
    public class MutableFirework : Firework
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MutableFirework"/> class.
        /// </summary>
        /// <param name="fireworkType">The type of the firework (or spark this firework has been originated from).</param>
        /// <param name="birthStepNumber">The number of step this firework was created at.</param>
        /// <param name="birthOrder">The number of firework in the collection of fireworks born by
        /// the same generator within one step.</param>
        /// <param name="coordinates">The firework coordinates.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"> if <paramref name="birthStepNumber"/>
        /// or <paramref name="birthOrder"/> is less than zero.</exception>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="coordinates"/>is <c>null</c>.</exception>
        public MutableFirework(FireworkType fireworkType, int birthStepNumber, int birthOrder, IDictionary<Dimension, double> coordinates)
            : base(fireworkType, birthStepNumber, birthOrder, coordinates)
        {
        }

        /// <summary>
        /// Updates the firework by simply copying the fields.
        /// </summary>
        /// <param name="newState">The new firework to copy values from.</param>
        /// <remarks>Only copies BirthStepNumber, Coordinates and Quality.</remarks>
        public void Update(Firework newState)
        {
            this.BirthStepNumber = newState.BirthStepNumber;
            this.Coordinates = newState.Coordinates;
            this.Quality = newState.Quality;
        }
    }
}