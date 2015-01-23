using System;
using System.Collections.Generic;

namespace FireworksNet.Model
{
    /// <summary>
    /// Represents a solution - a point in problem space defined by coordinates
    /// and corresponding quality (value of target function).
    /// </summary>
	public class Solution // TODO : IEquatable<Solution>
	{
        /// <summary>
        /// Gets solution coordinates in problem space.
        /// </summary>
        public IDictionary<Dimension, double> Coordinates { get; private set; }

        /// <summary>
        /// Gets or sets solution quality (value of target function).
        /// </summary>
        public double Quality { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="Solution"/> with
        /// defined coordinates and quality.
        /// </summary>
        /// <param name="coordinates">Solution coordinates in problem space.</param>
        /// <param name="quality">Solution quality (value of target function).</param>
        public Solution(IDictionary<Dimension, double> coordinates, double quality)
		{
            Coordinates = coordinates == null ? null : new Dictionary<Dimension, double>(coordinates);
			Quality = quality;
		}

        /// <summary>
        /// Initializes a new instance of <see cref="Solution"/> with
        /// defined coordinates.
        /// </summary>
        /// <param name="coordinates">Solution coordinates in problem space.</param>
        /// <remarks><see cref="Quality"/> is set to <see cref="double.NaN"/>.</remarks>
        public Solution(IDictionary<Dimension, double> coordinates)
            : this(coordinates, double.NaN)
		{
		}

        /// <summary>
        /// Initializes a new instance of <see cref="Solution"/> with
        /// defined quality.
        /// </summary>
        /// <param name="quality">Solution quality (value of target function).</param>
        /// <remarks><see cref="Coordinates"/> is set to <c>null</c>.</remarks>
        public Solution(double quality)
			: this(null, quality)
		{
		}
	}
}