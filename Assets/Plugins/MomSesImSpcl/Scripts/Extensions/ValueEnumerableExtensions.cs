#if ZLINQ
using System;
using MomSesImSpcl.Data;
using ZLinq;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="ValueEnumerable{TEnumerator,T}"/>.
    /// </summary>
    public static class ValueEnumerableExtensions
    {
        #region Methods
        /// <summary>
        /// Materializes the <see cref="ValueEnumerable{TEnumerator,T}"/> into an <see cref="ArrayPoolSlice{T}"/>.
        /// </summary>
        /// <param name="_ValueEnumerable"><see cref="ValueEnumerable{TEnumerator,T}"/>.</param>
        /// <param name="_Slice">A <see cref="Span{T}"/> that has exactly the same <see cref="Array.Length"/> as the original <see cref="ValueEnumerable{TEnumerator,T}"/>.</param>
        /// <param name="_ClearOnDispose"><see cref="ArrayPoolSlice{T}.clearOnDispose"/>.</param>
        /// <typeparam name="E">The <see cref="Type"/> of the <see cref="ValueEnumerator{TEnumerator,T}"/>.</typeparam>
        /// <typeparam name="A">The <see cref="Type"/> of the <see cref="Array"/>.</typeparam>
        /// <returns><see cref="ArrayPoolSlice{T}"/>.</returns>
        public static ArrayPoolSlice<A> ToArrayPoolSlice<E,A>(this ValueEnumerable<E,A> _ValueEnumerable, out Span<A> _Slice, bool _ClearOnDispose = false) where E : struct, IValueEnumerator<A>
        {
            var _arrayTuple = _ValueEnumerable.ToArrayPool();
            var _arrayPoolSlice = new ArrayPoolSlice<A>(_arrayTuple, _ClearOnDispose);
            
            _Slice = _arrayPoolSlice.GetSlice();
            return _arrayPoolSlice;
        }
        
        /// <summary>
        /// Materializes the <see cref="ValueEnumerable{TEnumerator,T}"/> into an <see cref="ArrayPoolSlice{T}"/>.
        /// </summary>
        /// <param name="_ValueEnumerable"><see cref="ValueEnumerable{TEnumerator,T}"/>.</param>
        /// <param name="_ClearOnDispose"><see cref="ArrayPoolSlice{T}.clearOnDispose"/>.</param> 
        /// <typeparam name="E">The <see cref="Type"/> of the <see cref="ValueEnumerator{TEnumerator,T}"/>.</typeparam>
        /// <typeparam name="A">The <see cref="Type"/> of the <see cref="Array"/>.</typeparam>
        /// <returns><see cref="ArrayPoolSlice{T}"/>.</returns>
        public static ArrayPoolSlice<A> ToArrayPoolSlice<E,A>(this ValueEnumerable<E,A> _ValueEnumerable, bool _ClearOnDispose = false) where E : struct, IValueEnumerator<A>
        {
            var _arrayTuple = _ValueEnumerable.ToArrayPool();
            return new ArrayPoolSlice<A>(_arrayTuple, _ClearOnDispose);
        }
        #endregion
    }
}
#endif