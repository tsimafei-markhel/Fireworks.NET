using System;
using System.Collections.Generic;
using FireworksNet.Model;

namespace FireworksNet.Selection
{
    /// <summary>
    /// Base class for selectors.
    /// </summary>
    public abstract class SelectorBase : ISelector
    {
        protected readonly int locationsNumber;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectorBase"/> class.
        /// </summary>
        /// <param name="locationsNumber">The number of <see cref="Firework"/>s to be selected.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"> if <paramref name="locationsNumber"/>
        /// is less than zero.</exception>
        protected SelectorBase(int locationsNumber)
        {
            if (locationsNumber < 0)
            {
                throw new ArgumentOutOfRangeException("locationsNumber");
            }

            this.locationsNumber = locationsNumber;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectorBase"/> class.
        /// </summary>
        /// <remarks>It is assumed that number of <see cref="Firework"/>s to be selected
        /// differs from step to step and hence is passed to the <c>Select</c> method.</remarks>
        protected SelectorBase()
            : this(0)
        {
        }

        /// <summary>
        /// Selects some predefined number of <see cref="Firework"/>s from
        /// the <paramref name="from"/> collection.
        /// </summary>
        /// <param name="from">A collection to select <see cref="Firework"/>s
        /// from.</param>
        /// <returns>
        /// A subset of <see cref="Firework"/>s.
        /// </returns>
        public virtual IEnumerable<Firework> Select(IEnumerable<Firework> from)
        {
            return this.Select(from, this.locationsNumber);
        }

        /// <summary>
        /// Selects <paramref name="numberToSelect"/> <see cref="Firework"/>s from
        /// the <paramref name="from"/> collection. Selected <see cref="Firework"/>s
        /// are stored in the new collection, <paramref name="from"/> is not modified.
        /// </summary>
        /// <param name="from">A collection to select <see cref="Firework"/>s
        /// from.</param>
        /// <param name="numberToSelect">The number of <see cref="Firework"/>s
        /// to select.</param>
        /// <returns>
        /// A subset of <see cref="Firework"/>s.
        /// </returns>
        public abstract IEnumerable<Firework> Select(IEnumerable<Firework> from, int numberToSelect);

        /// <summary>
        /// Selects some predefined number of <see cref="Firework"/>s from
        /// the <paramref name="from"/> parameters.
        /// </summary>
        /// <param name="from"><see cref="Firework"/>s to select from.</param>
        /// <returns>A subset of <see cref="Firework"/>s.</returns>
        public virtual IEnumerable<Firework> Select(params Firework[] from)
        {
            return this.Select(from);
        }

        /// <summary>
        /// Selects <paramref name="numberToSelect"/> <see cref="Firework"/>s from
        /// the <paramref name="from"/>.
        /// </summary>
        /// <param name="numberToSelect">The number of <see cref="Firework"/>s
        /// to select.</param>
        /// <param name="from"><see cref="Firework"/>s to select from.</param>
        /// <returns>A subset of <see cref="Firework"/>s.</returns>
        public virtual IEnumerable<Firework> Select(int numberToSelect, params Firework[] from)
        {
            return this.Select(from, numberToSelect);
        }
    }
}