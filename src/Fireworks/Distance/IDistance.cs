using System;
using Fireworks.Model;

namespace Fireworks.Distance
{
	public interface IDistance
	{
		Double Calculate(Double[] first, Double second);
		Double Calculate(Firework first, Firework second);
	}
}