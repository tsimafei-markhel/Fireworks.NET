using System;
using System.Collections.Generic;

namespace FireworksNet.Model
{
    public class FireworksExplosion : ExplosionBase
    {
        public IEnumerable<Firework> Fireworks { get; set; }

        public FireworksExplosion(int stepNumber, int fireworksNumber, IEnumerable<Firework> fireworks)
            : base(stepNumber, new Dictionary<FireworkType, int>() { { FireworkType.ExplosionSpark, fireworksNumber } })
        {
            if (fireworksNumber < 0)
            {
                throw new ArgumentOutOfRangeException("fireworksNumber");
            }

            if (fireworks == null)
            {
                throw new ArgumentNullException("fireworks");
            }

            this.Fireworks = fireworks;
        }
    }
}
