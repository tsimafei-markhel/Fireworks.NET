using System;
using System.Collections.Generic;
using FireworksNet.Model;

namespace FireworksNet.Distances
{
    public class EuclideanDistance : DistanceBase
    {
        public EuclideanDistance(IEnumerable<Dimension> dimensions)
            : base(dimensions)
        {
        }

        public override double Calculate(double[] first, double[] second)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }

            if (second == null)
            {
                throw new ArgumentNullException("second");
            }

            if (first.Length != second.Length)
            {
                throw new ArgumentException(string.Empty, "second");
            }

            return MathNet.Numerics.Distance.Euclidean(first, second);
        }
    }
}