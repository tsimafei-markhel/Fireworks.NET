using System;
using Fireworks.Model;

namespace Fireworks.Randomization
{
    public interface IRandom
    {
        Double GetNext(Double from, Double to);
		Double GetNext(Range allowedRange);
    }
}