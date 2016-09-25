using FireworksNet.State;

namespace FireworksNet.StopConditions
{
    /// <summary>
    /// Represents an algorithm stop condition.
    /// </summary>
    public interface IStopCondition
    {
        /// <summary>
        /// Tells if an algorithm that is currently in <paramref name="state"/> state
        /// should stop (and don't make further steps) or not.
        /// </summary>
        /// <param name="state">The current algorithm state.</param>
        /// <returns><c>true</c> if an algorithm that is currently in <paramref name="state"/>
        /// state should stop (and don't make further steps). Otherwise <c>false</c>.</returns>
        bool ShouldStop(IAlgorithmState state);
    }
}