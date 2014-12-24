using System;
using System.Collections.Generic;
using System.Linq;
using Fireworks.Model;
using MathNet.Numerics;

namespace Fireworks.Distances
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

		public double Calculate(Double[] first, Double[] second)
		{
			return Distance.Euclidean(first, second);
		}

		public double Calculate(Firework first, Firework second)
		{
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