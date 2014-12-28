using System;
using System.Globalization;
using MathNet.Numerics;

namespace FireworksNet.Extensions
{
    /// <summary>
    /// Contains helper extension methods for <see cref="Double"/>.
    /// </summary>
    public static class DoubleExtensions
    {
        /// <summary>
        /// Compares two <see cref="Double"/> and determines if they are equal within the
        /// <see cref="Double.Epsilon"/> error.
        /// </summary>
        /// <param name="left">The first value.</param>
        /// <param name="right">The second value.</param>
        /// <returns>True if <paramref name="left"/> is equal to <paramref name="right"/>
        /// within the <see cref="Double.Epsilon"/> error.</returns>
        public static Boolean IsEqual(this Double left, Double right)
        {
            return left.AlmostEqual(right, Double.Epsilon);
        }

        /// <summary>
        /// Compares two <see cref="Double"/> and determines if <paramref name="left"/>
        /// is smaller than <paramref name="right"/> within the <see cref="Double.Epsilon"/>
        /// error.
        /// </summary>
        /// <param name="left">The first value.</param>
        /// <param name="right">The second value.</param>
        /// <returns>True if <paramref name="left"/> is smaller than <paramref name="right"/>
        /// within the <see cref="Double.Epsilon"/> error.</returns>
        public static Boolean IsLess(this Double left, Double right)
        {
            return left.IsSmaller(right, Double.Epsilon);
        }

        /// <summary>
        /// Compares two <see cref="Double"/> and determines if <paramref name="left"/>
        /// is smaller than or equal to <paramref name="right"/> within the 
        /// <see cref="Double.Epsilon"/> error.
        /// </summary>
        /// <param name="left">The first value.</param>
        /// <param name="right">The second value.</param>
        /// <returns>True if <paramref name="left"/> is smaller than or equal to
        /// <paramref name="right"/> within the <see cref="Double.Epsilon"/> error.
        /// </returns>
        public static Boolean IsLessOrEqual(this Double left, Double right)
        {
            return left.IsSmaller(right, Double.Epsilon) || left.AlmostEqual(right, Double.Epsilon);
        }

        /// <summary>
        /// Compares two <see cref="Double"/> and determines if <paramref name="left"/>
        /// is greater than <paramref name="right"/> within the <see cref="Double.Epsilon"/>
        /// error.
        /// </summary>
        /// <param name="left">The first value.</param>
        /// <param name="right">The second value.</param>
        /// <returns>True if <paramref name="left"/> is greater than <paramref name="right"/>
        /// within the <see cref="Double.Epsilon"/> error.</returns>
        public static Boolean IsGreater(this Double left, Double right)
        {
            return left.IsLarger(right, Double.Epsilon);
        }

        /// <summary>
        /// Compares two <see cref="Double"/> and determines if <paramref name="left"/>
        /// is greater than or equal to <paramref name="right"/> within the 
        /// <see cref="Double.Epsilon"/> error.
        /// </summary>
        /// <param name="left">The first value.</param>
        /// <param name="right">The second value.</param>
        /// <returns>True if <paramref name="left"/> is greater than or equal to
        /// <paramref name="right"/> within the <see cref="Double.Epsilon"/> error.
        /// </returns>
        public static Boolean IsGreaterOrEqual(this Double left, Double right)
        {
            return left.IsLarger(right, Double.Epsilon) || left.AlmostEqual(right, Double.Epsilon);
        }

        /// <summary>
        /// Converts the numeric value of this instance to its equivalent string representation
        /// using the <see cref="CultureInfo.InvariantCulture"/> format information.
        /// </summary>
        /// <param name="value">Numeric value to be converted.</param>
        /// <returns>The string representation of the value of this instance as specified by provider.</returns>
        public static String ToStringInvariant(this Double value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }
    }
}