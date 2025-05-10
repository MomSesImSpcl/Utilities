using System.Runtime.CompilerServices;
using MomSesImSpcl.Utilities;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="short"/>.
    /// </summary>
    public static class ShortExtensions
    {
        #region Methods
        /// <summary>
        /// Creates a new <see cref="NumericEnumerator"/> to iterate over a sequence of <see cref="short"/>, starting at <c>0</c> and ending at <c>_End</c>.
        /// </summary>
        /// <param name="_End"><see cref="NumericEnumerator.end"/>.</param>
        /// <returns>A new <see cref="NumericEnumerator"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericEnumerator GetEnumerator(this short _End)
        {
            return new NumericEnumerator(_End);
        }
        
        /// <summary>
        /// Determines whether the specified short integer has a negative sign.
        /// </summary>
        /// <param name="_Short">The short integer to be evaluated.</param>
        /// <returns>true if the short integer is negative; otherwise, false.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasSign(short _Short) => _Short < 0;
        #endregion
    }
}
