using System;
using MomSesImSpcl.Utilities.Comparers;
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
        public static void ReturnToArrayPool<T>(this T[] _Array, uint _NewMaxAmount = 1) where T : unmanaged
        {
            ArrayPool<T>.Return(_Array, _NewMaxAmount);
        }
        
        /// <summary>
        /// Returns this <see cref="Array"/> to <see cref="ArrayPool{T}"/>.<see cref="ArrayPool{T}.concurrentArrayPool"/>.
        /// </summary>
        /// <param name="_Array">Can be any <see cref="Array"/>, even if it wasn't retrieved from the <see cref="ArrayPool{T}.concurrentArrayPool"/>.</param>
        /// <param name="_NewMaxAmount">The new <see cref="ArrayPool{T}.ArrayBucket.MaxAmount"/>.</param>
        /// <typeparam name="T">Must be a primitive <see cref="Type"/>.</typeparam>
        public static void ReturnToConcurrentArrayPool<T>(this T[] _Array, uint _NewMaxAmount = 1) where T : unmanaged
        {
            ArrayPool<T>.ReturnConcurrent(_Array, _NewMaxAmount);
        }

        /// <summary>
        /// Sorts an <see cref="Array"/> in ascending order. <br/>
        /// <b>This will sort the original <see cref="Array"/>.</b>
        /// </summary>
        /// <param name="_Array">The <see cref="Array"/> to sort.</param>
        /// <param name="_SortBy">The value to sort by.</param>
        /// <typeparam name="T">The <see cref="Type"/> of the <see cref="Array"/>.</typeparam>
        /// <typeparam name="V">The <see cref="Type"/> of the value to sort by.</typeparam>
        /// <returns>The sorted <see cref="Array"/>.</returns>
        public static T[] SortAscending<T,V>(this T[] _Array, Func<T,V> _SortBy) where V : IComparable<V>
        {
            var _comparer = new AscendingComparer<T,V>(_SortBy);
            Array.Sort(_Array, _comparer);
            return _Array;
        }
        
        /// <summary>
        /// Sorts an <see cref="Array"/> in descending order. <br/>
        /// <b>This will sort the original <see cref="Array"/>.</b>
        /// </summary>
        /// <param name="_Array">The <see cref="Array"/> to sort.</param>
        /// <param name="_SortBy">The value to sort by.</param>
        /// <typeparam name="T">The <see cref="Type"/> of the <see cref="Array"/>.</typeparam>
        /// <typeparam name="V">The <see cref="Type"/> of the value to sort by.</typeparam>
        /// <returns>The sorted <see cref="Array"/>.</returns>
        public static T[] SortDescending<T,V>(this T[] _Array, Func<T,V> _SortBy) where V : IComparable<V>
        {
            var _comparer = new DescendingComparer<T,V>(_SortBy);
            Array.Sort(_Array, _comparer);
            return _Array;
        }
        #endregion
    }
}
