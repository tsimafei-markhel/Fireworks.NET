using System;
using System.Collections.Generic;
using FireworksNet.Model;

namespace FireworksNet.Distances
{
    /// <summary>
    /// Distance calculator that finds Euclidean distance between two entities.
    /// </summary>
    public class EuclideanDistance : DistanceBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EuclideanDistance"/> class.
        /// </summary>
        /// <param name="dimensions">The collection of <see cref="Dimension"/>s
        /// - needed for <see cref="Solution"/>-to-<see cref="Double"/>-array
        /// conversion.</param>
        public EuclideanDistance(IEnumerable<Dimension> dimensions)
            : base(dimensions)
        {
        }

        /// <summary>
        /// Calculates Euclidean distance between two entities. Entities coordinates
        /// are represented by <paramref name="first"/> and <paramref name="second"/>.
        /// </summary>
        /// <param name="first">The first entity.</param>
        /// <param name="second">The second entity.</param>
        /// <returns>
        /// The distance between <paramref name="first"/> and
        /// <paramref name="second"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="first"/>
        /// or <paramref name="second"/> is <c>null</c>.</exception>
        /// <exception cref="System.ArgumentException"> if <paramref name="second"/>'s
        /// length is not equal to <paramref name="first"/>'s length.</exception>
        public override double Calculate(double[] first, double[] second)
        {
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            if (first.Length != second.Length)
            {
                throw new ArgumentException(string.Empty, nameof(second));
            }

            return MathNet.Numerics.Distance.Euclidean(first, second);
        }
    }
}