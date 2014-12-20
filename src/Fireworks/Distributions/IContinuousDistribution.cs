using System;
using System.Collections.Generic;

namespace Fireworks.Distributions
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
        Double Sample();

        /// <summary>
        /// Draws a sequence of random samples from the distribution.
        /// </summary>
        /// <returns>An infinite sequence of samples from the distribution.</returns>
        IEnumerable<Double> Samples();
    }
}