using System;
using System.Collections.Generic;
using System.Linq;
using Fireworks.Model;

namespace Fireworks.Problems
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

        public IEnumerable<Dimension> Dimensions { get; protected set; }

        public Problem(IEnumerable<Dimension> dimensions, Func<IDictionary<Dimension, Double>, Double> targetFunction)
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

            this.targetFunction = targetFunction;
            this.Dimensions = dimensions;
        }

        public virtual Double CalculateQuality(IDictionary<Dimension, Double> coordinateValues)
        {
            // TODO: Async?
            OnQualityCalculating(new QualityCalculatingEventArgs(coordinateValues));
            double result = targetFunction(coordinateValues);
            OnQualityCalculated(new QualityCalculatedEventArgs(coordinateValues, result));
            return result;
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