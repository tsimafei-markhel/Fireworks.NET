using System;
using System.Collections.Generic;
using FireworksNet.Model;

namespace FireworksNet.Problems
{
    public interface IStopCondition
    {
        bool ShouldStop(IEnumerable<Firework> currentFireworks);
    }
}