using System;
using System.Collections.Generic;
using System.Diagnostics;
using FireworksNet.Model;

namespace FireworksNet.Problems
{
    /// <summary>
    /// Describes a problem that is to be solved.
    /// </summary>
    public class Problem
    {
        private readonly Func<IDictionary<Dimension, double>, double> targetFunction;

        /// <summary>
        /// Fired before the solution quality is calculated.
        /// </summary>
        public event EventHandler<QualityCalculatingEventArgs> QualityCalculating;

        /// <summary>
        /// Fired after the solution quality is calculated.
        /// </summary>
        public event EventHandler<QualityCalculatedEventArgs> QualityCalculated;

        /// <summary>
        /// Gets the dimensions of the problem.
        /// </summary>
        public IList<Dimension> Dimensions { get; private set; }

        /// <summary>
        /// Gets the initial ranges for the problem dimensions.
        /// </summary>
        public IDictionary<Dimension, Range> InitialRanges { get; private set; }

        /// <summary>
        /// Gets the target of the problem (minimize or maximize it).
        /// </summary>
        public ProblemTarget Target { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="Problem"/> type.
        /// </summary>
        /// <param name="dimensions">The dimensions of the problem.</param>
        /// <param name="initialRanges">The initial ranges of the problem dimensions.</param>
        /// <param name="targetFunction">The quality function that needs to be optimized.</param>
        /// <param name="target">Target of the problem.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="dimensions"/> or
        /// <paramref name="initialRanges"/> or <paramref name="targetFunction"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">if <paramref name="dimensions"/>.Count is zero or
        /// <paramref name="dimensions"/>.Count differs from <paramref name="initialRanges"/>.Count
        /// or <paramref name="dimensions"/> does not contain same keys as <paramref name="initialRanges"/>.
        /// </exception>
        public Problem(IList<Dimension> dimensions, IDictionary<Dimension, Range> initialRanges, Func<IDictionary<Dimension, double>, double> targetFunction, ProblemTarget target)
        {
            if (dimensions == null)
            {
                throw new ArgumentNullException(nameof(dimensions));
            }

            if (dimensions.Count == 0)
            {
                throw new ArgumentException(string.Empty, nameof(dimensions));
            }

            if (initialRanges == null)
            {
                throw new ArgumentNullException(nameof(initialRanges));
            }

            if (initialRanges.Count != dimensions.Count)
            {
                throw new ArgumentException(string.Empty, nameof(initialRanges));
            }

            foreach(Dimension dimension in dimensions)
            {
                if (!initialRanges.ContainsKey(dimension))
                {
                    throw new ArgumentException(string.Empty, nameof(initialRanges));
                }
            }

            if (targetFunction == null)
            {
                throw new ArgumentNullException(nameof(targetFunction));
            }

            this.Dimensions = dimensions;
            this.InitialRanges = initialRanges;
            this.targetFunction = targetFunction;
            this.Target = target;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Problem"/> type with default initial ranges.
        /// </summary>
        /// <param name="dimensions">The dimensions of the problem.</param>
        /// <param name="targetFunction">The quality function that needs to be optimized.</param>
        /// <param name="target">Target of the problem.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="dimensions"/> or
        /// <paramref name="targetFunction"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">if <paramref name="dimensions"/>.Count is zero.
        /// </exception>
        public Problem(IList<Dimension> dimensions, Func<IDictionary<Dimension, double>, double> targetFunction, ProblemTarget target)
            : this(dimensions, Problem.CreateDefaultInitialRanges(dimensions), targetFunction, target)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Problem"/> type.
        /// </summary>
        /// <param name="dimensions">The dimensions of the problem.</param>
        /// <param name="targetFunction">The quality function that needs to be optimized.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="dimensions"/> or
        /// <paramref name="targetFunction"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">if <paramref name="dimensions"/>.Count is zero.
        /// </exception>
        public Problem(IList<Dimension> dimensions, Func<IDictionary<Dimension, double>, double> targetFunction)
            : this(dimensions, Problem.CreateDefaultInitialRanges(dimensions), targetFunction, ProblemTarget.Minimum)
        {
        }

        /// <summary>
        /// Calculates the quality of the solution.
        /// </summary>
        /// <param name="coordinateValues">The solution coordinates.</param>
        /// <returns>Quality of the solution at <paramref name="coordinateValues"/>
        /// coordinates.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="coordinateValues"/>
        /// is <c>null</c>.</exception>
        public virtual double CalculateQuality(IDictionary<Dimension, double> coordinateValues)
        {
            if (coordinateValues == null)
            {
                throw new ArgumentNullException(nameof(coordinateValues));
            }

            Debug.Assert(this.targetFunction != null, "Target function is null");

            this.OnQualityCalculating(new QualityCalculatingEventArgs(coordinateValues));
            double result = this.targetFunction(coordinateValues);
            this.OnQualityCalculated(new QualityCalculatedEventArgs(coordinateValues, result));
            return result;
        }

        /// <summary>
        /// Creates the collection of initial ranges for given <see cref="Dimension"/>s from
        /// ranges of <paramref name="dimensions"/>.
        /// </summary>
        /// <param name="dimensions">The collection of <see cref="Dimension"/>s to
        /// create initial ranges for.</param>
        /// <returns>The collection of initial ranges for given <see cref="Dimension"/>s from
        /// ranges of <paramref name="dimensions"/>.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="dimensions"/> is
        /// <c>null</c>.</exception>
        public static IDictionary<Dimension, Range> CreateDefaultInitialRanges(IList<Dimension> dimensions)
        {
            if (dimensions == null)
            {
                throw new ArgumentNullException(nameof(dimensions));
            }

            Dictionary<Dimension, Range> initialRanges = new Dictionary<Dimension, Range>(dimensions.Count);
            foreach (Dimension dimension in dimensions)
            {
                Debug.Assert(dimension != null, "Dimension is null");
                Debug.Assert(dimension.VariationRange != null, "Dimension variation range is null");

                initialRanges.Add(dimension, dimension.VariationRange);
            }

            return initialRanges;
        }

        /// <summary>
        /// Firing an event before calculating quality of a firework.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnQualityCalculating(QualityCalculatingEventArgs eventArgs)
        {
            this.QualityCalculating?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// Firing an event after calculating quality of a firework.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnQualityCalculated(QualityCalculatedEventArgs eventArgs)
        {
            this.QualityCalculated?.Invoke(this, eventArgs);
        }
    }
}