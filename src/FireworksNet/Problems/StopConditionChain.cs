using System;
using System.Collections.Generic;
using FireworksNet.Model;

namespace FireworksNet.Problems
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

        private AggregationOperator aggregationMode;

        private StopConditionChain()
        {
            stopConditions = new List<IStopCondition>();
            aggregationMode = AggregationOperator.None;
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

            return AddStopCondition(anotherStopCondition, AggregationOperator.And);
        }

        public StopConditionChain Or(IStopCondition anotherStopCondition)
        {
            if (anotherStopCondition == null)
            {
                throw new ArgumentNullException("anotherStopCondition");
            }

            return AddStopCondition(anotherStopCondition, AggregationOperator.Or);
        }

        public bool ShouldStop(AlgorithmState state)
        {
            switch (aggregationMode)
            {
                case AggregationOperator.And:
                    foreach (IStopCondition stopCondition in stopConditions)
                    {
						if (!stopCondition.ShouldStop(state))
                        {
                            return false;
                        }
                    }
                    
                    return true;

                case AggregationOperator.Or:
                    foreach (IStopCondition stopCondition in stopConditions)
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

        private StopConditionChain AddStopCondition(IStopCondition anotherStopCondition, AggregationOperator mode)
        {
            if (mode == AggregationOperator.None)
            {
                throw new ArgumentOutOfRangeException("mode");
            }

            if (aggregationMode == AggregationOperator.None)
            {
                aggregationMode = mode;
            }

            if (aggregationMode != mode)
            {
                throw new InvalidOperationException();
            }

            stopConditions.Add(anotherStopCondition);
            return this;
        }
    }
}