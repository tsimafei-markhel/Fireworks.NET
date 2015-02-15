using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FireworksNet.Distances;
using FireworksNet.Distributions;
using FireworksNet.Explode;
using FireworksNet.Extensions;
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
    public sealed class FireworksAlgorithm : IFireworksAlgorithm, IStepperFireworksAlgorithm
    {
        private const double normalDistributionMean = 1.0;
        private const double normalDistributionStdDev = 1.0;

        private readonly System.Random randomizer;
        private readonly IContinuousDistribution distribution;
        private readonly ISparkGenerator initialSparkGenerator;
        private readonly ISparkGenerator explosionSparkGenerator;
        private readonly ISparkGenerator specificSparkGenerator;
        private readonly IDistance distanceCalculator;
        private readonly ISelector locationSelector;
        private readonly ExploderSettings exploderSettings;
        private readonly IExploder exploder;

        /// <summary>
        /// Gets the problem to be solved by the algorithm.
        /// </summary>
        public Problem ProblemToSolve { get; private set; }

        /// <summary>
        /// Gets the stop condition for the algorithm.
        /// </summary>
        public IStopCondition StopCondition { get; private set; }

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

            this.randomizer = new DefaultRandom();
            this.distribution = new NormalDistribution(FireworksAlgorithm.normalDistributionMean, FireworksAlgorithm.normalDistributionStdDev);
            this.initialSparkGenerator = new InitialSparkGenerator(problem.Dimensions, problem.InitialRanges, this.randomizer);
            this.explosionSparkGenerator = new ExplosionSparkGenerator(problem.Dimensions, this.randomizer);
            this.specificSparkGenerator = new GaussianSparkGenerator(problem.Dimensions, this.distribution, this.randomizer);
            this.distanceCalculator = new EuclideanDistance(problem.Dimensions);
            this.locationSelector = new LocationSelector(this.distanceCalculator, new Func<IEnumerable<Firework>, Firework>(problem.GetBest), this.Settings.LocationsNumber);
            this.exploderSettings = new ExploderSettings()
            {
                ExplosionSparksNumberModifier = settings.ExplosionSparksNumberModifier,
                ExplosionSparksNumberLowerBound = settings.ExplosionSparksNumberLowerBound,
                ExplosionSparksNumberUpperBound = settings.ExplosionSparksNumberUpperBound,
                ExplosionSparksMaximumAmplitude = settings.ExplosionSparksMaximumAmplitude,
                SpecificSparksPerExplosionNumber = settings.SpecificSparksPerExplosionNumber
            };
            this.exploder = new Exploder(this.exploderSettings);
        }

        #region IFireworksAlgorithm methods

        /// <summary>
        /// Solves the specified problem by running the algorithm.
        /// </summary>
        /// <returns><see cref="Solution"/> instance that represents
        /// best solution found during the algorithm run.</returns>
        public Solution Solve()
        {
            AlgorithmState state = this.GetInitialState();

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
        /// Gets the initial algorithm state (before the run starts).
        /// </summary>
        /// <returns><see cref="AlgorithmState"/> instance that represents
        /// initial state (before the run starts).</returns>
        public AlgorithmState GetInitialState()
        {
            Debug.Assert(this.Settings != null, "Settings is null");
            Debug.Assert(this.initialSparkGenerator != null, "Initial spark generator is null");
            Debug.Assert(this.ProblemToSolve != null, "Problem to solve is null");

            InitialExplosion initialExplosion = new InitialExplosion(this.Settings.LocationsNumber);

            Debug.Assert(initialExplosion != null, "Initial explosion is null");

            IEnumerable<Firework> fireworks = this.initialSparkGenerator.CreateSparks(initialExplosion);

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
        public AlgorithmState MakeStep(AlgorithmState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException("state");
            }

            AlgorithmState newState = new AlgorithmState()
            {
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
        public Solution GetSolution(AlgorithmState state)
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
        public bool ShouldStop(AlgorithmState state)
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
        private void MakeStep(ref AlgorithmState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException("state");
            }

            Debug.Assert(state.StepNumber >= 0, "Negative step number");
            Debug.Assert(state.Fireworks != null, "State firework collection is null");
            Debug.Assert(this.Settings != null, "Settings is null");
            Debug.Assert(this.Settings.SpecificSparksNumber >= 0, "Negative settings specific spark number");
            Debug.Assert(this.randomizer != null, "Randomizer is null");
            Debug.Assert(this.Settings.LocationsNumber >= 0, "Negative settings locations number");
            Debug.Assert(this.exploder != null, "Exploder is null");
            Debug.Assert(this.explosionSparkGenerator != null, "Explosion spark generator is null");
            Debug.Assert(this.specificSparkGenerator != null, "Specific spark generator is null");
            Debug.Assert(this.locationSelector != null, "Location selector is null");
            Debug.Assert(this.ProblemToSolve != null, "Problem to solve is null");

            // Need to increase step number first. Otherwise, we'll get
            // BirthStepNumber for 1st generation fireworks == 0 just like
            // that of initial fireworks.
            state.StepNumber++;

            IEnumerable<double> fireworkQualities = state.Fireworks.Select(fw => fw.Quality);

            IEnumerable<Firework> explosionSparks = new List<Firework>();
            IEnumerable<Firework> specificSparks = new List<Firework>(this.Settings.SpecificSparksNumber);
            IEnumerable<int> specificSparkParentIndices = this.randomizer.NextInt32s(this.Settings.SpecificSparksNumber, 0, this.Settings.LocationsNumber);
            int currentFirework = 0;
            foreach (Firework firework in state.Fireworks)
            {
                Debug.Assert(firework != null, "Firework is null");

                ExplosionBase explosion = this.exploder.Explode(firework, fireworkQualities, state.StepNumber);
                Debug.Assert(explosion != null, "Explosion is null");

                IEnumerable<Firework> fireworkExplosionSparks = this.explosionSparkGenerator.CreateSparks(explosion);
                Debug.Assert(fireworkExplosionSparks != null, "Firework explosion sparks collection is null");

                explosionSparks = explosionSparks.Concat(fireworkExplosionSparks);
                if (specificSparkParentIndices.Contains(currentFirework))
                {
                    IEnumerable<Firework> fireworkSpecificSparks = this.specificSparkGenerator.CreateSparks(explosion);
                    Debug.Assert(fireworkSpecificSparks != null, "Firework specific sparks collection is null");

                    specificSparks = specificSparks.Concat(fireworkSpecificSparks);
                }

                currentFirework++;
            }

            this.CalculateQualities(explosionSparks);
            this.CalculateQualities(specificSparks);

            IEnumerable<Firework> allFireworks = state.Fireworks.Concat(explosionSparks.Concat(specificSparks));
            IEnumerable<Firework> selectedFireworks = this.locationSelector.Select(allFireworks);

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
        private void CalculateQualities(IEnumerable<Firework> fireworks)
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