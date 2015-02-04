using FireworksNet.Model;

namespace FireworksNet.Algorithm
{
	public interface IStepperFireworksAlgorithm : IFireworksAlgorithm
	{
		AlgorithmState GetInitialState();

		/// <remarks>This method should not modify <paramref name="state"/>.</remarks>
		AlgorithmState MakeStep(AlgorithmState state);

        /// <remarks>This method should not modify <paramref name="state"/>.</remarks>
        bool ShouldStop(AlgorithmState state);

		/// <remarks>This method should not modify <paramref name="state"/>.</remarks>
		Solution GetSolution(AlgorithmState state);
	}
}