using FireworksNet.Algorithm.Implementation;
using FireworksNet.Model;
using FireworksNet.Problems.Benchmark;
using FireworksNet.StopConditions;

namespace FireworksNet.Examples
{
    public class Program
    {
        private static void Main(string[] args)
        {
            // 1. Define a problem to solve
            Sphere2010 problem = Sphere2010.Create();

            // 2. Setup algorithm stop condition
            CounterStopCondition stopCondition = new CounterStopCondition(10000);
            problem.QualityCalculated += stopCondition.IncrementCounter;

            // 3. Initialize algorithm run settings
            FireworksAlgorithmSettings settings = new FireworksAlgorithmSettings()
            {
                LocationsNumber = 5,
                ExplosionSparksNumberModifier = 50.0,
                ExplosionSparksNumberLowerBound = 0.04,
                ExplosionSparksNumberUpperBound = 0.8,
                ExplosionSparksMaximumAmplitude = 40.0,
                SpecificSparksNumber = 5,
                SpecificSparksPerExplosionNumber = 1
            };

            // 4. Instantiate desired implementation of the algorithm (per 2010 paper in this case)
            FireworksAlgorithm fwa2010 = new FireworksAlgorithm(problem, stopCondition, settings);

            // 5. Finally, find a solution
            Solution solution = fwa2010.Solve();

            // TODO: Ideas for 'usage examples':
            //       1. Simple run: alg settings, one of the benchmark problems, get the solution
            //       2. Define user problem
            //       3. Composite stop condition
            //       4. Capture states after each step
        }
    }
}