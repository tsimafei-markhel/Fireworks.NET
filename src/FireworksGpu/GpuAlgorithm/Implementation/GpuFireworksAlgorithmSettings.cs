namespace FireworksGpu.GpuAlgorithm.Implementation
{
    /// <summary>
    /// Stores user-defined constants that control algorithm run.
    /// </summary>
    /// <remarks>Uses original notation from paper</remarks>
    public sealed class GpuFireworksAlgorithmSettings
    {
        /// <summary>
        /// A - Maximum explosion amplitude. 
        /// A recommended take 1. 
        /// </summary>
        public double ExplosionMaximumAmplitude { get; set; }        

        /// <summary>
        /// δ - Constant for scaling factor which is multiplied distance by each dimension. 
        /// δ should take (0; 1)
        /// </summary>
        public double AttractRepulseScalingFactor { set; get; }
    }
}
