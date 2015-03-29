namespace FireworksNet.Explode
{
    /// <summary>
    /// Stores user-defined constants that control algorithm run.
    /// </summary>
    /// <remarks>Uses original notation from paper</remarks>
    public class ParallelExploderSettings
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
