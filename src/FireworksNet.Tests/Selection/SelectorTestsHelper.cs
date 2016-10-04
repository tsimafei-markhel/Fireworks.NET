using System.Collections.Generic;
using System.Linq;
using FireworksNet.Model;

namespace FireworksNet.Tests.Selection
{
    public static class SelectorTestsHelper
    {
        public static IEnumerable<Firework> Fireworks { get; set; }

        public static IEnumerable<Firework> NearBestFireworks
        {
            get
            {
                return Fireworks.Skip<Firework>(1).Take<Firework>(SamplingNumber);
            }
        }

        public static IEnumerable<Firework> NonNearBestFirework
        {
            get
            {
                return Fireworks.Reverse<Firework>().Take<Firework>(SamplingNumber);
            }
        }

        public static IEnumerable<Firework> BestFireworks
        {
            get
            {
                return Fireworks.OrderBy(fr => fr.Quality).Take<Firework>(SamplingNumber);
            }
        }

        public static IEnumerable<Firework> NonBestFireworks
        {
            get
            {
                return Fireworks.OrderByDescending(fr => fr.Quality).Take<Firework>(SamplingNumber);
            }
        }

        public static IEnumerable<Firework> RandomFireworks
        {
            get
            {
                return Fireworks.Take<Firework>(SamplingNumber);
            }
        }

        public static Firework FirstBestFirework
        {
            get
            {
                return Fireworks.OrderBy(fr => fr.Quality).First<Firework>();
            }
        }

        public static int SamplingNumber { get; set; }

        public static int CountFireworks { get; set; }

        static SelectorTestsHelper()
        {
            SamplingNumber = 3;
            CountFireworks = 10;
            FormFireworks();
        }

        public static Firework GetBest(IEnumerable<Firework> fireworks)
        {
            return fireworks.OrderBy(fr => fr.Quality).First<Firework>();
        }

        //TODO: lazy initialization collection of fireworks
        private static void FormFireworks()
        {
            Range range = new Range(0, 10.0);
            List<Firework> fireworks = new List<Firework>();
            IDictionary<Dimension, double> coordinates;

            for (int i = 1; i < CountFireworks + 1; i++)
            {
                coordinates = new Dictionary<Dimension, double>();
                coordinates.Add(new Dimension(range), i);
                coordinates.Add(new Dimension(range), i);
                Firework firework = new Firework(FireworkType.Initial, 0, 0, coordinates);
                firework.Quality = i;
                fireworks.Add(firework);
            }

            Fireworks = fireworks;
        }
    }
}