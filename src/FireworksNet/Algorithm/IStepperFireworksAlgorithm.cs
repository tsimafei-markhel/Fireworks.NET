using FireworksNet.Model;
using FireworksNet.State;

namespace FireworksNet.Algorithm
{
    /// <summary>
    /// Fireworks algorithm with possibility of step-by-step execution.
    /// </summary>
    public interface IStepperFireworksAlgorithm : IFireworksAlgorithm
    {
        /// <summary>
        /// Creates the initial algorithm state (before the run starts).
        /// </summary>
        /// <returns>Instane of class implementing <see cref="IAlgorithmState"/>, that represents
        /// initial state (before the run starts).</returns>
        IAlgorithmState CreateInitialState();

        /// <summary>
        /// Represents one iteration of the algorithm.
        /// </summary>
        /// <param name="state">The state of the algorithm after the previous step
        /// or initial state.</param>
        /// <returns>State of the algorithm after the step.</returns>
        /// <remarks>This method should not modify <paramref name="state"/>.</remarks>
        IAlgorithmState MakeStep(IAlgorithmState state);

        /// <summary>
        /// Tells if no further steps should be made.
        /// </summary>
        /// <param name="state">The state of the algorithm after the previous step
        /// or initial state.</param>
        /// <returns><c>true</c> if next step should be made. Otherwise <c>false</c>.</returns>
        /// <remarks>This method should not modify <paramref name="state"/>.</remarks>
        bool ShouldStop(IAlgorithmState state);

        /// <summary>
        /// Determines the best found solution.
        /// </summary>
        /// <param name="state">The state of the algorithm after the previous step
        /// or initial state.</param>
        /// <returns><see cref="Solution"/> instance that represents
        /// best solution found during the algorithm run.</returns>
        /// <remarks>This method should not modify <paramref name="state"/>.</remarks>
        Solution GetSolution(IAlgorithmState state);
    }
}