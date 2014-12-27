using Fireworks.Model;
using Fireworks.Problems;

namespace Fireworks.Implementation
{
    public interface IFireworksAlgorithm
    {
        Firework Solve(Problem problem, AlgorithmSetup setup);
    }
}