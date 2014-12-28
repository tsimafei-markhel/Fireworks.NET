using FireworksNet.Model;

namespace FireworksNet.Explode
{
    public interface IExploder
    {
        Explosion Explode(Firework epicenter);
    }
}