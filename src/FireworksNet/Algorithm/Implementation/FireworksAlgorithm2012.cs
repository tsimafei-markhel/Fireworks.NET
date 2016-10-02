using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FireworksNet.Differentiation;
using FireworksNet.Distances;
using FireworksNet.Distributions;
using FireworksNet.Explode;
using FireworksNet.Extensions;
using FireworksNet.Fit;
using FireworksNet.Generation;
using FireworksNet.Model;
using FireworksNet.Problems;
using FireworksNet.Random;
using FireworksNet.Selection;
using FireworksNet.Selection.Extremum;
using FireworksNet.Solving;
using FireworksNet.State;
using FireworksNet.StopConditions;

namespace FireworksNet.Algorithm.Implementation
{
    /// <summary>
    /// Fireworks Algorithm implementation, per 2012 paper.
    /// </summary>
    public sealed class FireworksAlgorithm2012 : FireworksAlgorithmBase<FireworksAlgorithmSettings2012>, IStepperFireworksAlgorithm
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
        /// Gets or sets the sampling selector.
        /// </summary>
        public IFireworkSelector SamplingSelector { get; set; }

        /// <summary>
        /// Gets or sets the explosion settings.
        /// </summary>
        public ExploderSettings ExploderSettings { get; set; }

        /// <summary>
        /// Gets or sets the explosion generator.
        /// </summary>
        public IExploder Exploder { get; set; }

        /// <summary>
        /// Gets or sets the differentiator.
        /// </summary>
        public IDifferentiator Differentiator { get; set; }

        /// <summary>
        /// Gets or sets the ploynomial fit.
        /// </summary>
        public IFit PolynomialFit { get; set; }

        /// <summary>
        /// Gets or sets the function solver.
        /// </summary>
        public ISolver FunctionSolver { get; set; }

