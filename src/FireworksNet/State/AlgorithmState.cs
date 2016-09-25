using System;
using System.Collections.Generic;
using FireworksNet.Model;

namespace FireworksNet.State
{
    /// <summary>
    /// Stores current algorithm state.
    /// </summary>
    /// <remarks>This class is not thread-safe.</remarks>
    public class AlgorithmState : IAlgorithmState
    {
        /// <summary>
        /// Gets unique state identifier.
        /// </summary>
        public TId Id { get; private set; }

        /// <summary>
        /// Gets or sets a collection of current fireworks.
        /// </summary>
        public IEnumerable<Firework> Fireworks { get; protected set; }

        /// <summary>
        /// Gets or sets the step number.
        /// </summary>
        public int StepNumber { get; protected set; }

        /// <summary>
        /// Gets or sets the best solution among <see cref="AlgorithmState"/>.Fireworks.
        /// </summary>
        public Solution BestSolution { get; protected set; }

        /// <summary>
        /// Initializes a new instance of <see cref="AlgorithmState"/>.
        /// </summary>
        /// <param name="fireworks">A collection of fireworks for this state.</param>
        /// <param name="stepNumber">Current step number.</param>
        /// <param name="bestSolution">The best solution in this state.</param>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="fireworks"/> or
        /// <paramref name="bestSolution"/> is <c>null</c>.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException"> if <paramref name="stepNumber"/> is less than zero.</exception>
        public AlgorithmState(IEnumerable<Firework> fireworks, int stepNumber, Solution bestSolution)
        {
            if (fireworks == null)
            {
                throw new ArgumentNullException(nameof(fireworks));
            }

            if (stepNumber < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(stepNumber));
            }

            if (bestSolution == null)
            {
                throw new ArgumentNullException(nameof(bestSolution));
            }

            this.Id = new TId();
            this.Fireworks = fireworks;
            this.StepNumber = stepNumber;
            this.BestSolution = bestSolution;
        }
    }
}