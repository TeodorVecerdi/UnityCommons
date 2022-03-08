namespace UnityCommons {
    /// <summary>
    /// Represents a random number generator.
    /// </summary>
    public interface IRNGProvider {
        /// <summary>
        /// Seed used to generate values
        /// </summary>
        public uint Seed { get; set; }
        /// <summary>
        /// Returns a random integer based on the seed and <paramref name="iterations"/>.
        /// </summary>
        int GetInt(uint iterations);

        /// <summary>
        /// Returns a random float between 0 and 1 based on the seed and <paramref name="iterations"/>.
        /// </summary>
        /// <param name="iterations"></param>
        /// <returns></returns>
        float GetFloat(uint iterations);
    }
}