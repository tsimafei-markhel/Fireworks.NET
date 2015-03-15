using System.Diagnostics;
using System.Collections.Generic;

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
        private readonly System.Random generator;

        private IContinuousDistribution distribution;
        private IExploder exploder;
        private ISparkGenerator armSparkGenerator;
        private AlgorithmState state;

        public Problem ProblemToSolve { private set; get; }
        public IStopCondition StopCondition { private set; get; }
        public GpuFireworksAlgorithmSettings Settings { private set; get; }

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

            generator = new FireworksNet.Random.DefaultRandom();
            distribution = new ContinuousUniformDistribution(settings.Amplitude - settings.Delta, settings.Amplitude + settings.Delta);

            state = CreateInitialState();// order necessary: invoke before inialize armSparkGenerator!

            armSparkGenerator = new GpuAttractRepulseSparkGenerator(state, problem.Dimensions, distribution, generator);
            exploder = new GpuExploder(new GpuExplodeSettings()
            {                
                FixedQuantitySparks = settings.FixedQuantitySparks,
                Amplitude = settings.Amplitude
            });
        }

        public Solution Solve()
        {
            // TODO
            throw new System.NotImplementedException("Solve()");
        }

        public AlgorithmState MakeStep(AlgorithmState state)
        {
            // TODO
            throw new System.NotImplementedException();
        }

        public Solution GetSolution(AlgorithmState state)
        {
            if (state == null) { throw new System.ArgumentNullException("algorithm state cannot be null"); }

            return state.BestSolution;
        }

        public bool ShouldStop(AlgorithmState state)
        {
            if (state == null) { throw new System.ArgumentNullException("state cannot be null"); }

            return StopCondition.ShouldStop(state);
        }         

        public AlgorithmState CreateInitialState()
        {
            Debug.Assert(ProblemToSolve != null, "problem to solve cannot be null");
            Debug.Assert(Settings != null, "settings of algorithm connot be null");
            Debug.Assert(generator != null, "generator cannot be null");

            InitialExplosion explosion = new InitialExplosion(Settings.FixedQuantitySparks);
            InitialSparkGenerator sparkGenerator = new InitialSparkGenerator(ProblemToSolve.Dimensions, generator);
            AlgorithmState state = new AlgorithmState();

            state.Fireworks = sparkGenerator.CreateSparks(explosion);
            Debug.Assert(state.Fireworks != null, "state.fireworks cannot be null");
            state.BestSolution = ProblemToSolve.GetBest(state.Fireworks);
            state.StepNumber = 0;

            CalculateQualities(state.Fireworks);

            return state;
        }

        private void CalculateQualities(IEnumerable<Firework> sparks)
        {
            foreach (Firework spark in sparks)
            {
               spark.Quality = ProblemToSolve.CalculateQuality(spark.Coordinates);
            }
        }
    }
}
