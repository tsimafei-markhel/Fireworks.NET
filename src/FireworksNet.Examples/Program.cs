using FireworksNet.Algorithm;
using FireworksNet.Algorithm.Implementation;
using FireworksNet.Model;
using FireworksNet.Problems;
using FireworksNet.Problems.Benchmark;
using FireworksNet.StopConditions;

namespace FireworksNet.Examples
{
    public class Program
    {
        private static void Main()
        {
            RunFireworks2010();
            RunFireworks2012();

            // TODO: Ideas for 'usage examples':
            //       1. Simple run: alg settings, one of the benchmark problems, get the solution
            //       2. Define user problem
            //       3. Composite stop condition
            //       4. Capture states after each step
        }

        private static void RunFireworks2010()
        {
            // 1. Define a problem to solve
            Problem problem = Sphere.Create();

            // 2. Setup algorithm stop condition
            CounterStopCondition stopCondition = new CounterStopCondition(10000);
            problem.QualityCalculated += stopCondition.IncrementCounter;

            // 3. Initialize algorithm run settings
            FireworksAlgorithmSettings settings = new FireworksAlgorithmSettings
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
            IFireworksAlgorithm fwa2010 = new FireworksAlgorithm(problem, stopCondition, settings);

            // 5. Finally, find a solution
            Solution solution = fwa2010.Solve();
        }

        private static void RunFireworks2012()
        {
            // 1. Define a problem to solve
            Problem problem = Sphere2012.Create();

            // 2. Setup algorithm stop condition
            CounterStopCondition stopCondition = new CounterStopCondition(10000);
            problem.QualityCalculated += stopCondition.IncrementCounter;

            // 3. Initialize algorithm run settings
            FireworksAlgorithmSettings2012 settings = new FireworksAlgorithmSettings2012
            {
                LocationsNumber = 8,
                ExplosionSparksNumberModifier = 50.0,
                ExplosionSparksNumberLowerBound = 0.04,
                ExplosionSparksNumberUpperBound = 0.8,
                ExplosionSparksMaximumAmplitude = 40.0,
                SpecificSparksNumber = 8,
                SpecificSparksPerExplosionNumber = 1,
                FunctionOrder = 2,
                SamplingNumber = 5
            };

            // 4. Instantiate desired implementation of the algorithm (per 2012 paper in this case)
            IFireworksAlgorithm fwa2012 = new FireworksAlgorithm2012(problem, stopCondition, settings);

            // 5. Finally, find a solution
            Solution solution = fwa2012.Solve();
        }
    }
}