using System;
using System.Collections.Generic;
using System.Linq;

namespace IfLoooop.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="Dictionary{TKey,TValue}"/>.
    /// </summary>
    public static class DictionaryExtensions
    {
        #region Methods
        /// <summary>
        /// Returns the total count of all <see cref="IDictionary{TKey,TValue}.Values"/>.
        /// </summary>
        /// <param name="_Dictionary">The <see cref="IDictionary{TKey,TValue}"/> to get the total count of.</param>
        /// <typeparam name="K"><see cref="Type"/> of the <see cref="IDictionary{TKey,TValue}.Keys"/>.</typeparam>
        /// <typeparam name="V"><see cref="Type"/> of the <see cref="IDictionary{TKey,TValue}.Values"/>.</typeparam>
        /// <returns>The total count of all <see cref="IDictionary{TKey,TValue}.Values"/>.</returns>
        public static int Count<K, V>(this IDictionary<K, V[]> _Dictionary)
        {
            return _Dictionary.Values.Sum(_Value => _Value.Length);
        }
        
        /// <summary>
        /// Flattens the <see cref="IDictionary{TKey,TValue}.Values"/> of this <see cref="IDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <param name="_Dictionary">The <see cref="IDictionary{TKey,TValue}"/> to flatten the <see cref="IDictionary{TKey,TValue}.Values"/> of.</param>
        /// <typeparam name="K"><see cref="Type"/> of the <see cref="IDictionary{TKey,TValue}.Keys"/>.</typeparam>
        /// <typeparam name="V"><see cref="Type"/> of the <see cref="IDictionary{TKey,TValue}.Values"/>.</typeparam>
        /// <returns>A flattened collection of all <see cref="IDictionary{TKey,TValue}.Values"/>.</returns>
        public static IEnumerable<V> Flatten<K, V>(this IDictionary<K, V[]> _Dictionary)
        {
            return _Dictionary.SelectMany(_KeyValuePair => _KeyValuePair.Value);
        }

        /// <summary>
        /// Flattens the <see cref="IDictionary{TKey,TValue}.Values"/> of this <see cref="IDictionary{TKey,TValue}"/> and applies a transformation function to each element in the arrays.
        /// </summary>
        /// <param name="_Dictionary">The <see cref="IDictionary{TKey,TValue}"/> to flatten the <see cref="IDictionary{TKey,TValue}.Values"/> of.</param>
        /// <param name="_Action">The function to apply to each element of the arrays in the <see cref="IDictionary{TKey,TValue}"/>.</param>
        /// <typeparam name="K"><see cref="Type"/> of the <see cref="IDictionary{TKey,TValue}.Keys"/>.</typeparam>
        /// <typeparam name="V"><see cref="Type"/> of the <see cref="IDictionary{TKey,TValue}.Values"/>.</typeparam>
        /// <typeparam name="T">The <see cref="Type"/> returned after applying the transformation function to each array element.</typeparam>
        /// <returns>A transformed and flattened collection of all <see cref="IDictionary{TKey,TValue}.Values"/>.</returns>
        public static IEnumerable<T> Flatten<K, V, T>(this IDictionary<K, V[]> _Dictionary, Func<V, T> _Action)
        {
            return _Dictionary.SelectMany(_KeyValuePair => _KeyValuePair.Value.Select(_Action));
        }
        #endregion
    }
}