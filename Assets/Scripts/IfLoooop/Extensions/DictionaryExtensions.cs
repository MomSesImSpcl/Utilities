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
        /// Counts the total number of elements contained in the arrays which are the values of the given <see cref="IDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <typeparam name="K"><see cref="Type"/> of the <see cref="IDictionary{TKey,TValue}.Keys"/>.</typeparam>
        /// <typeparam name="V"><see cref="Type"/> of the <see cref="IDictionary{TKey,TValue}.Values"/>.</typeparam>
        /// <param name="_Dictionary">The <see cref="IDictionary{TKey,TValue}"/> containing the arrays to be counted.</param>
        /// <returns>The total count of elements in all arrays contained in the dictionary.</returns>
        public static int Count<K, V>(this IDictionary<K, V[]> _Dictionary)
        {
            return _Dictionary.Values.Sum(_Value => _Value.Length);
        }

        /// <summary>
        /// Flattens the arrays contained in the values of the given <see cref="IDictionary{TKey,TValue}"/> into a single sequence.
        /// </summary>
        /// <typeparam name="K"><see cref="Type"/> of the <see cref="IDictionary{TKey,TValue}.Keys"/>.</typeparam>
        /// <typeparam name="V"><see cref="Type"/> of the <see cref="IDictionary{TKey,TValue}.Values"/>.</typeparam>
        /// <param name="_Dictionary">The <see cref="IDictionary{TKey,TValue}"/> containing the arrays to be flattened.</param>
        /// <returns>A sequence containing all the elements in the arrays from the dictionary values.</returns>
        public static IEnumerable<V> Flatten<K, V>(this IDictionary<K, V[]> _Dictionary)
        {
            return _Dictionary.SelectMany(_KeyValuePair => _KeyValuePair.Value);
        }

        /// <summary>
        /// Flattens the arrays contained in the values of the given <see cref="IDictionary{TKey,TValue}"/> into a single sequence after transforming them using the specified function.
        /// </summary>
        /// <typeparam name="K"><see cref="Type"/> of the <see cref="IDictionary{TKey,TValue}.Keys"/>.</typeparam>
        /// <typeparam name="V"><see cref="Type"/> of the <see cref="IDictionary{TKey,TValue}.Values"/>.</typeparam>
        /// <typeparam name="T">The type of elements in the flattened and transformed sequence.</typeparam>
        /// <param name="_Dictionary">The <see cref="IDictionary{TKey,TValue}"/> containing the arrays to be flattened and transformed.</param>
        /// <param name="_Action">The function to transform each element in the arrays before flattening.</param>
        /// <returns>A sequence containing all the elements in the arrays from the dictionary values after being transformed.</returns>
        public static IEnumerable<T> Flatten<K, V, T>(this IDictionary<K, V[]> _Dictionary, Func<V, T> _Action)
        {
            return _Dictionary.SelectMany(_KeyValuePair => _KeyValuePair.Value.Select(_Action));
        }
        #endregion
    }
}