using FireworksNet.Model;

namespace FireworksNet.Algorithm
{
	public interface IStepperFireworksAlgorithm : IFireworksAlgorithm
	{
		AlgorithmState GetInitialState();

		/// <remarks>This method is should not modify <paramref name="state"/>.</remarks>
		AlgorithmState MakeStep(AlgorithmState state);

		bool ShouldStop();

		/// <remarks>This method is should not modify <paramref name="state"/>.</remarks>
		Solution GetSolution(AlgorithmState state);
	}
}