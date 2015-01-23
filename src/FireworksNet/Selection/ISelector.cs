using System;
using System.Collections.Generic;
using FireworksNet.Model;

namespace FireworksNet.Selection
{
	public interface ISelector
	{
		IEnumerable<Firework> Select(IEnumerable<Firework> from);
        IEnumerable<Firework> Select(IEnumerable<Firework> from, int numberToSelect);
	}
}