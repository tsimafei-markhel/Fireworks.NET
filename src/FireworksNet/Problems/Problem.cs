using System;
using System.Collections.Generic;
using System.Linq;
using FireworksNet.Extensions;
using FireworksNet.Model;

namespace FireworksNet.Problems
{
    public class QualityCalculatingEventArgs : EventArgs
    {
        public IDictionary<Dimension, Double> CoordinateValues { get; private set; }

        public QualityCalculatingEventArgs(IDictionary<Dimension, Double> coordinateValues)
        {
            CoordinateValues = coordinateValues;
        }
    }

    public class QualityCalculatedEventArgs : QualityCalculatingEventArgs
    {
        public Double Quality { get; private set; }

        public QualityCalculatedEventArgs(IDictionary<Dimension, Double> coordinateValues, Double quality)
            : base(coordinateValues)
        {
            Quality = quality;
        }
    }

    public class Problem
    {
        private readonly Func<IDictionary<Dimension, Double>, Double> targetFunction;

        public event EventHandler<QualityCalculatingEventArgs> QualityCalculating;

        public event EventHandler<QualityCalculatedEventArgs> QualityCalculated;

        public IEnumerable<Dimension> Dimensions { get; protected set; } // TODO: Really need 'protected set' here?

        public IDictionary<Dimension, Range> InitialDimensionRanges { get; protected set; } // TODO: Really need 'protected set' here?

        public IStopCondition StopCondition { get; protected set; } // TODO: Really need 'protected set' here?

        public ProblemTarget Target { get; protected set; } // TODO: Really need 'protected set' here?

        public Problem(IEnumerable<Dimension> dimensions, IDictionary<Dimension, Range> initialDimensionRanges, Func<IDictionary<Dimension, Double>, Double> targetFunction, IStopCondition stopCondition, ProblemTarget target)
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

        public Problem(IEnumerable<Dimension> dimensions, Func<IDictionary<Dimension, Double>, Double> targetFunction, IStopCondition stopCondition, ProblemTarget target)
            : this(dimensions, null, targetFunction, stopCondition, target)
        {
        }

        public Problem(IEnumerable<Dimension> dimensions, Func<IDictionary<Dimension, Double>, Double> targetFunction, IStopCondition stopCondition)
            : this(dimensions, null, targetFunction, stopCondition, ProblemTarget.Minimum)
        {
        }

        public virtual Double CalculateQuality(IDictionary<Dimension, Double> coordinateValues)
        {
            // TODO: Async?
            OnQualityCalculating(new QualityCalculatingEventArgs(coordinateValues));
            double result = targetFunction(coordinateValues);
            OnQualityCalculated(new QualityCalculatedEventArgs(coordinateValues, result));
            return result;
        }

        public virtual Firework GetBest(IEnumerable<Firework> fireworks)
        {
            // TODO: Cache IsGreater/IsLess in the class
            if (Target == ProblemTarget.Minimum)
            {
                return fireworks.Aggregate((agg, next) => next.Quality.IsLess(agg.Quality) ? next : agg);
            }
            else
            {
                return fireworks.Aggregate((agg, next) => next.Quality.IsGreater(agg.Quality) ? next : agg);
            }
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
    }
}