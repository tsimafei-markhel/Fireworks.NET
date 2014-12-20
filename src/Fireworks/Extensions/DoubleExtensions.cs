using System;
using System.Globalization;
using MathNet.Numerics;

namespace Fireworks.Extensions
{
    public static class DoubleExtensions
    {
        public static Boolean IsEqual(this Double left, Double right)
        {
            return left.AlmostEqual(right, Double.Epsilon);
        }

        public static Boolean IsSmaller(this Double left, Double right)
        {
            return left.IsSmaller(right, Double.Epsilon);
        }

        public static Boolean IsSmallerOrEqual(this Double left, Double right)
        {
            return left.IsSmaller(right, Double.Epsilon) || left.AlmostEqual(right, Double.Epsilon);
        }

        public static Boolean IsLarger(this Double left, Double right)
        {
            return left.IsLarger(right, Double.Epsilon);
        }

        public static Boolean IsLargerOrEqual(this Double left, Double right)
        {
            return left.IsLarger(right, Double.Epsilon) || left.AlmostEqual(right, Double.Epsilon);
        }

        public static String ToStringInvariant(this Double value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }
    }
}