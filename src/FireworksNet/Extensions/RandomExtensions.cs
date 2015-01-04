using System;
using FireworksNet.Model;

namespace FireworksNet.Extensions
{
    public static class RandomExtensions
    {
        public static Double NextDouble(this System.Random random, Double from, Double to)
        {
            return NextDoubleInternal(random, from, to - from);
        }

        public static Double NextDouble(this System.Random random, Range allowedRange)
        {
            bool gotCorrectValue = false;
            double correctValue;
            do
            {
                correctValue = NextDoubleInternal(random, allowedRange.Minimum, allowedRange.Length);
                gotCorrectValue = !((allowedRange.IsMinimumOpen && allowedRange.Minimum.IsEqual(correctValue))
                                 || (allowedRange.IsMaximumOpen && allowedRange.Maximum.IsEqual(correctValue)));
            }
            while (!gotCorrectValue);

            return correctValue;
        }

        private static Double NextDoubleInternal(System.Random random, Double from, Double intervalLength)
        {
            return from + random.NextDouble() * intervalLength;
        }
    }
}