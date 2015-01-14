using System;

namespace FireworksNet.Model
{
	/// <summary>
	/// Represents a continuous variable that can take any <see cref="Double"/> value
	/// within specified <see cref="Range"/>.
	/// </summary>
	/// <remarks>Immutable.</remarks>
	public class Dimension
	{
		/// <summary>
		/// Gets <see cref="Range"/> instance identifying minimal and maximal possible
		/// values for this dimension. Range ends are always closed.
		/// </summary>
		public Range VariationRange { get; private set; }

		/// <summary>
		/// Initializes new instance of <see cref="Dimension"/>.
		/// </summary>
		/// <param name="variationRange"><see cref="Range"/> instance that determines minimal and
		/// maximal possible values for this dimension.</param>
		public Dimension(Range variationRange)
		{
			VariationRange = variationRange;
		}

		/// <summary>
		/// Checks if a <paramref name="valueToCheck"/> is valid dimension coordinate.
		/// </summary>
		/// <param name="valueToCheck">Value to be checked.</param>
		/// <returns><c>true</c> if <paramref name="valueToCheck"/> can be used as a value
		/// of this parameter; otherwise <c>false</c>.</returns>
		public Boolean IsValueInBounds(Double valueToCheck)
		{
			return VariationRange.IsInRange(valueToCheck);
		}
	}
}