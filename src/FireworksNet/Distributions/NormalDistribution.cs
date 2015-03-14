using System.Collections.Generic;
using System.Diagnostics;
using MathNet.Numerics.Distributions;

namespace FireworksNet.Distributions
{
    /// <summary>
    /// Normal (Gaussian) distribution.
    /// </summary>
    public class NormalDistribution : IContinuousDistribution
    {
        private readonly Normal internalNormal;

        /// <summary>
        /// Initializes a new instance of the <see cref="NormalDistribution"/> class.
        /// </summary>
        /// <param name="mean">The mean.</param>
        /// <param name="standardDeviation">The standard deviation.</param>
        public NormalDistribution(double mean, double standardDeviation)
        {
            this.internalNormal = new Normal(mean, standardDeviation);
        }

        /// <summary>
        /// Draws a random sample from the distribution.
        /// </summary>
        /// <returns>
        /// A sample from the distribution.
        /// </returns>
        public double Sample()
        {
            Debug.Assert(this.internalNormal != null, "Internal distribution implementation is null");

            return this.internalNormal.Sample();
        }

        /// <summary>
        /// Draws a sequence of random samples from the distribution.
        /// </summary>
        /// <returns>
        /// An infinite sequence of samples from the distribution.
        /// </returns>
        public IEnumerable<double> Samples()
        {
            Debug.Assert(this.internalNormal != null, "Internal distribution implementation is null");

            return this.internalNormal.Samples();
        }
    }
}