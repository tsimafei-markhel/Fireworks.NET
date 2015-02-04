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

            // TODO: Always use 'this' to access members of the current instance. Move this rule to code convention.

            this.ProblemToSolve = problem;
			this.StopCondition = stopCondition;
            this.Settings = settings;

            this.randomizer = new DefaultRandom();
            this.distribution = new NormalDistribution(normalDistributionMean, normalDistributionStdDev);
            this.initialSparkGenerator = new InitialSparkGenerator(problem.Dimensions, problem.InitialRanges, this.randomizer);
            this.explosionSparkGenerator = new ExplosionSparkGenerator(problem.Dimensions, this.randomizer);
            this.specificSparkGenerator = new GaussianSparkGenerator(problem.Dimensions, this.distribution, this.randomizer);
            this.distanceCalculator = new EuclideanDistance(problem.Dimensions);
			this.locationSelector = new LocationSelector(this.distanceCalculator, new Func<IEnumerable<Firework>, Firework>(problem.GetBest), Settings.LocationsNumber);
            this.exploderSettings = new ExploderSettings()
            {
                ExplosionSparksNumberModifier = settings.ExplosionSparksNumberModifier,
                ExplosionSparksNumberLowerBound = settings.ExplosionSparksNumberLowerBound,
                ExplosionSparksNumberUpperBound = settings.ExplosionSparksNumberUpperBound,
                ExplosionSparksMaximumAmplitude = settings.ExplosionSparksMaximumAmplitude,
                SpecificSparksPerExplosionNumber = settings.SpecificSparksPerExplosionNumber
            };
            this.exploder = new Exploder(exploderSettings);
        }

        #region IFireworksAlgorithm methods

        public Solution Solve()
        {
            AlgorithmState state = GetInitialState();
            while (!ShouldStop(state))
            {
				MakeStep(ref state);
            }

            return GetSolution(state);
        }

        #endregion

        #region IStepperFireworksAlgorithm methods

        public AlgorithmState GetInitialState()
        {
            InitialExplosion initialExplosion = new InitialExplosion(Settings.LocationsNumber);
            IEnumerable<Firework> fireworks = initialSparkGenerator.CreateSparks(initialExplosion);

            CalculateQualities(fireworks);

            return new AlgorithmState()
            {
                BestSolution = ProblemToSolve.GetBest(fireworks),
                Fireworks = fireworks,
                StepNumber = 0
            };
        }

        public AlgorithmState MakeStep(AlgorithmState currentState)
        {
            if (currentState == null)
            {
                throw new ArgumentNullException("currentState");
            }

            AlgorithmState newState = new AlgorithmState()
            {
                BestSolution = currentState.BestSolution,
                Fireworks = currentState.Fireworks,
                StepNumber = currentState.StepNumber
            };

            MakeStep(ref newState);

            return newState;
        }

        public Solution GetSolution(AlgorithmState state)
        {
            return state.BestSolution;
        }

        public bool ShouldStop(AlgorithmState state)
        {
            return StopCondition.ShouldStop(state);
        }

        #endregion

        private void MakeStep(ref AlgorithmState state)
        {
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
            IEnumerable<Firework> specificSparks = new List<Firework>(Settings.SpecificSparksNumber);
            IEnumerable<int> specificSparkParentIndices = randomizer.NextInt32s(Settings.SpecificSparksNumber, 0, Settings.LocationsNumber);
            int currentFirework = 0;
            foreach (Firework firework in state.Fireworks)
            {
				Explosion explosion = exploder.Explode(firework, fireworkQualities, state.StepNumber);
                explosionSparks = explosionSparks.Concat(explosionSparkGenerator.CreateSparks(explosion));
                if (specificSparkParentIndices.Contains(currentFirework))
                {
                    specificSparks = specificSparks.Concat(specificSparkGenerator.CreateSparks(explosion));
                }

                currentFirework++;
            }

            CalculateQualities(explosionSparks);
            CalculateQualities(specificSparks);

            IEnumerable<Firework> allFireworks = state.Fireworks.Concat(explosionSparks.Concat(specificSparks));
            IEnumerable<Firework> selectedFireworks = locationSelector.Select(allFireworks);

			state.Fireworks = selectedFireworks;
			state.BestSolution = ProblemToSolve.GetBest(selectedFireworks);
        }

        private void CalculateQualities(IEnumerable<Firework> fireworks)
        {
            Debug.Assert(fireworks != null, "Collection of fireworks to calculate qualities for is null");
            Debug.Assert(ProblemToSolve != null, "Problem is null");

            foreach (Firework firework in fireworks)
            {
                Debug.Assert(firework != null, "Firework to calculate quality for is null");
                Debug.Assert(double.IsNaN(firework.Quality), "Excessive quality calculation");
                Debug.Assert(firework.Coordinates != null, "Firework coordinates collection is null");

                firework.Quality = ProblemToSolve.CalculateQuality(firework.Coordinates);
            }
        }
    }
}