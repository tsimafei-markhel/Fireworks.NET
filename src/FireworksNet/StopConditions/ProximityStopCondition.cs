using System;
using FireworksNet.Model;
using FireworksNet.State;

namespace FireworksNet.StopConditions
{
    /// <summary>
    /// Stops when some <see cref="Solution"/> (typically the current best one)
    /// approaches close enough to some expected <see cref="Solution"/>.
    /// </summary>
    public abstract class ProximityStopCondition : IStopCondition
    {
        /// <summary>
        /// Gets the expected <see cref="Solution"/> (an etalon).
        /// </summary>
        public Solution Expectation { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProximityStopCondition"/> class.
        /// </summary>
        /// <param name="expectation">The the expected <see cref="Solution"/> (an etalon).</param>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="expectation"/>
        /// is <c>null</c>.</exception>
        protected ProximityStopCondition(Solution expectation)
        {
            if (expectation == null)
            {
                throw new ArgumentNullException(nameof(expectation));
            }

            this.Expectation = expectation;
        }

        /// <summary>
        /// Tells if an algorithm that is currently in <paramref name="state"/> state
        /// should stop (and don't make further steps) or not.
        /// </summary>
        /// <param name="state">The current algorithm state.</param>
        /// <returns>
        /// <c>true</c> if an algorithm that is currently in <paramref name="state"/>
        /// state should stop (and don't make further steps). Otherwise <c>false</c>.
        /// </returns>
        public abstract bool ShouldStop(IAlgorithmState state);
    }
}