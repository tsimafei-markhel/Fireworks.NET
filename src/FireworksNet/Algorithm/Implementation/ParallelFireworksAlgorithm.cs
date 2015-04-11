using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using FireworksNet.Algorithm;
using FireworksNet.Problems;
using FireworksNet.StopConditions;
using FireworksNet.Model;
using FireworksNet.Explode;
using FireworksNet.Distributions;
using FireworksNet.Algorithm.Implementation;
using FireworksNet.Selection;

namespace FireworksNet.Algorithm
{
    /// <summary>
    /// Fireworks algorithm implementation based on GPU, as described in 2013 GPU paper.
    /// </summary>
    public class ParallelFireworksAlgorithm : IFireworksAlgorithm, IStepperFireworksAlgorithm
    {
        private readonly System.Random randomizer;
        private IContinuousDistribution distribution;
        private IExploder exploder;         
        private AlgorithmState state;

        /// <summary>
        /// Execute search.
        /// </summary>
        private IFireworkMutator researcher;

        /// <summary>
        /// Represent best solution in now.
        /// </summary>
        private Solution bestSolution;

        /// <summary>
        /// Gets the problem to be solved by the algorithm.
        /// </summary>
        public Problem ProblemToSolve { private set; get; }

        /// <summary>
        /// Gets the stop condition for the algorithm.
        /// </summary>
        public IStopCondition StopCondition { private set; get; }

        /// <summary>
        /// Gets the algorithm settings.
        /// </summary>
        public ParallelFireworksAlgorithmSettings Settings { private set; get; }       

        /// <summary>
        /// Initializes a new instance of the <see cref="ParallelFireworksAlgorithm"/> class.
        /// </summary>
        /// <param name="problem">The problem to be solved by the algorithm.</param>
        /// <param name="stopCondition">The stop condition for the algorithm.</param>
        /// <param name="settings">The algorithm settings.</param>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="problem"/>
        /// or <paramref name="stopCondition"/> or <paramref name="settings"/> is 
        /// <c>null</c>.</exception>
        public ParallelFireworksAlgorithm(Problem problem, IStopCondition stopCondition, ParallelFireworksAlgorithmSettings settings)
        {
            if (problem == null) 
            { 
                throw new System.ArgumentNullException("problem to solve"); 
            }
            
            if (stopCondition == null) 
            { 
                throw new System.ArgumentNullException("stop condition"); 
            }
            
            if (settings == null) 
            { 
                throw new System.ArgumentNullException("algorithm settings"); 
            }

            ProblemToSolve = problem;
            StopCondition = stopCondition;
            Settings = settings;

            this.randomizer = new FireworksNet.Random.DefaultRandom();
            this.distribution = new ContinuousUniformDistribution(settings.Amplitude - settings.Delta, settings.Amplitude + settings.Delta);

            this.state = CreateInitialState();// order necessary: invoke before initialize AttractRepulseSparkMutator!
            this.bestSolution = state.BestSolution;

            ISparkGenerator generator = new AttractRepulseSparkGenerator(bestSolution, problem.Dimensions, distribution, randomizer);
            IFireworkMutator mutator = new AttractRepulseSparkMutator(generator);
            IFireworkSelector selector = new BestFireworkSelector((fireworks) => fireworks.OrderBy(f => f.Quality).First<Firework>());//select best
            this.researcher = new SearchMutator(mutator, generator, selector, settings);
            
            this.exploder = new ParallelExploder(new ParallelExploderSettings()
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
            if (state == null) 
            { 
                throw new System.ArgumentNullException("algorithm state"); 
            }

            return state.BestSolution;
        }

        public bool ShouldStop(AlgorithmState state)
        {
            if (state == null) 
            { 
                throw new System.ArgumentNullException("state"); 
            }

            return StopCondition.ShouldStop(state);
        }         

        public AlgorithmState CreateInitialState()
        {
            Debug.Assert(ProblemToSolve != null, "Problem to solve cannot be null");
            Debug.Assert(Settings != null, "Settings of algorithm cannot be null");
            Debug.Assert(randomizer != null, "Generator cannot be null");

            InitialExplosion explosion = new InitialExplosion(Settings.FixedQuantitySparks);
            InitialSparkGenerator sparkGenerator = new InitialSparkGenerator(ProblemToSolve.Dimensions, randomizer);
            AlgorithmState state = new AlgorithmState();

            state.Fireworks = sparkGenerator.CreateSparks(explosion);

            Debug.Assert(state.Fireworks != null, "State.fireworks cannot be null");
            
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
