using System.Collections.Generic;
using FireworksNet.Model;

namespace FireworksNet.Selection
{
    /// <summary>
    /// Contains logic for selecting a subset of <see cref="Firework"/>s
    /// from a given set according to some rule.
    /// </summary>
    public interface IFireworkSelector
    {
        /// <summary>
        /// Selects some predefined number of <see cref="Firework"/>s from
        /// the <paramref name="from"/> collection.
        /// </summary>
        /// <param name="from">A collection to select <see cref="Firework"/>s
        /// from.</param>
        /// <returns>A subset of <see cref="Firework"/>s.</returns>
        IEnumerable<Firework> SelectFireworks(IEnumerable<Firework> from);

        /// <summary>
        /// Selects <paramref name="numberToSelect"/> <see cref="Firework"/>s from
        /// the <paramref name="from"/> collection.
        /// </summary>
        /// <param name="from">A collection to select <see cref="Firework"/>s
        /// from.</param>
        /// <param name="numberToSelect">The number of <see cref="Firework"/>s
        /// to select.</param>
        /// <returns>A subset of <see cref="Firework"/>s.</returns>
        IEnumerable<Firework> SelectFireworks(IEnumerable<Firework> from, int numberToSelect);

        /// <summary>
        /// Selects some predefined number of <see cref="Firework"/>s from
        /// the <paramref name="from"/> parameters.
        /// </summary>
        /// <param name="from"><see cref="Firework"/>s to select from.</param>
        /// <returns>A subset of <see cref="Firework"/>s.</returns>
        IEnumerable<Firework> SelectFireworks(params Firework[] from);

        /// <summary>
        /// Selects <paramref name="numberToSelect"/> <see cref="Firework"/>s from
        /// the <paramref name="from"/>.
        /// </summary>
        /// <param name="numberToSelect">The number of <see cref="Firework"/>s
        /// to select.</param>
        /// <param name="from"><see cref="Firework"/>s to select from.</param>
        /// <returns>A subset of <see cref="Firework"/>s.</returns>
        IEnumerable<Firework> SelectFireworks(int numberToSelect, params Firework[] from);
    }
}