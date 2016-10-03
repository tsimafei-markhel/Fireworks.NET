using System;
using FireworksNet.Model;

namespace FireworksNet.Examples
{
    public class Program
    {
        private static void Main()
        {
            WriteSolution("SimpleRun.RunFireworks2010", SimpleRun.RunFireworks2010());
            WriteSolution("SimpleRun.RunFireworks2012", SimpleRun.RunFireworks2012());

            Console.ReadKey();

            // TODO: Ideas for 'usage examples':
            //       1. Simple run: alg settings, one of the benchmark problems, get the solution
            //       2. Define user problem
            //       3. Composite stop condition
            //       4. Capture states after each step
        }

        private static void WriteSolution(string source, Solution solution)
        {
            ConsoleColor currentColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("=== " + source + " ===");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Quality: " + solution.Quality.ToString("F3"));
            Console.WriteLine();

            Console.ForegroundColor = currentColor;
        }
    }
}