using System;
using System.Collections.Generic;
using FireworksNet.Model;

namespace FireworksNet.Selection.Extremum
{
    /// <summary>
    /// Contains logic for selecting the best and the worst <see cref="Firework"/>s
    /// from a given set according to some rule.
    /// </summary>
    public interface IExtremumFireworkSelector
    {
        /// <summary>
        /// Fired before looking for the best firework.
        /// </summary>
        event EventHandler<ExtremumFireworkFindingEventArgs> BestFireworkFinding;

        /// <summary>
        /// Fired after the best firework is found.
        /// </summary>
        event EventHandler<ExtremumFireworkFoundEventArgs> BestFireworkFound;

        /// <summary>
        /// Fired before looking for the worst firework.
        /// </summary>
        event EventHandler<ExtremumFireworkFindingEventArgs> WorstFireworkFinding;

        /// <summary>
        /// Fired after the worst firework is found.
        /// </summary>
        event EventHandler<ExtremumFireworkFoundEventArgs> WorstFireworkFound;

        /// <summary>
        /// Selects the best of <see cref="Firework"/>s from
        /// the <paramref name="from"/> collection (in terms of firework quality).
        /// </summary>
        /// <param name="from">A collection to select <see cref="Firework"/>s
        /// from.</param>
        /// <returns>The best <see cref="Firework"/>.</returns>
        Firework SelectBest(IEnumerable<Firework> from);

        /// <summary>
        /// Selects the best of <see cref="Firework"/>s from
        /// the <paramref name="from"/> parameters (in terms of firework quality).
        /// </summary>
        /// <param name="from"><see cref="Firework"/>s to select the best one
        /// from.</param>
        /// <returns>The best <see cref="Firework"/>.</returns>
        Firework SelectBest(params Firework[] from);

        /// <summary>
        /// Selects the worst of <see cref="Firework"/>s from
        /// the <paramref name="from"/> collection (in terms of firework quality).
        /// </summary>
        /// <param name="from">A collection to select <see cref="Firework"/>s
        /// from.</param>
        /// <returns>The worst <see cref="Firework"/>.</returns>
        Firework SelectWorst(IEnumerable<Firework> from);

        /// <summary>
        /// Selects the worst of <see cref="Firework"/>s from
        /// the <paramref name="from"/> parameters (in terms of firework quality).
        /// </summary>
        /// <param name="from"><see cref="Firework"/>s to select the worst one
        /// from.</param>
        /// <returns>The worst <see cref="Firework"/>.</returns>
        Firework SelectWorst(params Firework[] from);
    }
}