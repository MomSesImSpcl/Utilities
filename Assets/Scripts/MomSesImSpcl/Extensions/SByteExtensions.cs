using System.Runtime.CompilerServices;
using MomSesImSpcl.Utilities;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="sbyte"/>.
    /// </summary>
    public static class SByteExtensions
    {
        #region Methods
        /// <summary>
        /// Creates a new <see cref="NumericEnumerator"/> to iterate over a sequence of <see cref="sbyte"/>, starting at <c>0</c> and ending at <c>_End</c>.
        /// </summary>
        /// <param name="_End"><see cref="NumericEnumerator.end"/>.</param>
        /// <returns>A new <see cref="NumericEnumerator"/>.</returns>
        public static NumericEnumerator GetEnumerator(this sbyte _End)
        {
            return new NumericEnumerator(_End);
        }
        
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
