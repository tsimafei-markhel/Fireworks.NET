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

		public Double Calculate(Firework first, Firework second)
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

        public Double Calculate(Firework first, Double[] second)
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

		private Double[] GetCoordinates(Firework firework)
		{
			System.Diagnostics.Debug.Assert(dimensions != null, "Dimension collection is null");
			System.Diagnostics.Debug.Assert(firework != null, "Firework is null");
			System.Diagnostics.Debug.Assert(firework.Coordinates != null, "Firework coordinate collection is null");

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

		private void GetCoordinates(Firework firstFirework, Firework secondFirework, out Double[] firstCoordinates, out Double[] secondCoordinates)
		{
			System.Diagnostics.Debug.Assert(dimensions != null, "Dimension collection is null");
			System.Diagnostics.Debug.Assert(firstFirework != null, "First firework is null");
			System.Diagnostics.Debug.Assert(secondFirework != null, "Second firework is null");
			System.Diagnostics.Debug.Assert(firstFirework.Coordinates != null, "First firework coordinate collection is null");
			System.Diagnostics.Debug.Assert(secondFirework.Coordinates != null, "Second firework coordinate collection is null");

			firstCoordinates = new double[dimensions.Count()];
			secondCoordinates = new double[dimensions.Count()];

			int dimensionCounter = 0;
			foreach (Dimension dimension in dimensions)
			{
				System.Diagnostics.Debug.Assert(dimension != null, "Dimension is null");

				firstCoordinates[dimensionCounter] = firstFirework.Coordinates[dimension];
				secondCoordinates[dimensionCounter] = secondFirework.Coordinates[dimension];

				dimensionCounter++;
			}
		}
    }
}