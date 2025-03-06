using System;
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
        /// Creates a new <see cref="IntEnumerator"/> to iterate over a sequence of <see cref="int"/>, over the given <see cref="Range"/>.
        /// </summary>
        /// <param name="_Range"></param>
        /// <returns>A new <see cref="IntEnumerator"/>.</returns>
        public static IntEnumerator GetEnumerator(this Range _Range)
        {
            return new IntEnumerator(_Range);
        }
        #endregion
    }
}