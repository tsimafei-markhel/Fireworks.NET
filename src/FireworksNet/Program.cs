using FireworksNet.Implementation;

namespace FireworksNet
{
    class Program
    {
        private readonly FireworksAlgorithmSettings Setup2010Paper = new FireworksAlgorithmSettings()
        {
            LocationsNumber = 5,
            ExplosionSparksNumberModifier = 50,
            ExplosionSparksNumberLowerBound = 0.04,
            ExplosionSparksNumberUpperBound = 0.8,
            ExplosionSparksMaximumAmplitude = 40,
            SpecificSparksNumber = 5,
            SpecificSparksPerExplosionNumber = 1
        };

        static void Main(string[] args)
        {
        }
    }
}