using System.Collections.Generic;
using System.Diagnostics;
using MathNet.Numerics.Distributions;

namespace FireworksNet.Distributions
{
    public class NormalDistribution : IContinuousDistribution
    {
        private readonly Normal internalNormal;

        public NormalDistribution(double mean, double stddev)
        {
            internalNormal = new Normal(mean, stddev);
        }

        public double Sample()
        {
            Debug.Assert(internalNormal != null, "Internal distribution implementation is null");

            return internalNormal.Sample();
        }

        public IEnumerable<double> Samples()
        {
            Debug.Assert(internalNormal != null, "Internal distribution implementation is null");

            return internalNormal.Samples();
        }
    }
}