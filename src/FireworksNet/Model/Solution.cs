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
		public IDictionary<Dimension, Double> Coordinates { get; private set; }

        /// <summary>
        /// Gets or sets solution quality (value of target function).
        /// </summary>
		public Double Quality { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="Solution"/> with
        /// defined coordinates and quality.
        /// </summary>
        /// <param name="coordinates">Solution coordinates in problem space.</param>
        /// <param name="quality">Solution quality (value of target function).</param>
		public Solution(IDictionary<Dimension, Double> coordinates, Double quality)
		{
			Coordinates = coordinates == null ? null : new Dictionary<Dimension, Double>(coordinates);
			Quality = quality;
		}

        /// <summary>
        /// Initializes a new instance of <see cref="Solution"/> with
        /// defined coordinates.
        /// </summary>
        /// <param name="coordinates">Solution coordinates in problem space.</param>
        /// <remarks><see cref="Quality"/> is set to <see cref="Double.NaN"/>.</remarks>
		public Solution(IDictionary<Dimension, Double> coordinates)
			: this(coordinates, Double.NaN)
		{
		}

        /// <summary>
        /// Initializes a new instance of <see cref="Solution"/> with
        /// defined quality.
        /// </summary>
        /// <param name="quality">Solution quality (value of target function).</param>
        /// <remarks><see cref="Coordinates"/> is set to <c>null</c>.</remarks>
		public Solution(Double quality)
			: this(null, quality)
		{
		}
	}
}