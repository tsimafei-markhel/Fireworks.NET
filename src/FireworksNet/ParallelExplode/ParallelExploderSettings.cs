using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireworksGpu.GpuExplode
{
    public class GpuExplodeSettings
    {
        /// <summary>
        /// m - Fixed quantity of spark, which generates each firework.
        /// m recommended be 16 or multiple of 16
        /// </summary>
        public int FixedQuantitySparks { set; get; }

        /// <summary>
        /// A - Maximum explosion amplitude. 
        /// A recommended take 1. 
        /// </summary>
        public double Amplitude { get; set; }      
    }
}
