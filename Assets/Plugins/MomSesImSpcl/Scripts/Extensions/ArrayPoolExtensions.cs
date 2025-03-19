using System;
using System.Buffers;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="ArrayPool{T}"/>.
    /// </summary>
    public static class ArrayPoolExtensions
    {
        #region Methods
        /// <summary>
        /// Rents an array from this <see cref="ArrayPool{T}"/> and stores a reference to in <c>_Array</c>.
        /// </summary>
        /// <param name="_ArrayPool">The <see cref="ArrayPool{T}"/> to rent from.</param>
        /// <param name="_MinimumLength">The min size of the rented array.</param>
        /// <param name="_Array">Reference to the rented array.</param>
        /// <typeparam name="T">The <see cref="Type"/> of the array elements.</typeparam>
        /// <returns>This <see cref="ArrayPool{T}"/>.</returns>
        public static ArrayPool<T> Rent<T>(this ArrayPool<T> _ArrayPool, int _MinimumLength, out T[] _Array)
        {
            _Array = _ArrayPool.Rent(_MinimumLength);
            return _ArrayPool;
        }
        #endregion
    }
}