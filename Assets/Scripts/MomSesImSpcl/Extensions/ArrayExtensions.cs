using System;
using MomSesImSpcl.Utilities.Pooling;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="Array"/>.
    /// </summary>
    public static class ArrayExtensions
    {
        #region Methods
        /// <summary>
        /// Returns this <see cref="Array"/> to <see cref="ArrayPool{T}"/>.<see cref="ArrayPool{T}.arrayPool"/>.
        /// </summary>
        /// <param name="_Array">Can be any <see cref="Array"/>, even if it wasn't retrieved from the <see cref="ArrayPool{T}.arrayPool"/>.</param>
        /// <param name="_NewMaxAmount">The new <see cref="ArrayPool{T}.ArrayBucket.MaxAmount"/>.</param>
        /// <typeparam name="T">Must be a primitive <see cref="Type"/>.</typeparam>
        public static void ReturnToArrayPool<T>(this T[] _Array, uint _NewMaxAmount = 1) where T : struct
        {
            ArrayPool<T>.Return(_Array, _NewMaxAmount);
        }
        
        /// <summary>
        /// Returns this <see cref="Array"/> to <see cref="ArrayPool{T}"/>.<see cref="ArrayPool{T}.concurrentArrayPool"/>.
        /// </summary>
        /// <param name="_Array">Can be any <see cref="Array"/>, even if it wasn't retrieved from the <see cref="ArrayPool{T}.concurrentArrayPool"/>.</param>
        /// <param name="_NewMaxAmount">The new <see cref="ArrayPool{T}.ArrayBucket.MaxAmount"/>.</param>
        /// <typeparam name="T">Must be a primitive <see cref="Type"/>.</typeparam>
        public static void ReturnToConcurrentArrayPool<T>(this T[] _Array, uint _NewMaxAmount = 1) where T : struct
        {
            ArrayPool<T>.ReturnConcurrent(_Array, _NewMaxAmount);
        }
        #endregion
    }
}