using System;
using System.Collections.Generic;

namespace FireworksNet.Model
{
    /// <summary>
    /// Represents mutable algorithm state.
    /// </summary>
    /// <remarks>This class is not thread-safe.</remarks>
    public class MutableAlgorithmState : AlgorithmState
    {
        /// <summary>
        /// Initializes a new instance of <see cref="MutableAlgorithmState"/>.
        /// </summary>
        /// <param name="fireworks">A collection of fireworks for this state.</param>
        /// <param name="stepNumber">Current step number.</param>
        /// <param name="bestSolution">The best solution in this state.</param>
        public MutableAlgorithmState(IEnumerable<Firework> fireworks, int stepNumber, Solution bestSolution)
            : base(fireworks, stepNumber, bestSolution)
        {
        }

        /// <summary>
        /// Updates the current instance with the new state.
        /// </summary>
        /// <param name="fireworks">New collection of fireworks for this state.</param>
        /// <param name="stepNumber">New step number.</param>
        /// <param name="bestSolution">New best solution in this state.</param>
        public void UpdateState(IEnumerable<Firework> fireworks, int stepNumber, Solution bestSolution)
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

            this.Fireworks = fireworks;
            this.StepNumber = stepNumber;
            this.BestSolution = bestSolution;
        }
    }
}