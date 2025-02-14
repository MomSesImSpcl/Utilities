#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using MomSesImSpcl.Data;
using UnityEngine;
using Random = UnityEngine.Random;

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
        /// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the entire
        /// </summary>
        /// <param name="_Enumerable">The <see cref="IEnumerable{T}"/> to search on</param>
        /// <param name="_Match">The condition to search for</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by match, if found; otherwise, -1</returns>
        public static int FindIndex<T>(this IEnumerable<T> _Enumerable, Predicate<T> _Match)
        {
            var _count = 0;
            using var _enumerator = _Enumerable.GetEnumerator();
            
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
        /// Searches for an element that matches the conditions defined by the specified predicate in parallel, and returns the zero-based index of the first occurrence within the entire sequence.
        /// </summary>
        /// <param name="_Enumerable">The <see cref="IEnumerable{T}"/> to search on</param>
        /// <param name="_Match">The condition to search for</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by match, if found; otherwise, -1</returns>
        public static int FindIndexParallel<T>(this IEnumerable<T> _Enumerable, Predicate<T> _Match)
        {
            var _array = _Enumerable as T[] ?? _Enumerable.ToArray();
            var _index = ParallelEnumerable.Range(0, _array.Length).FirstOrDefault(_Index => _Match(_array[_Index]));

            return _index != -1 ? _index : -1;
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate and returns the first occurrence within the entire enumerable or the provided default value if no match is found.
        /// </summary>
        /// <param name="_Enumerable">The <see cref="IEnumerable{T}"/> to search on.</param>
        /// <param name="_Match">The predicate to match elements against.</param>
        /// <param name="_DefaultValue">The default value to return if no match is found.</param>
        /// <typeparam name="T">The type of elements in the enumerable.</typeparam>
        /// <returns>The first occurrence of an element that matches the conditions defined by the predicate, or the default value if no match is found.</returns>
        public static T? FirstOrDefault<T>(this IEnumerable<T> _Enumerable, Predicate<T> _Match, T? _DefaultValue)
        {
            using var _enumerator = _Enumerable.GetEnumerator();
            
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
        /// Returns a random element from the specified enumerable.
        /// </summary>
        /// <param name="_Enumerable">The <see cref="IEnumerable{T}"/> to select a random element from.</param>
        /// <typeparam name="T">The type of the elements in the enumerable.</typeparam>
        /// <returns>A randomly selected element from the enumerable.</returns>
        public static T GetRandom<T>(this IEnumerable<T> _Enumerable)
        {
            var _array = _Enumerable.ToArray();
            var _index = Random.Range(0, _array.Length);
            
#if UNITY_EDITOR // TODO: Sometimes Index out of Range exception, no idea why.
            if (_index == -1 || _index >= _array.Length || _array.Length == 0)
            {
                Debug.LogError($"Index: {_index} | Array: {_array.Length}");
            }
#endif
            return _array[_index];
        }
        
        /// <summary>
        /// Returns random elements from the given enumerable.
        /// </summary>
        /// <param name="_Enumerable">The <see cref="IEnumerable{T}"/> to select random elements from.</param>
        /// <param name="_Amount">The number of elements to return.</param>
        /// <param name="_CanContainDuplicates">
        /// Set to <c>false</c> to only return unique elements. <br/>
        /// <b>The given <see cref="IEnumerable{T}"/> must contain at least as many elements as the requested amount.</b>
        /// </param>
        /// <typeparam name="T">The type of the elements in the enumerable.</typeparam>
        /// <returns>Randomly selected elements from the enumerable.</returns>
        /// <exception cref="ArgumentException">Thrown if the enumerable does not contain enough unique elements.</exception>
        public static IEnumerable<T> GetRandom<T>(this IEnumerable<T> _Enumerable, uint _Amount, bool _CanContainDuplicates)
        {
            var _array = _Enumerable.ToArray();
            var _random = new System.Random();

            switch (_CanContainDuplicates)
            {
                case false when _array.Length < _Amount:
                    throw new ArgumentException($"The enumerable does not contain enough unique elements. Required: {_Amount}, Available: {_array.Length}");
                case true:
                {
                    // ReSharper disable once InconsistentNaming
                    for (var i = 0; i < _Amount; i++)
                    {
                        yield return _array[_random.Next(_array.Length)];
                    }

                    break;
                }
                default:
                {
                    _array.Shuffle();
                    
                    // ReSharper disable once InconsistentNaming
                    for (var i = 0; i < _Amount; i++)
                    {
                        yield return _array[i];
                    }

                    break;
                }
            }
        }
        
        /// <summary>
        /// Returns a weighted random selection of elements from this <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="_Enumerable">The enumerable from which to select the elements.</param>
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
        public static IEnumerable<T> GetWeightedRandom<T,V>(this IEnumerable<T> _Enumerable, Func<T,V> _Value, IDictionary<V,uint> _Weights, uint _Amount, bool _CanContainDuplicates)
        {
            var _weightedElements = _Enumerable.Select(_Element => new
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

            var _random = new System.Random();
            
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
        /// <param name="_Enumerable">The <see cref="IEnumerable{T}"/> to select from</param>
        /// <param name="_MaxAmount">The maximum number of elements to select. If 0 or not specified, selects up to the total length of the collection</param>
        /// <typeparam name="T">The type of elements in the collection</typeparam>
        /// <returns>A random subset of elements from the original collection</returns>
        public static IEnumerable<T> GetRandomAmount<T>(this IEnumerable<T> _Enumerable, int _MaxAmount = 0)
        {
            var _array = _Enumerable.ToArray();
            var _take = Random.Range(1, _MaxAmount > 0 ? _MaxAmount : _array.Length);
            
            return _array.OrderBy(_ => Guid.NewGuid()).Take(_take);
        }

        /// <summary>
        /// Converts an <see cref="IEnumerable{T}"/> to a formatted JSON string with specified indentations and selected properties.
        /// </summary>
        /// <param name="_Enumerable">The enumerable collection to convert to JSON.</param>
        /// <param name="_Indentations">The number of spaces to use for indentation.</param>
        /// <param name="_Entries">Expressions specifying the properties to include in the JSON output.</param>
        /// <typeparam name="T">The type of the elements in the collection.</typeparam>
        /// <returns>A JSON string representing the collection with formatted entries and specified indentations.</returns>
        public static string PrettyJson<T>(this IEnumerable<T> _Enumerable, int _Indentations, params Expression<Func<T, object?>>[] _Entries)
        {
            var _array = _Enumerable.ToArray();
            var _entries = new List<Expression<Func<T, object?>>>(_Entries);
            var _lengths = new int[_entries.Count];
            var _output = new KeyValuePair<string, object?>[_array.Length][];
            
            // ReSharper disable once InconsistentNaming
            for (var i = 0; i < _array.Length; i++)
            {
                _output[i] = new KeyValuePair<string, object?>[_entries.Count];

                // ReSharper disable once InconsistentNaming
                for (var j = 0; j < _entries.Count; j++)
                {
                    MemberExpression? _memberExpression;

                    if (_entries[j].Body is UnaryExpression { Operand: MemberExpression } _unaryExpression)
                    {
                        // For boxed value types. (i.e., bool)
                        _memberExpression = _unaryExpression.Operand as MemberExpression;
                    }
                    else
                    {
                        // For reference types or direct member expressions.
                        _memberExpression = _entries[j].Body as MemberExpression;
                    }
                    
                    var _name = $"\"{_memberExpression!.Member.Name}\": ";
                    // ReSharper disable once RedundantAssignment
                    var _value = string.Empty;
                    var _lastElement = j != _entries.Count - 1 ? ", " : string.Empty;
                    
                    var _object = _entries[j].Compile().Invoke(_array[i]);
                    // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
                    if (_object is IEnumerable _enumerable and not string)
                    {
                        _value = $"[{string.Join(", ", from object? _entry in _enumerable select $"\"{_entry}\"")}]{_lastElement}";
                    }
                    else
                    {
                        _value = $"{(_object != null ? $"\"{_object}\"" : "null")}{_lastElement}";
                    }
                    
                    var _columnLength = _name.Length + _value.Length;
                    
                    _output[i][j] = new KeyValuePair<string, object?>(_name, _value);

                    if (_lengths[j] < _columnLength)
                    {
                        _lengths[j] = _columnLength;
                    }
                }
            }
            
            var _message = string.Empty;

            // ReSharper disable once InconsistentNaming
            for (var i = 0; i < _output.Length; i++)
            {
                _message += string.Empty.PadLeft(_Indentations) + "{ ";
                
                // ReSharper disable once InconsistentNaming
                for (var j = 0; j < _output[i].Length; j++)
                {
                    var _name = _output[i][j].Key;
                    var _value = _output[i][j].Value;
                    var _length = _lengths[j];
                    
                    var _string = $"{_name}{_value}";
                    
                    _message += _string.PadRight(_length);
                }

                _message += " }" + (i != _output.Length - 1 ? ",\n" : string.Empty);
            }
            
            return _message;
        }

        /// <summary>
        /// Prints the elements of the enumerable in a formatted, human-readable string.
        /// </summary>
        /// <param name="_Enumerable">The <see cref="IEnumerable{T}"/> to format and print.</param>
        /// <param name="_Title">An optional title to use for the formatted output.</param>
        /// <param name="_Entries">The properties or fields of the elements to include in the formatted output.</param>
        /// <typeparam name="T">The type of elements in the enumerable.</typeparam>
        /// <returns>A formatted string representing the elements of the enumerable.</returns>
        public static string PrintPretty<T>(this IEnumerable<T> _Enumerable, Expression<Func<T, object?>>? _Title = null, params Expression<Func<T, object?>>[] _Entries)
        {
            return PrintPretty(_Enumerable, true, true, _Title, _Entries);
        }

        /// <summary>
        /// Generates a formatted string representing the provided collection, with options for including a title and specific entries.
        /// </summary>
        /// <param name="_Enumerable">The <see cref="IEnumerable{T}"/> to be formatted.</param>
        /// <param name="_PrintName">A boolean flag indicating if the property names should be included in the output.</param>
        /// <param name="_Title">An optional expression defining the title for the formatted output.</param>
        /// <param name="_Entries">Expressions defining the entries to be included in the formatted output.</param>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <returns>A string representing the formatted output of the collection.</returns>
        public static string PrintPretty<T>(this IEnumerable<T> _Enumerable, bool _PrintName, Expression<Func<T, object?>>? _Title = null, params Expression<Func<T, object?>>[] _Entries)
        {
            return PrintPretty(_Enumerable, _PrintName, true, _Title, _Entries);
        }

        /// <summary>
        /// Formats the elements of the specified collection into a readable string representation.
        /// </summary>
        /// <param name="_Enumerable">The <see cref="IEnumerable{T}"/> containing the data to format.</param>
        /// <param name="_PrintName">A boolean flag indicating if the property names should be included in the output.</param>
        /// <param name="_AddNewLineAtEnd">A boolean flag indicating if a newline should be added at the end of the output.</param>
        /// <param name="_Title">An expression that selects the title property from the collection's elements.</param>
        /// <param name="_Entries">An array of expressions that select properties from the collection's elements to include in the output.</param>
        /// <typeparam name="T">The type of the elements in the collection.</typeparam>
        /// <returns>A formatted string representation of the collection elements.</returns>
        public static string PrintPretty<T>(this IEnumerable<T> _Enumerable, bool _PrintName = true, bool _AddNewLineAtEnd = true, Expression<Func<T, object?>>? _Title = null, params Expression<Func<T, object?>>[] _Entries)
        {
            var _array = _Enumerable.ToArray();
            var _entries = new List<Expression<Func<T, object?>>>(_Entries);

            var _colon = string.Empty;
            var _dash = string.Empty;
            var _title = string.Empty;
            var _additionalTitleCharacters = 1;
            var _additionalColumnCharacters = 0;
            
            if (_Title != null)
            {
                _entries.Insert(0, _Title);
                _title = "[{0}]";
            }
            
            var _lengths = new int[_entries.Count];
            var _output = new KeyValuePair<string, object?>[_array.Length][];
            
            _additionalTitleCharacters += Regex.Replace(_title, "[{]\\d+[}]", string.Empty).Length;
            
            if (_PrintName)
            {
                _colon = ": ";
                
                _additionalColumnCharacters += _colon.Length;   
            }
            // ReSharper disable once RedundantIfElseBlock
            else
            {
                if (_Title == null)
                {
                    _dash = "-";
                    
                    _additionalColumnCharacters += _dash.Length;
                }
            }
            
            // ReSharper disable once InconsistentNaming
            for (var i = 0; i < _array.Length; i++)
            {
                _output[i] = new KeyValuePair<string, object?>[_entries.Count];

                // ReSharper disable once InconsistentNaming
                for (var j = 0; j < _entries.Count; j++)
                {
                    var _name = string.Empty;
                    
                    if (_PrintName)
                    {
                        MemberExpression? _memberExpression;

                        if (_entries[j].Body is UnaryExpression { Operand: MemberExpression } _unaryExpression)
                        {
                            // For boxed value types. (i.e., bool)
                            _memberExpression = _unaryExpression.Operand as MemberExpression;
                        }
                        else
                        {
                            // For reference types or direct member expressions.
                            _memberExpression = _entries[j].Body as MemberExpression;
                        }   
                        
                        _name = _memberExpression!.Member.Name;
                    }
                    
                    // ReSharper disable once RedundantAssignment
                    var _value = string.Empty;
                    var _object = _entries[j].Compile().Invoke(_array[i]);
                    if (_object is IEnumerable _enumerable and not string)
                    {
                        _value = $"{string.Join(", ", from object? _entry in _enumerable select _entry)}";
                    }
                    else
                    {
                        _value = _object != null ? _object.ToString() : "null";
                    }
                    
                    var _columnLength = _name.Length + (_value?.Length ?? 0);

                    var _isTitle = _Title != null && j == 0;
                    if (_isTitle)
                    {
                        _columnLength += _additionalTitleCharacters;
                    }
                    else
                    {
                        _columnLength += _additionalColumnCharacters;
                    }
                    
                    _output[i][j] = new KeyValuePair<string, object?>(_name, _value);

                    if (_lengths[j] < _columnLength)
                    {
                        _lengths[j] = _columnLength;
                    }
                }
            }
            
            var _message = string.Empty;
            
            foreach (var _keyValuePairs in _output)
            {
                // ReSharper disable once InconsistentNaming
                for (var i = 0; i < _keyValuePairs.Length; i++)
                {
                    var _name = _keyValuePairs[i].Key;
                    var _value = _keyValuePairs[i].Value;
                    var _length = _lengths[i];
                    
                    string _string;
                    
                    var _isTitle = _Title != null && i == 0;
                    if (_isTitle)
                    {
                        _string = string.Format($"{_title} ", _value);
                        _length -= _name.Length;
                    }
                    else
                    {
                        _string = i == 0 ? $"{_dash}{_name}{_colon}{_value}" : $"{_name}{_colon}{_value}";
                    }
                    
                    _message += _string.PadRight(_length);
                    
                    var _isNotLast = i != _keyValuePairs.Length - 1;
                    if (!_isTitle && _isNotLast)
                    {
                        _message += " | ";
                    }
                }

                if (_AddNewLineAtEnd)
                {
                    _message += "\n";
                }
            }

            return _message;
        }

        /// <summary>
        /// Returns the elements at the given <c>_Indices</c> in this <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="_Enumerable">The <see cref="IEnumerable{T}"/> to be filtered.</param>
        /// <param name="_Indices">The indices of the elements in this <see cref="IEnumerable{T}"/> to return.</param>
        /// <typeparam name="T">Can be any <see cref="Type"/>.</typeparam>
        /// <returns>A <see cref="List{T}"/> containing the elements of this <see cref="IEnumerable{T}"/> at the given <c>_Indices</c>.</returns>
        public static List<T> Take<T>(this IEnumerable<T> _Enumerable, params int[] _Indices)
        {
            using var _enumerator = _Enumerable.GetEnumerator();
            var _list = new List<T>(_Indices.Length);
            var _counter = 0;
            
            while (_enumerator.MoveNext())
            {
                if (_Indices.Contains(_counter))
                {
                    _list.Add(_enumerator.Current);
                }
                
                _counter++;
            }

            return _list;
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
