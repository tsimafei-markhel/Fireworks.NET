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
    // Per 2010 paper
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

        public Problem ProblemToSolve { get; private set; }

        /// <summary>
        /// Gets or sets the stop condition that algorithm has to use.
        /// </summary>
        public IStopCondition StopCondition { get; private set; }

        public FireworksAlgorithmSettings Settings { get; private set; }

        public FireworksAlgorithm(Problem problem, IStopCondition stopCondition, FireworksAlgorithmSettings settings)
        {
            if (problem == null)
            {
                throw new ArgumentNullException("problem");
            }

            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            if (stopCondition == null)
            {
                throw new ArgumentNullException("stopCondition");
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

        public Solution GetSolution(AlgorithmState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException("state");
            }

            return state.BestSolution;
        }

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

        private void MakeStep(ref AlgorithmState state)
        {
            // TODO: Add asserts here

            if (state == null)
            {
                throw new ArgumentNullException("state");
            }

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
                Explosion explosion = this.exploder.Explode(firework, fireworkQualities, state.StepNumber);
                explosionSparks = explosionSparks.Concat(this.explosionSparkGenerator.CreateSparks(explosion));
                if (specificSparkParentIndices.Contains(currentFirework))
                {
                    specificSparks = specificSparks.Concat(this.specificSparkGenerator.CreateSparks(explosion));
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

        private void CalculateQualities(IEnumerable<Firework> fireworks)
        {
            Debug.Assert(fireworks != null, "Collection of fireworks to calculate qualities for is null");
            Debug.Assert(this.ProblemToSolve != null, "Problem is null");

            foreach (Firework firework in fireworks)
            {
                Debug.Assert(firework != null, "Firework to calculate quality for is null");
                Debug.Assert(double.IsNaN(firework.Quality), "Excessive quality calculation"); // If quality is not NaN, it most likely has been already calculated
                Debug.Assert(firework.Coordinates != null, "Firework coordinates collection is null");

                firework.Quality = this.ProblemToSolve.CalculateQuality(firework.Coordinates);
            }
        }
    }
}