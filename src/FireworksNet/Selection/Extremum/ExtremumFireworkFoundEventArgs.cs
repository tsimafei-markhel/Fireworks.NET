using System.Collections.Generic;
using FireworksNet.Model;

namespace FireworksNet.Selection.Extremum
{
    /// <summary>
    /// Arguments of the ExtremumFireworkFound event.
    /// </summary>
    public class ExtremumFireworkFoundEventArgs : ExtremumFireworkFindingEventArgs
    {
        /// <summary>
        /// Gets the extremum <see cref="Firework"/>.
        /// </summary>
        public Firework BestFirework { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ExtremumFireworkFoundEventArgs"/> type.
        /// </summary>
        /// <param name="fireworksToCheck">The collection of <see cref="Firework"/>s to
        /// find the best one among.</param>
        /// <param name="extremumFirework">The extremum <see cref="Firework"/> found.</param>
        public ExtremumFireworkFoundEventArgs(IEnumerable<Firework> fireworksToCheck, Firework extremumFirework)
            : base(fireworksToCheck)
        {
            this.BestFirework = extremumFirework;
        }
    }
}