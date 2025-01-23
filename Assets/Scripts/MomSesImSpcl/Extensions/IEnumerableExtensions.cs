#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
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
            var _multiplier = _array.Length < 4 ? 10 : 1;
            var _index = Mathf.FloorToInt(Random.Range(0, _array.Length * _multiplier) / (float)_multiplier);

            return _array[_index];
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
        #endregion
    }
}
