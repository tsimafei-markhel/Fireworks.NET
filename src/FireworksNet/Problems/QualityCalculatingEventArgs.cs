using System;
using System.Collections.Generic;
using FireworksNet.Model;

namespace FireworksNet.Problems
{
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
}