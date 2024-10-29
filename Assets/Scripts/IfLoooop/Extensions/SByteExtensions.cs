using System.Runtime.CompilerServices;

namespace IfLoooop.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="sbyte"/>.
    /// </summary>
    public static class SByteExtensions
    {
        #region Methods
        /// <summary>
        /// Determines whether a given sbyte value has a negative sign.
        /// </summary>
        /// <param name="_SByte">The sbyte value to check.</param>
        /// <returns>true if the sbyte value is negative; otherwise, false.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasSign(sbyte _SByte) => !(_SByte >= 0f);
        #endregion
    }
}