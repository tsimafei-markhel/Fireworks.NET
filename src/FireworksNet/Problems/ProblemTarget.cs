
namespace FireworksNet.Problems
{
    /// <summary>
    /// Denotes what we are looking for: a minimum or a maximum
    /// of the function under investigation.
    /// </summary>
    public enum ProblemTarget
    {
        /// <summary>
        /// Target function has to be minimized.
        /// </summary>
        Minimum = 0,

        /// <summary>
        /// Target function has to be maximized.
        /// </summary>
        Maximum
    }
}