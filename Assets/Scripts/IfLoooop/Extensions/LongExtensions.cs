using System.Runtime.CompilerServices;

namespace IfLoooop.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="long"/>.
    /// </summary>
    public static class LongExtensions
    {
        #region Methods
        /// <summary>
        /// Determines whether the specified long integer has a sign (is negative).
        /// </summary>
        /// <param name="_Long">The long integer to check for a sign.</param>
        /// <returns>
        /// <see langword="true"/> if the specified long integer is negative; otherwise, <see langword="false"/>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasSign(long _Long) => !(_Long >= 0f);
        #endregion
    }
}