using System;
using System.Collections.Generic;
using System.Linq;
using Fireworks.Extensions;
using Fireworks.Model;

namespace Fireworks
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

	/// <summary>
	/// Temp holder for algorithm pieces
	/// </summary>
    public class Fireworks
    {
        public static double CalcAmplitude(int fireworkNumber, double maximumExplosionAmplitude, IList<double> fireworkQualities)
        {
			double minFireworkQuality = fireworkQualities.Aggregate((agg, next) => next.IsLess(agg) ? next : agg); // Min() won't use my Double extensions
            return maximumExplosionAmplitude * (fireworkQualities[fireworkNumber] - minFireworkQuality + double.Epsilon) / (fireworkQualities.Sum(fq => fq - minFireworkQuality) + double.Epsilon);
        }

		public static double CalcExplosionSparksNumberExact(int fireworkNumber, double explosionSparksNumberModifier, IList<double> fireworkQualities)
		{
			double maxFireworkQuality = fireworkQualities.Aggregate((agg, next) => next.IsGreater(agg) ? next : agg); // Max() won't use my Double extensions
			return explosionSparksNumberModifier * (maxFireworkQuality - fireworkQualities[fireworkNumber] + double.Epsilon) / (fireworkQualities.Sum(fq => maxFireworkQuality - fq) + double.Epsilon);
		}

		public static int CalcExplosionSparksNumber(int fireworkNumber, double explosionSparksNumberModifier, IList<double> fireworkQualities, double explosionSparksConstA, double explosionSparksConstB)
		{
			double explosionSparksNumberExact = CalcExplosionSparksNumberExact(fireworkNumber, explosionSparksNumberModifier, fireworkQualities);
			
			// TODO: per 2010 paper: A < B < 1, A and B are user-defined constants
			if (explosionSparksNumberExact.IsLess(explosionSparksConstA * explosionSparksNumberModifier))
			{
				return (int)RoundAwayFromZero(explosionSparksConstA * explosionSparksNumberModifier);
			}
			else if (explosionSparksNumberExact.IsGreater(explosionSparksConstB * explosionSparksNumberModifier))
			{
				return (int)RoundAwayFromZero(explosionSparksConstB * explosionSparksNumberModifier);
			}
			
			// else:
			return (int)RoundAwayFromZero(explosionSparksNumberExact);
		}

		// Helper
		public static double RoundAwayFromZero(double value)
		{
			return Math.Round(value, MidpointRounding.AwayFromZero);
		}

        // That's a quality (fitness) function. TODO: delegate?
        private static double CalcQuality(double[] firework)
        {
            return 0.0;
        }

		// allCurrentFireworks include:
		// - fireworks existed in the beginning of the current step;
		// - explosion sparks generated on this step;
		// - specific sparks generated on this step.
		public static IEnumerable<Firework> SelectLocations(IEnumerable<Firework> allCurrentFireworks, int desiredLocationsNumber)
		{
			List<Firework> selectedLocations = new List<Firework>(desiredLocationsNumber);

			// 1. Find a firework with best quality - it will be kept anyways
			Firework bestFirework = GetBestFirework(allCurrentFireworks);
			selectedLocations.Add(bestFirework);

			// 2. Calculate distances between all fireworks
			IDictionary<Firework, Double> distances = CalculateDistances(allCurrentFireworks);

			// 3. Calculate probabilities for each firework
			IDictionary<Firework, Double> probabilities = CalculateSortedProbabilities(distances);

			// 4. Select desiredLocationsNumber - 1 of fireworks based on the probabilities
			IEnumerable<Firework> otherSelectedLocations = probabilities.Where(p => p.Key != bestFirework)
																		.Take(desiredLocationsNumber - 1)
																		.Select(p => p.Key);
			selectedLocations.AddRange(otherSelectedLocations);

			return selectedLocations;
		}

		private static IDictionary<Firework, double> CalculateSortedProbabilities(IDictionary<Firework, double> distances)
		{
			// TODO: Probably I should separate calculation from sorting...
			// TODO: Use SortedDictionary<Firework, double> here to get it sorted
			throw new NotImplementedException();
		}

		private static IDictionary<Firework, double> CalculateDistances(IEnumerable<Firework> allCurrentFireworks)
		{
			throw new NotImplementedException();
		}

		private static Firework GetBestFirework(IEnumerable<Firework> fireworks)
		{
			// TODO: Looking for MAX here (could be MIN)
			return fireworks.Aggregate((agg, next) => next.Quality.IsGreater(agg.Quality) ? next : agg);
		}
    }
}