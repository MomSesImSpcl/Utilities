#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Random = UnityEngine.Random;

namespace IfLoooop.Extensions
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
        /// <param name="_Enumerable">The <see cref="IEnumerable{T}"/> to search in.</param>
        /// <param name="_Match">The condition to search for.</param>
        /// <typeparam name="T">Can be any <see cref="Type"/>.</typeparam>
        /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by match, if found; otherwise, -1.</returns>
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
        /// Returns the first element of a sequence, or <c>_DefaultValue</c> if no element is found.
        /// </summary>
        /// <param name="_Enumerable">The <see cref="IEnumerable{T}"/> to search in.</param>
        /// <param name="_Match">The condition to search for.</param>
        /// <param name="_DefaultValue">The value to return when no element could be found.</param>
        /// <typeparam name="T">Can be any <see cref="Type"/>.</typeparam>
        /// <returns>The first element of a sequence, or <c>_DefaultValue</c> if no element is found.</returns>
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
        /// Returns a random element from the given <c>_Collection</c>.
        /// </summary>
        /// <param name="_Enumerable">The <c>_Collection</c> to get a random element from.</param>
        /// <typeparam name="T">Can be any <see cref="Type"/>.</typeparam>
        /// <returns>A random element from the given <c>_Collection</c>.</returns>
        public static T GetRandom<T>(this IEnumerable<T> _Enumerable)
        {
            var _array = _Enumerable.ToArray();
            var _index = Random.Range(0, _array.Length);

            return _array[_index];
        }
        
        /// <summary>
        /// Returns a random amount of unique elements from the given <c>_Collection</c>.
        /// </summary>
        /// <param name="_Enumerable">The collection to get the random elements from.</param>
        /// <param name="_MaxAmount">The maximum amount of elements to return. (0 = <c>_Collection.Count()</c>)</param>
        /// <typeparam name="T">Can be any <see cref="Type"/>.</typeparam>
        /// <returns>A random amount of unique elements from the given <c>_Collection</c>.</returns>
        public static IEnumerable<T> GetRandomAmount<T>(this IEnumerable<T> _Enumerable, int _MaxAmount = 0)
        {
            var _array = _Enumerable.ToArray();
            var _take = Random.Range(1, _MaxAmount > 0 ? _MaxAmount : _array.Length);
            
            return _array.OrderBy(_ => Guid.NewGuid()).Take(_take);
        }
        
        /// <summary>
        /// Formats and aligns the output of this collection into a table-like representation of the given <c>_Entries</c> as a <c>.json</c>-<see cref="string"/>.
        /// </summary>
        /// <param name="_Enumerable">The collection that holds the given <c>_Entries</c>.</param>
        /// <param name="_Indentations">Amount of preceding whitespaces.</param>
        /// <param name="_Entries">The values to print.</param>
        /// <typeparam name="T">Can be any <see cref="Type"/>.</typeparam>
        /// <returns>A table-like representation of the given <c>_Entries</c> as a <c>.json</c>-<see cref="string"/>.</returns>
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
        /// Formats and aligns the output of this collection into a table-like representation of the given <c>_Entries</c>.
        /// </summary>
        /// <param name="_Enumerable">The collection that holds the given <c>_Entries</c>.</param>
        /// <param name="_Title">Will be displayed as the first value.</param>
        /// <param name="_Entries">The values to print.</param>
        /// <typeparam name="T">Can be any <see cref="Type"/>.</typeparam>
        /// <returns>A table-like representation of the given <c>_Entries</c>.</returns>
        public static string PrintPretty<T>(this IEnumerable<T> _Enumerable, Expression<Func<T, object?>>? _Title = null, params Expression<Func<T, object?>>[] _Entries)
        {
            return PrintPretty(_Enumerable, true, true, _Title, _Entries);
        }
        
        /// <summary>
        /// Formats and aligns the output of this collection into a table-like representation of the given <c>_Entries</c>.
        /// </summary>
        /// <param name="_Enumerable">The collection that holds the given <c>_Entries</c>.</param>
        /// <param name="_PrintName">Set to <c>false</c> to not print the name.</param>
        /// <param name="_Title">Will be displayed as the first value.</param>
        /// <param name="_Entries">The values to print.</param>
        /// <typeparam name="T">Can be any <see cref="Type"/>.</typeparam>
        /// <returns>A table-like representation of the given <c>_Entries</c>.</returns>
        public static string PrintPretty<T>(this IEnumerable<T> _Enumerable, bool _PrintName, Expression<Func<T, object?>>? _Title = null, params Expression<Func<T, object?>>[] _Entries)
        {
            return PrintPretty(_Enumerable, _PrintName, true, _Title, _Entries);
        }
        
        /// <summary>
        /// Formats and aligns the output of this collection into a table-like representation of the given <c>_Entries</c>.
        /// </summary>
        /// <param name="_Enumerable">The collection that holds the given <c>_Entries</c>.</param>
        /// <param name="_PrintName">Set to <c>false</c> to not print the name.</param>
        /// <param name="_AddNewLineAtEnd">Set to <c>false</c> to not add a <see cref="Environment.NewLine"/> at the..</param>
        /// <param name="_Title">Will be displayed as the first value.</param>
        /// <param name="_Entries">The values to print.</param>
        /// <typeparam name="T">Can be any <see cref="Type"/>.</typeparam>
        /// <returns>A table-like representation of the given <c>_Entries</c>.</returns>
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
        #endregion
    }
}