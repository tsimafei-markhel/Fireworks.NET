using System;
using System.Collections.Generic;
using FireworksNet.Model;

namespace FireworksNet.Explode
{
    public interface IExploder
    {
        Explosion Explode(Firework epicenter, IEnumerable<Double> currentFireworkQualities, Int32 currentStepNumber);
    }
}