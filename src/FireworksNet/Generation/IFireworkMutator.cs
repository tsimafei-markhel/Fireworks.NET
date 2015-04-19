using FireworksNet.Model;

namespace FireworksNet.Explode
{
    /// <summary>
    /// Firework mutator interface.
    /// </summary>
    public interface IFireworkMutator
    {
        void MutateFirework(ref MutableFirework mutableFirework, FireworkExplosion explosion);
    }
}
