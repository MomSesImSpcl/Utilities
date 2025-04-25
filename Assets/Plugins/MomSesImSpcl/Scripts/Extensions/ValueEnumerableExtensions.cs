using System;
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
        /// Materialized the <see cref="ValueEnumerable{TEnumerator,T}"/> into a <see cref="Span{T}"/> from a pooled <see cref="Array"/>.
        /// </summary>
        /// <param name="_ValueEnumerable"><see cref="ValueEnumerable{TEnumerator,T}"/>.</param>
        /// <typeparam name="E">The <see cref="Type"/> of the <see cref="ValueEnumerator{TEnumerator,T}"/>.</typeparam>
        /// <typeparam name="A">The <see cref="Type"/> of the <see cref="Array"/>.</typeparam>
        /// <returns>A <see cref="Span{T}"/> that has exactly the same <see cref="Array.Length"/> as the original <see cref="ValueEnumerable{TEnumerator,T}"/>.</returns>
        public static Span<A> ToSlicedArrayPool<E,A>(this ValueEnumerable<E,A> _ValueEnumerable) where E : struct, IValueEnumerator<A>
        {
            var _arrayTuple = _ValueEnumerable.ToArrayPool();
            return _arrayTuple.Array.AsSpan(0, _arrayTuple.Size);
        }
        #endregion
    }
}