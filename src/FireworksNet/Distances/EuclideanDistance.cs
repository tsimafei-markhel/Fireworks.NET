using System;
using System.Collections.Generic;
using System.Linq;
using FireworksNet.Model;
using MathNet.Numerics;

namespace FireworksNet.Distances
{
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
			return Distance.Euclidean(first, second);
		}

		public Double Calculate(Firework first, Firework second)
		{
			if (first == second)
			{
				return 0.0;
			}

			// TODO: Move this method to base abstract class
			double[] firstCoords = new double[dimensions.Count()];
			double[] secondCoords = new double[dimensions.Count()];

			int dimensionCounter = 0;
			foreach (Dimension dimension in dimensions)
			{
				firstCoords[dimensionCounter] = first.Coordinates[dimension];
				secondCoords[dimensionCounter] = second.Coordinates[dimension];
				dimensionCounter++;
			}

			return Calculate(firstCoords, secondCoords);
		}
	}
}