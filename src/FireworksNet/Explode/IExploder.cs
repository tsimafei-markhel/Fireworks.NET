using System;
using FireworksNet.Model;

namespace FireworksNet.Explode
{
    public interface IExploder
    {
        // TODO: Make it per-run, not per-step so that one instance per an algorithm run is created
        Explosion Explode(Firework epicenter, Int32 currentStepNumber);
    }
}