using System;
using System.Collections.Generic;
using FireworksNet.Distributions;
using FireworksNet.Explode;
using FireworksNet.Model;
using FireworksNet.Problems;
using FireworksNet.Randomization;

namespace FireworksNet.Implementation
{
    // Per 2010 paper
    public sealed class FireworksAlgorithm : IFireworksAlgorithm
    {
        private readonly IRandom randomizer;
        private readonly IContinuousDistribution distribution;
        private readonly ISparkGenerator initialSparkGenerator;
        private readonly ISparkGenerator explosionSparkGenerator;
        private readonly ISparkGenerator specificSparkGenerator;
        private readonly ExploderSettings exploderSettings;
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
            this.exploderSettings = new ExploderSettings()
            {
                ExplosionSparksNumberModifier = settings.ExplosionSparksNumberModifier,
                ExplosionSparksNumberLowerBound = settings.ExplosionSparksNumberLowerBound,
                ExplosionSparksNumberUpperBound = settings.ExplosionSparksNumberUpperBound,
                ExplosionSparksMaximumAmplitude = settings.ExplosionSparksMaximumAmplitude,
                SpecificSparksNumber = settings.SpecificSparksNumber,
                MinAllowedExplosionSparksNumberExact = settings.MinAllowedExplosionSparksNumberExact,
                MaxAllowedExplosionSparksNumberExact = settings.MaxAllowedExplosionSparksNumberExact,
                MinAllowedExplosionSparksNumber = settings.MinAllowedExplosionSparksNumber,
                MaxAllowedExplosionSparksNumber = settings.MaxAllowedExplosionSparksNumber
            }; // TODO: AutoMapper or something like this can be used here.

            this.stepNumber = 0;
        }

        public Firework Solve()
        {
            stepNumber = 0;

            InitialExplosion initialExplosion = new InitialExplosion(Settings.LocationsNumber);
            IEnumerable<Firework> fireworks = initialSparkGenerator.CreateSparks(initialExplosion);
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
            throw new NotImplementedException();
        }
    }
}