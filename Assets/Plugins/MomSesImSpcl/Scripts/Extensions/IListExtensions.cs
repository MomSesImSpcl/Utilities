#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Random = UnityEngine.Random;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="IList{T}"/>
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class IListExtensions
    {
        #region Methods
        /// <summary>
        /// Removes and returns the first item in the list.
        /// </summary>
        /// <param name="_IList">The list from which the first item will be removed and returned.</param>
        /// <typeparam name="T">The <see cref="Type"/> of the elements in the list.</typeparam>
        /// <returns>The first item that was removed from the list.</returns>
        public static T Dequeue<T>(this IList<T> _IList)
        {
            var _firstEntry = _IList[0];
            _IList.RemoveAt(0);

            return _firstEntry;
        }

        /// <summary>
        /// Tries to remove and return the first item in the <see cref="IList{T}"/>.
        /// </summary>
        /// <param name="_IList">The list from which the first item will be removed and returned.</param>
        /// <param name="_Item">The first item that was removed from the <see cref="IList{T}"/> or <c>null</c>.</param>
        /// <typeparam name="T">The <see cref="Type"/> of the elements in the list.</typeparam>
        /// <returns><c>true</c> if the item was removed, otherwise <c>false</c>.</returns>
        public static bool TryDequeue<T>(this IList<T> _IList, out T? _Item)
        {
            if (_IList.Count > 0)
            {
                _Item = _IList.Dequeue();
                return true;
            }

            _Item = default;
            return false;
        }
        
        /// <summary>
        /// Returns a random element from this <see cref="Array"/>.
        /// </summary>
        /// <param name="_IList">The <see cref="Array"/> to select a random element from.</param>
        /// <typeparam name="T">The <see cref="Type"/> of the elements in the <see cref="Array"/>.</typeparam>
        /// <returns>A randomly selected element from the <see cref="Array"/>.</returns>
        public static T GetRandom<T>(this IList<T> _IList)
        {
            return _IList[Random.Range(0, _IList.Count)];
        }
        
        /// <summary>
        /// Returns random elements from the given <see cref="IList{T}"/>.
        /// </summary>
        /// <param name="_IList">The <see cref="IList{T}"/> to select random elements from.</param>
        /// <param name="_Amount">The number of elements to return.</param>
        /// <param name="_CanContainDuplicates">
        /// Set to <c>false</c> to only return unique elements. <br/>
        /// <b>The given <see cref="IList{T}"/> must contain at least as many elements as the requested amount.</b>
        /// </param>
        /// <typeparam name="T">The <see cref="Type"/> of the elements in the <see cref="IList{T}"/>.</typeparam>
        /// <returns>Randomly selected elements from the enumerable.</returns>
        /// <exception cref="ArgumentException">Thrown if the enumerable does not contain enough unique elements.</exception>
        public static IEnumerable<T> GetRandom<T>(this IList<T> _IList, uint _Amount, bool _CanContainDuplicates)
        {
            var _random = new System.Random();
            
            switch (_CanContainDuplicates)
            {
                case false when _IList.Count < _Amount:
                    throw new ArgumentException($"The enumerable does not contain enough unique elements. Required: {_Amount}, Available: {_IList.Count}");
                case true:
                {
                    // ReSharper disable once InconsistentNaming
                    for (var i = 0; i < _Amount; i++)
                    {
                        yield return _IList[_random.Next(_IList.Count)];
                    }
            
                    break;
                }
                default:
                {
                    _IList.Shuffle();
                    
                    // ReSharper disable once InconsistentNaming
                    for (var i = 0; i < _Amount; i++)
                    {
                        yield return _IList[i];
                    }
            
                    break;
                }
            }
        }
        
        /// <summary>
        /// Selects a random subset of elements from the given <see cref="IList{T}"/>.
        /// </summary>
        /// <param name="_IList">The <see cref="IList{T}"/> to select from</param>
        /// <param name="_MaxAmount">The maximum number of elements to select. If 0 or not specified, selects up to the total length of the collection</param>
        /// <typeparam name="T">The <see cref="Type"/> of elements in the collection</typeparam>
        /// <returns>A random subset of elements from the original collection</returns>
        public static IEnumerable<T> GetRandomAmount<T>(this IList<T> _IList, int _MaxAmount = 0)
        {
            var _take = Random.Range(1, _MaxAmount > 0 ? _MaxAmount : _IList.Count);
            
            return _IList.OrderBy(_ => Guid.NewGuid()).Take(_take);
        }

        /// <summary>
        /// Returns the median value from a collection of numbers.
        /// </summary>
        /// <param name="_IList">The <see cref="IList{T}"/> to get the median of.</param>
        /// <param name="_Sort">Set this to <c>false</c> if the collection is already sorted.</param>
        /// <typeparam name="T">Must be a numeric <see cref="Type"/>.</typeparam>
        /// <returns>The median value as a <see cref="decimal"/>.</returns>
        public static decimal Median<T>(this IList<T> _IList, bool _Sort = true) where T : unmanaged, IFormattable
        {
            var _collection = _Sort ? _IList.OrderBy(_Numbers => _Numbers).ToArray() : _IList;
            var _count = _collection.Count;
            var _midIndex = _count / 2;
            
            if (_count % 2 == 0)
            {
                var _left = Convert.ToDecimal(_collection[_midIndex - 1]);
                var _right = Convert.ToDecimal(_collection[_midIndex]);
                
                return (_left + _right) * .5m;
            }

            return Convert.ToDecimal(_collection[_midIndex]);
        }
        
        /// <summary>
        /// Moves an item from one index to another within the list.
        /// </summary>
        /// <param name="_IList">The list containing the item to be moved.</param>
        /// <param name="_OldIndex">The current index of the item.</param>
        /// <param name="_NewIndex">The new index to which the item should be moved.</param>
        /// <typeparam name="T">The type of the elements in the list.</typeparam>
        public static void Move<T>(this IList<T> _IList, int _OldIndex, int _NewIndex)
        {
            var _item = _IList[_OldIndex];
            
            _IList.RemoveAt(_OldIndex);
            _IList.Insert(_NewIndex, _item);
        }

        /// <summary>
        /// Moves the given item to the new index in the list.
        /// </summary>
        /// <param name="_IList">The list containing the item to be moved.</param>
        /// <param name="_Item">The item to move.</param>
        /// <param name="_NewIndex">The new index to which the item should be moved.</param>
        /// <typeparam name="T">The type of the elements in the list.</typeparam>
        public static void Move<T>(this IList<T> _IList, T _Item, int _NewIndex)
        {
            _IList.Remove(_Item);
            _IList.Insert(_NewIndex, _Item);
        }
        
        /// <summary>
        /// Populates this <see cref="IList{T}"/> with elements from the given <c>_Factory</c>-method.
        /// </summary>
        /// <param name="_IList">The <see cref="IList{T}"/> to populate.</param>
        /// <param name="_Amount">The number of elements to <see cref="IList{T}.Add"/> to the <see cref="IList{T}"/>.</param>
        /// <param name="_Factory">Defines how the elements should be created.</param>
        /// <typeparam name="T">The <see cref="Type"/> of the <see cref="IList{T}"/>.</typeparam>
        /// <typeparam name="N">Must be a numeric <see cref="Type"/> that fits inside a <see cref="uint"/>.</typeparam>
        /// <returns>The populated <see cref="IList{T}"/>.</returns>
        public static IList<T> Populate<T,N>(this IList<T> _IList, N _Amount, Func<T> _Factory) where N : unmanaged, IFormattable
        {
            return _IList.As<ICollection<T>>().Populate(_Amount, _Factory).As<IList<T>>()!;
        }
        
        /// <summary>
        /// Converts an <see cref="IList{T}"/> to a formatted JSON string with specified indentations and selected properties.
        /// </summary>
        /// <param name="_IList">The enumerable collection to convert to JSON.</param>
        /// <param name="_Indentations">The number of spaces to use for indentation.</param>
        /// <param name="_Entries">Expressions specifying the properties to include in the JSON output.</param>
        /// <typeparam name="T">The <see cref="Type"/> of the elements in the collection.</typeparam>
        /// <returns>A JSON string representing the collection with formatted entries and specified indentations.</returns>
        public static string PrettyJson<T>(this IList<T> _IList, int _Indentations, params Expression<Func<T, object?>>[] _Entries)
        {
            var _entries = new List<Expression<Func<T, object?>>>(_Entries);
            var _lengths = new int[_entries.Count];
            var _output = new KeyValuePair<string, object?>[_IList.Count][];
            
            // ReSharper disable once InconsistentNaming
            for (var i = 0; i < _IList.Count; i++)
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
                    
                    var _object = _entries[j].Compile().Invoke(_IList[i]);
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
        /// <param name="_IList">The <see cref="IList{T}"/> to format and print.</param>
        /// <param name="_Title">An optional title to use for the formatted output.</param>
        /// <param name="_Entries">The properties or fields of the elements to include in the formatted output.</param>
        /// <typeparam name="T">The type of elements in the enumerable.</typeparam>
        /// <returns>A formatted string representing the elements of the enumerable.</returns>
        public static string PrintPretty<T>(this IList<T> _IList, Expression<Func<T, object?>>? _Title = null, params Expression<Func<T, object?>>[] _Entries)
        {
            return PrintPretty(_IList, true, true, _Title, _Entries);
        }

        /// <summary>
        /// Generates a formatted string representing the provided collection, with options for including a title and specific entries.
        /// </summary>
        /// <param name="_IList">The <see cref="IList{T}"/> to be formatted.</param>
        /// <param name="_PrintName">A boolean flag indicating if the property names should be included in the output.</param>
        /// <param name="_Title">An optional expression defining the title for the formatted output.</param>
        /// <param name="_Entries">Expressions defining the entries to be included in the formatted output.</param>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <returns>A string representing the formatted output of the collection.</returns>
        public static string PrintPretty<T>(this IList<T> _IList, bool _PrintName, Expression<Func<T, object?>>? _Title = null, params Expression<Func<T, object?>>[] _Entries)
        {
            return PrintPretty(_IList, _PrintName, true, _Title, _Entries);
        }

        /// <summary>
        /// Formats the elements of the specified collection into a readable string representation.
        /// </summary>
        /// <param name="_IList">The <see cref="IList{T}"/> containing the data to format.</param>
        /// <param name="_PrintName">A boolean flag indicating if the property names should be included in the output.</param>
        /// <param name="_AddNewLineAtEnd">A boolean flag indicating if a newline should be added at the end of the output.</param>
        /// <param name="_Title">An expression that selects the title property from the collection's elements.</param>
        /// <param name="_Entries">An array of expressions that select properties from the collection's elements to include in the output.</param>
        /// <typeparam name="T">The type of the elements in the collection.</typeparam>
        /// <returns>A formatted string representation of the collection elements.</returns>
        public static string PrintPretty<T>(this IList<T> _IList, bool _PrintName = true, bool _AddNewLineAtEnd = true, Expression<Func<T, object?>>? _Title = null, params Expression<Func<T, object?>>[] _Entries)
        {
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
            var _output = new KeyValuePair<string, object?>[_IList.Count][];
            
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
            for (var i = 0; i < _IList.Count; i++)
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
                    var _object = _entries[j].Compile().Invoke(_IList[i]);
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
        /// Removes the first element in this <see cref="IList{T}"/> that matches the given <c>_Condition</c>. <br/>
        /// <i>If no element inside the <see cref="IList{T}"/> matches the given <c>_Condition</c>, nothing will be removed.</i>
        /// </summary>
        /// <param name="_IList">The <see cref="IList{T}"/> to remove the element from.</param>
        /// <param name="_Condition">The condition that must be met for the element to be removed.</param>
        /// <typeparam name="T">The <see cref="Type"/> of the element.</typeparam>
        public static void Remove<T>(this IList<T> _IList, Predicate<T> _Condition)
        {
            var _index = _IList.FindIndex(_Condition);

            if (_index != -1)
            {
                _IList.RemoveAt(_index);
            }
        }
        
        /// <summary>
        /// Randomizes the order of elements in the list. <br/>
        /// <i>Uses the Fisher-Yates shuffle.</i>
        /// </summary>
        /// <param name="_IList">The list to be shuffled.</param>
        /// <typeparam name="T">The type of the elements in the list.</typeparam>
        /// <returns>The shuffled <see cref="IList{T}"/>.</returns>
        public static IList<T> Shuffle<T>(this IList<T> _IList)
        {
            var _random = new System.Random();
            
            // ReSharper disable once InconsistentNaming
            for (var i = _IList.Count - 1; i > 0; i--)
            {
                // ReSharper disable once InconsistentNaming
                var j = _random.Next(i + 1);
                
                (_IList[i], _IList[j]) = (_IList[j], _IList[i]);
            }

            return _IList;
        }
        
        /// <summary>
        /// Removes an element from the <see cref="IList{T}"/> by swapping it with the last element and then removing the last element. <br/>
        /// <b>Don't use this when the order of the elements matters.</b>
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of elements in the <see cref="IList{T}"/>.</typeparam>
        /// <param name="_IList">The <see cref="IList{T}"/> from which to remove an element.</param>
        /// <param name="_Index">The index of the element to remove.</param>
        public static void SwapAndPop<T>(this IList<T> _IList, int _Index)
        {
            var _lastIndex = _IList.Count - 1;
            _IList[_Index] = _IList[_lastIndex];
            _IList.RemoveAt(_lastIndex);
        }

        /// <summary>
        /// Tries to retrieve the element at the given <c>_Index</c>. <br/>
        /// </summary>
        /// <param name="_IList">The <see cref="IList{T}"/> to retrieve the element from.</param>
        /// <param name="_Index">The index to retrieve the element at.</param>
        /// <param name="_Element">Will hold the retrieved element or <c>null</c>, if the given <c>_Index</c> was out of range.</param>
        /// <typeparam name="T">Must be a nullable <see cref="Type"/>.</typeparam>
        /// <returns><c>true</c> if the element was successfully retrieved, otherwise <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGet<T>(this IList<T> _IList, int _Index, out T? _Element)
        {
            return _Index < _IList.Count ? (_Element = _IList[_Index]).AsBool() : (_Element = default).AsBool();
        }
        #endregion
    }
}
