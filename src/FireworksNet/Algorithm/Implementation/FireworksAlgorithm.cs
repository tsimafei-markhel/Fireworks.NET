using System;
using System.Collections.Generic;
using System.Linq;
using FireworksNet.Distances;
using FireworksNet.Distributions;
using FireworksNet.Explode;
using FireworksNet.Extensions;
using FireworksNet.Model;
using FireworksNet.Problems;
using FireworksNet.Random;
using FireworksNet.Selection;

namespace FireworksNet.Algorithm.Implementation
{
    // Per 2010 paper
    public sealed class FireworksAlgorithm : IFireworksAlgorithm
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

        public FireworksAlgorithmSettings Settings { get; private set; }
        
        public FireworksAlgorithm(Problem problem, FireworksAlgorithmSettings settings)
        {
            if (problem == null)
            {
                throw new ArgumentNullException("problem");
            }

            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            this.ProblemToSolve = problem;
            this.Settings = settings;

            this.randomizer = new DefaultRandom();
            this.distribution = new NormalDistribution(normalDistributionMean, normalDistributionStdDev);
            this.initialSparkGenerator = new InitialSparkGenerator(problem.Dimensions, problem.InitialDimensionRanges, this.randomizer);
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

        public Solution Solve()
        {
            InitialExplosion initialExplosion = new InitialExplosion(Settings.LocationsNumber);
            IEnumerable<Firework> fireworks = initialSparkGenerator.CreateSparks(initialExplosion);

            CalculateQualities(fireworks);

			AlgorithmState state = new AlgorithmState()
			{
				BestSolution = ProblemToSolve.GetBest(fireworks),
				Fireworks = fireworks,
				StepNumber = 0
			};

            while (!ProblemToSolve.StopCondition.ShouldStop(fireworks))
            {
				MakeStep(ref state);
            }

			return state.BestSolution;
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

		public void MakeStep(ref AlgorithmState state)
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
            System.Diagnostics.Debug.Assert(fireworks != null, "Collection of fireworks to calculate qualities for is null");
            System.Diagnostics.Debug.Assert(ProblemToSolve != null, "Problem is null");

            foreach (Firework firework in fireworks)
            {
                System.Diagnostics.Debug.Assert(firework != null, "Firework to calculate quality for is null");
                System.Diagnostics.Debug.Assert(double.IsNaN(firework.Quality), "Excessive quality calculation");
                System.Diagnostics.Debug.Assert(firework.Coordinates != null, "Firework coordinates collection is null");

                firework.Quality = ProblemToSolve.CalculateQuality(firework.Coordinates);
            }
        }
    }
}