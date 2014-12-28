using System;
using FireworksNet.Extensions;
using FireworksNet.Model;

namespace FireworksNet.Randomization
{
    public class DefaultRandom : IRandom
    {
        private readonly Random randomizer = new Random();

        public Double GetNext(Double from, Double to)
        {
            return GetNextInternal(from, to - from);
        }

        public Double GetNext(Range allowedRange)
        {
            bool gotCorrectValue = false;
            double correctValue;
            do
            {
                correctValue = GetNextInternal(allowedRange.Minimum, allowedRange.Length);
                gotCorrectValue = !((allowedRange.IsMinimumOpen && allowedRange.Minimum.IsEqual(correctValue))
                                 || (allowedRange.IsMaximumOpen && allowedRange.Maximum.IsEqual(correctValue)));
            }
            while (!gotCorrectValue);

            return correctValue;
        }

        private Double GetNextInternal(Double from, Double intervalLength)
        {
            return from + randomizer.NextDouble() * intervalLength;
        }
    }
}