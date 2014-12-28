using System;
using System.Collections.Generic;
using MathNet.Numerics.Distributions;

namespace FireworksNet.Distributions
{
    public class NormalDistribution : IContinuousDistribution
    {
        private readonly Normal internalNormal;

        public NormalDistribution(Double mean, Double stddev)
        {
            internalNormal = new Normal(mean, stddev);
        }

        public double Sample()
        {
            return internalNormal.Sample();
        }

        public IEnumerable<Double> Samples()
        {
            return internalNormal.Samples();
        }
    }
}