using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fireworks
{
    public interface IRandomizer
    {
        double GetNext(double from, double to);
    }
}
