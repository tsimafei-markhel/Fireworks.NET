using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FireworksNet.Model;

namespace FireworksNet.Distances
{
    public abstract class Distance : IDistance
    {
        private readonly IEnumerable<Dimension> dimensions;

        protected Distance(IEnumerable<Dimension> dimensions)
        {
            if (dimensions == null)
            {
                throw new ArgumentNullException("dimensions");
            }

            this.dimensions = dimensions;
        }

        public abstract double Calculate(double[] first, double[] second);

        public virtual double Calculate(Solution first, Solution second)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }

            if (second == null)
            {
                throw new ArgumentNullException("second");
            }

            if (first == second)
            {
                return 0.0;
            }

            double[] firstCoordinates;
            double[] secondCoordinates;
            this.GetCoordinates(first, second, out firstCoordinates, out secondCoordinates);

            Debug.Assert(firstCoordinates != null, "First coordinate collection is null");
            Debug.Assert(secondCoordinates != null, "Second coordinate collection is null");

            return this.Calculate(firstCoordinates, secondCoordinates);
        }

        public virtual double Calculate(Solution first, double[] second)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }

            if (second == null)
            {
                throw new ArgumentNullException("second");
            }

            double[] firstCoordinates = this.GetCoordinates(first);

            Debug.Assert(firstCoordinates != null, "First coordinate collection is null");
            Debug.Assert(firstCoordinates.Length == second.Length, "First and Second coordinate collections have different length");

            return this.Calculate(firstCoordinates, second);
        }

        protected virtual double[] GetCoordinates(Solution solution)
        {
            Debug.Assert(this.dimensions != null, "Dimension collection is null");
            Debug.Assert(solution != null, "Solution is null");
            Debug.Assert(solution.Coordinates != null, "Solution coordinate collection is null");

            double[] coordinates = new double[this.dimensions.Count()];

            int dimensionCounter = 0;
            foreach (Dimension dimension in this.dimensions)
            {
                Debug.Assert(dimension != null, "Dimension is null");

                coordinates[dimensionCounter] = solution.Coordinates[dimension];
                dimensionCounter++;
            }

            return coordinates;
        }

        protected virtual void GetCoordinates(Solution first, Solution second, out double[] firstCoordinates, out double[] secondCoordinates)
        {
            Debug.Assert(this.dimensions != null, "Dimension collection is null");
            Debug.Assert(first != null, "First solution is null");
            Debug.Assert(second != null, "Second solution is null");
            Debug.Assert(first.Coordinates != null, "First solution coordinate collection is null");
            Debug.Assert(second.Coordinates != null, "Second solution coordinate collection is null");

            firstCoordinates = new double[this.dimensions.Count()];
            secondCoordinates = new double[this.dimensions.Count()];

            int dimensionCounter = 0;
            foreach (Dimension dimension in this.dimensions)
            {
                Debug.Assert(dimension != null, "Dimension is null");

                firstCoordinates[dimensionCounter] = first.Coordinates[dimension];
                secondCoordinates[dimensionCounter] = second.Coordinates[dimension];

                dimensionCounter++;
            }
        }
    }
}