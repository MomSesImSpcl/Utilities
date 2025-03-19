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