#nullable enable
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MomSesImSpcl.Utilities.Comparers;
using MomSesImSpcl.Utilities.Pooling;
using Random = UnityEngine.Random;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="Array"/>.
    /// </summary>
    public static class ArrayExtensions
    {
        #region Methods
        /// <summary>
        /// Returns a random element from this <see cref="Array"/>.
        /// </summary>
        /// <param name="_Array">The <see cref="Array"/> to select a random element from.</param>
        /// <typeparam name="T">The <see cref="Type"/> of the elements in the <see cref="Array"/>.</typeparam>
        /// <returns>A randomly selected element from the <see cref="Array"/>.</returns>
        public static T GetRandom<T>(this T[] _Array)
        {
            return _Array[Random.Range(0, _Array.Length)];
        }
        
        /// <summary>
        /// Returns random elements from the given <see cref="Array"/>.
        /// </summary>
        /// <param name="_Array">The <see cref="Array"/> to select random elements from.</param>
        /// <param name="_Amount">The number of elements to return.</param>
        /// <param name="_CanContainDuplicates">
        /// Set to <c>false</c> to only return unique elements. <br/>
        /// <b>The given <see cref="Array"/> must contain at least as many elements as the requested amount.</b>
        /// </param>
        /// <typeparam name="T">The <see cref="Type"/> of the elements in the <see cref="Array"/>.</typeparam>
        /// <returns>Randomly selected elements from the enumerable.</returns>
        /// <exception cref="ArgumentException">Thrown if the enumerable does not contain enough unique elements.</exception>
        public static IEnumerable<T> GetRandom<T>(this T[] _Array, uint _Amount, bool _CanContainDuplicates)
        {
            foreach (var _element in ((IList<T>)_Array).GetRandom(_Amount, _CanContainDuplicates))
            {
                yield return _element;
            }
        }
        
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
        /// Selects a random subset of elements from the given <see cref="IList{T}"/>.
        /// </summary>
        /// <param name="_Array">The <see cref="IList{T}"/> to select from</param>
        /// <param name="_MaxAmount">The maximum number of elements to select. If 0 or not specified, selects up to the total length of the collection</param>
        /// <typeparam name="T">The <see cref="Type"/> of elements in the collection</typeparam>
        /// <returns>A random subset of elements from the original collection</returns>
        public static IEnumerable<T> GetRandomAmount<T>(this T[] _Array, int _MaxAmount = 0)
        {
            return ((IList<T>)_Array).GetRandomAmount(_MaxAmount);
        }

        /// <summary>
        /// Converts an <see cref="Array"/> to a formatted JSON string with specified indentations and selected properties.
        /// </summary>
        /// <param name="_Array">The enumerable collection to convert to JSON.</param>
        /// <param name="_Indentations">The number of spaces to use for indentation.</param>
        /// <param name="_Entries">Expressions specifying the properties to include in the JSON output.</param>
        /// <typeparam name="T">The <see cref="Type"/> of the elements in the collection.</typeparam>
        /// <returns>A JSON string representing the collection with formatted entries and specified indentations.</returns>
        public static string PrettyJson<T>(this T[] _Array, int _Indentations, params Expression<Func<T, object?>>[] _Entries)
        {
            return ((IList<T>)_Array).PrettyJson(_Indentations, _Entries);
        }
        
        /// <summary>
        /// Prints the elements of the enumerable in a formatted, human-readable string.
        /// </summary>
        /// <param name="_Array">The <see cref="Array"/> to format and print.</param>
        /// <param name="_Title">An optional title to use for the formatted output.</param>
        /// <param name="_Entries">The properties or fields of the elements to include in the formatted output.</param>
        /// <typeparam name="T">The type of elements in the enumerable.</typeparam>
        /// <returns>A formatted string representing the elements of the enumerable.</returns>
        public static string PrintPretty<T>(this T[] _Array, Expression<Func<T, object?>>? _Title = null, params Expression<Func<T, object?>>[] _Entries)
        {
            return PrintPretty(_Array, true, true, _Title, _Entries);
        }

        /// <summary>
        /// Generates a formatted string representing the provided collection, with options for including a title and specific entries.
        /// </summary>
        /// <param name="_Array">The <see cref="Array"/> to be formatted.</param>
        /// <param name="_PrintName">A boolean flag indicating if the property names should be included in the output.</param>
        /// <param name="_Title">An optional expression defining the title for the formatted output.</param>
        /// <param name="_Entries">Expressions defining the entries to be included in the formatted output.</param>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <returns>A string representing the formatted output of the collection.</returns>
        public static string PrintPretty<T>(this T[] _Array, bool _PrintName, Expression<Func<T, object?>>? _Title = null, params Expression<Func<T, object?>>[] _Entries)
        {
            return PrintPretty(_Array, _PrintName, true, _Title, _Entries);
        }

        /// <summary>
        /// Formats the elements of the specified collection into a readable string representation.
        /// </summary>
        /// <param name="_Array">The <see cref="Array"/> containing the data to format.</param>
        /// <param name="_PrintName">A boolean flag indicating if the property names should be included in the output.</param>
        /// <param name="_AddNewLineAtEnd">A boolean flag indicating if a newline should be added at the end of the output.</param>
        /// <param name="_Title">An expression that selects the title property from the collection's elements.</param>
        /// <param name="_Entries">An array of expressions that select properties from the collection's elements to include in the output.</param>
        /// <typeparam name="T">The type of the elements in the collection.</typeparam>
        /// <returns>A formatted string representation of the collection elements.</returns>
        public static string PrintPretty<T>(this T[] _Array, bool _PrintName = true, bool _AddNewLineAtEnd = true, Expression<Func<T, object?>>? _Title = null, params Expression<Func<T, object?>>[] _Entries)
        {
            return ((IList<T>)_Array).PrintPretty(_PrintName, _AddNewLineAtEnd, _Title, _Entries);
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
