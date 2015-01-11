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

            IDictionary<Firework, Explosion> explosions = new Dictionary<Firework, Explosion>(Settings.LocationsNumber);
            IEnumerable<Firework> explosionSparks = new List<Firework>();
            IEnumerable<Firework> specificSparks = new List<Firework>(Settings.SpecificSparksNumber);
            IEnumerable<int> specificSparkParentIndices = randomizer.NextInt32s(Settings.SpecificSparksNumber, 0, Settings.LocationsNumber);
            int currentFirework = 0;
            foreach (Firework firework in currentFireworks)
            {
                Explosion explosion = exploder.Explode(firework, fireworkQualities, stepNumber);
                explosions.Add(firework, explosion);
                explosionSparks = explosionSparks.Concat(explosionSparkGenerator.CreateSparks(explosion));
                if (specificSparkParentIndices.Contains(currentFirework))
                {
                    specificSparks = specificSparks.Concat(specificSparkGenerator.CreateSparks(explosion));
                }

                currentFirework++;
            }

            IEnumerable<Firework> allFireworks = currentFireworks.Concat(explosionSparks.Concat(specificSparks));
            return SelectLocations(allFireworks);
        }

        // allCurrentFireworks include:
        // - fireworks existed in the beginning of the current step;
        // - explosion sparks generated on this step;
        // - specific sparks generated on this step.
        private IEnumerable<Firework> SelectLocations(IEnumerable<Firework> allCurrentFireworks)
        {
            List<Firework> selectedLocations = new List<Firework>(Settings.LocationsNumber);

            // 1. Find a firework with best quality - it will be kept anyways
            Firework bestFirework = ProblemToSolve.GetBest(allCurrentFireworks);
            selectedLocations.Add(bestFirework);

            // 2. Calculate distances between all fireworks
            IDictionary<Firework, Double> distances = CalculateDistances(allCurrentFireworks);

            // 3. Calculate probabilities for each firework
            IDictionary<Firework, Double> probabilities = CalculateProbabilities(distances);

            // 4. Select desiredLocationsNumber - 1 of fireworks based on the probabilities
            IOrderedEnumerable<KeyValuePair<Firework, Double>> sortedProbabilities = probabilities.OrderByDescending(p => p.Value, new DoubleExtensionComparer());
            IEnumerable<Firework> otherSelectedLocations = sortedProbabilities.Where(sp => sp.Key != bestFirework).Take(Settings.LocationsNumber - 1).Select(sp => sp.Key);
            selectedLocations.AddRange(otherSelectedLocations);

            return selectedLocations;
        }

        private IDictionary<Firework, Double> CalculateProbabilities(IDictionary<Firework, Double> distances)
        {
            Dictionary<Firework, double> probabilities = new Dictionary<Firework, double>(distances.Count());
            double distancesSum = distances.Values.Sum();
            foreach (KeyValuePair<Firework, double> distance in distances)
            {
                double probability = distance.Value / distancesSum;
                probabilities.Add(distance.Key, probability);
            }

            return probabilities;
        }

        private IDictionary<Firework, Double> CalculateDistances(IEnumerable<Firework> allCurrentFireworks)
        {
            Dictionary<Firework, double> distances = new Dictionary<Firework, double>(allCurrentFireworks.Count());
            foreach (Firework firework in allCurrentFireworks)
            {
                distances.Add(firework, 0.0);
                foreach (Firework otherFirework in allCurrentFireworks)
                {
                    distances[firework] += distanceCalculator.Calculate(firework, otherFirework);
                }
            }

            return distances;
        }
    }
}