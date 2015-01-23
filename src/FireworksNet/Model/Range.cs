using System;
using System.Globalization;
using FireworksNet.Extensions;

namespace FireworksNet.Model
{
	/// <summary>
	/// Represents an interval (range).
	/// </summary>
	/// <remarks>Immutable.</remarks>
	public sealed class Range : IEquatable<Range>, IFormattable
	{
		/// <summary>
		/// Stores string format of the <see cref="Range"/> determined
		/// during initialization. It should be used when interval boundaries
		/// have to be formatted.
		/// </summary>
		private String cachedStringFormat;

		/// <summary>
		/// Gets lower boundary of the <see cref="Range"/>.
		/// </summary>
        public double Minimum { get; private set; }

		/// <summary>
		/// Gets upper boundary of the <see cref="Range"/>.
		/// </summary>
        public double Maximum { get; private set; }

		/// <summary>
		/// Gets <see cref="Range"/> length. Always positive.
		/// </summary>
		/// <remarks><see cref="Range.Length"/> = <see cref="Math.Abs"/>(
		/// <see cref="Range.Maximum"/> - <see cref="Range.Minimum"/>).</remarks>
        public double Length { get; private set; }

		/// <summary>
		/// Gets whether a lower boundary of <see cref="Range"/> is open 
		/// (i.e. minimum possible value is exclusive) or closed
		/// (i.e. minimum possible value is inclusive).
		/// </summary>
        public bool IsMinimumOpen { get; private set; }

		/// <summary>
		/// Gets whether an upper boundary of <see cref="Range"/> is open 
		/// (i.e. maximum possible value is exclusive) or closed
		/// (i.e. maximum possible value is inclusive).
		/// </summary>
        public bool IsMaximumOpen { get; private set; }

		/// <summary>
		/// Gets whether <see cref="Range"/> is open. <c>true</c> if both 
		/// <see cref="Range.IsMinimumOpen"/> and <see cref="Range.IsMaximumOpen"/>
		/// are <c>true</c>.
		/// </summary>
        public bool IsOpen { get; private set; }

		/// <summary>
		/// Initializes new instance of <see cref="Range"/>.
		/// </summary>
		/// <param name="minimum">Lower boundary.</param>
		/// <param name="isMinimumOpen">Whether lower boundary is open (exclusive) or not.</param>
		/// <param name="maximum">Upper boundary.</param>
		/// <param name="isMaximumOpen">Whether upper boundary is open (exclusive) or not.</param>
		/// <exception cref="ArgumentOutOfRangeException">If <paramref name="minimum"/> or
        /// <paramref name="maximum"/> is <see cref="double.NaN"/>. If <paramref name="minimum"/>
		/// is greater than <paramref name="maximum"/>.</exception>
        /// <remarks><see cref="double.NegativeInfinity"/> and <see cref="double.PositiveInfinity"/>
		/// boundaries will be always open (exclusive).</remarks>
		public Range(
            double minimum,
            bool isMinimumOpen,
            double maximum,
            bool isMaximumOpen)
		{
			ValidateBoundaries(minimum, maximum);

			Minimum = minimum;
			Maximum = maximum;
			Length = Math.Abs(Maximum - Minimum);
            IsMinimumOpen = double.IsNegativeInfinity(Minimum) ? true : isMinimumOpen;
            IsMaximumOpen = double.IsPositiveInfinity(Maximum) ? true : isMaximumOpen;
			IsOpen = IsMinimumOpen && IsMaximumOpen;

			cachedStringFormat = (IsMinimumOpen ? "(" : "[") + "{1}, {2}" + (IsMaximumOpen ? ")" : "]");
		}

		/// <summary>
		/// Initializes new instance of <see cref="Range"/> which is
		/// closed (inclusive) from both sides.
		/// </summary>
		/// <param name="minimum">Lower boundary.</param>
		/// <param name="maximum">Upper boundary.</param>
		/// <exception cref="ArgumentOutOfRangeException">If <paramref name="minimum"/> or
        /// <paramref name="maximum"/> is <see cref="double.NaN"/>. If <paramref name="minimum"/>
		/// is greater than <paramref name="maximum"/>.</exception>
		public Range(
            double minimum,
            double maximum)
			: this(minimum, false, maximum, false)
		{
		}

