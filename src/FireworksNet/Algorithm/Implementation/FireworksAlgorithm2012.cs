﻿using System;
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
using FireworksNet.Solving;
using FireworksNet.StopConditions;

namespace FireworksNet.Algorithm.Implementation
{
    /// <summary>
    /// Fireworks Algorithm implementation, per 2012 paper.
    /// </summary>
    public sealed class FireworksAlgorithm2012 : FireworksAlgorithmBase<FireworksAlgorithmSettings2012>, IFireworksAlgorithm, IStepperFireworksAlgorithm
    {
        private const double normalDistributionMean = 1.0;
        private const double normalDistributionStdDev = 1.0;

        private readonly IDifferentiator differentiator;
        private readonly IFireworkSelector samplingSelector;
        private readonly IFit polynomialFit;
        private readonly ISolver functionSolver;
        private readonly EliteStrategyGenerator eliteStrategyGenerator;

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
        /// Initializes a new instance of the <see cref="FireworksAlgorithm2012"/> class.
        /// </summary>
        /// <param name="problem">The problem to be solved by the algorithm.</param>
        /// <param name="stopCondition">The stop condition for the algorithm.</param>
        /// <param name="settings">The algorithm settings.</param>
        public FireworksAlgorithm2012(Problem problem, IStopCondition stopCondition, FireworksAlgorithmSettings2012 settings)
            : base(problem, stopCondition, settings)
        {
            this.differentiator = new Differentiator();
            this.samplingSelector = new BestFireworkSelector(new Func<IEnumerable<Firework>, Firework>(problem.GetBest));
            this.polynomialFit = new PolynomialFit(this.Settings.FunctionOrder);
            this.functionSolver = new Solver();
            this.eliteStrategyGenerator = new LS2EliteStrategyGenerator(problem.Dimensions, this.polynomialFit, this.differentiator, this.functionSolver);

            this.Randomizer = new DefaultRandom();
            this.Distribution = new NormalDistribution(FireworksAlgorithm2012.normalDistributionMean, FireworksAlgorithm2012.normalDistributionStdDev);
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
        public AlgorithmState CreateInitialState()
        {
            Debug.Assert(this.Settings != null, "Settings is null");
            Debug.Assert(this.InitialSparkGenerator != null, "Initial spark generator is null");
            Debug.Assert(this.ProblemToSolve != null, "Problem to solve is null");

            InitialExplosion initialExplosion = new InitialExplosion(this.Settings.LocationsNumber);

            Debug.Assert(initialExplosion != null, "Initial explosion is null");

            IEnumerable<Firework> fireworks = this.InitialSparkGenerator.CreateSparks(initialExplosion);

            Debug.Assert(fireworks != null, "Initial firework collection is null");

            this.CalculateQualities(fireworks);

            return new AlgorithmState
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
                throw new ArgumentNullException(nameof(state));
            }

            AlgorithmState newState = new AlgorithmState
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
                throw new ArgumentNullException(nameof(state));
            }

            Debug.Assert(state.StepNumber >= 0, "Negative step number");
            Debug.Assert(state.Fireworks != null, "State firework collection is null");
            Debug.Assert(this.Settings != null, "Settings is null");
            Debug.Assert(this.Randomizer != null, "Randomizer is null");
            Debug.Assert(this.Settings.LocationsNumber >= 0, "Negative settings locations number");
            Debug.Assert(this.Exploder != null, "Exploder is null");
            Debug.Assert(this.ExplosionSparkGenerator != null, "Explosion spark generator is null");
            Debug.Assert(this.ProblemToSolve != null, "Problem to solve is null");

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
            List<Firework> selectedFireworks = this.LocationSelector.SelectFireworks(allFireworks).ToList();

            Firework worstFirework = this.SelectWorstFirework(selectedFireworks);

            IEnumerable<Firework> samplingFireworks = this.samplingSelector.SelectFireworks(selectedFireworks, this.Settings.SamplingNumber);
            ExplosionBase eliteExplosion = new EliteExplosion(state.StepNumber, this.Settings.SamplingNumber, samplingFireworks);

            Firework eliteFirework = this.eliteStrategyGenerator.CreateSpark(eliteExplosion);
            eliteFirework.Quality = this.ProblemToSolve.CalculateQuality(eliteFirework.Coordinates);

            if (this.IsReplaceWorstWithElite(worstFirework, eliteFirework))
            {
                selectedFireworks.Remove(worstFirework);
                selectedFireworks.Add(eliteFirework);
            }

            state.Fireworks = selectedFireworks;
            state.BestSolution = this.ProblemToSolve.GetBest(selectedFireworks);
        }

        /// <summary>
        /// Chooses the worst firework among <paramref name="fireworks"/>.
        /// </summary>
        /// <param name="fireworks">Fireworks to choose the worst one from.</param>
        /// <returns>The worst firework among <paramref name="fireworks"/>.</returns>
        private Firework SelectWorstFirework(IEnumerable<Firework> fireworks)
        {
            Debug.Assert(this.ProblemToSolve != null, "Problem to solve is null");
            Debug.Assert(fireworks != null, "Firework collection is null");

            if (this.ProblemToSolve.Target == ProblemTarget.Minimum)
            {
                return fireworks.OrderByDescending(fw => fw.Quality).First();
            }
            else
            {
                return fireworks.OrderBy(fw => fw.Quality).First();
            }
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
        private bool IsReplaceWorstWithElite(Firework worst, Firework elite)
        {
            Debug.Assert(worst != null, "Worst firework is null");
            Debug.Assert(elite != null, "Elite firework is null");
            Debug.Assert(this.ProblemToSolve != null, "Problem to solve is null");

            return this.ProblemToSolve.Target == ProblemTarget.Minimum ? worst.Quality.IsGreater(elite.Quality) : worst.Quality.IsLess(elite.Quality);
        }
    }
}