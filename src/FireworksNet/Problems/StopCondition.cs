using System;
using System.Collections.Generic;
using FireworksNet.Model;

namespace FireworksNet.Problems
{
    // Conditions can be either all AND or all OR, cannot mix them
    public abstract class StopCondition : IStopCondition
    {
        private enum AggregationOperator
        {
            None = 0,
            And,
            Or
        }

        private readonly IList<IStopCondition> stopConditions;

        private AggregationOperator aggregationMode;

        protected StopCondition()
        {
            stopConditions = new List<IStopCondition>();
            aggregationMode = AggregationOperator.None;
        }

        public abstract Boolean CheckCondition(IEnumerable<Firework> currentFireworks);

        public virtual Boolean ShouldStop(IEnumerable<Firework> currentFireworks)
        {
            switch (aggregationMode)
            {
                case AggregationOperator.And:
                    foreach (IStopCondition stopCondition in stopConditions)
                    {
                        if (!stopCondition.ShouldStop(currentFireworks))
                        {
                            return false;
                        }
                    }
                    
                    break;

                case AggregationOperator.Or:
                    foreach (IStopCondition stopCondition in stopConditions)
                    {
                        if (stopCondition.ShouldStop(currentFireworks))
                        {
                            return true;
                        }
                    }

                    break;
            }

            return CheckCondition(currentFireworks);
        }

        public virtual IStopCondition And(IStopCondition anotherStopCondition)
        {
            if (anotherStopCondition == null)
            {
                throw new ArgumentNullException("anotherStopCondition");
            }

            return AddStopCondition(anotherStopCondition, AggregationOperator.And);
        }

        public virtual IStopCondition Or(IStopCondition anotherStopCondition)
        {
            if (anotherStopCondition == null)
            {
                throw new ArgumentNullException("anotherStopCondition");
            }

            return AddStopCondition(anotherStopCondition, AggregationOperator.Or);
        }

        private IStopCondition AddStopCondition(IStopCondition anotherStopCondition, AggregationOperator mode)
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
            return (IStopCondition)this;
        }
    }
}