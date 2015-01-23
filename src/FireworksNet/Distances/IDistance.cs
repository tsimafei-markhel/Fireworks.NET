using System;
using FireworksNet.Model;

namespace FireworksNet.Distances
{
	public interface IDistance
	{
		Double Calculate(Double[] first, Double[] second);
		Double Calculate(Solution first, Solution second);
		Double Calculate(Solution first, Double[] second);
	}
}