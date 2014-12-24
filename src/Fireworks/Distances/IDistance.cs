using System;
using Fireworks.Model;

namespace Fireworks.Distances
{
	public interface IDistance
	{
		Double Calculate(Double[] first, Double second);
		Double Calculate(Firework first, Firework second);
	}
}