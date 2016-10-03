using FireworksNet.Algorithm;
using FireworksNet.Algorithm.Implementation;
using FireworksNet.Model;
using FireworksNet.Problems;
using FireworksNet.Problems.Benchmark;
using FireworksNet.StopConditions;

namespace FireworksNet.Examples
{
    public static class SimpleRun
    {
        public static Solution RunFireworks2010()
        {
            // 1. Define a problem to solve.
            // Here, one of the benchmark problems is used, for which
            // solution is well-known.
            // You would want to use Problem class to define your own problems
            // or inherit from it in case you need more flexibility.
            Problem problem = Sphere.Create();

            // 2. Setup algorithm stop condition.
            // Using CounterStopCondition to count a number of calculations
            // of the problem function.
            CounterStopCondition stopCondition = new CounterStopCondition(10000);
            // Need to subsctibe to QualityCalculated event of the
            // problem in order for stop condition to work.
            // This is the reason to use CounterStopCondition type here
            // instead of IStopCondition.
            problem.QualityCalculated += stopCondition.IncrementCounter;

            // 3. Initialize algorithm run settings.
            // Define algorithm settings based on the target problem.
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

            // 4. Instantiate desired implementation of the algorithm (per 2010 paper in this case).
            // Choose the predefined algorithm or build your own by implementing IFireworksAlgorithm.
            IFireworksAlgorithm fwa2010 = new FireworksAlgorithm(problem, stopCondition, settings);

            // 5. Finally, find a solution.
            // Here, algorithm will be doing steps until the stop condition
            // is met. In this example, algorithm will be running until
            // target function is calculated 10000 times.
            Solution solution = fwa2010.Solve();

            return solution;
        }

        public static Solution RunFireworks2012()
        {
            // 1. Define a problem to solve.
            Problem problem = Sphere2012.Create();

            // 2. Setup algorithm stop condition.
            CounterStopCondition stopCondition = new CounterStopCondition(10000);
            problem.QualityCalculated += stopCondition.IncrementCounter;

            // 3. Initialize algorithm run settings.
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

            // 4. Instantiate desired implementation of the algorithm (per 2012 paper in this case).
            IFireworksAlgorithm fwa2012 = new FireworksAlgorithm2012(problem, stopCondition, settings);

            // 5. Finally, find a solution.
            Solution solution = fwa2012.Solve();

            return solution;
        }
    }
}