using System.Collections.Generic;
using Fireworks.Model;

namespace Fireworks.Explode
{
	// Naming convention for implementations of this interface:
	// - conventional (i.e. per 2010 paper) implementations are named w/o
	//   any prefixes (e.g. ExplosionSparkGenerator)
	// - enhanced/alternative implementations are named with corresponding
	//   prefix (e.g. TODO)
	public interface ISparkGenerator
	{
		IEnumerable<Firework> CreateSparks(Explosion explosion);
        Firework CreateSpark(Explosion explosion);
	}
}