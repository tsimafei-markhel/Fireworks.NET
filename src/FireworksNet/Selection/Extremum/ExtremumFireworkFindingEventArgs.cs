using System;
using System.Collections.Generic;
using FireworksNet.Model;

namespace FireworksNet.Selection.Extremum
{
    /// <summary>
    /// Arguments of the ExtremumFireworkFinding event.
    /// </summary>
    public class ExtremumFireworkFindingEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the collection of <see cref="Firework"/>s to find the extremum one among.
        /// </summary>
        public IEnumerable<Firework> FireworksToCheck { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ExtremumFireworkFindingEventArgs"/> type.
        /// </summary>
        /// <param name="fireworksToCheck">The collection of <see cref="Firework"/>s to
        /// find the extremum one among.</param>
        public ExtremumFireworkFindingEventArgs(IEnumerable<Firework> fireworksToCheck)
        {
            this.FireworksToCheck = fireworksToCheck;
        }
    }
}