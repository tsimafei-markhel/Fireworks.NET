using FireworksNet.Model;
using FireworksNet.Problems;
using FireworksNet.StopConditions;

namespace FireworksNet.Algorithm
{
    public interface IFireworksAlgorithm
    {
        Problem ProblemToSolve { get; }
		IStopCondition StopCondition { get; }

        Solution Solve();
    }
}