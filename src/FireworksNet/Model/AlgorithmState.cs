using System;
using System.Collections.Generic;

namespace FireworksNet.Model
{
    /// <summary>
    /// Stores current algorithm state.
    /// </summary>
    /// <remarks>This class is not thread-safe.</remarks>
    public class AlgorithmState // TODO: : IEquatable<AlgorithmState>
    {
        private readonly TId id = new TId();
        private IEnumerable<Firework> fireworks;
        private int stepNumber;
        private Solution bestSolution;

        /// <summary>
        /// Gets unique state identifier.
        /// </summary>
        public TId Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        /// Gets or sets a collection of current fireworks.
        /// </summary>
        /// <exception cref="System.ArgumentNullException"> if value is null.</exception>
        public IEnumerable<Firework> Fireworks
        {
            get
            {
                return this.fireworks;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                this.fireworks = value;
            }
        }

        /// <summary>
        /// Gets or sets the step number.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException"> if value is less than 0.</exception>
        public int StepNumber
        {
            get
            {
                return this.stepNumber;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                this.stepNumber = value;
            }
        }

        /// <summary>
        /// Gets or sets the best solution among <see cref="AlgorithmState.Fireworks"/>.
        /// </summary>
        /// <exception cref="System.ArgumentNullException"> if value is null.</exception>
        public Solution BestSolution
        {
            get
            {
                return this.bestSolution;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                this.bestSolution = value;
            }
        }
    }
}