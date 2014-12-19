using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fireworks
{
    public interface IRandom
    {
        double GetNext(double from, double to);
    }
}
