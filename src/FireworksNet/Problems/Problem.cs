using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FireworksNet.Extensions;
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
        /// Fired before looking for the best firework.
        /// </summary>
        public event EventHandler<BestFireworkFindingEventArgs> BestFireworkFinding;

        /// <summary>
        /// Fired after the best firework is found.
        /// </summary>
        public event EventHandler<BestFireworkFoundEventArgs> BestFireworkFound;

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
        /// Gets the best <see cref="Firework"/> among the <paramref name="fireworks"/>.
        /// </summary>
        /// <param name="fireworks">The collection of <see cref="Firework"/>s to look
        /// the best one among.</param>
        /// <returns>The best <see cref="Firework"/> among the 
        /// <paramref name="fireworks"/>.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="fireworks"/> is
        /// <c>null</c>.</exception>
        public virtual Firework GetBest(IEnumerable<Firework> fireworks)
        {
            if (fireworks == null)
            {
                throw new ArgumentNullException(nameof(fireworks));
            }

            this.OnBestFireworkFinding(new BestFireworkFindingEventArgs(fireworks));

            Firework bestFirework = null;
            if (this.Target == ProblemTarget.Minimum)
            {
                bestFirework = fireworks.Aggregate(this.GetLessQualityFirework);
            }
            else
            {
                bestFirework = fireworks.Aggregate(this.GetGreaterQualityFirework);
            }

            this.OnBestFireworkFound(new BestFireworkFoundEventArgs(fireworks, bestFirework));
            return bestFirework;
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
        /// Gets <see cref="Firework"/> with minimum quality.
        /// </summary>
        /// <param name="currentMinimum">Current minimum quality <see cref="Firework"/>.</param>
        /// <param name="candidate">The <see cref="Firework"/> to be compared with
        /// <paramref name="currentMinimum"/>.</param>
        /// <returns>The <see cref="Firework"/> with minimum quality.</returns>
        protected virtual Firework GetLessQualityFirework(Firework currentMinimum, Firework candidate)
        {
            Debug.Assert(currentMinimum != null, "Current minimum is null");
            Debug.Assert(candidate != null, "Candidate for minimum is null");

            return candidate.Quality.IsLess(currentMinimum.Quality) ? candidate : currentMinimum;
        }

        /// <summary>
        /// Gets <see cref="Firework"/> with maximum quality.
        /// </summary>
        /// <param name="currentMaximum">Current maximum quality <see cref="Firework"/>.</param>
        /// <param name="candidate">The <see cref="Firework"/> to be compared with
        /// <paramref name="currentMaximum"/>.</param>
        /// <returns>The <see cref="Firework"/> with maximum quality.</returns>
        protected virtual Firework GetGreaterQualityFirework(Firework currentMaximum, Firework candidate)
        {
            Debug.Assert(currentMaximum != null, "Current maximum is null");
            Debug.Assert(candidate != null, "Candidate for maximum is null");

            return candidate.Quality.IsGreater(currentMaximum.Quality) ? candidate : currentMaximum;
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

        /// <summary>
        /// Firing an event before searching for the best firework.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnBestFireworkFinding(BestFireworkFindingEventArgs eventArgs)
        {
            this.BestFireworkFinding?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// Firing an event after finding the best firework.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnBestFireworkFound(BestFireworkFoundEventArgs eventArgs)
        {
            this.BestFireworkFound?.Invoke(this, eventArgs);
        }
    }

    /// <summary>
    /// Arguments of the Problem.QualityCalculating event.
    /// </summary>
    public class QualityCalculatingEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the coordinates of the solution to calculate quality for.
        /// </summary>
        public IDictionary<Dimension, double> CoordinateValues { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="QualityCalculatingEventArgs"/> type.
        /// </summary>
        /// <param name="coordinateValues">The coordinates of the solution to calculate
        /// quality for.</param>
        public QualityCalculatingEventArgs(IDictionary<Dimension, double> coordinateValues)
        {
            this.CoordinateValues = coordinateValues;
        }
    }

    /// <summary>
    /// Arguments of the Problem.QualityCalculated event.
    /// </summary>
    public class QualityCalculatedEventArgs : QualityCalculatingEventArgs
    {
        /// <summary>
        /// Gets the quality of the solution.
        /// </summary>
        public double Quality { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="QualityCalculatedEventArgs"/> type.
        /// </summary>
        /// <param name="coordinateValues">The coordinates of the solution to calculate
        /// quality for.</param>
        /// <param name="quality">The calculated solution quality.</param>
        public QualityCalculatedEventArgs(IDictionary<Dimension, double> coordinateValues, double quality)
            : base(coordinateValues)
        {
            this.Quality = quality;
        }
    }

    /// <summary>
    /// Arguments of the Problem.BestFireworkFinding event.
    /// </summary>
    public class BestFireworkFindingEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the collection of <see cref="Firework"/>s to find the best one among.
        /// </summary>
        public IEnumerable<Firework> FireworksToCheck { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="BestFireworkFindingEventArgs"/> type.
        /// </summary>
        /// <param name="fireworksToCheck">The collection of <see cref="Firework"/>s to
        /// find the best one among.</param>
        public BestFireworkFindingEventArgs(IEnumerable<Firework> fireworksToCheck)
        {
            this.FireworksToCheck = fireworksToCheck;
        }
    }

    /// <summary>
    /// Arguments of the Problem.BestFireworkFound event.
    /// </summary>
    public class BestFireworkFoundEventArgs : BestFireworkFindingEventArgs
    {
        /// <summary>
        /// Gets the best <see cref="Firework"/>.
        /// </summary>
        public Firework BestFirework { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="BestFireworkFoundEventArgs"/> type.
        /// </summary>
        /// <param name="fireworksToCheck">The collection of <see cref="Firework"/>s to
        /// find the best one among.</param>
        /// <param name="bestFirework">The best <see cref="Firework"/> found.</param>
        public BestFireworkFoundEventArgs(IEnumerable<Firework> fireworksToCheck, Firework bestFirework)
            : base(fireworksToCheck)
        {
            this.BestFirework = bestFirework;
        }
    }
}