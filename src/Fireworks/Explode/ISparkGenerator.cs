using Fireworks.Model;

namespace Fireworks.Explode
{
	public interface ISparkGenerator
	{
		Firework CreateSparks(Explosion explosion);
	}
}