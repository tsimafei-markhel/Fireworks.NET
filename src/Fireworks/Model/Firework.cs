using System;
using System.Collections.Generic;

namespace Fireworks.Model
{
	public class Firework
	{
		public String Id { get; private set; }

		public FireworkType Type { get; private set; }

		public Int32 BirthStepNumber { get; private set; }

		public Double Quality { get; set; }

		public IDictionary<Dimension, Double> Coordinates { get; private set; }

		public Firework(FireworkType type, Int32 birthStepNumber)
			: this(type, birthStepNumber, new Dictionary<Dimension, Double>())
		{
		}

		public Firework(FireworkType type, Int32 birthStepNumber, IDictionary<Dimension, Double> coordinates)
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
			Quality = Double.NaN;
			Coordinates = new Dictionary<Dimension, Double>(coordinates);
		}
	}
}