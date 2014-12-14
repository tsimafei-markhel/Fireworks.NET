using System;
using System.Collections.Generic;
using System.Linq;

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
        public static void GenerateExplosionSparks(double[] fireworkCoords, double amplitude, IRandomizer randomizer, int dimensionsCount, double[] dimensionsMin, double[] dimensionsMax)
        {
            double[] sparkCoords = fireworkCoords;
            double offsetDisplacement = amplitude * randomizer.GetNext(-1.0, 1.0);
            int[] shiftCoord = new int[dimensionsCount];
            for (int i = 0; i < dimensionsCount; i++)
            {
                shiftCoord[i] = (int)Math.Round(randomizer.GetNext(0.0, 1.0));
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

        // TODO:
        //public static double CalcExplosionSparksNumberExact(int fireworkNumber, double explosionSparksNumberModifier, IList<double> fireworkQualities)
        //{
        //    double maxFireworkQuality = fireworkQualities.Max();
        //    return explosionSparksNumberModifier * (fireworkQualities[fireworkNumber] - maxFireworkQuality + double.Epsilon) / (fireworkQualities.Sum(fq => fq - maxFireworkQuality) + double.Epsilon);
        //}

        // That's a quality (fitness) function. TODO: delegate?
        private static double CalcQuality(double[] firework)
        {
            return 0.0;
        }
    }
}