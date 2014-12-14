using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    if (IsOutOfBounds(i, sparkCoords[i]))
                    {
                        sparkCoords[i] = dimensionsMin[i] + Math.Abs(sparkCoords[i]) % (dimensionsMax[i] - dimensionsMin[i]);
                    }
                }
            }
        }

        public static bool IsOutOfBounds(int dimension, double coordValue)
        {
            return false;
        }
    }
}