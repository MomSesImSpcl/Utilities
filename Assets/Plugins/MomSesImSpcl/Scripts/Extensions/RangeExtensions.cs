using System;
using System.Runtime.CompilerServices;
using MomSesImSpcl.Utilities;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="Range"/>.
    /// </summary>
    public static class RangeExtensions
    {
        #region Methods
        /// <summary>
        /// Creates a new <see cref="NumericEnumerator"/> to iterate over a sequence of <see cref="int"/>, over the given <see cref="Range"/>.
        /// </summary>
        /// <param name="_Range"></param>
        /// <returns>A new <see cref="NumericEnumerator"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericEnumerator GetEnumerator(this Range _Range)
        {
            return new NumericEnumerator(_Range);
        }
        #endregion
    }
}
