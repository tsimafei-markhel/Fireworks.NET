﻿using System;
using System.Collections.Generic;

namespace FireworksNet.Model
{
	/// <summary>
	/// Stores current algorithm state.
	/// </summary>
	/// <remarks>This class is not thread-safe.</remarks>
	public class AlgorithmState
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
                return id;
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
				return fireworks;
			}

			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}

				fireworks = value;
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
				return stepNumber;
			}

			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				stepNumber = value;
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
				return bestSolution;
			}

			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}

				bestSolution = value;
			}
		}
	}
}