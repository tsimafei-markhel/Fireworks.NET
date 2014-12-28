using FireworksNet.Model;
using FireworksNet.Problems;

namespace FireworksNet.Implementation
{
    public interface IFireworksAlgorithm
    {
        Firework Solve(Problem problem, AlgorithmSetup setup);
    }
}