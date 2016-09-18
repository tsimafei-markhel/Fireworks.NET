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
        /// <param name="coordinates">The firework coordinates.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"> if <paramref name="birthStepNumber"/>is less than zero.</exception>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="coordinates"/>is <c>null</c>.</exception>
        public MutableFirework(FireworkType fireworkType, int birthStepNumber, IDictionary<Dimension, double> coordinates)
            : base(fireworkType, birthStepNumber, coordinates) 
        {
        }

        /// <summary>
        /// Updates the firework by simply copying the fields.
        /// </summary>
        /// <param name="newState">New state of firework.</param>
        public void Update(Firework newState)
        {
            this.BirthStepNumber = newState.BirthStepNumber;
            this.Coordinates = newState.Coordinates;
            this.Quality = newState.Quality;
        }
    }
}
