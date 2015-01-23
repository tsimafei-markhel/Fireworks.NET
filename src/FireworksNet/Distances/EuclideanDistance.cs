using System;
using System.Collections.Generic;
using System.Linq;
using FireworksNet.Model;
using MathNet.Numerics;

namespace FireworksNet.Distances
{
	// TODO: Once another distance calculator is required, come up with base class.
	public class EuclideanDistance : IDistance
	{
		private readonly IEnumerable<Dimension> dimensions;

		public EuclideanDistance(IEnumerable<Dimension> dimensions)
		{
			if (dimensions == null)
			{
				throw new ArgumentNullException("dimensions");
			}

			this.dimensions = dimensions;
		}

		public Double Calculate(Double[] first, Double[] second)
		{
			if (first == null)
			{
				throw new ArgumentNullException("first");
			}

			if (second == null)
			{
				throw new ArgumentNullException("second");
			}

			return Distance.Euclidean(first, second);
		}

		public Double Calculate(Solution first, Solution second)
		{
			if (first == null)
			{
				throw new ArgumentNullException("first");
			}

			if (second == null)
			{
				throw new ArgumentNullException("second");
			}

			if (first == second)
			{
				return 0.0;
			}

			double[] firstCoordinates;
			double[] secondCoordinates;
			GetCoordinates(first, second, out firstCoordinates, out secondCoordinates);

			return Calculate(firstCoordinates, secondCoordinates);
		}

		public Double Calculate(Solution first, Double[] second)
        {
			if (first == null)
			{
				throw new ArgumentNullException("first");
			}

			if (second == null)
			{
				throw new ArgumentNullException("second");
			}

			return Calculate(GetCoordinates(first), second);
        }

		private Double[] GetCoordinates(Solution firework)
		{
			System.Diagnostics.Debug.Assert(dimensions != null, "Dimension collection is null");
			System.Diagnostics.Debug.Assert(firework != null, "Solution is null");
			System.Diagnostics.Debug.Assert(firework.Coordinates != null, "Solution coordinate collection is null");

			double[] coordinates = new double[dimensions.Count()];

			int dimensionCounter = 0;
			foreach (Dimension dimension in dimensions)
			{
				System.Diagnostics.Debug.Assert(dimension != null, "Dimension is null");

				coordinates[dimensionCounter] = firework.Coordinates[dimension];

				dimensionCounter++;
			}

			return coordinates;
		}

		private void GetCoordinates(Solution first, Solution second, out Double[] firstCoordinates, out Double[] secondCoordinates)
		{
			System.Diagnostics.Debug.Assert(dimensions != null, "Dimension collection is null");
			System.Diagnostics.Debug.Assert(first != null, "First solution is null");
			System.Diagnostics.Debug.Assert(second != null, "Second solution is null");
			System.Diagnostics.Debug.Assert(first.Coordinates != null, "First solution coordinate collection is null");
			System.Diagnostics.Debug.Assert(second.Coordinates != null, "Second solution coordinate collection is null");

			firstCoordinates = new double[dimensions.Count()];
			secondCoordinates = new double[dimensions.Count()];

			int dimensionCounter = 0;
			foreach (Dimension dimension in dimensions)
			{
				System.Diagnostics.Debug.Assert(dimension != null, "Dimension is null");

				firstCoordinates[dimensionCounter] = first.Coordinates[dimension];
				secondCoordinates[dimensionCounter] = second.Coordinates[dimension];

				dimensionCounter++;
			}
		}
    }
}