using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.Distributions;

namespace Fireworks
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

    public class Fireworks
    {
		// TODO: Add ISparkGenerator
		// TODO: Add ExplosionSparkGenerator - as conventional explosion spark generator (impl. ISparkGenerator)
		// TODO: ExplosionSparkGenerator: IRandomizer and collection of Parameters - thru ctor
		// TODO: ExplosionSparkGenerator: fireworkCoords (coords of the firework that produces the spark being generated) - pass to GenerateSpark method
		// TODO: ExplosionSparkGenerator: pass amplitude as member of Explosion arg?
		// Firework newSpark = CreateSpark(Explosion explosion, Firework parentFirework);
		public static double[] GenerateExplosionSpark(double[] fireworkCoords, double amplitude, IRandomizer randomizer, int dimensionsCount, double[] dimensionsMin, double[] dimensionsMax)
        {
            double[] sparkCoords = fireworkCoords;
            double offsetDisplacement = amplitude * randomizer.GetNext(-1.0, 1.0);
            int[] shiftCoord = new int[dimensionsCount];
            for (int i = 0; i < dimensionsCount; i++)
            {
				shiftCoord[i] = (int)RoundAwayFromZero(randomizer.GetNext(0.0, 1.0));
            }

            for (int i = 0; i < dimensionsCount; i++)
            {
                if (shiftCoord[i] == 1)
                {
                    sparkCoords[i] += offsetDisplacement;
                    if (IsOutOfBounds(i, sparkCoords[i], dimensionsMin, dimensionsMax))
                    {
                        sparkCoords[i] = dimensionsMin[i] + Math.Abs(sparkCoords[i]) % (dimensionsMax[i] - dimensionsMin[i]);
                    }
                }
            }

			return sparkCoords;
        }

        public static bool IsOutOfBounds(int dimension, double coordValue, double[] dimensionsMin, double[] dimensionsMax)
        {
            // TODO: Compare doubles properly
            // TODO: Consider reverting the method (i.e. IsInBounds)
            return (coordValue > dimensionsMax[dimension]) || (coordValue < dimensionsMin[dimension]);
        }

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
			
			// TODO: Compare doubles properly
			// TODO: 2010 paper states "A < B < 1", 2013 paper does not
			if (explosionSparksNumberExact < explosionSparksConstA * explosionSparksNumberModifier)
			{
				return (int)RoundAwayFromZero(explosionSparksConstA * explosionSparksNumberModifier);
			}
			else if (explosionSparksNumberExact > explosionSparksConstB * explosionSparksNumberModifier)
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
		// TODO: ExplosionSparkGenerator: fireworkCoords (coords of the firework that produces the spark being generated) - pass to GenerateSpark method
		// Firework newSpark = CreateSpark(Explosion explosion, Firework parentFirework);
		public static double[] GenerateGaussianSpark(double[] fireworkCoords, IRandomizer randomizer, int dimensionsCount, double[] dimensionsMin, double[] dimensionsMax)
		{
			double[] sparkCoords = fireworkCoords;
			double offsetDisplacement = Normal.Sample(1.0, 1.0); // Coefficient of Gaussian explosion
			int[] shiftCoord = new int[dimensionsCount];
			for (int i = 0; i < dimensionsCount; i++)
			{
				shiftCoord[i] = (int)RoundAwayFromZero(randomizer.GetNext(0.0, 1.0));
			}

			for (int i = 0; i < dimensionsCount; i++)
			{
				if (shiftCoord[i] == 1)
				{
					sparkCoords[i] *= offsetDisplacement;
					if (IsOutOfBounds(i, sparkCoords[i], dimensionsMin, dimensionsMax))
					{
						sparkCoords[i] = dimensionsMin[i] + Math.Abs(sparkCoords[i]) % (dimensionsMax[i] - dimensionsMin[i]);
					}
				}
			}

			return sparkCoords;
		}
    }
}