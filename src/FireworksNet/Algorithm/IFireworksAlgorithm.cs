using FireworksNet.Model;
using FireworksNet.Problems;
using FireworksNet.StopConditions;

namespace FireworksNet.Algorithm
{
    /// <summary>
    /// Represents a fireworks algorithm.
    /// </summary>
    public interface IFireworksAlgorithm
    {
        /// <summary>
        /// Gets the problem to be solved by the algorithm.
        /// </summary>
        Problem ProblemToSolve { get; }

        /// <summary>
        /// Gets the stop condition for the algorithm.
        /// </summary>
        IStopCondition StopCondition { get; }

        /// <summary>
        /// Solves the specified problem by running the algorithm.
        /// </summary>
        /// <returns><see cref="Solution"/> instance that represents
        /// best solution found during the algorithm run.</returns>
        Solution Solve();
    }
}