using FireworksNet.Model;

namespace FireworksNet.Problems
{
    public interface IStopCondition
    {
        bool ShouldStop(AlgorithmState state);
    }
}