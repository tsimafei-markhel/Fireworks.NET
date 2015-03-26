using System;
using System.Diagnostics;
using FireworksNet.Extensions;
using FireworksNet.Model;

namespace FireworksNet.StopConditions
{
    /// <summary>
    /// Stops when quality of some <see cref="Solution"/> (typically
    /// the current best one) gets close enough to the quality of
    /// the etalon <see cref="Solution"/>.
    /// </summary>
    public class QualityProximityStopCondition : ProximityStopCondition
    {
        /// <summary>
        /// Gets the quality threshold.
        /// </summary>
        public double QualityThreshold { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="QualityProximityStopCondition"/> class.
        /// </summary>
        /// <param name="expectation">The the expected <see cref="Solution"/> (an etalon).</param>
        /// <param name="qualityThreshold">The quality threshold.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"> if <paramref name="qualityThreshold"/>
        /// is <see cref="Double.NaN"/> or <see cref="Double.PositiveInfinity"/> or
        /// <see cref="Double.NegativeInfinity"/>.</exception>
        public QualityProximityStopCondition(Solution expectation, double qualityThreshold)
            : base(expectation)
        {
            if (double.IsNaN(qualityThreshold) || double.IsInfinity(qualityThreshold))
            {
                throw new ArgumentOutOfRangeException("qualityThreshold");
            }

            this.QualityThreshold = qualityThreshold;
        }

        /// <summary>
        /// Tells if an algorithm that is currently in <paramref name="state"/> state
        /// should stop (and don't make further steps) or not. Stops if absolute value
        /// of the difference between the quality of <paramref name="state"/>'s Best
        /// Solution and <see cref="QualityProximityStopCondition.Expectation"/> is less
        /// than or equal to <see cref="QualityProximityStopCondition.QualityThreshold"/>.
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
                throw new ArgumentNullException("state");
            }

            Debug.Assert(state.BestSolution != null, "State best solution is null");
            Debug.Assert(this.Expectation != null, "Expectation is null");
            Debug.Assert(!double.IsNaN(this.QualityThreshold), "Quality threshold is NaN");
            Debug.Assert(!double.IsInfinity(this.QualityThreshold), "Quality threshold is Infinity");

            double qualityProximity = Math.Abs(state.BestSolution.Quality - Expectation.Quality);

            Debug.Assert(!double.IsNaN(qualityProximity), "Quality proximity is NaN");
            Debug.Assert(!double.IsInfinity(qualityProximity), "Quality proximity is Infinity");

            return qualityProximity.IsLessOrEqual(this.QualityThreshold);
        }
    }
}