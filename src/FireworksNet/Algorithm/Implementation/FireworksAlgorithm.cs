using System;
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
using FireworksNet.StopConditions;

namespace FireworksNet.Algorithm.Implementation
{
    /// <summary>
    /// Fireworks Algorithm implementation, per 2010 paper.
    /// </summary>
    public sealed class FireworksAlgorithm : FireworksAlgorithmBase<FireworksAlgorithmSettings>, IFireworksAlgorithm, IStepperFireworksAlgorithm
    {
        private const double normalDistributionMean = 1.0;
        private const double normalDistributionStdDev = 1.0;

        /// <summary>
        /// Gets or sets the randomizer.
        /// </summary>
        public System.Random Randomizer { get; set; }

        /// <summary>
        /// Gets or sets the continuous univariate probability distribution.
        /// </summary>
        public IContinuousDistribution Distribution { get; set; }

        /// <summary>
        /// Gets or sets the initial spark generator.
        /// </summary>
        public ISparkGenerator InitialSparkGenerator { get; set; }

        /// <summary>
        /// Gets or sets the explosion spark generator.
        /// </summary>
        public ISparkGenerator ExplosionSparkGenerator { get; set; }

        /// <summary>
        /// Gets or sets the specific spark generator.
        /// </summary>
        public ISparkGenerator SpecificSparkGenerator { get; set; }

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
        public IExploder Exploder { get; set; }

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
            this.Distribution = new NormalDistribution(FireworksAlgorithm.normalDistributionMean, FireworksAlgorithm.normalDistributionStdDev);
            this.InitialSparkGenerator = new InitialSparkGenerator(problem.Dimensions, problem.InitialRanges, this.Randomizer);
            this.ExplosionSparkGenerator = new ExplosionSparkGenerator(problem.Dimensions, this.Randomizer);
            this.SpecificSparkGenerator = new GaussianSparkGenerator(problem.Dimensions, this.Distribution, this.Randomizer);
            this.DistanceCalculator = new EuclideanDistance(problem.Dimensions);
            this.LocationSelector = new DistanceBasedFireworkSelector(this.DistanceCalculator, new Func<IEnumerable<Firework>, Firework>(problem.GetBest), this.Settings.LocationsNumber);
            this.ExploderSettings = new ExploderSettings
            {
                ExplosionSparksNumberModifier = settings.ExplosionSparksNumberModifier,
                ExplosionSparksNumberLowerBound = settings.ExplosionSparksNumberLowerBound,
                ExplosionSparksNumberUpperBound = settings.ExplosionSparksNumberUpperBound,
                ExplosionSparksMaximumAmplitude = settings.ExplosionSparksMaximumAmplitude,
                SpecificSparksPerExplosionNumber = settings.SpecificSparksPerExplosionNumber
            };
            this.Exploder = new Exploder(this.ExploderSettings);
        }

        #region IFireworksAlgorithm methods

        /// <summary>
        /// Solves the specified problem by running the algorithm.
        /// </summary>
        /// <returns><see cref="Solution"/> instance that represents
        /// best solution found during the algorithm run.</returns>
        public override Solution Solve()
        {
            AlgorithmState state = this.CreateInitialState(true);

            Debug.Assert(state != null, "Initial state is null");

            while (!this.ShouldStop(state))
            {
                Debug.Assert(state != null, "Current state is null");

                this.MakeStep(state, true);

                Debug.Assert(state != null, "Updated state is null");
            }

            return this.GetSolution(state);
        }

        #endregion

        #region IStepperFireworksAlgorithm methods

        /// <summary>
        /// Creates the initial algorithm state (before the run starts).
        /// </summary>
        /// <returns><see cref="AlgorithmState"/> instance that represents
        /// initial state (before the run starts).</returns>
        /// <remarks>On each call re-creates the initial state (i.e. returns 
        /// new object each time).</remarks>
        public AlgorithmState CreateInitialState()
        {
            return this.CreateInitialState(false);
        }

        /// <summary>
        /// Represents one iteration of the algorithm.
        /// </summary>
        /// <param name="state">The state of the algorithm after the previous step
        /// or initial state.</param>
        /// <returns>State of the algorithm after the step.</returns>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="state"/>
        /// is <c>null</c>.</exception>
        public AlgorithmState MakeStep(AlgorithmState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            return this.MakeStep(state, false);
        }

        /// <summary>
        /// Determines the best found solution.
        /// </summary>
        /// <param name="state">The state of the algorithm after the previous step
        /// or initial state.</param>
        /// <returns><see cref="Solution"/> instance that represents
        /// best solution found during the algorithm run.</returns>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="state"/>
        /// is <c>null</c>.</exception>
        /// <remarks>This method does not modify <paramref name="state"/>.</remarks>
        public Solution GetSolution(AlgorithmState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            return state.BestSolution;
        }

