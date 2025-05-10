using System.Runtime.CompilerServices;
using MomSesImSpcl.Utilities;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="byte"/>.
    /// </summary>
    public static class ByteExtensions
    {
        #region Methods
        /// <summary>
        /// Creates a new <see cref="NumericEnumerator"/> to iterate over a sequence of <see cref="byte"/>, starting at <c>0</c> and ending at <c>_End</c>.
        /// </summary>
        /// <param name="_End"><see cref="NumericEnumerator.end"/>.</param>
        /// <returns>A new <see cref="NumericEnumerator"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericEnumerator GetEnumerator(this byte _End)
        {
            return new NumericEnumerator(_End);
        }
        #endregion
    }
}
