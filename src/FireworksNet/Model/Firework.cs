using System;
using System.Collections.Generic;

namespace FireworksNet.Model
{
	public class Firework : Solution
	{
		public TId Id { get; private set; }

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

            this.Id = new TId();
			this.Type = type;
			this.BirthStepNumber = birthStepNumber;
		}

        public Firework(FireworkType type, int birthStepNumber)
            : this(type, birthStepNumber, new Dictionary<Dimension, double>())
		{
		}
	}
}