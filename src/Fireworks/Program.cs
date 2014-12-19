using System;
using System.Collections.Generic;
using System.Linq;
using Fireworks.Model;
using Fireworks.Randomization;
using MathNet.Numerics;
using MathNet.Numerics.Distributions;

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
        public static double CalcAmplitude(int fireworkNumber, double explosionAmplitudeModifier, IList<double> fireworkQualities)
        {
            double minFireworkQuality = fireworkQualities.Min();
            return explosionAmplitudeModifier * (fireworkQualities[fireworkNumber] - minFireworkQuality + double.Epsilon) / (fireworkQualities.Sum(fq => fq - minFireworkQuality) + double.Epsilon);
        }

		public static double CalcExplosionSparksNumberExact(int fireworkNumber, double explosionSparksNumberModifier, IList<double> fireworkQualities)
		{
			double maxFireworkQuality = fireworkQualities.Max();
			return explosionSparksNumberModifier * (maxFireworkQuality - fireworkQualities[fireworkNumber] + double.Epsilon) / (fireworkQualities.Sum(fq => maxFireworkQuality - fq) + double.Epsilon);
		}

		public static int CalcExplosionSparksNumber(int fireworkNumber, double explosionSparksNumberModifier, IList<double> fireworkQualities, double explosionSparksConstA, double explosionSparksConstB)
		{
			double explosionSparksNumberExact = CalcExplosionSparksNumberExact(fireworkNumber, explosionSparksNumberModifier, fireworkQualities);
			
			// TODO: per 2010 paper: A < B < 1, A and B are user-defined constants
			if (explosionSparksNumberExact.IsSmaller(explosionSparksConstA * explosionSparksNumberModifier, double.Epsilon))
			{
				return (int)RoundAwayFromZero(explosionSparksConstA * explosionSparksNumberModifier);
			}
			else if (explosionSparksNumberExact.IsLarger(explosionSparksConstB * explosionSparksNumberModifier, double.Epsilon))
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

		// TODO: Add GaussianSparkGenerator - as conventional Gaussian spark generator (impl. ISparkGenerator)
		// TODO: ExplosionSparkGenerator: IRandomizer and collection of Parameters - thru ctor
		// Firework newSpark = CreateSpark(Explosion explosion, Firework parentFirework);
		public static Firework GenerateGaussianSpark(Explosion explosion, IEnumerable<Dimension> dimensions, IRandom randomizer)
		{
			// TODO: Think over explosion.ParentFirework.BirthStepNumber + 1. Is that correct?
			Firework spark = new Firework(FireworkType.SpecificSpark, explosion.ParentFirework.BirthStepNumber + 1, explosion.ParentFirework.Coordinates);

			double offsetDisplacement = Normal.Sample(1.0, 1.0); // Coefficient of Gaussian explosion
			foreach (Dimension dimension in dimensions)
			{
				if ((int)RoundAwayFromZero(randomizer.GetNext(0.0, 1.0)) == 1)
				{
					spark.Coordinates[dimension] *= offsetDisplacement;
					if (!dimension.IsValueInBounds(spark.Coordinates[dimension]))
					{
						spark.Coordinates[dimension] = dimension.VariationRange.Minimum + Math.Abs(spark.Coordinates[dimension]) % dimension.VariationRange.Length;
					}
				}
			}

			return spark;
		}
    }
}