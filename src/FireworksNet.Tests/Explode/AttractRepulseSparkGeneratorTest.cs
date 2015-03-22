using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FireworksNet.ParallelExplode;
using FireworksNet.Explode;
using FireworksNet.Problems;
using FireworksNet.Model;
using FireworksNet.Distributions;
using FireworksNet.Problems.Benchmark;
using FireworksNet.Tests.Extensions;
using FireworksNet.Random;

namespace FireworksNet.Tests.Explode
{
    public class AttractRepulseSparkGeneratorTest
    {
        private const double normalDistributionMean = 1.0;
        private const double normalDistributionStdDev = 1.0;

        public static Problem problem;

        private IExploder exploder;
        private ParallelExploderSettings settings;
        private ISparkGenerator sparkGenerator;
        private Solution bestSolution;
        
        private readonly System.Random randomizer;
        private readonly IContinuousDistribution distribution;

        public AttractRepulseSparkGeneratorTest()
        {
            problem = ExploderTestHelper.ProblemToSolve;
            bestSolution = ExploderTestHelper.BestFirework;
            settings = new ParallelExploderSettings();            
            exploder = new ParallelExploder(settings);
            randomizer = new DefaultRandom();
            distribution = new NormalDistribution(normalDistributionMean, normalDistributionStdDev);
            sparkGenerator = new AttractRepulseSparkGenerator(ref bestSolution, problem.Dimensions, distribution, randomizer);            
        }

        [Fact]
        public void CreateSparkTyped_MustReturnNotNullFirework()
        {
            ExplosionBase explosion = exploder.Explode(ExploderTestHelper.Epicentr, ExploderTestHelper.Qualities, 1);
            Firework spark = sparkGenerator.CreateSpark(explosion);

            Assert.NotNull(spark);
        }         

        [Fact]
        public void CreateIntaceOfAttractRepulseGenerator_Pass2ndParameterAsNull_ExceptionThrown()
        {             
            ISparkGenerator sg = new AttractRepulseSparkGenerator(ref bestSolution, null, distribution, randomizer);

            Assert.Null(sg);
        }

        [Fact]
        public void CreateIntaceOfAttractRepulseGenerator_Pass3ndParameterAsNull_ExceptionThrown()
        {
            ISparkGenerator sg = new AttractRepulseSparkGenerator(ref bestSolution, problem.Dimensions, null, randomizer);

            Assert.Null(sg);
        }

        [Fact]
        public void CreateIntaceOfAttractRepulseGenerator_Pass4ndParameterAsNull_ExceptionThrown()
        {
            ISparkGenerator sg = new AttractRepulseSparkGenerator(ref bestSolution, problem.Dimensions, distribution, null);

            Assert.Null(sg);
        }
    }
}
