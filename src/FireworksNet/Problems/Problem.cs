using System;
using System.Collections.Generic;
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

        public IEnumerable<Dimension> Dimensions { get; private set; }

		public IDictionary<Dimension, Range> InitialDimensionRanges { get; private set; }

		public IStopCondition StopCondition { get; private set; }

		public ProblemTarget Target { get; private set; }

        public Problem(IEnumerable<Dimension> dimensions, IDictionary<Dimension, Range> initialDimensionRanges, Func<IDictionary<Dimension, double>, double> targetFunction, IStopCondition stopCondition, ProblemTarget target)
        {
            if (dimensions == null)
            {
                throw new ArgumentNullException("dimensions");
            }

            if (dimensions.Count() == 0)
            {
                throw new ArgumentException(string.Empty, "dimensions");
            }

            if (targetFunction == null)
            {
                throw new ArgumentNullException("targetFunction");
            }

            if (stopCondition == null)
            {
                throw new ArgumentNullException("stopCondition");
            }

            this.Dimensions = dimensions;
            // TODO: Need validation to make sure dimensions and initialDimensionRanges contain the same Dimension instances
            this.InitialDimensionRanges = initialDimensionRanges;
            this.targetFunction = targetFunction;
            this.StopCondition = stopCondition;
            this.Target = target;
        }

        public Problem(IEnumerable<Dimension> dimensions, Func<IDictionary<Dimension, double>, double> targetFunction, IStopCondition stopCondition, ProblemTarget target)
            : this(dimensions, null, targetFunction, stopCondition, target)
        {
        }

        public Problem(IEnumerable<Dimension> dimensions, Func<IDictionary<Dimension, double>, double> targetFunction, IStopCondition stopCondition)
            : this(dimensions, null, targetFunction, stopCondition, ProblemTarget.Minimum)
        {
        }

        public virtual double CalculateQuality(IDictionary<Dimension, double> coordinateValues)
        {
            OnQualityCalculating(new QualityCalculatingEventArgs(coordinateValues));
            double result = targetFunction(coordinateValues);
            OnQualityCalculated(new QualityCalculatedEventArgs(coordinateValues, result));
            return result;
        }

        public virtual Firework GetBest(IEnumerable<Firework> fireworks)
        {
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

		protected virtual Firework GetLessQualityFirework(Firework currentMin, Firework candidate)
		{
			return candidate.Quality.IsLess(currentMin.Quality) ? candidate : currentMin;
		}

		protected virtual Firework GetGreaterQualityFirework(Firework currentMax, Firework candidate)
		{
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