using FireworksNet.Algorithm.Implementation;
using FireworksNet.Model;
using FireworksNet.Problems.Benchmark;

namespace FireworksNet.Console
{
	public class Program
	{
		private static void Main(string[] args)
		{
			FireworksAlgorithmSettings fwaSettings = new FireworksAlgorithmSettings()
			{
				LocationsNumber = 5,
				ExplosionSparksNumberModifier = 50.0,
				ExplosionSparksNumberLowerBound = 0.04,
				ExplosionSparksNumberUpperBound = 0.8,
				ExplosionSparksMaximumAmplitude = 40.0,
				SpecificSparksNumber = 5,
				SpecificSparksPerExplosionNumber = 1
			};

			FireworksAlgorithm fwa = new FireworksAlgorithm(Sphere2010.Create(), fwaSettings);
			Solution solution = fwa.Solve();
		}
	}
}