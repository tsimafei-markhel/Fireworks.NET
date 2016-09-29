using FireworksNet.Model;

namespace FireworksNet.Mutation
{
    /// <summary>
    /// The Firework mutator interface.
    /// </summary>
    public interface IFireworkMutator
    {
        /// <summary>
        /// Changes the <paramref name="firework"/>.
        /// </summary>
        /// <param name="firework">The <see cref="MutableFirework"/> to be changed.</param>
        /// <param name="explosion">The <see cref="FireworkExplosion"/> that
        /// contains explosion characteristics.</param>
        void MutateFirework(ref MutableFirework firework, FireworkExplosion explosion);
    }
}