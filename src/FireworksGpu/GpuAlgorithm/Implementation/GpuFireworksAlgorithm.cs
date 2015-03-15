using FireworksNet.Algorithm;
using FireworksNet.Problems;
using FireworksNet.StopConditions;
using FireworksNet.Model;
using FireworksNet.Explode;
using FireworksGpu.GpuExplode;

namespace FireworksGpu.GpuAlgorithm
{
    /// <summary>
    /// Fireworks algorithm implementation based on gpu
    /// </summary>
    public class GpuFireworksAlgorithm : IFireworksAlgorithm, IStepperFireworksAlgorithm
    {
        public Problem ProblemToSolve { private set; get; }

        public IStopCondition StopCondition { private set; get; }

        public Solution Solve()
        {
            AlgorithmState state = new AlgorithmState();
            
            
            
           

            throw new System.NotImplementedException();
        }

        public AlgorithmState CreateInitialState()
        {
            // TODO
            throw new System.NotImplementedException();
        }

        public AlgorithmState MakeStep(AlgorithmState state)
        {
            // TODO
            throw new System.NotImplementedException();
        }

        public bool ShouldStop(AlgorithmState state)
        {
            // TODO
            throw new System.NotImplementedException();
        }

        public Solution GetSolution(AlgorithmState state)
        {
            // TODO
            throw new System.NotImplementedException();
        }
    }
}
