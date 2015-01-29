using System;
using System.Collections.Generic;

namespace FireworksNet.Model
{
	public class Firework : Solution
	{
		public String Id { get; private set; } // TODO: Hide Id type behind some TId, like in Opt?..

		public FireworkType Type { get; private set; }

        public int BirthStepNumber { get; private set; }

        public Firework(FireworkType type, int birthStepNumber, IDictionary<Dimension, double> coordinates)
            : base(coordinates, double.NaN)
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

        public Firework(FireworkType type, int birthStepNumber)
            : this(type, birthStepNumber, new Dictionary<Dimension, double>())
		{
		}
	}
}