using System.Diagnostics.CodeAnalysis;

namespace MomSesImSpcl.Utilities
{
    /// <summary>
    /// Categories for selecting optimized prime numbers for the given use case.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum PrimeNumberUseCase
    {
        /// <summary>
        /// Used to mix multiple values into a single seed or hash with minimal collision; ideal for combining turn, player ID, etc.
        /// </summary>
        BitMixing,
        /// <summary>
        /// Used to generate deterministic seeds for random number generators based on consistent game state values.
        /// </summary>
        DeterministicRandom,
        /// <summary>
        /// Used in general-purpose hashing algorithms like FNV-1a or MurmurHash to spread input values evenly.
        /// </summary>
        Hashing,
        /// <summary>
        /// Used to generate minimal or perfect hash maps, ensuring unique, collision-free keys for known sets.
        /// </summary>
        PerfectHashing,
        /// <summary>
        /// Used inside pseudorandom number generator math (e.g., LCGs or Lehmer RNGs) as modulus or multiplier constants.
        /// </summary>
        PRNG,
        /// <summary>
        /// Used to seed or structure noise functions, terrain generators, and other procedural content systems.
        /// </summary>
        ProceduralGeneration,
        /// <summary>
        /// Used to hash 2D or 3D spatial coordinates into a single index value with low collision (e.g., in voxel engines).
        /// </summary>
        SpatialHashing,
        /// <summary>
        /// Used to create unique, consistent identifiers from structured data like usernames, object compositions, etc.
        /// </summary>
        UniqueIdGeneration,
    }
}