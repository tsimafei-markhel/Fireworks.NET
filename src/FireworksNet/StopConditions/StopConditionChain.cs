using System;
using System.Collections.Generic;
using FireworksNet.Model;

namespace FireworksNet.StopConditions
{
    // Conditions can be either all AND or all OR, cannot mix them
    // Works in short circuit manner
    public sealed class StopConditionChain : IStopCondition
    {
        private enum AggregationOperator
        {
            None = 0,
            And,
            Or
        }

        private readonly IList<IStopCondition> stopConditions;

        private StopConditionChain.AggregationOperator aggregationMode;

        private StopConditionChain()
        {
            this.stopConditions = new List<IStopCondition>();
            this.aggregationMode = StopConditionChain.AggregationOperator.None;
        }

        public static StopConditionChain From(IStopCondition firstStopCondition)
        {
            if (firstStopCondition == null)
            {
                throw new ArgumentNullException("firstStopCondition");
            }

            StopConditionChain chain = new StopConditionChain();
            chain.stopConditions.Add(firstStopCondition);

            return chain;
        }

        public StopConditionChain And(IStopCondition anotherStopCondition)
        {
            if (anotherStopCondition == null)
            {
                throw new ArgumentNullException("anotherStopCondition");
            }

            return this.AddStopCondition(anotherStopCondition, StopConditionChain.AggregationOperator.And);
        }

        public StopConditionChain Or(IStopCondition anotherStopCondition)
        {
            if (anotherStopCondition == null)
            {
                throw new ArgumentNullException("anotherStopCondition");
            }

            return this.AddStopCondition(anotherStopCondition, StopConditionChain.AggregationOperator.Or);
        }

        public bool ShouldStop(AlgorithmState state)
        {
            switch (this.aggregationMode)
            {
                case StopConditionChain.AggregationOperator.And:
                    foreach (IStopCondition stopCondition in this.stopConditions)
                    {
                        if (!stopCondition.ShouldStop(state))
                        {
                            return false;
                        }
                    }
                    
                    return true;

                case StopConditionChain.AggregationOperator.Or:
                    foreach (IStopCondition stopCondition in this.stopConditions)
                    {
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

        private StopConditionChain AddStopCondition(IStopCondition anotherStopCondition, StopConditionChain.AggregationOperator mode)
        {
            if (mode == StopConditionChain.AggregationOperator.None)
            {
                throw new ArgumentOutOfRangeException("mode");
            }

            if (this.aggregationMode == StopConditionChain.AggregationOperator.None)
            {
                this.aggregationMode = mode;
            }

            if (this.aggregationMode != mode)
            {
                throw new InvalidOperationException();
            }

            this.stopConditions.Add(anotherStopCondition);
            return this;
        }
    }
}