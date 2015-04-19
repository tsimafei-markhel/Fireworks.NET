using System;

namespace FireworksNet.Differentiation
{
    public interface IDifferentiator
    {
        Func<double, double> Differentiate(Func<double, double> func);
    }
}
