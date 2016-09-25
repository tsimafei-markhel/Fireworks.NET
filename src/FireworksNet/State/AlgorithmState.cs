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
        private IEnumerable<Firework> fireworks;
        private int stepNumber;
        private Solution bestSolution;

        /// <summary>
        /// Gets unique state identifier.
        /// </summary>
        public TId Id { get; private set; }

        /// <summary>
        /// Gets or sets a collection of current fireworks.
        /// </summary>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="value"/>
        /// is <c>null</c>.</exception>
        public IEnumerable<Firework> Fireworks
        {
            get { return this.fireworks; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                this.fireworks = value;
            }
        }

        /// <summary>
        /// Gets or sets the step number.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException"> if <paramref name="value"/>
        /// is less than zero.</exception>
        public int StepNumber
        {
            get { return this.stepNumber; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                this.stepNumber = value;
            }
        }

        /// <summary>
        /// Gets or sets the best solution among <see cref="AlgorithmState"/>.Fireworks.
        /// </summary>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="value"/>
        /// is <c>null</c>.</exception>
        public Solution BestSolution
        {
            get { return this.bestSolution; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                this.bestSolution = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="AlgorithmState"/>.
        /// </summary>
        /// <param name="fireworks">A collection of fireworks for this state.</param>
        /// <param name="stepNumber">Current step number.</param>
        /// <param name="bestSolution">The best solution in this state.</param>
        public AlgorithmState(IEnumerable<Firework> fireworks, int stepNumber, Solution bestSolution)
        {
            this.Id = new TId();
            this.Fireworks = fireworks;
            this.StepNumber = stepNumber;
            this.BestSolution = bestSolution;
        }
    }
}