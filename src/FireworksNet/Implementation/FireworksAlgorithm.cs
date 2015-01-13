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

namespace FireworksNet.Implementation
{
    // Per 2010 paper
    public sealed class FireworksAlgorithm : IFireworksAlgorithm
    {
        private readonly System.Random randomizer;
        private readonly IContinuousDistribution distribution;
        private readonly ISparkGenerator initialSparkGenerator;
        private readonly ISparkGenerator explosionSparkGenerator;
        private readonly ISparkGenerator specificSparkGenerator;
        private readonly IDistance distanceCalculator;
		private readonly ISelector locationSelector;
        private readonly ExploderSettings exploderSettings;
        private readonly IExploder exploder;
        private int stepNumber;

        public Problem ProblemToSolve { get; private set; }

        public FireworksAlgorithmSettings Settings { get; private set; }
        
        public FireworksAlgorithm(Problem problem, FireworksAlgorithmSettings settings)
        {
            this.ProblemToSolve = problem;
            this.Settings = settings;

            this.randomizer = new DefaultRandom();
            this.distribution = new NormalDistribution(1.0, 1.0);
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
            }; // TODO: AutoMapper or something like this can be used here.
            this.exploder = new Exploder(exploderSettings);

            this.stepNumber = 0;
        }

        public Firework Solve()
        {
            stepNumber = 0;

            InitialExplosion initialExplosion = new InitialExplosion(Settings.LocationsNumber);
            IEnumerable<Firework> fireworks = initialSparkGenerator.CreateSparks(initialExplosion);

            foreach (Firework firework in fireworks)
            {
                firework.Quality = ProblemToSolve.CalculateQuality(firework.Coordinates);
            }

            while (!ProblemToSolve.StopCondition.ShouldStop(fireworks))
            {
                fireworks = MakeStep(fireworks);
                stepNumber++;
            }

            throw new NotImplementedException();
        }

        // Does not change state of this instance - TODO: poor design?
        public IEnumerable<Firework> MakeStep(IEnumerable<Firework> currentFireworks)
        {
            IEnumerable<double> fireworkQualities = currentFireworks.Select(fw => fw.Quality);

            IEnumerable<Firework> explosionSparks = new List<Firework>();
            IEnumerable<Firework> specificSparks = new List<Firework>(Settings.SpecificSparksNumber);
            IEnumerable<int> specificSparkParentIndices = randomizer.NextInt32s(Settings.SpecificSparksNumber, 0, Settings.LocationsNumber);
            int currentFirework = 0;
            foreach (Firework firework in currentFireworks)
            {
                Explosion explosion = exploder.Explode(firework, fireworkQualities, stepNumber);
                explosionSparks = explosionSparks.Concat(explosionSparkGenerator.CreateSparks(explosion));
                if (specificSparkParentIndices.Contains(currentFirework))
                {
                    specificSparks = specificSparks.Concat(specificSparkGenerator.CreateSparks(explosion));
                }

                currentFirework++;
            }

            foreach (Firework explosionSpark in explosionSparks)
            {
                explosionSpark.Quality = ProblemToSolve.CalculateQuality(explosionSpark.Coordinates);
            }

            foreach (Firework specificSpark in specificSparks)
            {
                specificSpark.Quality = ProblemToSolve.CalculateQuality(specificSpark.Coordinates);
            }

            IEnumerable<Firework> allFireworks = currentFireworks.Concat(explosionSparks.Concat(specificSparks));
            return locationSelector.Select(allFireworks);
        }
    }
}