        /// <summary>
        /// Gets or sets the elite strategy generator.
        /// </summary>
        public ISparkGenerator EliteStrategyGenerator { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FireworksAlgorithm2012"/> class.
        /// </summary>
        /// <param name="problem">The problem to be solved by the algorithm.</param>
        /// <param name="stopCondition">The stop condition for the algorithm.</param>
        /// <param name="settings">The algorithm settings.</param>
        public FireworksAlgorithm2012(Problem problem, IStopCondition stopCondition, FireworksAlgorithmSettings2012 settings)
            : base(problem, stopCondition, settings)
        {
            this.Randomizer = new DefaultRandom();
            this.BestWorstFireworkSelector = new ExtremumFireworkSelector(problem.Target);
            this.Distribution = new NormalDistribution(FireworksAlgorithm2012.normalDistributionMean, FireworksAlgorithm2012.normalDistributionStdDev);
            this.InitialSparkGenerator = new InitialSparkGenerator(problem.Dimensions, problem.InitialRanges, this.Randomizer);
            this.ExplosionSparkGenerator = new ExplosionSparkGenerator2012(problem.Dimensions, this.Randomizer);
            this.SpecificSparkGenerator = new GaussianSparkGenerator2012(problem.Dimensions, this.Distribution);
            this.DistanceCalculator = new EuclideanDistance(problem.Dimensions);
            this.LocationSelector = new DistanceBasedFireworkSelector(this.DistanceCalculator, this.BestWorstFireworkSelector, this.Settings.LocationsNumber);
            this.SamplingSelector = new BestFireworkSelector(new Func<IEnumerable<Firework>, Firework>(this.BestWorstFireworkSelector.SelectBest));
            this.ExploderSettings = new ExploderSettings
            {
                ExplosionSparksNumberModifier = settings.ExplosionSparksNumberModifier,
                ExplosionSparksNumberLowerBound = settings.ExplosionSparksNumberLowerBound,
                ExplosionSparksNumberUpperBound = settings.ExplosionSparksNumberUpperBound,
                ExplosionSparksMaximumAmplitude = settings.ExplosionSparksMaximumAmplitude,
                SpecificSparksPerExplosionNumber = settings.SpecificSparksPerExplosionNumber
            };

            this.Exploder = new Exploder(this.ExploderSettings, this.BestWorstFireworkSelector);
            this.Differentiator = new Differentiator();
            this.PolynomialFit = new PolynomialFit(this.Settings.FunctionOrder);
            this.FunctionSolver = new Solver();
            this.EliteStrategyGenerator = new LS2EliteStrategyGenerator(problem.Dimensions, this.PolynomialFit, this.Differentiator, this.FunctionSolver);
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
        /// Represents one iteration of the algorithm.
        /// </summary>
        public void MakeStepInternal()
        {
            Debug.Assert(this.state.StepNumber >= 0, "Negative step number");
            Debug.Assert(this.state.StepNumber < int.MaxValue, "Updated step number is int.MaxValue, further steps will be incorrect");
            Debug.Assert(this.state.Fireworks != null, "State firework collection is null");
            Debug.Assert(this.Settings != null, "Settings is null");
            Debug.Assert(this.Randomizer != null, "Randomizer is null");
            Debug.Assert(this.Settings.LocationsNumber >= 0, "Negative settings locations number");
            Debug.Assert(this.Exploder != null, "Exploder is null");
            Debug.Assert(this.ExplosionSparkGenerator != null, "Explosion spark generator is null");
            Debug.Assert(this.BestWorstFireworkSelector != null, "Best-Worst firework selector is null");

            int stepNumber = this.state.StepNumber + 1;

            IEnumerable<int> specificSparkParentIndices = this.Randomizer.NextUniqueInt32s(this.Settings.SpecificSparksNumber, 0, this.Settings.LocationsNumber);

            IEnumerable<Firework> explosionSparks = new List<Firework>();
            IEnumerable<Firework> specificSparks = new List<Firework>(this.Settings.SpecificSparksNumber);
            int currentFirework = 0;
            foreach (Firework firework in this.state.Fireworks)
            {
                Debug.Assert(firework != null, "Firework is null");

                ExplosionBase explosion = this.Exploder.Explode(firework, this.state.Fireworks, stepNumber);
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
            IList<Firework> selectedFireworks = this.LocationSelector.SelectFireworks(allFireworks).ToList();

            IEnumerable<Firework> samplingFireworks = this.SamplingSelector.SelectFireworks(selectedFireworks, this.Settings.SamplingNumber);
            ExplosionBase eliteExplosion = new EliteExplosion(stepNumber, this.Settings.SamplingNumber, samplingFireworks);
            Firework eliteFirework = this.EliteStrategyGenerator.CreateSpark(eliteExplosion);
            this.CalculateQuality(eliteFirework);

            Firework worstFirework = this.BestWorstFireworkSelector.SelectWorst(selectedFireworks);
            if (this.ShouldReplaceWorstWithElite(worstFirework, eliteFirework))
            {
                selectedFireworks.Remove(worstFirework);
                selectedFireworks.Add(eliteFirework);
            }

            this.state.Fireworks = selectedFireworks;
            this.state.StepNumber = stepNumber;
            this.state.BestSolution = this.BestWorstFireworkSelector.SelectBest(selectedFireworks);
        }

        /// <summary>
        /// Compares two <see cref="Firework"/>s and determines if it is necessary to replace the worst one 
        /// with the elite one according to the elite strategy.
        /// </summary>
        /// <param name="worst">The worst firework on current step.</param>
        /// <param name="elite">The elite firework on current step calculated by 
        /// elite strategy</param>
        /// <returns><c>true</c> if necessary replace <paramref name="worst"/> with 
        /// <paramref name="elite"/>.</returns>
        public bool ShouldReplaceWorstWithElite(Firework worst, Firework elite)
        {
            Debug.Assert(worst != null, "Worst firework is null");
            Debug.Assert(elite != null, "Elite firework is null");
            Debug.Assert(this.ProblemToSolve != null, "Problem to solve is null");

            return this.ProblemToSolve.Target == ProblemTarget.Minimum ? worst.Quality.IsGreater(elite.Quality) : worst.Quality.IsLess(elite.Quality);
        }
    }
}