        /// <summary>
        /// Tells if no further steps should be made.
        /// </summary>
        /// <param name="state">The state of the algorithm after the previous step
        /// or initial state.</param>
        /// <returns><c>true</c> if next step should be made. Otherwise 
        /// <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="state"/>
        /// is <c>null</c>.</exception>
        /// <remarks>This method does not modify <paramref name="state"/>.</remarks>
        public bool ShouldStop(AlgorithmState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            Debug.Assert(this.StopCondition != null, "Stop condition is null");

            return this.StopCondition.ShouldStop(state);
        }

        #endregion

        /// <summary>
        /// Creates the initial algorithm state (before the run starts).
        /// </summary>
        /// <param name="isMutable">Whether the initial state has to be mutable or not.</param>
        /// <returns><see cref="AlgorithmState"/> instance that represents
        /// initial state (before the run starts).</returns>
        /// <remarks>On each call re-creates the initial state (i.e. returns 
        /// new object each time).</remarks>
        public AlgorithmState CreateInitialState(bool isMutable)
        {
            Debug.Assert(this.Settings != null, "Settings is null");
            Debug.Assert(this.InitialSparkGenerator != null, "Initial spark generator is null");
            Debug.Assert(this.ProblemToSolve != null, "Problem to solve is null");

            InitialExplosion initialExplosion = new InitialExplosion(this.Settings.LocationsNumber);

            Debug.Assert(initialExplosion != null, "Initial explosion is null");

            IEnumerable<Firework> fireworks = this.InitialSparkGenerator.CreateSparks(initialExplosion);

            Debug.Assert(fireworks != null, "Initial firework collection is null");

            this.CalculateQualities(fireworks);

            return isMutable
                ? new MutableAlgorithmState(fireworks, 0, this.ProblemToSolve.GetBest(fireworks))
                : new AlgorithmState(fireworks, 0, this.ProblemToSolve.GetBest(fireworks));
        }

        /// <summary>
        /// Represents one iteration of the algorithm.
        /// </summary>
        /// <param name="state">The state of the algorithm after the previous step
        /// or initial state.</param>
        /// <param name="isMutable">Whether the state is mutable or not.</param>
        /// <returns>State of the algorithm after the step.</returns>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="state"/>
        /// is <c>null</c>.</exception>
        public AlgorithmState MakeStep(AlgorithmState state, bool isMutable)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            Debug.Assert(state.StepNumber >= 0, "Negative step number");
            Debug.Assert(state.Fireworks != null, "State firework collection is null");
            Debug.Assert(this.Settings != null, "Settings is null");
            Debug.Assert(this.Settings.SpecificSparksNumber >= 0, "Negative settings specific spark number");
            Debug.Assert(this.Randomizer != null, "Randomizer is null");
            Debug.Assert(this.Settings.LocationsNumber >= 0, "Negative settings locations number");
            Debug.Assert(this.Exploder != null, "Exploder is null");
            Debug.Assert(this.ExplosionSparkGenerator != null, "Explosion spark generator is null");
            Debug.Assert(this.SpecificSparkGenerator != null, "Specific spark generator is null");
            Debug.Assert(this.LocationSelector != null, "Location selector is null");
            Debug.Assert(this.ProblemToSolve != null, "Problem to solve is null");

            int stepNumber = state.StepNumber + 1;

            IEnumerable<double> fireworkQualities = state.Fireworks.Select(fw => fw.Quality);
            IEnumerable<int> specificSparkParentIndices = this.Randomizer.NextUniqueInt32s(this.Settings.SpecificSparksNumber, 0, this.Settings.LocationsNumber);

            IEnumerable<Firework> explosionSparks = new List<Firework>();
            IEnumerable<Firework> specificSparks = new List<Firework>(this.Settings.SpecificSparksNumber);
            int currentFirework = 0;
            foreach (Firework firework in state.Fireworks)
            {
                Debug.Assert(firework != null, "Firework is null");

                ExplosionBase explosion = this.Exploder.Explode(firework, fireworkQualities, stepNumber);
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

            IEnumerable<Firework> allFireworks = state.Fireworks.Concat(explosionSparks.Concat(specificSparks));
            IEnumerable<Firework> selectedFireworks = this.LocationSelector.SelectFireworks(allFireworks);

            if (isMutable)
            {
                MutableAlgorithmState mutableState = state as MutableAlgorithmState;
                if (state == null)
                {
                    throw new InvalidOperationException();
                }

                mutableState.UpdateState(selectedFireworks, stepNumber, this.ProblemToSolve.GetBest(selectedFireworks));
                return mutableState;
            }

            return new AlgorithmState(selectedFireworks, stepNumber, this.ProblemToSolve.GetBest(selectedFireworks));
        }
    }
}