using System.Runtime.CompilerServices;
using MomSesImSpcl.Utilities;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="long"/>.
    /// </summary>
    public static class LongExtensions
    {
        #region Methods
        /// <summary>
        /// Creates a new <see cref="NumericEnumerator"/> to iterate over a sequence of <see cref="long"/>, starting at <c>0</c> and ending at <c>_End</c>.
        /// </summary>
        /// <param name="_End"><see cref="NumericEnumerator.end"/>.</param>
        /// <returns>A new <see cref="NumericEnumerator"/>.</returns>
        public static NumericEnumerator GetEnumerator(this long _End)
        {
            return new NumericEnumerator(_End);
        }
        
        /// <summary>
        /// Determines whether the specified long integer has a sign (is negative).
        /// </summary>
        /// <param name="_Long">The long integer to check for a sign.</param>
        /// <returns>
        /// <see langword="true"/> if the specified long integer is negative; otherwise, <see langword="false"/>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasSign(long _Long) => _Long < 0;
        #endregion
    }
}