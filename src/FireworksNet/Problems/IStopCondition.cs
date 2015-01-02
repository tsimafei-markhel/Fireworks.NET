using System;
using System.Collections.Generic;
using FireworksNet.Model;

namespace FireworksNet.Problems
{
    public interface IStopCondition
    {
        Boolean ShouldStop(IEnumerable<Firework> currentFireworks);
    }
}