using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FireworksNet.Extensions;
using FireworksNet.Model;
using FireworksNet.Problems;

namespace FireworksNet.Selection.Extremum
{
    /// <summary>
    /// Contains logic for selecting the best and the worst <see cref="Firework"/>s
    /// from a given set according to some rule.
    /// </summary>
    public class ExtremumFireworkSelector : IExtremumFireworkSelector
    {
        private readonly ProblemTarget problemTarget;

        /// <summary>
        /// Fired before looking for the best firework.
        /// </summary>
        public event EventHandler<ExtremumFireworkFindingEventArgs> BestFireworkFinding;

        /// <summary>
        /// Fired after the best firework is found.
        /// </summary>
        public event EventHandler<ExtremumFireworkFoundEventArgs> BestFireworkFound;

        /// <summary>
        /// Fired before looking for the worst firework.
        /// </summary>
        public event EventHandler<ExtremumFireworkFindingEventArgs> WorstFireworkFinding;

        /// <summary>
        /// Fired after the worst firework is found.
        /// </summary>
        public event EventHandler<ExtremumFireworkFoundEventArgs> WorstFireworkFound;

        /// <summary>
        /// Initializes a new instance of <see cref="ExtremumFireworkSelector"/> class.
        /// </summary>
        /// <param name="problemTarget">Target of the problem under investigation.</param>
        public ExtremumFireworkSelector(ProblemTarget problemTarget)
        {
            this.problemTarget = problemTarget;
        }

        /// <summary>
        /// Selects the best of <see cref="Firework"/>s from
        /// the <paramref name="from"/> parameters (in terms of firework quality).
        /// </summary>
        /// <param name="from"><see cref="Firework"/>s to select the best one
        /// from.</param>
        /// <returns>The best <see cref="Firework"/>.</returns>
        public Firework SelectBest(params Firework[] from)
        {
            Debug.Assert(from != null, "Firework parameters is null");

            return this.SelectBest((IEnumerable<Firework>)from);
        }

        /// <summary>
        /// Selects the best of <see cref="Firework"/>s from
        /// the <paramref name="from"/> collection (in terms of firework quality).
        /// </summary>
        /// <param name="from">A collection to select <see cref="Firework"/>s
        /// from.</param>
        /// <returns>The best <see cref="Firework"/>.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="from"/> is
        /// <c>null</c>.</exception>
        public Firework SelectBest(IEnumerable<Firework> from)
        {
            if (from == null)
            {
                throw new ArgumentNullException(nameof(from));
            }

            this.OnBestFireworkFinding(new ExtremumFireworkFindingEventArgs(from));

            Firework bestFirework = null;
            if (this.problemTarget == ProblemTarget.Minimum)
            {
                bestFirework = from.Aggregate(this.GetLessQualityFirework);
            }
            else
            {
                bestFirework = from.Aggregate(this.GetGreaterQualityFirework);
            }

            this.OnBestFireworkFound(new ExtremumFireworkFoundEventArgs(from, bestFirework));
            return bestFirework;
        }

        /// <summary>
        /// Selects the worst of <see cref="Firework"/>s from
        /// the <paramref name="from"/> parameters (in terms of firework quality).
        /// </summary>
        /// <param name="from"><see cref="Firework"/>s to select the worst one
        /// from.</param>
        /// <returns>The worst <see cref="Firework"/>.</returns>
        public Firework SelectWorst(params Firework[] from)
        {
            Debug.Assert(from != null, "Firework parameters is null");

            return this.SelectWorst((IEnumerable<Firework>)from);
        }

        /// <summary>
        /// Selects the worst of <see cref="Firework"/>s from
        /// the <paramref name="from"/> collection (in terms of firework quality).
        /// </summary>
        /// <param name="from">A collection to select <see cref="Firework"/>s
        /// from.</param>
        /// <returns>The worst <see cref="Firework"/>.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="from"/> is
        /// <c>null</c>.</exception>
        public Firework SelectWorst(IEnumerable<Firework> from)
        {
            if (from == null)
            {
                throw new ArgumentNullException(nameof(from));
            }

            this.OnWorstFireworkFinding(new ExtremumFireworkFindingEventArgs(from));

            Firework worstFirework = null;
            if (this.problemTarget == ProblemTarget.Minimum)
            {
                worstFirework = from.Aggregate(this.GetGreaterQualityFirework);
            }
            else
            {
                worstFirework = from.Aggregate(this.GetLessQualityFirework);
            }

            this.OnWorstFireworkFound(new ExtremumFireworkFoundEventArgs(from, worstFirework));
            return worstFirework;
        }

        /// <summary>
        /// Gets <see cref="Firework"/> with minimum quality.
        /// </summary>
        /// <param name="currentMinimum">Current minimum quality <see cref="Firework"/>.</param>
        /// <param name="candidate">The <see cref="Firework"/> to be compared with
        /// <paramref name="currentMinimum"/>.</param>
        /// <returns>The <see cref="Firework"/> with minimum quality.</returns>
        protected virtual Firework GetLessQualityFirework(Firework currentMinimum, Firework candidate)
        {
            Debug.Assert(currentMinimum != null, "Current minimum is null");
            Debug.Assert(candidate != null, "Candidate for minimum is null");

            return candidate.Quality.IsLess(currentMinimum.Quality) ? candidate : currentMinimum;
        }

        /// <summary>
        /// Gets <see cref="Firework"/> with maximum quality.
        /// </summary>
        /// <param name="currentMaximum">Current maximum quality <see cref="Firework"/>.</param>
        /// <param name="candidate">The <see cref="Firework"/> to be compared with
        /// <paramref name="currentMaximum"/>.</param>
        /// <returns>The <see cref="Firework"/> with maximum quality.</returns>
        protected virtual Firework GetGreaterQualityFirework(Firework currentMaximum, Firework candidate)
        {
            Debug.Assert(currentMaximum != null, "Current maximum is null");
            Debug.Assert(candidate != null, "Candidate for maximum is null");

            return candidate.Quality.IsGreater(currentMaximum.Quality) ? candidate : currentMaximum;
        }

        /// <summary>
        /// Firing an event before searching for the best firework.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnBestFireworkFinding(ExtremumFireworkFindingEventArgs eventArgs)
        {
            this.BestFireworkFinding?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// Firing an event after finding the best firework.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnBestFireworkFound(ExtremumFireworkFoundEventArgs eventArgs)
        {
            this.BestFireworkFound?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// Firing an event before searching for the worst firework.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnWorstFireworkFinding(ExtremumFireworkFindingEventArgs eventArgs)
        {
            this.WorstFireworkFinding?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// Firing an event after finding the worst firework.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnWorstFireworkFound(ExtremumFireworkFoundEventArgs eventArgs)
        {
            this.WorstFireworkFound?.Invoke(this, eventArgs);
        }
    }
}