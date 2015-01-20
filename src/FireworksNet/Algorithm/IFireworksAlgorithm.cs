using System.Collections.Generic;
using FireworksNet.Model;
using FireworksNet.Problems;

namespace FireworksNet.Algorithm
{
    public interface IFireworksAlgorithm
    {
        Problem ProblemToSolve { get; }

        Solution Solve();
        IEnumerable<Firework> MakeStep(IEnumerable<Firework> currentFireworks); // TODO: Maintain a state that should contain current step data
    }
}