		/// <summary>
		/// Checks whether a <paramref name="value"/> belongs to the <see cref="Range"/>.
		/// </summary>
		/// <param name="value">Value that needs to be tested.</param>
		/// <returns><c>true</c> if <paramref name="value"/> belongs to the <see cref="Range"/>;
		/// otherwise <c>false</c>.</returns>
        public bool IsInRange(double value)
		{
			if (value.IsLess(Minimum) || value.IsGreater(Maximum))
			{
				return false;
			}

            if (value.IsGreater(Minimum) && value.IsLess(Maximum))
			{
				return true;
			}

            bool isMinEdge = value.IsEqual(Minimum);
			if (IsMinimumOpen && isMinEdge)
			{
				return false;
			}

            bool isMaxEdge = value.IsEqual(Maximum);
			if (IsMaximumOpen && isMaxEdge)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Checks whether an <paramref name="otherRange"/> belongs to the <see cref="Range"/>.
		/// </summary>
		/// <param name="otherRange"><see cref="Range"/> that needs to be tested.</param>
		/// <returns><c>true</c> if both <see cref="Range.Minimum"/> and <see cref="Range.Maximum"/>
		/// of <paramref name="otherRange"/> belong to the <see cref="Range"/>;
		/// otherwise <c>false</c>.</returns>
        public bool IsInRange(Range otherRange)
		{
			return IsInRange(otherRange.Minimum) && IsInRange(otherRange.Maximum);
		}

		#region ToString() overloads

		/// <summary>
		/// Converts <see cref="Range"/> value to string with desired <paramref name="format"/> 
        /// for <see cref="double"/> to <see cref="String"/> conversion.
		/// </summary>
        /// <param name="format"><see cref="double"/> to <see cref="String"/> format.</param>
		/// <returns>String representation of this <see cref="Range"/> instance. Boundaries
		/// are formatted with <paramref name="format"/>.</returns>
		public String ToString(String format)
		{
			return ToString(format, CultureInfo.CurrentCulture);
		}

		/// <summary>
		/// Formats the value of the current instance using the specified format.
		/// </summary>
		/// <param name="format">The format to use or a null reference to use
		/// the default format.</param>
		/// <param name="formatProvider">The provider to use to format the value or a null reference
		/// to obtain the numeric format information from the current locale
		/// setting of the operating system.</param>
		/// <returns>The value of the current instance in the specified format.</returns>
		public String ToString(String format, IFormatProvider formatProvider)
		{
			return String.Format(cachedStringFormat, Minimum.ToString(format, formatProvider), Maximum.ToString(format, formatProvider));
		}

		/// <summary>
		/// Converts <see cref="Range"/> value to <see cref="String"/> 
		/// with <see cref="CultureInfo.Invariant"/>.
		/// </summary>
		/// <returns>String representation of this <see cref="Range"/> 
		/// instance - for <see cref="CultureInfo.Invariant"/>.</returns>
		public String ToStringInvariant()
		{
			return ToStringInvariant((String)null);
		}

		/// <summary>
		/// Converts <see cref="Range"/> value to string with desired <paramref name="format"/> 
        /// for <see cref="double"/> to <see cref="String"/> conversion and <see cref="CultureInfo.Invariant"/>.
		/// </summary>
        /// <param name="format"><see cref="double"/> to <see cref="String"/> format.</param>
		/// <returns>String representation of this <see cref="Range"/> instance. Boundaries
		/// are formatted with <paramref name="format"/> - for <see cref="CultureInfo.Invariant"/>.</returns>
		public String ToStringInvariant(String format)
		{
			return String.Format(cachedStringFormat, Minimum.ToString(format, CultureInfo.InvariantCulture), Maximum.ToString(format, CultureInfo.InvariantCulture));
		}

		#endregion

		#region Factory methods

		/// <summary>
		/// Creates new instance of <see cref="Invariant"/>. Boundaries are calculated from
		/// <paramref name="mean"/> and <paramref name="deviationValue"/>; ends are closed (inclusive).
		/// </summary>
		/// <param name="mean">Mean (middle) of the interval.</param>
		/// <param name="deviationValue">Desired <see cref="Range.Length"/> / 2.</param>
		/// <returns>New instance of <see cref="Invariant"/>. Its minimum is
		/// <paramref name="mean"/> - <paramref name="deviationValue"/> and its
		/// maximum is <paramref name="mean"/> + <paramref name="deviationValue"/>.</returns>
		/// <exception cref="ArgumentOutOfRangeException">If <paramref name="mean"/> 
        /// or <paramref name="deviationValue"/> is <see cref="double.NaN"/>. If 
		/// <paramref name="mean"/> or <paramref name="deviationValue"/> is 
        /// <see cref="double.NegativeInfinity"/> or <see cref="double.PositiveInfinity"/>.
		/// If <paramref name="deviationValue"/> is less than zero.</exception>
        public static Range Create(double mean, double deviationValue)
		{
			return Create(mean, deviationValue, false, false);
		}

		/// <summary>
		/// Creates new instance of <see cref="Invariant"/>. Boundaries are calculated from
		/// <paramref name="mean"/> and <paramref name="deviationValue"/>.
		/// </summary>
		/// <param name="mean">Mean (middle) of the interval.</param>
		/// <param name="deviationValue">Desired <see cref="Range.Length"/> / 2.</param>
		/// <param name="isMinimumOpen">Whether lower boundary is open (exclusive) or not.</param>
		/// <param name="isMaximumOpen">Whether upper boundary is open (exclusive) or not.</param>
		/// <returns>New instance of <see cref="Invariant"/>. Its minimum is
		/// <paramref name="mean"/> - <paramref name="deviationValue"/> and its
		/// maximum is <paramref name="mean"/> + <paramref name="deviationValue"/>.</returns>
		/// <exception cref="ArgumentOutOfRangeException">If <paramref name="mean"/> or 
        /// <paramref name="deviationValue"/> is <see cref="double.NaN"/>. If 
		/// <paramref name="mean"/> or <paramref name="deviationValue"/> is 
        /// <see cref="double.NegativeInfinity"/> or <see cref="double.PositiveInfinity"/>.
		/// If <paramref name="deviationValue"/> is less than zero.</exception>
        public static Range Create(double mean, double deviationValue, bool isMinimumOpen, bool isMaximumOpen)
		{
			ValidateMean(mean);
			ValidateDeviationValue(deviationValue);

            double min = mean - deviationValue;
            double max = mean + deviationValue;

			return new Range(min, isMinimumOpen, max, isMaximumOpen);
		}

		/// <summary>
		/// Creates new instance of <see cref="Invariant"/>. Boundaries are calculated from
		/// <paramref name="mean"/> and <paramref name="deviationPercent"/>; ends are closed (inclusive).
		/// </summary>
		/// <param name="mean">Mean (middle) of the interval.</param>
		/// <param name="deviationPercent">Desired distance between <paramref name="mean"/>
		/// and resulting <see cref="Range.Minimum"/> (or <see cref="Range.Minimum"/>)
		/// expressed in percents of <paramref name="mean"/>.</param>
		/// <returns>New instance of <see cref="Invariant"/> which is <paramref name="mean"/>
		/// +- <paramref name="deviationPercent"/> * <paramref name="mean"/>.</returns>
		/// <exception cref="ArgumentOutOfRangeException">If <paramref name="mean"/> is 
        /// <see cref="double.NaN"/>. If <paramref name="mean"/> is <see cref="double.NegativeInfinity"/>
        /// or <see cref="double.PositiveInfinity"/>. If <paramref name="deviationPercent"/> is less than zero.
		/// </exception>
        public static Range Create(double mean, int deviationPercent)
		{
			return Create(mean, deviationPercent, false, false);
		}

		/// <summary>
		/// Creates new instance of <see cref="Invariant"/>. Boundaries are calculated from
		/// <paramref name="mean"/> and <paramref name="deviationPercent"/>.
		/// </summary>
		/// <param name="mean">Mean (middle) of the interval.</param>
		/// <param name="deviationPercent">Desired distance between <paramref name="mean"/>
		/// and resulting <see cref="Range.Minimum"/> (or <see cref="Range.Minimum"/>)
		/// expressed in percents of <paramref name="mean"/>.</param>
		/// <param name="isMinimumOpen">Whether lower boundary is open (exclusive) or not.</param>
		/// <param name="isMaximumOpen">Whether upper boundary is open (exclusive) or not.</param>
		/// <returns>New instance of <see cref="Invariant"/> which is <paramref name="mean"/>
		/// +- <paramref name="deviationPercent"/> * <paramref name="mean"/>.</returns>
        /// <exception cref="ArgumentException">If <paramref name="mean"/> is <see cref="double.NaN"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException">If <paramref name="mean"/> is
        /// <see cref="double.NegativeInfinity"/> or <see cref="double.PositiveInfinity"/>.
		/// If <paramref name="deviationPercent"/> is less than zero.</exception>
        public static Range Create(double mean, int deviationPercent, bool isMinimumOpen, bool isMaximumOpen)
		{
			ValidateMean(mean);
			ValidateDeviationPercent(deviationPercent);

            double min = double.NaN;
            double max = double.NaN;
            double deviationValue = deviationPercent / 100.0;

            if (mean.IsGreaterOrEqual(0.0))
			{
				min = mean - deviationValue * mean;
				max = mean + deviationValue * mean;
			}
			else
			{
				min = mean + deviationValue * mean;
				max = mean - deviationValue * mean;
			}

			return new Range(min, isMinimumOpen, max, isMaximumOpen);
		}

		/// <summary>
		/// Creates new instance of <see cref="Invariant"/>. Boundaries are calculated from
		/// <paramref name="mean"/> and <paramref name="deviationValue"/> but never exceed
		/// the restrictions; ends are closed (inclusive).
		/// </summary>
		/// <param name="mean">Mean (middle) of the interval.</param>
		/// <param name="deviationValue">Desired <see cref="Range.Length"/> / 2.</param>
		/// <param name="minRestriction">Lower boundary restriction. <see cref="Range.Minimum"/>
		/// of the resulting <see cref="Range"/> instance will not be less than this value.</param>
		/// <param name="maxRestriction">Upper boundary restriction. <see cref="Range.Maximum"/>
		/// of the resulting <see cref="Range"/> instance will not be greater than this value.</param>
		/// <returns>New instance of <see cref="Invariant"/>. Its minimum is
		/// MAX(<paramref name="mean"/> - <paramref name="deviationValue"/>; <paramref name="minRestriction"/>)
		/// and its maximum is MIN(<paramref name="mean"/> + <paramref name="deviationValue"/>;
		/// <paramref name="maxRestriction"/>).</returns>
		/// <exception cref="ArgumentOutOfRangeException">If <paramref name="mean"/> or 
		/// <paramref name="deviationValue"/> or <paramref name="minRestriction"/> or 
        /// <paramref name="maxRestriction"/> is <see cref="double.NaN"/>. If 
		/// <paramref name="mean"/> or <paramref name="deviationValue"/> is 
        /// <see cref="double.NegativeInfinity"/> or <see cref="double.PositiveInfinity"/>.
		/// If <paramref name="deviationValue"/> is less than zero.
		/// If <paramref name="minRestriction"/> is greater than <paramref name="maxRestriction"/>.
		/// </exception>
		public static Range CreateWithRestrictions(
            double mean,
            double deviationValue,
            double minRestriction,
            double maxRestriction)
		{
			return CreateWithRestrictions(mean, deviationValue, minRestriction, maxRestriction, false, false);
		}

		/// <summary>
		/// Creates new instance of <see cref="Invariant"/>. Boundaries are calculated from
		/// <paramref name="mean"/> and <paramref name="deviationValue"/> but never exceed
		/// the restrictions.
		/// </summary>
		/// <param name="mean">Mean (middle) of the interval.</param>
		/// <param name="deviationValue">Desired <see cref="Range.Length"/> / 2.</param>
		/// <param name="minRestriction">Lower boundary restriction. <see cref="Range.Minimum"/>
		/// of the resulting <see cref="Range"/> instance will not be less than this value.</param>
		/// <param name="maxRestriction">Upper boundary restriction. <see cref="Range.Maximum"/>
		/// of the resulting <see cref="Range"/> instance will not be greater than this value.</param>
		/// <param name="isMinimumOpen">Whether lower boundary is open (exclusive) or not.</param>
		/// <param name="isMaximumOpen">Whether upper boundary is open (exclusive) or not.</param>
		/// <returns>New instance of <see cref="Invariant"/>. Its minimum is
		/// MAX(<paramref name="mean"/> - <paramref name="deviationValue"/>; <paramref name="minRestriction"/>)
		/// and its maximum is MIN(<paramref name="mean"/> + <paramref name="deviationValue"/>;
		/// <paramref name="maxRestriction"/>).</returns>
		/// <exception cref="ArgumentOutOfRangeException">If <paramref name="mean"/> or 
		/// <paramref name="deviationValue"/> or <paramref name="minRestriction"/> or 
        /// <paramref name="maxRestriction"/> is <see cref="double.NaN"/>. If <paramref name="mean"/>
        /// or <paramref name="deviationValue"/> is <see cref="double.NegativeInfinity"/> or
        /// <see cref="double.PositiveInfinity"/>. If <paramref name="deviationValue"/> is
		/// less than zero. If <paramref name="minRestriction"/> is greater than
		/// <paramref name="maxRestriction"/>.</exception>
		public static Range CreateWithRestrictions(
            double mean,
            double deviationValue,
            double minRestriction,
            double maxRestriction,
            bool isMinimumOpen,
            bool isMaximumOpen)
		{
			ValidateMean(mean);
			ValidateDeviationValue(deviationValue);
			ValidateBoundaries(minRestriction, maxRestriction);

            double min = mean - deviationValue;
            if (min.IsLess(minRestriction))
			{
				min = minRestriction;
			}

            double max = mean + deviationValue;
            if (max.IsGreater(maxRestriction))
			{
				max = maxRestriction;
			}

			return new Range(min, isMinimumOpen, max, isMaximumOpen);
		}

		/// <summary>
		/// Creates new instance of <see cref="Invariant"/>. Boundaries are calculated from
		/// <paramref name="mean"/> and <paramref name="deviationPercent"/> but never exceed
		/// the restrictions.
		/// </summary>
		/// <param name="mean">Mean (middle) of the interval.</param>
		/// <param name="deviationPercent">Desired distance between <paramref name="mean"/>
		/// and resulting <see cref="Range.Minimum"/> (or <see cref="Range.Minimum"/>)
		/// expressed in percents of <paramref name="mean"/>.</param>
		/// <param name="minRestriction">Lower boundary restriction. <see cref="Range.Minimum"/>
		/// of the resulting <see cref="Range"/> instance will not be less than this value.</param>
		/// <param name="maxRestriction">Upper boundary restriction. <see cref="Range.Maximum"/>
		/// of the resulting <see cref="Range"/> instance will not be greater than this value.</param>
		/// <returns>New instance of <see cref="Invariant"/> which is <paramref name="mean"/>
		/// +- <paramref name="deviationPercent"/> * <paramref name="mean"/>. Restrictions
		/// are applied and can replace <see cref="Range.Minimum"/> and <see cref="Range.Maximum"/>
		/// in the result.</returns>
		/// <exception cref="ArgumentOutOfRangeException">If <paramref name="mean"/> or 
		/// <paramref name="minRestriction"/> or <paramref name="maxRestriction"/> is 
        /// <see cref="double.NaN"/>. If <paramref name="mean"/> is <see cref="double.NegativeInfinity"/>
        /// or <see cref="double.PositiveInfinity"/>. If <paramref name="deviationPercent"/> is less than 
		/// zero. If <paramref name="minRestriction"/> is greater than <paramref name="maxRestriction"/>.
		/// </exception>
		public static Range CreateWithRestrictions(
            double mean,
            int deviationPercent,
            double minRestriction,
            double maxRestriction)
		{
			return CreateWithRestrictions(mean, deviationPercent, minRestriction, maxRestriction, false, false);
		}

		/// <summary>
		/// Creates new instance of <see cref="Invariant"/>. Boundaries are calculated from
		/// <paramref name="mean"/> and <paramref name="deviationPercent"/> but never exceed
		/// the restrictions.
		/// </summary>
		/// <param name="mean">Mean (middle) of the interval.</param>
		/// <param name="deviationPercent">Desired distance between <paramref name="mean"/>
		/// and resulting <see cref="Range.Minimum"/> (or <see cref="Range.Minimum"/>)
		/// expressed in percents of <paramref name="mean"/>.</param>
		/// <param name="minRestriction">Lower boundary restriction. <see cref="Range.Minimum"/>
		/// of the resulting <see cref="Range"/> instance will not be less than this value.</param>
		/// <param name="maxRestriction">Upper boundary restriction. <see cref="Range.Maximum"/>
		/// of the resulting <see cref="Range"/> instance will not be greater than this value.</param>
		/// <param name="isMinimumOpen">Whether lower boundary is open (exclusive) or not.</param>
		/// <param name="isMaximumOpen">Whether upper boundary is open (exclusive) or not.</param>
		/// <returns>New instance of <see cref="Invariant"/> which is <paramref name="mean"/>
		/// +- <paramref name="deviationPercent"/> * <paramref name="mean"/>. Restrictions
		/// are applied and can replace <see cref="Range.Minimum"/> and <see cref="Range.Maximum"/>
		/// in the result.</returns>
		/// <exception cref="ArgumentOutOfRangeException">If <paramref name="mean"/> or 
		/// <paramref name="minRestriction"/> or <paramref name="maxRestriction"/> is 
        /// <see cref="double.NaN"/>. If <paramref name="mean"/> is <see cref="double.NegativeInfinity"/>
        /// or <see cref="double.PositiveInfinity"/>. If <paramref name="deviationPercent"/> 
		/// is less than zero. If <paramref name="minRestriction"/> is greater than <paramref name="maxRestriction"/>.
		/// </exception>
		public static Range CreateWithRestrictions(
            double mean,
            int deviationPercent,
            double minRestriction,
            double maxRestriction,
            bool isMinimumOpen,
            bool isMaximumOpen)
		{
			ValidateMean(mean);
			ValidateDeviationPercent(deviationPercent);
			ValidateBoundaries(minRestriction, maxRestriction);

            double min = double.NaN;
            double max = double.NaN;
            double deviationValue = deviationPercent / 100.0;

            if (mean.IsGreaterOrEqual(0.0))
			{
				min = mean - deviationValue * mean;
				max = mean + deviationValue * mean;
			}
			else
			{
				min = mean + deviationValue * mean;
				max = mean - deviationValue * mean;
			}

			if (min.IsLess(minRestriction))
			{
				min = minRestriction;
			}

            if (max.IsGreater(maxRestriction))
			{
				max = maxRestriction;
			}

			return new Range(min, isMinimumOpen, max, isMaximumOpen);
		}

		#endregion

		#region Validation

		/// <summary>
		/// Validates mean (middle) value of the <see cref="Range"/>.
		/// </summary>
		/// <param name="mean">Mean (middle) value of the <see cref="Range"/>
		/// that needs to be validated.</param>
		/// <exception cref="ArgumentOutOfRangeException">If <paramref name="mean"/> is
        /// <see cref="double.NaN"/>. If <paramref name="mean"/> is 
        /// <see cref="double.NegativeInfinity"/> or <see cref="double.PositiveInfinity"/>.
		/// </exception>
        private static void ValidateMean(double mean)
		{
            if (double.IsNaN(mean))
			{
				throw new ArgumentOutOfRangeException("mean");
			}

            if (double.IsInfinity(mean))
			{
				throw new ArgumentOutOfRangeException("mean");
			}
		}

		/// <summary>
		/// Validates deviation value of the <see cref="Range"/>.
		/// </summary>
		/// <param name="deviationValue">Deviation value of the <see cref="Range"/>
		/// that needs to be validated.</param>
		/// <exception cref="ArgumentOutOfRangeException">If <paramref name="deviationValue"/>
        /// is <see cref="double.NaN"/>. If <paramref name="deviationValue"/>
        /// is <see cref="double.NegativeInfinity"/> or <see cref="double.PositiveInfinity"/>
		/// or is less than zero.</exception>
        private static void ValidateDeviationValue(double deviationValue)
		{
            if (double.IsNaN(deviationValue))
			{
				throw new ArgumentOutOfRangeException("deviationValue");
			}

            if (double.IsInfinity(deviationValue))
			{
				throw new ArgumentOutOfRangeException("deviationValue");
			}

            if (deviationValue.IsLess(0.0))
			{
				throw new ArgumentOutOfRangeException("deviationValue");
			}
		}

		/// <summary>
		/// Validates deviation percent of the <see cref="Range"/>.
		/// </summary>
		/// <param name="deviationPercent">Deviation percent of the <see cref="Range"/>
		/// that needs to be validated.</param>
		/// <exception cref="ArgumentOutOfRangeException">If <paramref name="deviationPercent"/>
		/// is less than zero.</exception>
        private static void ValidateDeviationPercent(int deviationPercent)
		{
			if (deviationPercent < 0)
			{
				throw new ArgumentOutOfRangeException("deviationPercent");
			}
		}

		/// <summary>
		/// Validates lower and upper boundaries of the <see cref="Range"/>.
		/// </summary>
		/// <param name="minimum">Lower boundary of the <see cref="Range"/>
		/// that needs to be validated.</param>
		/// <param name="maximum">Upper boundary of the <see cref="Range"/>
		/// that needs to be validated.</param>
		/// <exception cref="ArgumentOutOfRangeException">If <paramref name="minimum"/>
        /// or <paramref name="maximum"/> is <see cref="double.NaN"/>. If 
		/// <paramref name="minimum"/> is greater than <paramref name="maximum"/>.
		/// </exception>
        private static void ValidateBoundaries(double minimum, double maximum)
		{
            if (double.IsNaN(minimum))
			{
				throw new ArgumentOutOfRangeException("minimum");
			}

            if (double.IsNaN(maximum))
			{
				throw new ArgumentOutOfRangeException("maximum");
			}

            if (minimum.IsGreater(maximum))
			{
				throw new ArgumentOutOfRangeException("minimum");
			}
		}

		#endregion

		#region Object overrides

		/// <summary>
		/// Determines whether the specified <see cref="Range"/> is equal to the current one.
		/// </summary>
		/// <param name="obj">The <see cref="Range"/> object to compare with the current one.</param>
		/// <returns><c>true</c> if the specified <see cref="Range"/> is equal to the current one;
		/// otherwise <c>false</c>.</returns>
        public override bool Equals(object obj)
		{
            return Equals(obj as Range);
		}

		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>A hash code for the current <see cref="Range"/>.</returns>
        public override int GetHashCode()
		{
			// http://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode
			unchecked
			{
                int hash = 17;
				hash = hash * 29 + Minimum.GetHashCode();
				hash = hash * 29 + Maximum.GetHashCode();
				hash = hash * 29 + IsMinimumOpen.GetHashCode();
				hash = hash * 29 + IsMaximumOpen.GetHashCode();

				return hash;
			}
		}

		/// <summary>
		/// Returns a string that represents the current <see cref="Range"/> instance.
		/// </summary>
		/// <returns>A string that represents the current <see cref="Range"/> instance.</returns>
		public override String ToString()
		{
			return ToString((String)null, CultureInfo.CurrentCulture);
		}

		#endregion

		#region Comparison operators

		/// <summary>
		/// Determines whether values of two instances of <see cref="Range"/> are equal.
		/// </summary>
		/// <param name="left">First instance of <see cref="Range"/>.</param>
		/// <param name="right">Second instance of <see cref="Range"/>.</param>
		/// <returns><c>true</c> if <paramref name="left"/> value is equal to the <paramref name="right"/> 
		/// value; otherwise <c>false</c>.</returns>
        public static bool operator ==(Range left, Range right)
		{
            if (object.ReferenceEquals((object)left, (object)null))
            {
                return object.ReferenceEquals((object)right, (object)null);
            }

            return left.Equals(right);
		}

		/// <summary>
		/// Determines whether values of two instances of <see cref="Range"/> are not equal.
		/// </summary>
		/// <param name="left">First instance of <see cref="Range"/>.</param>
		/// <param name="right">Second instance of <see cref="Range"/>.</param>
		/// <returns><c>true</c> if <paramref name="left"/> value is not equal to the <paramref name="right"/> 
		/// value; otherwise <c>false</c>.</returns>
        public static bool operator !=(Range left, Range right)
		{
            if (object.ReferenceEquals((object)left, (object)null))
            {
                return !object.ReferenceEquals((object)right, (object)null);
            }

            return !left.Equals(right);
		}

		#endregion

		#region IEquatable<Range>

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns><c>true</c> if the current object is equal to the other parameter; otherwise <c>false</c>.</returns>
        public bool Equals(Range other)
		{
            if (object.ReferenceEquals((object)other, (object)null))
            {
                return false;
            }

            if (object.ReferenceEquals((object)other, (object)this))
            {
                return true;
            }

            return (Minimum.IsEqual(other.Minimum)) &&
                   (Maximum.IsEqual(other.Maximum)) &&
                   (IsMinimumOpen == other.IsMinimumOpen) &&
                   (IsMaximumOpen == other.IsMaximumOpen);
		}

		#endregion
	}
}