using System;
using System.Collections.Generic;
using System.Diagnostics;
using FireworksNet.State;

namespace FireworksNet.StopConditions
{
    /// <summary>
    /// Allows to "chain" stop conditions, making algorithm stop when either
    /// all or at least one of the chained conditions are true.
    /// </summary>
    /// <remarks>Works in short circuit manner. Cannot mix ANDs and ORs.</remarks>
    public sealed class StopConditionChain : IStopCondition
    {
        /// <summary>
        /// Chaining operation.
        /// </summary>
        private enum AggregationOperator
        {
            /// <summary>
            /// The default operation that has no effect.
            /// </summary>
            None = 0,

            /// <summary>
            /// The "AND" - algorithm will stop when all of the 
            /// chained conditions are true.
            /// </summary>
            And,

            /// <summary>
            /// The "OR" - algorithm will stop when at least one of
            /// the chained conditions is true.
            /// </summary>
            Or
        }

        private readonly IList<IStopCondition> stopConditions;
        private StopConditionChain.AggregationOperator aggregationMode;

        /// <summary>
        /// Prevents a default instance of the <see cref="StopConditionChain"/>
        /// class from being created.
        /// </summary>
        private StopConditionChain()
        {
            this.stopConditions = new List<IStopCondition>();
            this.aggregationMode = StopConditionChain.AggregationOperator.None;
        }

        /// <summary>
        /// Creates a new instance of <see cref="StopConditionChain"/> and adds
        /// the specified stop condition as first in the chain.
        /// </summary>
        /// <param name="firstStopCondition">The first stop condition.</param>
        /// <returns>A new instance of <see cref="StopConditionChain"/> with
        /// <paramref name="firstStopCondition"/> as first in the chain.</returns>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="firstStopCondition"/>
        /// is <c>null</c>.</exception>
        public static StopConditionChain From(IStopCondition firstStopCondition)
        {
            if (firstStopCondition == null)
            {
                throw new ArgumentNullException(nameof(firstStopCondition));
            }

            StopConditionChain chain = new StopConditionChain();

            Debug.Assert(chain.stopConditions != null, "Chain stop condition collection is null");

            chain.stopConditions.Add(firstStopCondition);

            return chain;
        }

        /// <summary>
        /// Adds the specified stop condition to the chain with AND operation.
        /// </summary>
        /// <param name="anotherStopCondition">Another stop condition.</param>
        /// <returns>Stop condition chain with added <paramref name="anotherStopCondition"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="anotherStopCondition"/>
        /// is <c>null</c>.</exception>
        public StopConditionChain And(IStopCondition anotherStopCondition)
        {
            if (anotherStopCondition == null)
            {
                throw new ArgumentNullException(nameof(anotherStopCondition));
            }

            return this.AddStopCondition(anotherStopCondition, StopConditionChain.AggregationOperator.And);
        }

        /// <summary>
        /// Adds the specified stop condition to the chain with OR operation.
        /// </summary>
        /// <param name="anotherStopCondition">Another stop condition.</param>
        /// <returns>Stop condition chain with added <paramref name="anotherStopCondition"/>.</returns>
        /// <exception cref="System.ArgumentNullException"> if <paramref name="anotherStopCondition"/>
        /// is <c>null</c>.</exception>
        public StopConditionChain Or(IStopCondition anotherStopCondition)
        {
            if (anotherStopCondition == null)
            {
                throw new ArgumentNullException(nameof(anotherStopCondition));
            }

            return this.AddStopCondition(anotherStopCondition, StopConditionChain.AggregationOperator.Or);
        }

        /// <summary>
        /// Tells if an algorithm that is currently in <paramref name="state"/> state
        /// should stop (and don't make further steps) or not.
        /// </summary>
        /// <param name="state">The current algorithm state.</param>
        /// <returns>
        /// <c>true</c> if an algorithm that is currently in <paramref name="state"/>
        /// state should stop (and don't make further steps). Otherwise <c>false</c>.
        /// </returns>
        /// <exception cref="System.InvalidOperationException"> if unsupported
        /// <see cref="StopConditionChain.AggregationOperator"/> is used to
        /// chain stop conditions.</exception>
        public bool ShouldStop(IAlgorithmState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            switch (this.aggregationMode)
            {
                case StopConditionChain.AggregationOperator.And:
                    foreach (IStopCondition stopCondition in this.stopConditions)
                    {
                        Debug.Assert(stopCondition != null, "Stop condition is null");

                        if (!stopCondition.ShouldStop(state))
                        {
                            return false;
                        }
                    }
                    
                    return true;

                case StopConditionChain.AggregationOperator.Or:
                    foreach (IStopCondition stopCondition in this.stopConditions)
                    {
                        Debug.Assert(stopCondition != null, "Stop condition is null");

                        if (stopCondition.ShouldStop(state))
                        {
                            return true;
                        }
                    }

                    return false;

                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Adds the stop condition to the chain and ties it to the chain with specified
        /// operator.
        /// </summary>
        /// <param name="anotherStopCondition">Another stop condition.</param>
        /// <param name="mode">The mode (operator).</param>
        /// <returns>Stop condition chain with added <paramref name="anotherStopCondition"/>.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"> if <paramref name="mode"/>
        /// differs from one of the supported operations or is the default one.</exception>
        /// <exception cref="System.InvalidOperationException"> if trying to add new stop condition
        /// with the operator that differs from previous ones.</exception>
        private StopConditionChain AddStopCondition(IStopCondition anotherStopCondition, StopConditionChain.AggregationOperator mode)
        {
            if (mode == StopConditionChain.AggregationOperator.None)
            {
                throw new ArgumentOutOfRangeException(nameof(mode));
            }

            if (this.aggregationMode == StopConditionChain.AggregationOperator.None)
            {
                this.aggregationMode = mode;
            }

            if (this.aggregationMode != mode)
            {
                throw new InvalidOperationException();
            }

            Debug.Assert(this.stopConditions != null, "Stop condition collection is null");

            this.stopConditions.Add(anotherStopCondition);
            return this;
        }
    }
}