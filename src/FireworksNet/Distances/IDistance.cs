using System;
using FireworksNet.Model;

namespace FireworksNet.Distances
{
	public interface IDistance
	{
        double Calculate(double[] first, double[] second);
        double Calculate(Solution first, Solution second);
        double Calculate(Solution first, double[] second);
	}
}