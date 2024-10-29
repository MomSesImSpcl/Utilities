using System.Runtime.CompilerServices;

namespace IfLoooop.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="short"/>.
    /// </summary>
    public static class ShortExtensions
    {
        #region Methods
        /// <summary>
        /// Determines whether the specified short integer has a negative sign.
        /// </summary>
        /// <param name="_Short">The short integer to be evaluated.</param>
        /// <returns>true if the short integer is negative; otherwise, false.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasSign(short _Short) => !(_Short >= 0f);
        #endregion
    }
}