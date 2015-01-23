using System.Collections.Generic;

namespace FireworksNet.Distributions
{
    /// <summary>
    /// Continuous Univariate Probability Distribution.
    /// </summary>
    public interface IContinuousDistribution
    {  
        /// <summary>
        /// Draws a random sample from the distribution.
        /// </summary>
        /// <returns>A sample from the distribution.</returns>
        double Sample();

        /// <summary>
        /// Draws a sequence of random samples from the distribution.
        /// </summary>
        /// <returns>An infinite sequence of samples from the distribution.</returns>
        IEnumerable<double> Samples();
    }
}