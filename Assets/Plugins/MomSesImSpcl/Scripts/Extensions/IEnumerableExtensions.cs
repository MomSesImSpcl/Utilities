#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MomSesImSpcl.Data;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="IEnumerable{T}"/>
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class IEnumerableExtensions
    {
        #region Methods
        /// <summary>
        /// Returns a sequence that contains all elements from this <see cref="IEnumerable{T}"/> except the specified match element.
        /// </summary>
        /// <param name="_IEnumerable">The <see cref="IEnumerable{T}"/> to search on.</param>
        /// <param name="_Match">The element to exclude from the sequence.</param>
        /// <typeparam name="T">The <see cref="Type"/> of elements in the sequence.</typeparam>
        /// <returns>An <see cref="IEnumerable{T}"/> that excludes the specified element.</returns>
        public static IEnumerable<T> Exclude<T>(this IEnumerable<T> _IEnumerable, T _Match)
        {
            using var _enumerator = _IEnumerable.GetEnumerator();

            while (_enumerator.MoveNext())
            {
                var _current = _enumerator.Current;
                
                if (!EqualityComparer<T>.Default.Equals(_current, _Match))
                {
                    yield return _current;
                }
            }
        }
        
        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence.
        /// </summary>
        /// <param name="_IEnumerable">The <see cref="IEnumerable{T}"/> to search on.</param>
        /// <param name="_Match">The condition to search for</param>
        /// <typeparam name="T">The <see cref="Type"/> of elements in the sequence.</typeparam>
        /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by match, if found; otherwise, -1</returns>
        public static int FindIndex<T>(this IEnumerable<T> _IEnumerable, Predicate<T> _Match)
        {
            var _count = 0;
            using var _enumerator = _IEnumerable.GetEnumerator();
            
            while (_enumerator.MoveNext())
            {
                if (_Match(_enumerator.Current))
                {
                    return _count;
                }
            
                _count++;
            }
            
            return -1;
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence.
        /// </summary>
        /// <param name="_IEnumerable">The <see cref="IEnumerable{T}"/> to search on.</param>
        /// <param name="_Item">The item that is being searched for.</param>
        /// <param name="_Match">The condition to search for.</param>
        /// <typeparam name="T">The <see cref="Type"/> of the collection.</typeparam>
        /// <typeparam name="U">The <see cref="Type"/> of the condition to search for.</typeparam>
        /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by match, if found; otherwise, -1.</returns>
        public static int FindIndex<T,U>(this IEnumerable<T> _IEnumerable, U _Item, Func<T,U,bool> _Match)
        {
            var _count = 0;
            using var _enumerator = _IEnumerable.GetEnumerator();
            
            while (_enumerator.MoveNext())
            {
                if (_Match(_enumerator.Current, _Item))
                {
                    return _count;
                }
            
                _count++;
            }
            
            return -1;
        }
        
        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate in parallel, and returns the zero-based index of the first occurrence within the entire sequence.
        /// </summary>
        /// <param name="_IEnumerable">The <see cref="IEnumerable{T}"/> to search on</param>
        /// <param name="_Match">The condition to search for</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by match, if found; otherwise, -1</returns>
        public static int FindIndexParallel<T>(this IEnumerable<T> _IEnumerable, Predicate<T> _Match)
        {
            var _array = _IEnumerable as T[] ?? _IEnumerable.ToArray();
            var _index = ParallelEnumerable.Range(0, _array.Length).FirstOrDefault(_Index => _Match(_array[_Index]));

            return _index != -1 ? _index : -1;
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate and returns the first occurrence within the entire enumerable or the provided default value if no match is found.
        /// </summary>
        /// <param name="_IEnumerable">The <see cref="IEnumerable{T}"/> to search on.</param>
        /// <param name="_Match">The predicate to match elements against.</param>
        /// <param name="_DefaultValue">The default value to return if no match is found.</param>
        /// <typeparam name="T">The type of elements in the enumerable.</typeparam>
        /// <returns>The first occurrence of an element that matches the conditions defined by the predicate, or the default value if no match is found.</returns>
        public static T? FirstOrDefault<T>(this IEnumerable<T> _IEnumerable, Predicate<T> _Match, T? _DefaultValue)
        {
            using var _enumerator = _IEnumerable.GetEnumerator();
            
            while (_enumerator.MoveNext())
            {
                if (_Match(_enumerator.Current))
                {
                    return _enumerator.Current;
                }
            }
            
            return _DefaultValue;
        }
        
        /// <summary>
        /// Iterates over each element in the <see cref="IEnumerable"/> and executes the provided <see cref="Action{T}"/>.
        /// </summary>
        /// <param name="_IEnumerable">The <see cref="IEnumerable{T}"/> to iterate over.</param>
        /// <param name="_Action">The <see cref="Action{T}"/> to perform + the current index of the element in the <see cref="IEnumerable{T}"/>.</param>
        /// <typeparam name="T">The <see cref="Type"/> of the <see cref="IEnumerable{T}"/>.</typeparam>
        public static void For<T>(this IEnumerable<T> _IEnumerable, Action<T,int> _Action)
        {
            using var _enumerator = _IEnumerable.GetEnumerator();

            var _index = 0;
            while (_enumerator.MoveNext())
            {
                _Action(_enumerator.Current, _index++);
            }
        }

        /// <summary>
        /// Iterates over each element in the <see cref="IEnumerable"/> and executes the provided <see cref="Action{T}"/>.
        /// </summary>
        /// <param name="_IEnumerable">The <see cref="IEnumerable{T}"/> to iterate over.</param>
        /// <param name="_Action">The <see cref="Action{T}"/> to perform.</param>
        /// <typeparam name="T">The <see cref="Type"/> of the <see cref="IEnumerable{T}"/>.</typeparam>
        public static void ForEach<T>(this IEnumerable<T> _IEnumerable, Action<T> _Action)
        {
            foreach (var _element in _IEnumerable)
            {
                _Action(_element);
            }
        }
        
        /// <summary>
        /// Returns a random element from the specified enumerable.
        /// </summary>
        /// <param name="_IEnumerable">The <see cref="IEnumerable{T}"/> to select a random element from.</param>
        /// <typeparam name="T">The type of the elements in the enumerable.</typeparam>
        /// <returns>A randomly selected element from the enumerable.</returns>
        public static T GetRandom<T>(this IEnumerable<T> _IEnumerable)
        {
            return _IEnumerable.ToArray().GetRandom();
        }
        
        /// <summary>
        /// Returns random elements from the given enumerable.
        /// </summary>
        /// <param name="_IEnumerable">The <see cref="IEnumerable{T}"/> to select random elements from.</param>
        /// <param name="_Amount">The number of elements to return.</param>
        /// <param name="_CanContainDuplicates">
        /// Set to <c>false</c> to only return unique elements. <br/>
        /// <b>The given <see cref="IEnumerable{T}"/> must contain at least as many elements as the requested amount.</b>
        /// </param>
        /// <typeparam name="T">The type of the elements in the enumerable.</typeparam>
        /// <returns>Randomly selected elements from the enumerable.</returns>
        /// <exception cref="ArgumentException">Thrown if the enumerable does not contain enough unique elements.</exception>
        public static IEnumerable<T> GetRandom<T>(this IEnumerable<T> _IEnumerable, uint _Amount, bool _CanContainDuplicates)
        {
            foreach (var _element in _IEnumerable.ToArray().GetRandom(_Amount, _CanContainDuplicates))
            {
                yield return _element;
            }
        }
        
        /// <summary>
        /// Returns a weighted random selection of elements from this <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="_IEnumerable">The enumerable from which to select the elements.</param>
        /// <param name="_Value">The that determines the weight factor.</param>
        /// <param name="_Weights">
        /// Maps every possible <c>_Value</c> to its corresponding weight. <br/>
        /// <i>Values with a higher weight factor will be more frequent in the returned collection.</i>
        /// </param>
        /// <param name="_Amount">The number of elements to select.</param>
        /// <param name="_CanContainDuplicates"><c>true</c> if the same element can be selected multiple times, <c>false</c> if each returned element should be unique.</param>
        /// <typeparam name="T">The <see cref="Type"/> of the elements in the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <typeparam name="V">The <see cref="Type"/> of the weighting key (<c>_Value</c>) used to look up the weights in the <c>_Weights</c> <see cref="IDictionary{TKey,TValue}"/>.</typeparam>
        /// <returns>A <see cref="IEnumerable{T}"/> of randomly selected elements of type, weighted by their associated weights.</returns>
        /// <exception cref="ArgumentException"> <br/>
        /// -If the given <c>_Weights</c> <see cref="IDictionary{TKey,TValue}"/> has no weight value that is greater than <c>0</c>. <br/>
        /// -If <paramref name="_CanContainDuplicates"/> is <c>false</c> and there are fewer unique elements with positive weights than <paramref name="_Amount"/>.
        /// </exception>
        public static IEnumerable<T> GetWeightedRandom<T,V>(this IEnumerable<T> _IEnumerable, Func<T,V> _Value, IDictionary<V,uint> _Weights, uint _Amount, bool _CanContainDuplicates)
        {
            var _weightedElements = _IEnumerable.Select(_Element => new
            {
                Element = _Element,
                Weight = _Weights.TryGetValue(_Value(_Element), out var _weight) ? _weight : 0
                
            }).Where(_WeightedElement => _WeightedElement.Weight > 0).ToList();
            
            if (_weightedElements.Count == 0)
            {
                throw new ArgumentException("No elements with positive weight found.");
            }

            if (!_CanContainDuplicates && _weightedElements.Count < _Amount)
            {
                throw new ArgumentException($"Not enough unique elements. Required: {_Amount}, Found: {_weightedElements.Count}");
            }

            var _random = new Random();
            
            if (_CanContainDuplicates)
            {
                ulong _totalWeight = 0;
                var _cumulativeWeights = new ulong[_weightedElements.Count];
    
                // ReSharper disable once InconsistentNaming
                for (var i = 0; i < _weightedElements.Count; i++)
                {
                    _totalWeight += _weightedElements[i].Weight;
                    _cumulativeWeights[i] = _totalWeight;
                }
            
                // ReSharper disable once InconsistentNaming
                for (var i = 0; i < _Amount; i++)
                {
                    var _randomNumber = (ulong)(_random.NextDouble() * _totalWeight);
                    var _index = Array.BinarySearch(_cumulativeWeights, _randomNumber);

                    if (_index < 0)
                    {
                        _index = ~_index;
                    }

                    yield return _weightedElements[_index].Element;
                }
            }
            else
            {
                var _totalWeight = (ulong)_weightedElements.Sum(_WeightedElement => _WeightedElement.Weight);
                
                // ReSharper disable once InconsistentNaming
                for (var i = 0; i < _Amount; i++)
                {
                    var _index = 0;
                    var _cumulativeWeight = 0UL;
                    var _randomNumber = (ulong)(_random.NextDouble() * _totalWeight);
                
                    for (; _index < _weightedElements.Count; _index++)
                    {
                        if ((_cumulativeWeight += _weightedElements[_index].Weight) > _randomNumber)
                        {
                            break;
                        }
                    }
                
                    var _element = _weightedElements[_index].Element;
                    _totalWeight -= _weightedElements[_index].Weight;
                    _weightedElements.SwapAndPop(_index);
                
                    yield return _element;
                }
            }
        }
        
        /// <summary>
        /// Selects a random subset of elements from the given <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="_IEnumerable">The <see cref="IEnumerable{T}"/> to select from</param>
        /// <param name="_MaxAmount">The maximum number of elements to select. If 0 or not specified, selects up to the total length of the collection</param>
        /// <typeparam name="T">The type of elements in the collection</typeparam>
        /// <returns>A random subset of elements from the original collection</returns>
        public static IEnumerable<T> GetRandomAmount<T>(this IEnumerable<T> _IEnumerable, int _MaxAmount = 0)
        {
            return _IEnumerable.ToArray().GetRandomAmount(_MaxAmount);
        }

        /// <summary>
        /// Returns the median value from a collection of numbers.
        /// </summary>
        /// <param name="_IEnumerable">The <see cref="IEnumerable{T}"/> to get the median of.</param>
        /// <param name="_Sort">Set this to <c>false</c> if the collection is already sorted.</param>
        /// <typeparam name="T">Must be a numeric <see cref="Type"/>.</typeparam>
        /// <returns>The median value as a <see cref="decimal"/>.</returns>
        public static decimal Median<T>(this IEnumerable<T> _IEnumerable, bool _Sort = true) where T : unmanaged, IFormattable
        {
            return _IEnumerable.ToArray().Median(_Sort);
        }
        
        /// <summary>
        /// Converts an <see cref="IEnumerable{T}"/> to a formatted JSON string with specified indentations and selected properties.
        /// </summary>
        /// <param name="_IEnumerable">The enumerable collection to convert to JSON.</param>
        /// <param name="_Indentations">The number of spaces to use for indentation.</param>
        /// <param name="_Entries">Expressions specifying the properties to include in the JSON output.</param>
        /// <typeparam name="T">The type of the elements in the collection.</typeparam>
        /// <returns>A JSON string representing the collection with formatted entries and specified indentations.</returns>
        public static string PrettyJson<T>(this IEnumerable<T> _IEnumerable, int _Indentations, params Expression<Func<T, object?>>[] _Entries)
        {
            return _IEnumerable.ToArray().PrettyJson(_Indentations, _Entries);
        }

        /// <summary>
        /// Prints the elements of the enumerable in a formatted, human-readable string.
        /// </summary>
        /// <param name="_IEnumerable">The <see cref="IEnumerable{T}"/> to format and print.</param>
        /// <param name="_Title">An optional title to use for the formatted output.</param>
        /// <param name="_Entries">The properties or fields of the elements to include in the formatted output.</param>
        /// <typeparam name="T">The type of elements in the enumerable.</typeparam>
        /// <returns>A formatted string representing the elements of the enumerable.</returns>
        public static string PrintPretty<T>(this IEnumerable<T> _IEnumerable, Expression<Func<T, object?>>? _Title = null, params Expression<Func<T, object?>>[] _Entries)
        {
            return PrintPretty(_IEnumerable, true, true, _Title, _Entries);
        }

        /// <summary>
        /// Generates a formatted string representing the provided collection, with options for including a title and specific entries.
        /// </summary>
        /// <param name="_IEnumerable">The <see cref="IEnumerable{T}"/> to be formatted.</param>
        /// <param name="_PrintName">A boolean flag indicating if the property names should be included in the output.</param>
        /// <param name="_Title">An optional expression defining the title for the formatted output.</param>
        /// <param name="_Entries">Expressions defining the entries to be included in the formatted output.</param>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <returns>A string representing the formatted output of the collection.</returns>
        public static string PrintPretty<T>(this IEnumerable<T> _IEnumerable, bool _PrintName, Expression<Func<T, object?>>? _Title = null, params Expression<Func<T, object?>>[] _Entries)
        {
            return PrintPretty(_IEnumerable, _PrintName, true, _Title, _Entries);
        }

        /// <summary>
        /// Formats the elements of the specified collection into a readable string representation.
        /// </summary>
        /// <param name="_IEnumerable">The <see cref="IEnumerable{T}"/> containing the data to format.</param>
        /// <param name="_PrintName">A boolean flag indicating if the property names should be included in the output.</param>
        /// <param name="_AddNewLineAtEnd">A boolean flag indicating if a newline should be added at the end of the output.</param>
        /// <param name="_Title">An expression that selects the title property from the collection's elements.</param>
        /// <param name="_Entries">An array of expressions that select properties from the collection's elements to include in the output.</param>
        /// <typeparam name="T">The type of the elements in the collection.</typeparam>
        /// <returns>A formatted string representation of the collection elements.</returns>
        public static string PrintPretty<T>(this IEnumerable<T> _IEnumerable, bool _PrintName = true, bool _AddNewLineAtEnd = true, Expression<Func<T, object?>>? _Title = null, params Expression<Func<T, object?>>[] _Entries)
        {
            return _IEnumerable.ToArray().PrintPretty(_PrintName, _AddNewLineAtEnd, _Title, _Entries);
        }
        
        /// <summary>
        /// Returns the elements at the given <c>_Indices</c> in this <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="_IEnumerable">The <see cref="IEnumerable{T}"/> to be filtered.</param>
        /// <param name="_Indices">The indices of the elements in this <see cref="IEnumerable{T}"/> to return.</param>
        /// <typeparam name="T">Can be any <see cref="Type"/>.</typeparam>
        /// <returns>A <see cref="Array"/> containing the elements of this <see cref="IEnumerable{T}"/> at the given <c>_Indices</c>.</returns>
        public static T[] Take<T>(this IEnumerable<T> _IEnumerable, params int[] _Indices)
        {
            using var _enumerator = _IEnumerable.GetEnumerator();
            var _array = new T[_Indices.Length];
            var _enumerableIndex = 0;
            var _arrayIndex = 0;
            
            while (_enumerator.MoveNext())
            {
                if (_Indices.Contains(_enumerableIndex))
                {
                    _array[_arrayIndex++] = _enumerator.Current;
                }
                
                _enumerableIndex++;
            }

            return _array;
        }
        
#if ODIN_INSPECTOR
        /// <summary>
        /// Creates a new <see cref="Data.SerializedDictionary{K,V}"/> from this <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="_IEnumerable">The <see cref="IEnumerable{T}"/> to create a <see cref="Data.SerializedDictionary{K,V}"/> from.</param>
        /// <param name="_Key"><see cref="Data.SerializedKeyValuePair{K,V}.Key"/>.</param>
        /// <param name="_Value"><see cref="Data.SerializedKeyValuePair{K,V}.Value"/>.</param>
        /// <typeparam name="T">The <see cref="Type"/> of the elements inside the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <typeparam name="K">The <see cref="Type"/> of the <see cref="Data.SerializedDictionary{K,V}.Keys"/>.</typeparam>
        /// <typeparam name="V">The <see cref="Type"/> of the <see cref="Data.SerializedDictionary{K,V}.Values"/>.</typeparam>
        /// <returns>A new <see cref="Data.SerializedDictionary{K,V}"/>.</returns>
        public static SerializedDictionary<K,V> ToSerializedDictionary<T,K,V>(this IEnumerable<T> _IEnumerable, Func<T,K> _Key, Func<T,V> _Value)
        {
            return new SerializedDictionary<K, V>(_IEnumerable.ToDictionary(_Key, _Value));
        }
#endif
        #endregion
    }
}
