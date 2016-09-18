using System;
using System.Diagnostics;
using FireworksNet.Distances;
using FireworksNet.Extensions;
using FireworksNet.Model;

namespace FireworksNet.StopConditions
{
    /// <summary>
    /// Stops when coordinates of some <see cref="Solution"/> (typically
    /// the current best one) gets close enough to the coordinates of
    /// the etalon <see cref="Solution"/>.
    /// </summary>
    public class CoordinateProximityStopCondition : ProximityStopCondition
    {
        /// <summary>
        /// Gets the distance calculator.
        /// </summary>
        public IDistance DistanceCalculator { get; private set; }

        /// <summary>
        /// Gets the distance threshold.
        /// </summary>
        public double DistanceThreshold { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateProximityStopCondition"/> class.
        /// </summary>
        /// <param name="expectation">The the expected <see cref="Solution"/> (an etalon).</param>
        /// <param name="distanceCalculator">The distance calculator.</param>
        /// <param name="distanceThreshold">The distance threshold.</param>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="distanceCalculator"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException"> if <paramref name="distanceThreshold"/>
        /// is <see cref="Double.NaN"/> or <see cref="Double.PositiveInfinity"/> or
        /// <see cref="Double.NegativeInfinity"/>.</exception>
        public CoordinateProximityStopCondition(Solution expectation, IDistance distanceCalculator, double distanceThreshold)
            : base(expectation)
        {
            if (distanceCalculator == null)
            {
                throw new ArgumentNullException(nameof(distanceCalculator));
            }

            if (double.IsNaN(distanceThreshold) || double.IsInfinity(distanceThreshold))
            {
                throw new ArgumentOutOfRangeException(nameof(distanceThreshold));
            }

            this.DistanceCalculator = distanceCalculator;
            this.DistanceThreshold = distanceThreshold;
        }

        /// <summary>
        /// Tells if an algorithm that is currently in <paramref name="state"/> state
        /// should stop (and don't make further steps) or not. Stops if the distance
        /// between the <paramref name="state"/>'s Best Solution and the 
        /// <see cref="CoordinateProximityStopCondition"/>.Expectation is less than
        /// or equal to <see cref="CoordinateProximityStopCondition"/>.DistanceThreshold.
        /// </summary>
        /// <param name="state">The current algorithm state.</param>
        /// <returns>
        /// <c>true</c> if an algorithm that is currently in <paramref name="state"/>
        /// state should stop (and don't make further steps). Otherwise <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="state"/>
        /// is <c>null</c>.</exception>
        public override bool ShouldStop(AlgorithmState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            Debug.Assert(this.DistanceCalculator != null, "Distance calculator is null");
            Debug.Assert(state.BestSolution != null, "State best solution is null");
            Debug.Assert(this.Expectation != null, "Expectation is null");
            Debug.Assert(!double.IsNaN(this.DistanceThreshold), "Distance threshold is NaN");
            Debug.Assert(!double.IsInfinity(this.DistanceThreshold), "Distance threshold is Infinity");

            double distance = this.DistanceCalculator.Calculate(state.BestSolution, this.Expectation);

            Debug.Assert(!double.IsNaN(distance), "Distance is NaN");
            Debug.Assert(!double.IsInfinity(distance), "Distance is Infinity");

            return distance.IsLessOrEqual(this.DistanceThreshold);
        }
    }
}