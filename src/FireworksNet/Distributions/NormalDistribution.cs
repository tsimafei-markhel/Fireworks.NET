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
            this.internalNormal = new Normal(mean, stddev);
        }

        public double Sample()
        {
            Debug.Assert(this.internalNormal != null, "Internal distribution implementation is null");

            return this.internalNormal.Sample();
        }

        public IEnumerable<double> Samples()
        {
            Debug.Assert(this.internalNormal != null, "Internal distribution implementation is null");

            return this.internalNormal.Samples();
        }
    }
}