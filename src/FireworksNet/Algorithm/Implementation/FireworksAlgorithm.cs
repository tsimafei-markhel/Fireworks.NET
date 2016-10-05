using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FireworksNet.Distances;
using FireworksNet.Distributions;
using FireworksNet.Explode;
using FireworksNet.Extensions;
using FireworksNet.Generation;
using FireworksNet.Model;
using FireworksNet.Problems;
using FireworksNet.Random;
using FireworksNet.Selection;
using FireworksNet.Selection.Extremum;
using FireworksNet.State;
using FireworksNet.StopConditions;

namespace FireworksNet.Algorithm.Implementation
{
    /// <summary>
    /// Fireworks Algorithm implementation, per 2010 paper.
    /// </summary>
    public sealed class FireworksAlgorithm : FireworksAlgorithmBase<FireworksAlgorithmSettings>, IStepperFireworksAlgorithm
    {
        private const double normalDistributionMean = 1.0;
        private const double normalDistributionStdDev = 1.0;

        private AlgorithmState state;

        /// <summary>
        /// Gets or sets the randomizer.
        /// </summary>
        public System.Random Randomizer { get; set; }

        /// <summary>
        /// Gets or sets the extremum firework selector.
        /// </summary>
        public IExtremumFireworkSelector BestWorstFireworkSelector { get; set; }

        /// <summary>
        /// Gets or sets the continuous univariate probability distribution.
        /// </summary>
        public IContinuousDistribution Distribution { get; set; }

        /// <summary>
        /// Gets or sets the initial spark generator.
        /// </summary>
        public ISparkGenerator<InitialExplosion> InitialSparkGenerator { get; set; }

        /// <summary>
        /// Gets or sets the explosion spark generator.
        /// </summary>
        public ISparkGenerator<FireworkExplosion> ExplosionSparkGenerator { get; set; }

        /// <summary>
        /// Gets or sets the specific spark generator.
        /// </summary>
        public ISparkGenerator<FireworkExplosion> SpecificSparkGenerator { get; set; }

        /// <summary>
        /// Gets or sets the distance calculator.
        /// </summary>
        public IDistance DistanceCalculator { get; set; }

        /// <summary>
        /// Gets or sets the location selector.
        /// </summary>
        public IFireworkSelector LocationSelector { get; set; }

        /// <summary>
        /// Gets or sets the explosion settings.
        /// </summary>
        public ExploderSettings ExploderSettings { get; set; }

