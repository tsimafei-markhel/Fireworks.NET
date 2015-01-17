using System;
using System.Collections.Generic;
using System.Linq;
using FireworksNet.Distances;
using FireworksNet.Model;

namespace FireworksNet.Selection
{
	public class LocationSelector : ISelector
	{
		private readonly IDistance distanceCalculator;
		private readonly Func<IEnumerable<Firework>, Firework> bestFireworkSelector;
		private readonly Int32 locationsNumber;

		public LocationSelector(IDistance distanceCalculator, Func<IEnumerable<Firework>, Firework> bestFireworkSelector, Int32 locationsNumber)
		{
			if (distanceCalculator == null)
			{
				throw new ArgumentNullException("distanceCalculator");
			}

			if (bestFireworkSelector == null)
			{
				throw new ArgumentNullException("bestFireworkSelector");
			}

			if (locationsNumber < 0)
			{
				throw new ArgumentOutOfRangeException("locationsNumber");
			}

			this.locationsNumber = locationsNumber;
			this.distanceCalculator = distanceCalculator;
			this.bestFireworkSelector = bestFireworkSelector;
		}

		public LocationSelector(IDistance distanceCalculator, Func<IEnumerable<Firework>, Firework> bestFireworkSelector)
			: this(distanceCalculator, bestFireworkSelector, 0)
		{
		}

		public virtual IEnumerable<Firework> Select(IEnumerable<Firework> from)
		{
			return Select(from, locationsNumber);
		}

		public virtual IEnumerable<Firework> Select(IEnumerable<Firework> from, Int32 numberToSelect)
		{
			if (from == null)
			{
				throw new ArgumentNullException("from");
			}

			if (numberToSelect < 0)
			{
				throw new ArgumentOutOfRangeException("numberToSelect");
			}

			if (numberToSelect > from.Count())
			{
				throw new ArgumentOutOfRangeException("numberToSelect"); // TODO: Or just return as much as we have?..
			}

			if (numberToSelect == from.Count())
			{
				return from; // TODO: Or make a copy?..
			}

			List<Firework> selectedLocations = new List<Firework>(numberToSelect);
			if (numberToSelect == 0)
			{
				return selectedLocations;
			}

			// 1. Find a firework with best quality - it will be kept anyways
			Firework bestFirework = bestFireworkSelector(from);
			selectedLocations.Add(bestFirework);

            if (numberToSelect > 1)
            {
                // 2. Calculate distances between all fireworks
                IDictionary<Firework, Double> distances = CalculateDistances(from);

                // 3. Calculate probabilities for each firework
                IDictionary<Firework, Double> probabilities = CalculateProbabilities(distances);

                // 4. Select desiredLocationsNumber - 1 of fireworks based on the probabilities
                IOrderedEnumerable<KeyValuePair<Firework, Double>> sortedProbabilities = probabilities.OrderByDescending(p => p.Value, new DoubleExtensionComparer());
                IEnumerable<Firework> otherSelectedLocations = sortedProbabilities.Where(sp => sp.Key != bestFirework).Take(numberToSelect - 1).Select(sp => sp.Key);
                selectedLocations.AddRange(otherSelectedLocations);
            }

			return selectedLocations;
		}

		protected virtual IDictionary<Firework, Double> CalculateDistances(IEnumerable<Firework> allCurrentFireworks)
		{
			Dictionary<Firework, double> distances = new Dictionary<Firework, double>(allCurrentFireworks.Count());
			foreach (Firework firework in allCurrentFireworks)
			{
				distances.Add(firework, 0.0);
				foreach (Firework otherFirework in allCurrentFireworks)
				{
					distances[firework] += distanceCalculator.Calculate(firework, otherFirework);
				}
			}

			return distances;
		}

		protected virtual IDictionary<Firework, Double> CalculateProbabilities(IDictionary<Firework, Double> distances)
		{
			Dictionary<Firework, double> probabilities = new Dictionary<Firework, double>(distances.Count());
			double distancesSum = distances.Values.Sum();
			foreach (KeyValuePair<Firework, double> distance in distances)
			{
				double probability = distance.Value / distancesSum;
				probabilities.Add(distance.Key, probability);
			}

			return probabilities;
		}
	}
}