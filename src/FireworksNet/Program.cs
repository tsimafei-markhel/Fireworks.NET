using System;
using System.Collections.Generic;
using System.Linq;
using FireworksNet.Distances;
using FireworksNet.Extensions;
using FireworksNet.Implementation;
using FireworksNet.Model;

namespace FireworksNet
{
    class Program
    {
        private readonly FireworksAlgorithmSettings Setup2010Paper = new FireworksAlgorithmSettings()
        {
            LocationsNumber = 5,
            ExplosionSparksNumberModifier = 50,
            ExplosionSparksNumberLowerBound = 0.04,
            ExplosionSparksNumberUpperBound = 0.8,
            ExplosionSparksMaximumAmplitude = 40,
            SpecificSparksNumber = 5,
            SpecificSparksPerExplosionNumber = 1
        };

        static void Main(string[] args)
        {
        }
    }

	/// <summary>
	/// Temp holder for algorithm pieces
	/// </summary>
    public class Fireworks
    {
		// allCurrentFireworks include:
		// - fireworks existed in the beginning of the current step;
		// - explosion sparks generated on this step;
		// - specific sparks generated on this step.
		public static IEnumerable<Firework> SelectLocations(IEnumerable<Firework> allCurrentFireworks, int desiredLocationsNumber, IEnumerable<Dimension> dimensions)
		{
			List<Firework> selectedLocations = new List<Firework>(desiredLocationsNumber);

			// 1. Find a firework with best quality - it will be kept anyways
			Firework bestFirework = GetBestFirework(allCurrentFireworks);
			selectedLocations.Add(bestFirework);

			// 2. Calculate distances between all fireworks
			IDictionary<Firework, Double> distances = CalculateDistances(allCurrentFireworks, dimensions);

			// 3. Calculate probabilities for each firework
			IDictionary<Firework, Double> probabilities = CalculateProbabilities(distances);

			// 4. Select desiredLocationsNumber - 1 of fireworks based on the probabilities
			IOrderedEnumerable<KeyValuePair<Firework, Double>> sortedProbabilities = probabilities.OrderByDescending(p => p.Value, new DoubleExtensionComparer());
			IEnumerable<Firework> otherSelectedLocations = sortedProbabilities.Where(sp => sp.Key != bestFirework).Take(desiredLocationsNumber - 1).Select(sp => sp.Key);
			selectedLocations.AddRange(otherSelectedLocations);

			return selectedLocations;
		}

		private static IDictionary<Firework, Double> CalculateProbabilities(IDictionary<Firework, Double> distances)
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

		private static IDictionary<Firework, Double> CalculateDistances(IEnumerable<Firework> allCurrentFireworks, IEnumerable<Dimension> dimensions)
		{
			IDistance distanceCalculator = new EuclideanDistance(dimensions);
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

        // TODO: Remove this once CalculateDistances is moved away from here
		private static Firework GetBestFirework(IEnumerable<Firework> fireworks)
		{
			return fireworks.Aggregate((agg, next) => next.Quality.IsGreater(agg.Quality) ? next : agg);
		}
    }
}