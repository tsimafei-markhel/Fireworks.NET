using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FireworksNet.Extensions;
using FireworksNet.Model;

namespace FireworksNet.Problems
{
    public class Problem
    {
        private readonly Func<IDictionary<Dimension, double>, double> targetFunction;

        public event EventHandler<QualityCalculatingEventArgs> QualityCalculating;

        public event EventHandler<QualityCalculatedEventArgs> QualityCalculated;

		public event EventHandler<BestFireworkFindingEventArgs> BestFireworkFinding;

		public event EventHandler<BestFireworkFoundEventArgs> BestFireworkFound;

        public IList<Dimension> Dimensions { get; private set; }

		public IDictionary<Dimension, Range> InitialRanges { get; private set; }

		public ProblemTarget Target { get; private set; }

		public Problem(IList<Dimension> dimensions, IDictionary<Dimension, Range> initialRanges, Func<IDictionary<Dimension, double>, double> targetFunction, ProblemTarget target)
        {
            if (dimensions == null)
            {
                throw new ArgumentNullException("dimensions");
            }

			if (dimensions.Count == 0)
			{
				throw new ArgumentException(string.Empty, "dimensions");
			}

			if (initialRanges == null)
			{
				throw new ArgumentNullException("initialRanges");
			}

			if (initialRanges.Count != dimensions.Count)
			{
				throw new ArgumentException(string.Empty, "initialRanges");
			}

			foreach(Dimension dimension in dimensions)
			{
				if (!initialRanges.ContainsKey(dimension))
				{
					throw new ArgumentException(string.Empty, "initialRanges");
				}
			}

            if (targetFunction == null)
            {
                throw new ArgumentNullException("targetFunction");
            }

            this.Dimensions = dimensions;
			this.InitialRanges = initialRanges;
            this.targetFunction = targetFunction;
            this.Target = target;
        }

		public Problem(IList<Dimension> dimensions, Func<IDictionary<Dimension, double>, double> targetFunction, ProblemTarget target)
			: this(dimensions, CreateDefaultInitialRanges(dimensions), targetFunction, target)
        {
        }

		public Problem(IList<Dimension> dimensions, Func<IDictionary<Dimension, double>, double> targetFunction)
			: this(dimensions, CreateDefaultInitialRanges(dimensions), targetFunction, ProblemTarget.Minimum)
        {
        }

        public virtual double CalculateQuality(IDictionary<Dimension, double> coordinateValues)
        {
			if (coordinateValues == null)
			{
				throw new ArgumentNullException("coordinateValues");
			}

			Debug.Assert(targetFunction != null, "Target function is null");

            OnQualityCalculating(new QualityCalculatingEventArgs(coordinateValues));
            double result = targetFunction(coordinateValues);
            OnQualityCalculated(new QualityCalculatedEventArgs(coordinateValues, result));
            return result;
        }

        public virtual Firework GetBest(IEnumerable<Firework> fireworks)
        {
			if (fireworks == null)
			{
				throw new ArgumentNullException("fireworks");
			}

			OnBestFireworkFinding(new BestFireworkFindingEventArgs(fireworks));

			Firework bestFirework = null;
            if (Target == ProblemTarget.Minimum)
            {
				bestFirework = fireworks.Aggregate(GetLessQualityFirework);
            }
            else
            {
				bestFirework = fireworks.Aggregate(GetGreaterQualityFirework);
            }

			OnBestFireworkFound(new BestFireworkFoundEventArgs(fireworks, bestFirework));
			return bestFirework;
        }

		public static IDictionary<Dimension, Range> CreateDefaultInitialRanges(IList<Dimension> dimensions)
		{
			if (dimensions == null)
			{
				throw new ArgumentNullException("dimensions");
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

		protected virtual Firework GetLessQualityFirework(Firework currentMin, Firework candidate)
		{
			Debug.Assert(currentMin != null, "Current minimum is null");
			Debug.Assert(candidate != null, "Candidate for minimum is null");

			return candidate.Quality.IsLess(currentMin.Quality) ? candidate : currentMin;
		}

		protected virtual Firework GetGreaterQualityFirework(Firework currentMax, Firework candidate)
		{
			Debug.Assert(currentMax != null, "Current maximum is null");
			Debug.Assert(candidate != null, "Candidate for maximum is null");

			return candidate.Quality.IsGreater(currentMax.Quality) ? candidate : currentMax;
		}

        protected virtual void OnQualityCalculating(QualityCalculatingEventArgs eventArgs)
        {
            EventHandler<QualityCalculatingEventArgs> handler = QualityCalculating;
            if (handler != null)
            {
                handler(this, eventArgs);
            }
        }

        protected virtual void OnQualityCalculated(QualityCalculatedEventArgs eventArgs)
        {
            EventHandler<QualityCalculatedEventArgs> handler = QualityCalculated;
            if (handler != null)
            {
                handler(this, eventArgs);
            }
        }

		protected virtual void OnBestFireworkFinding(BestFireworkFindingEventArgs eventArgs)
		{
			EventHandler<BestFireworkFindingEventArgs> handler = BestFireworkFinding;
			if (handler != null)
			{
				handler(this, eventArgs);
			}
		}

		protected virtual void OnBestFireworkFound(BestFireworkFoundEventArgs eventArgs)
		{
			EventHandler<BestFireworkFoundEventArgs> handler = BestFireworkFound;
			if (handler != null)
			{
				handler(this, eventArgs);
			}
		}
    }

	public class QualityCalculatingEventArgs : EventArgs
	{
        public IDictionary<Dimension, double> CoordinateValues { get; private set; }

        public QualityCalculatingEventArgs(IDictionary<Dimension, double> coordinateValues)
		{
			CoordinateValues = coordinateValues;
		}
	}

	public class QualityCalculatedEventArgs : QualityCalculatingEventArgs
	{
        public double Quality { get; private set; }

        public QualityCalculatedEventArgs(IDictionary<Dimension, double> coordinateValues, double quality)
			: base(coordinateValues)
		{
			Quality = quality;
		}
	}

	public class BestFireworkFindingEventArgs : EventArgs
	{
		public IEnumerable<Firework> FireworksToCheck { get; private set; }

		public BestFireworkFindingEventArgs(IEnumerable<Firework> fireworksToCheck)
		{
			FireworksToCheck = fireworksToCheck;
		}
	}

	public class BestFireworkFoundEventArgs : EventArgs
	{
		public IEnumerable<Firework> FireworksToCheck { get; private set; }
		public Firework BestFirework { get; private set; }

		public BestFireworkFoundEventArgs(IEnumerable<Firework> fireworksToCheck, Firework bestFirework)
		{
			FireworksToCheck = fireworksToCheck;
			BestFirework = bestFirework;
		}
	}
}