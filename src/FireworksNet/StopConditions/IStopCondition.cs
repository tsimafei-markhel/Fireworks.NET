using FireworksNet.Model;

namespace FireworksNet.StopConditions
{
    public interface IStopCondition
    {
        bool ShouldStop(AlgorithmState state);
    }
}