using Fireworks.Model;

namespace Fireworks.Explode
{
    public interface IExploder
    {
        Explosion Explode(Firework epicenter);
    }
}