using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FireworksNet.Problems;
using FireworksNet.Model;
using FireworksNet.Problems.Benchmark;

namespace FireworksNet.Tests.Explode
{
    class ExploderTestHelper
    {
        public static Problem ProblemToSolve { set; get; }

        public static IEnumerable<Firework> Fireworks { get; set; }
       
        public static IEnumerable<double> Qualities
        {
            get
            {
                return Fireworks.Select(fw => fw.Quality);
            }
        }
               
        /// <summary>
        /// Return firework for explosion. By default this first firework.
        /// </summary>
        public static Firework Epicenter
        {
            get { return Fireworks.First(); }
        }

        public static Firework BestFirework
        {
            get { return Fireworks.OrderBy(fr => fr.Quality).First<Firework>(); }
        }

        public static int QuantityFireworks { set; get; }

        static ExploderTestHelper()
        {
            ProblemToSolve = Sphere.Create();
            QuantityFireworks = 10;
            CreateFireworks();
        }       
         
        /// <summary>
        /// Create sample fireworks for test AttractRepulseGenerator
        /// </summary>
        private static void CreateFireworks()
        {
            List<Firework> fireworks = new List<Firework>();
            IList<Dimension> dimensions = ProblemToSolve.Dimensions;         
            
            for (int i = 1; i < QuantityFireworks + 1; i++)
            {
                IDictionary<Dimension, double> coordinates = new Dictionary<Dimension, double>();        
                foreach (Dimension dimension in dimensions)
                {                     
                    Range range = dimension.VariationRange;
                    coordinates.Add(dimension, range.Minimum + (range.Maximum - range.Minimum) / (i + 1));                          
                }
                Firework firework = new Firework(FireworkType.SpecificSpark, 0, coordinates);    
                firework.Quality = ProblemToSolve.CalculateQuality(firework.Coordinates);
                fireworks.Add(firework);
            }

            Fireworks = fireworks;
        }
    }
}
