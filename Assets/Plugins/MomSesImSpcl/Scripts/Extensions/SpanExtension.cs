using System;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="Span{T}"/>.
    /// </summary>
    public static class SpanExtension
    {
        #region Methods
        /// <summary>
        /// Applies an accumulator function over a <see cref="Span{T}"/>, starting with the specified seed value, and returns the final accumulated result.
        /// </summary>
        /// <param name="_Span">The <see cref="Span{T}"/> whose elements will be processed.</param>
        /// <param name="_Seed">The initial accumulator value.</param>
        /// <param name="_Func">A function that combines the current accumulator value with each element in the <see cref="Span{T}"/>.</param>
        /// <typeparam name="T">The type of elements in the <see cref="Span{T}"/>.</typeparam>
        /// <typeparam name="TAccumulate">The type of the accumulated value.</typeparam>
        /// <returns>The final accumulated result after processing all elements in the <see cref="Span{T}"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="_Func"/> is <b>null</b>.</exception>
        public static TAccumulate Aggregate<T, TAccumulate>(this Span<T> _Span, TAccumulate _Seed, Func<TAccumulate, T, TAccumulate> _Func)
        {
            if (_Func is null)
            {
                throw new ArgumentNullException(nameof(_Func));
            }

            var _result = _Seed;

            for (var _i = 0; _i < _Span.Length; _i++)
            {
                _result = _Func(_result, _Span[_i]);
            }

            return _result;
        }
            
        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence.
        /// </summary>
        /// <param name="_Span">The <see cref="Span{T}"/> to search on.</param>
        /// <param name="_Item">The item that is being searched for.</param>
        /// <param name="_Match">The condition to search for.</param>
        /// <typeparam name="T">The <see cref="Type"/> of the collection.</typeparam>
        /// <typeparam name="U">The <see cref="Type"/> of the condition to search for.</typeparam>
        /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by match, if found; otherwise, -1.</returns>
        public static int FindIndex<T,U>(this Span<T> _Span, U _Item, Func<T,U,bool> _Match)
        {
            // ReSharper disable once InconsistentNaming
            for (var i = 0; i < _Span.Length; i++)
            {
                if (_Match(_Span[i], _Item))
                {
                    return i;
                }
            }
            
            return -1;
        }


        public static T FirstOrDefault<T,U>(this Span<T> _Span, U _Item, Func<T,U,bool> _Match)
        {
            // ReSharper disable once InconsistentNaming
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < _Span.Length; i++)
            {
                if (_Match(_Span[i], _Item))
                {
                    return _Span[i];
                }
            }

            return default;
        }
        
        
        /// <summary>
        /// Returns a random element from this <see cref="Span{T}"/>.
        /// </summary>
        /// <param name="_Span">The <see cref="Span{T}"/> to get a random element from.</param>
        /// <typeparam name="T">The <see cref="Type"/> of the <see cref="Span{T}"/>.</typeparam>
        /// <returns>A random element from this <see cref="Span{T}"/>.</returns>
        public static T GetRandom<T>(this Span<T> _Span)
        {
            var _index = UnityEngine.Random.Range(0, _Span.Length);
            return _Span[_index];
        }
        #endregion
    }
}
