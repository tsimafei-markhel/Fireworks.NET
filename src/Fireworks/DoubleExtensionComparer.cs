using System;
using System.Collections.Generic;
using Fireworks.Extensions;

namespace Fireworks
{
	public class DoubleExtensionComparer : IComparer<Double>
	{
		public Int32 Compare(Double x, Double y)
		{
			if (x.IsLess(y))
			{
				return -1;
			}
			else if (x.IsGreater(y))
			{
				return 1;
			}

			return 0;
		}
	}
}