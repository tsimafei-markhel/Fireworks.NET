using System;
using System.Collections.Generic;

namespace FireworksNet.Model
{
	public class Solution
	{
		public IDictionary<Dimension, Double> Coordinates { get; private set; }

		public Double Quality { get; set; }

		public Solution(IDictionary<Dimension, Double> coordinates, Double quality)
		{
			if (coordinates == null)
			{
				throw new ArgumentNullException("coordinates");
			}

			if (double.IsInfinity(quality))
			{
				throw new ArgumentOutOfRangeException("quality");
			}

			Coordinates = new Dictionary<Dimension, Double>(coordinates);
			Quality = quality;
		}

		public Solution(IDictionary<Dimension, Double> coordinates)
			: this(coordinates, Double.NaN)
		{
		}
	}
}