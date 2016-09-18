using System;
using FireworksNet.Model;
using FireworksNet.Problems;
using FireworksNet.StopConditions;

namespace FireworksNet.Algorithm.Implementation
{
    /// <summary>
    /// Base class for Fireworks Algorithm implementation.
    /// </summary>
    /// <typeparam name="TSettings">Algorithm settings type.</typeparam>
    public abstract class FireworksAlgorithmBase<TSettings> : IFireworksAlgorithm where TSettings : class
    {
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
        public TSettings Settings { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FireworksAlgorithmBase{TSettings}"/> class.
        /// </summary>
        /// <param name="problem">The problem to be solved by the algorithm.</param>
        /// <param name="stopCondition">The stop condition for the algorithm.</param>
        /// <param name="settings">The algorithm settings.</param>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="problem"/>
        /// or <paramref name="stopCondition"/> or <paramref name="settings"/> is 
        /// <c>null</c>.</exception>
        protected FireworksAlgorithmBase(Problem problem, IStopCondition stopCondition, TSettings settings)
        {
            if (problem == null)
            {
                throw new ArgumentNullException(nameof(problem));
            }

            if (stopCondition == null)
            {
                throw new ArgumentNullException(nameof(stopCondition));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            this.ProblemToSolve = problem;
            this.StopCondition = stopCondition;
            this.Settings = settings;
        }

        /// <summary>
        /// Solves the specified problem by running the algorithm.
        /// </summary>
        /// <returns><see cref="Solution"/> instance that represents
        /// best solution found during the algorithm run.</returns>
        public abstract Solution Solve();
    }
}
