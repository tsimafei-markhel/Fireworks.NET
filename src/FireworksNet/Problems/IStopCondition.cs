using System;
using System.Collections.Generic;
using FireworksNet.Model;

namespace FireworksNet.Problems
{
    public interface IStopCondition
    {
        Boolean ShouldStop(IEnumerable<Firework> currentFireworks);

        // TODO: These two seem to belong to another interface... maybe extension? Or static class ConditionChain.From(firstCondition).And(secondCondition).And(...
        IStopCondition And(IStopCondition anotherStopCondition);
        IStopCondition Or(IStopCondition anotherStopCondition);
    }
}