        /// <summary>
        /// Gets or sets the explosion generator.
        /// </summary>
        public IExploder<FireworkExplosion> Exploder { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FireworksAlgorithm"/> class.
        /// </summary>
        /// <param name="problem">The problem to be solved by the algorithm.</param>
        /// <param name="stopCondition">The stop condition for the algorithm.</param>
        /// <param name="settings">The algorithm settings.</param>
        public FireworksAlgorithm(Problem problem, IStopCondition stopCondition, FireworksAlgorithmSettings settings)
            : base(problem, stopCondition, settings)
        {
            this.Randomizer = new DefaultRandom();
            this.BestWorstFireworkSelector = new ExtremumFireworkSelector(problem.Target);
            this.Distribution = new NormalDistribution(FireworksAlgorithm.normalDistributionMean, FireworksAlgorithm.normalDistributionStdDev);
            this.InitialSparkGenerator = new InitialSparkGenerator(problem.Dimensions, problem.InitialRanges, this.Randomizer);
            this.ExplosionSparkGenerator = new ExplosionSparkGenerator(problem.Dimensions, this.Randomizer);
            this.SpecificSparkGenerator = new GaussianSparkGenerator(problem.Dimensions, this.Distribution, this.Randomizer);
            this.DistanceCalculator = new EuclideanDistance(problem.Dimensions);
            this.LocationSelector = new DistanceBasedFireworkSelector(this.DistanceCalculator, this.BestWorstFireworkSelector, this.Settings.LocationsNumber);
            this.ExploderSettings = new ExploderSettings
            {
                ExplosionSparksNumberModifier = settings.ExplosionSparksNumberModifier,
                ExplosionSparksNumberLowerBound = settings.ExplosionSparksNumberLowerBound,
                ExplosionSparksNumberUpperBound = settings.ExplosionSparksNumberUpperBound,
                ExplosionSparksMaximumAmplitude = settings.ExplosionSparksMaximumAmplitude,
                SpecificSparksPerExplosionNumber = settings.SpecificSparksPerExplosionNumber
            };

            this.Exploder = new Exploder(this.ExploderSettings, this.BestWorstFireworkSelector);
        }

        #region IFireworksAlgorithm methods

        /// <summary>
        /// Solves the specified problem by running the algorithm.
        /// </summary>
        /// <returns><see cref="Solution"/> instance that represents
        /// best solution found during the algorithm run.</returns>
        public override Solution Solve()
        {
            this.InitializeInternal();

            Debug.Assert(this.state != null, "Initial state is null");

            while (!this.ShouldStop())
            {
                Debug.Assert(this.state != null, "Current state is null");

                this.MakeStepInternal();

                Debug.Assert(this.state != null, "Updated state is null");
            }

            return this.state.BestSolution;
        }

        #endregion

        #region IStepperFireworksAlgorithm methods

        /// <summary>
        /// Creates the initial algorithm state (before the run starts).
        /// </summary>
        /// <returns>Instane of class implementing <see cref="IAlgorithmState"/>, that represents
        /// initial state (before the run starts).</returns>
        /// <remarks>On each call re-creates the initial state (i.e. returns 
        /// new object each time).</remarks>
        public IAlgorithmState Initialize()
        {
            this.InitializeInternal();

            Debug.Assert(this.state != null, "State is null");

            return new AlgorithmState(this.state.Fireworks, this.state.StepNumber, this.state.BestSolution);
        }

        /// <summary>
        /// Makes another iteration of the algorithm.
        /// </summary>
        /// <returns>State of the algorithm after the step.</returns>
        public IAlgorithmState MakeStep()
        {
            this.MakeStepInternal();

            Debug.Assert(this.state != null, "State is null");

            return new AlgorithmState(this.state.Fireworks, this.state.StepNumber, this.state.BestSolution);
        }

        /// <summary>
        /// Tells if no further steps should be made.
        /// </summary>
        /// <returns><c>true</c> if next step should be made. Otherwise 
        /// <c>false</c>.</returns>
        public bool ShouldStop()
        {
            Debug.Assert(this.StopCondition != null, "Stop condition is null");
            Debug.Assert(this.state != null, "State is null");

            return this.StopCondition.ShouldStop(this.state);
        }

        #endregion

        /// <summary>
        /// Creates the initial algorithm state (before the run starts).
        /// </summary>
        /// <remarks>On each call re-creates the initial state (i.e. returns 
        /// new object each time).</remarks>
        public void InitializeInternal()
        {
            Debug.Assert(this.Settings != null, "Settings is null");
            Debug.Assert(this.InitialSparkGenerator != null, "Initial spark generator is null");
            Debug.Assert(this.BestWorstFireworkSelector != null, "Best-Worst firework selector is null");

            InitialExplosion initialExplosion = new InitialExplosion(this.Settings.LocationsNumber);

            Debug.Assert(initialExplosion != null, "Initial explosion is null");

            IEnumerable<Firework> fireworks = this.InitialSparkGenerator.CreateSparks(initialExplosion);

            Debug.Assert(fireworks != null, "Initial firework collection is null");

            this.CalculateQualities(fireworks);

            this.state = new AlgorithmState(fireworks, 0, this.BestWorstFireworkSelector.SelectBest(fireworks));
        }

        /// <summary>
        /// Makes another iteration of the algorithm.
        /// </summary>
        public void MakeStepInternal()
        {
            Debug.Assert(this.state.StepNumber >= 0, "Negative step number");
            Debug.Assert(this.state.StepNumber < int.MaxValue, "Updated step number is int.MaxValue, further steps will be incorrect");
            Debug.Assert(this.state.Fireworks != null, "State firework collection is null");
            Debug.Assert(this.Settings != null, "Settings is null");
            Debug.Assert(this.Settings.SpecificSparksNumber >= 0, "Negative settings specific spark number");
            Debug.Assert(this.Randomizer != null, "Randomizer is null");
            Debug.Assert(this.Settings.LocationsNumber >= 0, "Negative settings locations number");
            Debug.Assert(this.Exploder != null, "Exploder is null");
            Debug.Assert(this.ExplosionSparkGenerator != null, "Explosion spark generator is null");
            Debug.Assert(this.SpecificSparkGenerator != null, "Specific spark generator is null");
            Debug.Assert(this.LocationSelector != null, "Location selector is null");
            Debug.Assert(this.BestWorstFireworkSelector != null, "Best-Worst firework selector is null");

            int stepNumber = this.state.StepNumber + 1;

            IEnumerable<int> specificSparkParentIndices = this.Randomizer.NextUniqueInt32s(this.Settings.SpecificSparksNumber, 0, this.Settings.LocationsNumber);

            IEnumerable<Firework> explosionSparks = new List<Firework>();
            IEnumerable<Firework> specificSparks = new List<Firework>(this.Settings.SpecificSparksNumber);
            int currentFirework = 0;
            foreach (Firework firework in this.state.Fireworks)
            {
                Debug.Assert(firework != null, "Firework is null");

                FireworkExplosion explosion = this.Exploder.Explode(firework, this.state.Fireworks, stepNumber);
                Debug.Assert(explosion != null, "Explosion is null");

                IEnumerable<Firework> fireworkExplosionSparks = this.ExplosionSparkGenerator.CreateSparks(explosion);
                Debug.Assert(fireworkExplosionSparks != null, "Firework explosion sparks collection is null");

                explosionSparks = explosionSparks.Concat(fireworkExplosionSparks);
                if (specificSparkParentIndices.Contains(currentFirework))
                {
                    IEnumerable<Firework> fireworkSpecificSparks = this.SpecificSparkGenerator.CreateSparks(explosion);
                    Debug.Assert(fireworkSpecificSparks != null, "Firework specific sparks collection is null");

                    specificSparks = specificSparks.Concat(fireworkSpecificSparks);
                }

                currentFirework++;
            }

            this.CalculateQualities(explosionSparks);
            this.CalculateQualities(specificSparks);

            IEnumerable<Firework> allFireworks = this.state.Fireworks.Concat(explosionSparks.Concat(specificSparks));
            IEnumerable<Firework> selectedFireworks = this.LocationSelector.SelectFireworks(allFireworks);

            this.state.Fireworks = selectedFireworks;
            this.state.StepNumber = stepNumber;
            this.state.BestSolution = this.BestWorstFireworkSelector.SelectBest(selectedFireworks);
        }
    }
}