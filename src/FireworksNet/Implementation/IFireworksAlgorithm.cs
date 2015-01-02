using System.Collections.Generic;
using FireworksNet.Model;
using FireworksNet.Problems;

namespace FireworksNet.Implementation
{
    public interface IFireworksAlgorithm
    {
        Firework Solve(Problem problem, AlgorithmSetup setup);
        IEnumerable<Firework> MakeStep(IEnumerable<Firework> currentFireworks, Problem problem, AlgorithmSetup setup);
    }
}