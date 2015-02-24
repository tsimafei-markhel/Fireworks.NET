using System.Collections.Generic;
using System.Linq;
using FireworksNet.Model;

namespace FireworksNet.Tests.Selection
{
    public static class DataTestSelector
    {
        private static IEnumerable<Firework> allFireworks;
        private static int minimumRange = 0;
        private static int maximumRange = 10;
        private static int countFireworks = 10;
        private static int samplingNumber = 3;

        static DataTestSelector()
        {
            FormationFireworks();
        }

        public static IEnumerable<Firework> Fireworks
        {
            get
            {
                return allFireworks;
            }
        }

        public static IEnumerable<Firework> NearBestFireworks
        {
            get
            {
                return allFireworks.Skip<Firework>(1).Take<Firework>(samplingNumber);
            }
        }

        public static IEnumerable<Firework> NonNearBestFirework
        {
            get
            {
                return allFireworks.Reverse<Firework>().Take<Firework>(samplingNumber);
            }
        }

        public static IEnumerable<Firework> BestFireworks
        {
            get
            {
                return allFireworks.Take(samplingNumber);
            }
        }

        public static IEnumerable<Firework> NonBestFireworks
        {
            get
            {
                return allFireworks.Reverse<Firework>().Take<Firework>(samplingNumber);
            }
        }

        public static Firework OneBestFirework
        {
            get
            {
                return allFireworks.First<Firework>();
            }
        }

        public static int SamplingNumber
        {
            get
            {
                return samplingNumber;
            }

            set
            {
                samplingNumber = value;
            }
        }

        private static void FormationFireworks()
        {
            Range range = new Range(minimumRange, maximumRange);
            List<Firework> fireworks = new List<Firework>();
            IDictionary<Dimension, double> coordinates = new Dictionary<Dimension, double>();

            for (int i = 1; i < countFireworks + 1; i++)
            {
                coordinates = new Dictionary<Dimension, double>();
                coordinates.Add(new Dimension(range), i);
                coordinates.Add(new Dimension(range), i);
                Firework firework = new Firework(FireworkType.Initial, 0, coordinates);
                fireworks.Add(firework);
            }
            allFireworks = fireworks;
        }
    }
}