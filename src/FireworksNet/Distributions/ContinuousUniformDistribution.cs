using System.Collections.Generic;
using System.Diagnostics;
using MathNet.Numerics.Distributions;

namespace FireworksNet.Distributions
{
    /// <summary>
    /// Continuous Uniform distribution.
    /// </summary>
    public class ContinuousUniformDistribution : IContinuousDistribution
    {
        private readonly ContinuousUniform internalContinuousUniform;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContinuousUniformDistribution"/> class.
        /// </summary>
        /// <param name="lower">Lower bound. Range: lower ≤ upper.</param>
        /// <param name="upper">Upper bound. Range: lower ≤ upper.</param>
        public ContinuousUniformDistribution(double lower, double upper)
        {
            this.internalContinuousUniform = new ContinuousUniform(lower, upper);
        }

        /// <summary>
        /// Draws a random sample from the distribution.
        /// </summary>
        /// <returns>
        /// A sample from the distribution.
        /// </returns>
        public double Sample()
        {
            Debug.Assert(this.internalContinuousUniform != null, "Internal distribution implementation is null");

            return this.internalContinuousUniform.Sample();
        }

        /// <summary>
        /// Draws a sequence of random samples from the distribution.
        /// </summary>
        /// <returns>
        /// An infinite sequence of samples from the distribution.
        /// </returns>
        public IEnumerable<double> Samples()
        {
            Debug.Assert(this.internalContinuousUniform != null, "Internal distribution implementation is null");

            return this.internalContinuousUniform.Samples();
        }
    }
}