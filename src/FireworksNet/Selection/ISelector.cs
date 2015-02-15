using System.Collections.Generic;
using FireworksNet.Model;

namespace FireworksNet.Selection
{
    /// <summary>
    /// Contains logic for selecting a subset of <see cref="Firework"/>s
    /// from a given set according to some rule.
    /// </summary>
    public interface ISelector
    {
        /// <summary>
        /// Selects some predefined number of <see cref="Firework"/>s from
        /// the <paramref name="from"/> collection.
        /// </summary>
        /// <param name="from">A collection to select <see cref="Firework"/>s
        /// from.</param>
        /// <returns>A subset of <see cref="Firework"/>s.</returns>
        IEnumerable<Firework> Select(IEnumerable<Firework> from);

        /// <summary>
        /// Selects <paramref name="numberToSelect"/> <see cref="Firework"/>s from
        /// the <paramref name="from"/> collection.
        /// </summary>
        /// <param name="from">A collection to select <see cref="Firework"/>s
        /// from.</param>
        /// <param name="numberToSelect">The number of <see cref="Firework"/>s
        /// to select.</param>
        /// <returns>A subset of <see cref="Firework"/>s.</returns>
        IEnumerable<Firework> Select(IEnumerable<Firework> from, int numberToSelect);
    }
}