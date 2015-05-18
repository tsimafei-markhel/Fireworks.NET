using FireworksNet.Selection;

namespace FireworksNet.Algorithm.Implementation
{
    /// <summary>
    /// Stores user-defined constants that control algorithm run.
    /// </summary>
    /// <remarks>Uses notation described in 2012 paper.</remarks>
    public sealed class FireworksAlgorithmSettings2012 : FireworksAlgorithmSettings
    {       
        /// <summary>
        /// Order of polynomial function.
        /// </summary>
        public int FunctionOrder { get; set; }

        /// <summary>
        /// The number of sparks to select.
        /// </summary>
        public int SamplingNumber { get; set; }
    }
}
