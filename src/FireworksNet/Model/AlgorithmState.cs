using System;
using System.Collections.Generic;

namespace FireworksNet.Model
{
    /// <summary>
    /// Stores current algorithm state.
    /// </summary>
    /// <remarks>This class is not thread-safe.</remarks>
    public class AlgorithmState
    {
        /// <summary>
        /// Gets unique state identifier.
        /// </summary>
        public TId Id { get; private set; }

        /// <summary>
        /// Gets or sets a collection of current fireworks.
        /// </summary>
        /// <exception cref="System.ArgumentNullException"> if value is null.</exception>
        public IEnumerable<Firework> Fireworks { get; private set; }

        /// <summary>
        /// Gets or sets the step number.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException"> if value is less than zero.</exception>
        public int StepNumber { get; private set; }

        /// <summary>
        /// Gets or sets the best solution among <see cref="AlgorithmState"/>.Fireworks.
        /// </summary>
        /// <exception cref="System.ArgumentNullException"> if value is null.</exception>
        public Solution BestSolution { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="AlgorithmState"/>.
        /// </summary>
        /// <param name="fireworks">A collection of fireworks for this state.</param>
        /// <param name="stepNumber">Current step number.</param>
        /// <param name="bestSolution">The best solution in this state.</param>
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