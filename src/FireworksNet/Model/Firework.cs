using System;
using System.Collections.Generic;

namespace FireworksNet.Model
{
	public class Firework : Solution
	{
		public String Id { get; private set; }

		public FireworkType Type { get; private set; }

		public Int32 BirthStepNumber { get; private set; }

		public Firework(FireworkType type, Int32 birthStepNumber, IDictionary<Dimension, Double> coordinates)
			: base(coordinates, Double.NaN)
		{
			if (birthStepNumber < 0)
			{
				throw new ArgumentOutOfRangeException("birthStepNumber");
			}

			if (coordinates == null)
			{
				throw new ArgumentNullException("coordinates");
			}

			Id = Guid.NewGuid().ToString();
			Type = type;
			BirthStepNumber = birthStepNumber;
		}

		public Firework(FireworkType type, Int32 birthStepNumber)
			: this(type, birthStepNumber, new Dictionary<Dimension, Double>())
		{
		}
	}
}