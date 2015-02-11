using System;
using System.Collections.Generic;

namespace FireworksNet.Model
{
    public class FireworkExplosion : Explosion
    {
        public Firework ParentFirework { get; private set; }

        public double Amplitude { get; private set; }

        public FireworkExplosion(Firework parentFirework, int stepNumber, double amplitude, IDictionary<FireworkType, int> sparkCounts)
            : base(stepNumber, sparkCounts)
        {
            if (parentFirework == null)
            {
                throw new ArgumentNullException("parentFirework");
            }

            if (double.IsNaN(amplitude) || double.IsInfinity(amplitude))
            {
                throw new ArgumentOutOfRangeException("amplitude");
            }

            this.ParentFirework = parentFirework;
            this.Amplitude = amplitude;
        }
    }
}