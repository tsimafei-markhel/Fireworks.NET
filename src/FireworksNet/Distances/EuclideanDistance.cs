using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public double Calculate(double[] first, double[] second)
		{
			if (first == null)
			{
				throw new ArgumentNullException("first");
			}

			if (second == null)
			{
				throw new ArgumentNullException("second");
			}

			if (first.Length != second.Length)
			{
				throw new ArgumentException(string.Empty, "second");
			}

			return Distance.Euclidean(first, second);
		}

        public double Calculate(Solution first, Solution second)
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

            Debug.Assert(firstCoordinates != null, "First coordinate collection is null");
            Debug.Assert(secondCoordinates != null, "Second coordinate collection is null");

			return Calculate(firstCoordinates, secondCoordinates);
		}

        public double Calculate(Solution first, double[] second)
        {
			if (first == null)
			{
				throw new ArgumentNullException("first");
			}

			if (second == null)
			{
				throw new ArgumentNullException("second");
			}

            double[] firstCoordinates = GetCoordinates(first);

            Debug.Assert(firstCoordinates != null, "First coordinate collection is null");
			Debug.Assert(firstCoordinates.Length == second.Length, "First and Second coordinate collections have different length");

            return Calculate(firstCoordinates, second);
        }

        private double[] GetCoordinates(Solution solution)
		{
			Debug.Assert(dimensions != null, "Dimension collection is null");
			Debug.Assert(solution != null, "Solution is null");
			Debug.Assert(solution.Coordinates != null, "Solution coordinate collection is null");

			double[] coordinates = new double[dimensions.Count()];

			int dimensionCounter = 0;
			foreach (Dimension dimension in dimensions)
			{
				Debug.Assert(dimension != null, "Dimension is null");

				coordinates[dimensionCounter] = solution.Coordinates[dimension];
				dimensionCounter++;
			}

			return coordinates;
		}

        private void GetCoordinates(Solution first, Solution second, out double[] firstCoordinates, out double[] secondCoordinates)
		{
			Debug.Assert(dimensions != null, "Dimension collection is null");
			Debug.Assert(first != null, "First solution is null");
			Debug.Assert(second != null, "Second solution is null");
			Debug.Assert(first.Coordinates != null, "First solution coordinate collection is null");
			Debug.Assert(second.Coordinates != null, "Second solution coordinate collection is null");

			firstCoordinates = new double[dimensions.Count()];
			secondCoordinates = new double[dimensions.Count()];

			int dimensionCounter = 0;
			foreach (Dimension dimension in dimensions)
			{
				Debug.Assert(dimension != null, "Dimension is null");

				firstCoordinates[dimensionCounter] = first.Coordinates[dimension];
				secondCoordinates[dimensionCounter] = second.Coordinates[dimension];

				dimensionCounter++;
			}
		}
    }
}