using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FireworksNet.Model;

namespace FireworksNet.Distances
{
    /// <summary>
    /// Base class for distance calculators.
    /// </summary>
    public abstract class DistanceBase : IDistance
    {
        /// <summary>
        /// A collection of <see cref="Dimension"/>s - needed for 
        /// <see cref="Solution"/>-to-<see cref="Double"/>-array conversion.
        /// </summary>
        private readonly IEnumerable<Dimension> dimensions;

        /// <summary>
        /// Initializes a new instance of <see cref="DistanceBase"/> with
        /// defined collection of <see cref="Dimension"/>s.
        /// </summary>
        /// <param name="dimensions">The collection of <see cref="Dimension"/>s
        /// - needed for <see cref="Solution"/>-to-<see cref="Double"/>-array
        /// conversion.</param>
        protected DistanceBase(IEnumerable<Dimension> dimensions)
        {
            if (dimensions == null)
            {
                throw new ArgumentNullException(nameof(dimensions));
            }

            this.dimensions = dimensions;
        }

        /// <summary>
        /// Calculates distance between two entities. Entities coordinates 
        /// are represented by <paramref name="first"/> and <paramref name="second"/>.
        /// </summary>
        /// <param name="first">The first entity.</param>
        /// <param name="second">The second entity.</param>
        /// <returns>The distance between <paramref name="first"/> and 
        /// <paramref name="second"/>.</returns>
        public abstract double Calculate(double[] first, double[] second);

        /// <summary>
        /// Calculates distance between two <see cref="Solution"/>s. Solution coordinates 
        /// are to be stored in <paramref name="first"/> and <paramref name="second"/>.
        /// </summary>
        /// <param name="first">The first solution.</param>
        /// <param name="second">The second solution.</param>
        /// <returns>The distance between <paramref name="first"/> and 
        /// <paramref name="second"/>.</returns>
        public virtual double Calculate(Solution first, Solution second)
        {
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
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

        /// <summary>
        /// Calculates distance between a solution and an entity. Entity coordinates 
        /// are represented by <paramref name="first"/> and <paramref name="second"/>.
        /// </summary>
        /// <param name="first">The solution.</param>
        /// <param name="second">The entity.</param>
        /// <returns>The distance between <paramref name="first"/> and 
        /// <paramref name="second"/>.</returns>
        public virtual double Calculate(Solution first, double[] second)
        {
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            double[] firstCoordinates = this.GetCoordinates(first);

            Debug.Assert(firstCoordinates != null, "First coordinate collection is null");
            Debug.Assert(firstCoordinates.Length == second.Length, "First and Second coordinate collections have different length");

            return this.Calculate(firstCoordinates, second);
        }

        /// <summary>
        /// Converts <paramref name="solution"/> to <see cref="Double"/> array.
        /// </summary>
        /// <param name="solution">The <see cref="Solution"/> instance to be converted.</param>
        /// <returns><see cref="Double"/> array that corresponds to the coordinates
        /// of <paramref name="solution"/>.</returns>
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

        /// <summary>
        /// Converts <paramref name="first"/> and <paramref name="second"/> to
        /// <see cref="Double"/> arrays.
        /// </summary>
        /// <param name="first">The first <see cref="Solution"/> instance to be converted.</param>
        /// <param name="second">The second <see cref="Solution"/> instance to be converted.</param>
        /// <param name="firstCoordinates">The <see cref="Double"/> array that corresponds to the 
        /// <paramref name="first"/> solution.</param>
        /// <param name="secondCoordinates">The <see cref="Double"/> array that corresponds to the 
        /// <paramref name="second"/> solution.</param>
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