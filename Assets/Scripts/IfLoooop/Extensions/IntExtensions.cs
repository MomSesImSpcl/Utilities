using System.Runtime.CompilerServices;

namespace IfLoooop.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="int"/>.
    /// </summary>
    public static class IntExtensions
    {
        #region Methods
        /// <summary>
        /// Determines whether the specified integer has a negative sign.
        /// </summary>
        /// <param name="_Int">The integer to check for a negative sign.</param>
        /// <returns>
        /// true if the specified integer is negative; otherwise, false.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasSign(int _Int) => !(_Int >= 0f);
        #endregion
    }
}