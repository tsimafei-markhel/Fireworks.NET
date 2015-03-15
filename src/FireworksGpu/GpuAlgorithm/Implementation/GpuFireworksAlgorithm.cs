using FireworksNet.Algorithm;
using FireworksNet.Problems;
using FireworksNet.StopConditions;
using FireworksNet.Model;
using FireworksNet.Explode;
using FireworksNet.Distributions;
using FireworksGpu.GpuExplode;
using FireworksGpu.GpuAlgorithm.Implementation;

namespace FireworksGpu.GpuAlgorithm
{
    /// <summary>
    /// Fireworks algorithm implementation based on gpu
    /// </summary>
    public class GpuFireworksAlgorithm : IFireworksAlgorithm, IStepperFireworksAlgorithm
    {
        public Problem ProblemToSolve { private set; get; }
        public IStopCondition StopCondition { private set; get; }
        public GpuFireworksAlgorithmSettings Settings { private set; get; }

        private IContinuousDistribution distribution;

        private ISparkGenerator initialSparkGenerator;
        private ISparkGenerator exlosionSparkGenerator;
        private ISparkGenerator specificSparkGenerator;

        /// <summary>
        /// Create instance of GpuFireworksAlgorithm
        /// </summary>
        /// <param name="problem">problem to solve</param>
        /// <param name="stopCondition">terminate condition</param>
        /// <param name="settings">setting of algorithm</param>
        public GpuFireworksAlgorithm(Problem problem, IStopCondition stopCondition, GpuFireworksAlgorithmSettings settings)
        {
            if (problem == null) { throw new System.ArgumentNullException("problem to solve"); }
            if (stopCondition == null) { throw new System.ArgumentNullException("stop condition"); }
            if (settings == null) { throw new System.ArgumentNullException("gpu algorithm settings"); }

            ProblemToSolve = problem;
            StopCondition = stopCondition;
            Settings = settings;

            distribution = new ContinuousUniformDistribution(settings.Amplitude - settings.Delta, settings.Amplitude + settings.Delta);
        }


        public Solution Solve()
        {
            AlgorithmState state = CreateInitialState();
            
            

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
