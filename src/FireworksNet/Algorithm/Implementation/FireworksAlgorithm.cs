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
    public class FireworksAlgorithm : IFireworksAlgorithm, IStepperFireworksAlgorithm
    {
        private const double normalDistributionMean = 1.0;
        private const double normalDistributionStdDev = 1.0;

        /// <summary>
        /// Gets the randomizer.
        /// </summary>
        protected System.Random Randomizer { get; private set; }

        /// <summary>
        /// Gets the continuous univariate probability distribution.
        /// </summary>
        protected IContinuousDistribution Distribution { get; private set; }

        /// <summary>
        /// Gets the initial spark generator.
        /// </summary>
        protected ISparkGenerator InitialSparkGenerator { get; private set; }

        /// <summary>
        /// Gets the explosion spark generator.
        /// </summary>
        protected ISparkGenerator ExplosionSparkGenerator { get; private set; }

        /// <summary>
        /// Gets the specific spark generator.
        /// </summary>
        protected ISparkGenerator SpecificSparkGenerator { get; private set; }

        /// <summary>
        /// Gets the distance calculator.
        /// </summary>
        protected IDistance DistanceCalculator { get; private set; }

        /// <summary>
        /// Gets the location selector.
        /// </summary>
        protected IFireworkSelector LocationSelector { get; private set; }

        /// <summary>
        /// Gets the explosion settings.
        /// </summary>
        protected ExploderSettings ExploderSettings { get; private set; }

        /// <summary>
        /// Gets the explosion generator.
        /// </summary>
        protected IExploder Exploder { get; private set; }

        /// <summary>
        /// Gets the problem to be solved by the algorithm.
        /// </summary>
        public Problem ProblemToSolve { get; protected set; }

        /// <summary>
        /// Gets the stop condition for the algorithm.
        /// </summary>
        public IStopCondition StopCondition { get; protected set; }

        /// <summary>
        /// Gets the algorithm settings.
        /// </summary>
        public FireworksAlgorithmSettings Settings { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FireworksAlgorithm"/> class.
        /// </summary>
        /// <param name="problem">The problem to be solved by the algorithm.</param>
        /// <param name="stopCondition">The stop condition for the algorithm.</param>
        /// <param name="settings">The algorithm settings.</param>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="problem"/>
        /// or <paramref name="stopCondition"/> or <paramref name="settings"/> is 
        /// <c>null</c>.</exception>
        public FireworksAlgorithm(Problem problem, IStopCondition stopCondition, FireworksAlgorithmSettings settings)
        {
            if (problem == null)
            {
                throw new ArgumentNullException("problem");
            }

            if (stopCondition == null)
            {
                throw new ArgumentNullException("stopCondition");
            }

            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            this.ProblemToSolve = problem;
            this.StopCondition = stopCondition;
            this.Settings = settings;

            this.Randomizer = new DefaultRandom();
            this.Distribution = new NormalDistribution(FireworksAlgorithm.normalDistributionMean, FireworksAlgorithm.normalDistributionStdDev);
            this.InitialSparkGenerator = new InitialSparkGenerator(problem.Dimensions, problem.InitialRanges, this.Randomizer);
            this.ExplosionSparkGenerator = new ExplosionSparkGenerator(problem.Dimensions, this.Randomizer);
            this.SpecificSparkGenerator = new GaussianSparkGenerator(problem.Dimensions, this.Distribution, this.Randomizer);
            this.DistanceCalculator = new EuclideanDistance(problem.Dimensions);
            this.LocationSelector = new DistanceBasedFireworkSelector(this.DistanceCalculator, new Func<IEnumerable<Firework>, Firework>(problem.GetBest), this.Settings.LocationsNumber);
            this.ExploderSettings = new ExploderSettings()
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
        public virtual Solution Solve()
        {
            AlgorithmState state = this.CreateInitialState();

            Debug.Assert(state != null, "Initial state is null");

            while (!this.ShouldStop(state))
            {
                Debug.Assert(state != null, "Current state is null");

                this.MakeStep(ref state);

                Debug.Assert(state != null, "Current state is null");
            }

            Debug.Assert(state != null, "Final state is null");

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
        public virtual AlgorithmState CreateInitialState()
        {
            Debug.Assert(this.Settings != null, "Settings is null");
            Debug.Assert(this.InitialSparkGenerator != null, "Initial spark generator is null");
            Debug.Assert(this.ProblemToSolve != null, "Problem to solve is null");

            InitialExplosion initialExplosion = new InitialExplosion(this.Settings.LocationsNumber);

            Debug.Assert(initialExplosion != null, "Initial explosion is null");

            IEnumerable<Firework> fireworks = this.InitialSparkGenerator.CreateSparks(initialExplosion);

            Debug.Assert(fireworks != null, "Initial firework collection is null");

            this.CalculateQualities(fireworks);

            return new AlgorithmState()
            {
                BestSolution = this.ProblemToSolve.GetBest(fireworks),
                Fireworks = fireworks,
                StepNumber = 0
            };
        }

        /// <summary>
        /// Represents one iteration of the algorithm.
        /// </summary>
        /// <param name="state">The state of the algorithm after the previous step
        /// or initial state.</param>
        /// <returns>State of the algorithm after the step.</returns>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="state"/>
        /// is <c>null</c>.</exception>
        /// <remarks>This method does not modify <paramref name="state"/>.</remarks>
        public virtual AlgorithmState MakeStep(AlgorithmState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException("state");
            }

            AlgorithmState newState = new AlgorithmState()
            {
                // TODO: Add copy constructor
                BestSolution = state.BestSolution,
                Fireworks = state.Fireworks,
                StepNumber = state.StepNumber
            };

            this.MakeStep(ref newState);

            Debug.Assert(newState != null, "New state is null");

            return newState;
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
        public virtual Solution GetSolution(AlgorithmState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException("state");
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
        public virtual bool ShouldStop(AlgorithmState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException("state");
            }

            Debug.Assert(this.StopCondition != null, "Stop condition is null");

            return this.StopCondition.ShouldStop(state);
        }

        #endregion

        /// <summary>
        /// Makes the algorithm step.
        /// </summary>
        /// <param name="state">The algorithm state after previous step or initial 
        /// state.</param>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="state"/>
        /// is <c>null</c>.</exception>
        /// <remarks>This method does modify the <paramref name="state"/>.</remarks>
        protected virtual void MakeStep(ref AlgorithmState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException("state");
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

            // Need to increase step number first. Otherwise, we'll get
            // BirthStepNumber for 1st generation fireworks == 0 just like
            // that of initial fireworks.
            state.StepNumber++;

            IEnumerable<double> fireworkQualities = state.Fireworks.Select(fw => fw.Quality);

            IEnumerable<Firework> explosionSparks = new List<Firework>();
            IEnumerable<Firework> specificSparks = new List<Firework>(this.Settings.SpecificSparksNumber);
            IEnumerable<int> specificSparkParentIndices = this.Randomizer.NextUniqueInt32s(this.Settings.SpecificSparksNumber, 0, this.Settings.LocationsNumber);
            int currentFirework = 0;
            foreach (Firework firework in state.Fireworks)
            {
                Debug.Assert(firework != null, "Firework is null");

                ExplosionBase explosion = this.Exploder.Explode(firework, fireworkQualities, state.StepNumber);
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

            state.Fireworks = selectedFireworks;
            state.BestSolution = this.ProblemToSolve.GetBest(selectedFireworks);
        }

        /// <summary>
        /// Calculates the qualities for each <see cref="Firework"/> in
        /// <paramref name="fireworks"/> collection.
        /// </summary>
        /// <param name="fireworks">The fireworks to calculate qualities for.</param>
        /// <remarks>It is expected that none of the <paramref name="fireworks"/>
        /// has its quality calculated before.</remarks>
        protected virtual void CalculateQualities(IEnumerable<Firework> fireworks)
        {
            Debug.Assert(fireworks != null, "Fireworks collection is null");
            Debug.Assert(this.ProblemToSolve != null, "Problem to solve is null");

            foreach (Firework firework in fireworks)
            {
                Debug.Assert(firework != null, "Firework is null");
                Debug.Assert(double.IsNaN(firework.Quality), "Excessive quality calculation"); // If quality is not NaN, it most likely has been already calculated
                Debug.Assert(firework.Coordinates != null, "Firework coordinates collection is null");

                firework.Quality = this.ProblemToSolve.CalculateQuality(firework.Coordinates);
            }
        }
    }
}