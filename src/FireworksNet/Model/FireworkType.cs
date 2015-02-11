
namespace FireworksNet.Model
{
    /// <summary>
    /// Represents a firework type (i.e. how it was produced).
    /// </summary>
    public enum FireworkType
    {
        /// <summary>
        /// The initial firework.
        /// </summary>
        Initial = 0,

        /// <summary>
        /// Firework that is the explosion spark.
        /// </summary>
        ExplosionSpark,

        /// <summary>
        /// Firework that is the specific spark.
        /// </summary>
        SpecificSpark
    }
}