namespace FireworksNet.ParallelExplode
{
    /// <summary>
    /// Firework mutator interface
    /// </summary>
    public interface IFireworkMutator
    {
        void MutateFirework(ref FireworksNet.Model.Firework bestFirework, FireworksNet.Model.FireworkExplosion explosion);
    }
}
