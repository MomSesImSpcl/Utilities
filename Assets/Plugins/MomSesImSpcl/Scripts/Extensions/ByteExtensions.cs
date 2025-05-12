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
        /// Returns this <see cref="byte"/> as a <see cref="float"/>.
        /// </summary>
        /// <param name="_Byte">The <see cref="byte"/> to cast to a <see cref="float"/>.</param>
        /// <returns>This <see cref="byte"/> cast to a <see cref="float"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float AsFloat(this byte _Byte)
        {
            return (float)_Byte;
        }
        
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
