using System;
using FireworksNet.Model;

namespace FireworksNet.Randomization
{
    public interface IRandom
    {
        Double GetNext(Double from, Double to);
		Double GetNext(Range allowedRange);
    }
}