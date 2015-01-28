using System.Collections.Generic;
using FireworksNet.Model;
using FireworksNet.Problems;

namespace FireworksNet.Algorithm
{
    public interface IFireworksAlgorithm
    {
        Problem ProblemToSolve { get; }

        Solution Solve();

		/// <remarks>This method is should not modify <paramref name="currentState"/>.</remarks>
		AlgorithmState MakeStep(AlgorithmState currentState);

		void MakeStep(ref AlgorithmState state);

		// TODO: AlgorithmState GetInitialState()   - or this does not belong here? Maybe create IStepperFireworksAlgorithm that would allow that?..
		// TODO: bool ShouldStop() (maybe take it from the interface?)   - or this does not belong here? Maybe create IStepperFireworksAlgorithm that would allow that?..
    }
}