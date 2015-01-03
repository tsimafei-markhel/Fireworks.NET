using System;
using FireworksNet.Model;

namespace FireworksNet.Distances
{
	public interface IDistance
	{
		Double Calculate(Double[] first, Double[] second);
		Double Calculate(Firework first, Firework second);
        Double Calculate(Firework first, Double[] second);
	}
}