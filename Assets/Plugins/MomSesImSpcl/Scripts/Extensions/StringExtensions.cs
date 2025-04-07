#nullable enable
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using MomSesImSpcl.Utilities.Logging;
using UnityEngine;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="string"/>.
    /// </summary>
    public static class StringExtensions
    {
        #region Methods
        /// <summary>
        /// Wraps this <see cref="string"/> in a Rich Text bold tag.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to wrap.</param>
        /// <returns>The <see cref="string"/> wrapped in an bold tag.</returns>
        public static string Bold(this string? _String)
        {
            return $"<b>{_String.OrNull()}</b>";
        }
        
        /// <summary>
        /// Wraps this <see cref="string"/> in a Rich Text color tag.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to wrap.</param>
        /// <param name="_Color">The <see cref="RichTextColor"/> to display the <see cref="string"/> in.</param>
        /// <returns>The <see cref="string"/> wrapped in a color tag.</returns>
        public static string Color(this string? _String, RichTextColor _Color)
        {
            return $"<color={_Color.GetName()}>{_String.OrNull()}</color>";
        }
        
        /// <summary>
        /// Converts the specified string to title case.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to convert.</param>
        /// <returns>The converted <see cref="string"/> in title case.</returns>
        public static string ConvertToTitleCase(this string _String)
        {
            return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(_String.ToLower());
        }

        /// <summary>
        /// Inserts spaces before each uppercase letter in a camelCase string.
        /// </summary>
        /// <param name="_String">The camelCase <see cref="string"/> to format.</param>
        /// <returns>The formatted <see cref="string"/> with spaces before each uppercase letter.</returns>
        public static string CamelCaseSpacing(this string _String)
        {
            return new Regex("([a-z])([A-Z])").Replace(_String, "$1 $2");
        }

        /// <summary>
        /// Escapes specific HTML tags in the provided string to avoid parsing them as rich text.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> containing rich text to escape.</param>
        /// <returns>The modified <see cref="string"/> with specific HTML tags escaped.</returns>
        public static string EscapeRichText(this string _String)
        {
            return Regex.Replace(_String, @"(</?)([biua])((?:\s+[^>]*?)?(?:>|$))", _Match =>
            {
                var _prefix = _Match.Groups[1].Value;
                var _tag = _Match.Groups[2].Value;
                var _suffix = _Match.Groups[3].Value;
                
                var _replacement = _tag switch
                {
                    "b" => "β",
                    "i" => "ι",
                    "u" => "υ",
                    "a" => "α",
                    _ => _tag
                };

                return $"{_prefix}{_replacement}{_suffix}";
            });
        }

        /// <summary>
        /// Extracts a substring from the specified string that occurs after the specified pattern.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to search within.</param>
        /// <param name="_StartPattern">The <see cref="string"/> pattern to search for.</param>
        /// <param name="_Length">The length of the substring to extract. If 0, extracts until the end of the string.</param>
        /// <param name="_Occurence">
        /// The occurrence of the pattern to look for. <br/>
        /// <i>Set to &lt; 1 to search for the last occurence.</i>
        /// </param>
        /// <param name="_RightToLeft">Determines the search direction. If true, searches from right to left.</param>
        /// <returns>The extracted substring, or null if the start pattern is not found.</returns>
        public static string? ExtractAfter(this string _String, string _StartPattern, int _Length = 0, uint _Occurence = 1, bool _RightToLeft = false)
        {
            return _String.ExtractAfter(_StartPattern, _RightToLeft, _Length, _Occurence);
        }

        /// <summary>
        /// Extracts a substring from the given string that occurs after the specified pattern.
        /// </summary>
        /// <param name="_Input">The string to search within.</param>
        /// <param name="_Pattern">The pattern to search for.</param>
        /// <param name="_RightToLeft">If true, searches from right to left.</param>
        /// <param name="_Length">The length of the substring to extract. If 0, extracts to the end of the string.</param>
        /// <param name="_Occurrence">
        /// The occurrence of the pattern to look for. <br/>
        /// <i>Set to &lt; 1 to search for the last occurence.</i>
        /// </param>
        /// <returns>The extracted substring, or null if the pattern is not found.</returns>
        public static string? ExtractAfter(this string _Input, string _Pattern, bool _RightToLeft, int _Length = 0, uint _Occurrence = 1)
        {
            if (_Occurrence < 1)
            {
                _RightToLeft = !_RightToLeft;
                _Occurrence = 1;
            }
    
            var _offset = 0;
            
            if (!_RightToLeft)
            {
                var _regex = new Regex(_Pattern);
                
                // ReSharper disable once InconsistentNaming
                for (uint i = 0; i < _Occurrence; i++)
                {
                    var _match = _regex.Match(_Input, _offset);

                    if (!_match.Success)
                    {
                        return null;
                    }
                    
                    _offset = _match.Index + _match.Length;
                }
            }
            else
            {
                var _regex = new Regex(_Pattern, RegexOptions.RightToLeft);
                Match? _match = null;
                
                var _searchLength = _Input.Length;
                
                // ReSharper disable once InconsistentNaming
                for (uint i = 0; i < _Occurrence; i++)
                {
                    _match = _regex.Match(_Input, 0, _searchLength);
                    
                    if (!_match.Success)
                    {
                        return null;
                    }
                    
                    _searchLength = _match.Index;
                }
                
                _offset = _match!.Index + _match.Length;
            }
            
            var _end = _Length > 0 ? Mathf.Min(_offset + _Length, _Input.Length) : _Input.Length;
                
            return _Input[_offset.._end];
        }
        
        /// <summary>
        /// Extracts a substring from the specified string that occurs before the first instance of the given start pattern.
        /// </summary>
        /// <param name="_String">The string to extract from.</param>
        /// <param name="_StartPattern">The start pattern that identifies where the substring begins.</param>
        /// <param name="_Length">The number of characters to extract. If zero, extracts up to the start pattern.</param>
        /// <param name="_Occurence">
        /// The occurrence of the pattern to look for. <br/>
        /// <i>Set to &lt; 1 to search for the last occurence.</i>
        /// </param>
        /// <param name="_RightToLeft">Specifies whether to search from right to left. Default is false.</param>
        /// <returns>The extracted substring or null if the start pattern is not found.</returns>
        public static string? ExtractBefore(this string _String, string _StartPattern, int _Length = 0, uint _Occurence = 1, bool _RightToLeft = false)
        {
            return _String.ExtractBefore(_StartPattern, _RightToLeft, _Length, _Occurence);
        }
        
        /// <summary>
        /// Extracts a substring from the specified string that occurs before the given start pattern.
        /// </summary>
        /// <param name="_Input">The <see cref="string"/> to extract from.</param>
        /// <param name="_StartPattern">The start pattern to look for.</param>
        /// <param name="_RightToLeft">Specifies whether to search right to left. Defaults to false.</param>
        /// <param name="_Length">The length of the substring to be extracted. Defaults to 0.</param>
        /// <param name="_Occurrence">
        /// The occurrence of the pattern to look for. <br/>
        /// <i>Set to &lt; 1 to search for the last occurence.</i>
        /// </param>
        /// <returns>The extracted <see cref="string"/> or null if the start pattern is not found.</returns>
        public static string? ExtractBefore(this string _Input, string _StartPattern, bool _RightToLeft, int _Length = 0, uint _Occurrence = 1)
        {
            if (_Occurrence < 1)
            {
                _RightToLeft = !_RightToLeft;
                _Occurrence = 1;
            }

            var _offset = 0;

            if (!_RightToLeft)
            {
                var _regex = new Regex(_StartPattern);
                var _searchOffset = 0;
                
                // ReSharper disable once InconsistentNaming
                for (uint i = 1; i <= _Occurrence; i++)
                {
                    var _match = _regex.Match(_Input, _searchOffset);

                    if (!_match.Success)
                    {
                        return null;
                    }
                    
                    if (i == _Occurrence)
                    {
                        _offset = _match.Index;
                    }
                    else
                    {
                        _searchOffset = _match.Index + _match.Length;
                    }
                }
            }
            else
            {
                var _regex = new Regex(_StartPattern, RegexOptions.RightToLeft);
                var _searchLength = _Input.Length;
                Match? _match = null;
             
                // ReSharper disable once InconsistentNaming
                for (uint i = 0; i < _Occurrence; i++)
                {
                    _match = _regex.Match(_Input, 0, _searchLength);

                    if (!_match.Success)
                    {
                        return null;
                    }
                    
                    _searchLength = _match.Index;
                }
                
                _offset = _match!.Index;
            }
            
            if (_Length > 0)
            {
                var _start = _offset > _Length ? _offset - _Length : 0;
                
                return _Input[_start.._offset];
            }

            return _Input[.._offset];
        }
        
        /// <summary>
        /// Extracts a substring from the specified string that is found between the given start and end patterns.
        /// </summary>
        /// <param name="_String">The string to search within.</param>
        /// <param name="_StartPattern">The start pattern to find.</param>
        /// <param name="_EndPattern">The end pattern to find.</param>
        /// <param name="_Occurence">
        /// The occurrence of the pattern to look for. <br/>
        /// <i>Set to &lt; 1 to search for the last occurence.</i>
        /// </param>
        /// <param name="_RightToLeft">Determines the direction of the search.</param>
        /// <returns>The extracted substring, or null if the patterns are not found.</returns>
        public static string? ExtractBetween(this string _String, string _StartPattern, string _EndPattern, uint _Occurence = 1, bool _RightToLeft = false)
        {
            return ExtractBetween(_String, _StartPattern, _EndPattern, _RightToLeft, _Occurence);
        }
        
        /// <summary>
        /// Extracts a substring from the specified string that is found between the provided start and end patterns.
        /// </summary>
        /// <param name="_String">The string to extract the substring from.</param>
        /// <param name="_StartPattern">The pattern marking the beginning of the substring to extract.</param>
        /// <param name="_EndPattern">The pattern marking the end of the substring to extract.</param>
        /// <param name="_RightToLeft">Indicates whether to search from right to left.</param>
        /// <param name="_Occurence">
        /// The occurrence of the pattern to look for. <br/>
        /// <i>Set to &lt; 1 to search for the last occurence.</i>
        /// </param>
        /// <returns>The extracted substring, or null if no match is found.</returns>
        public static string? ExtractBetween(this string _String, string _StartPattern, string _EndPattern, bool _RightToLeft, uint _Occurence = 1)
        {
            if (_Occurence < 1)
            {
                _RightToLeft = !_RightToLeft;
                _Occurence = 1;
            }

            var _searchStart = 0;
            var _searchEnd = _String.Length;

            var _combinedRegex = new Regex($"{_StartPattern}.*?{_EndPattern}", _RightToLeft ? RegexOptions.RightToLeft : RegexOptions.None);
            var _startRegex = new Regex(_StartPattern);
            var _endRegex = new Regex(_EndPattern);

            var _start = -1;
            var _length = 0;

            // ReSharper disable once InconsistentNaming
            for (uint i = 0; i < _Occurence; i++)
            {
                var _combinedMatch = _combinedRegex.Match(_String, _searchStart, _searchEnd - _searchStart);
                if (!_combinedMatch.Success)
                    return null;

                var _startMatch = _startRegex.Match(_String, _combinedMatch.Index, _combinedMatch.Length);
                if (!_startMatch.Success)
                    return null;

                var _startPatternEnd = _combinedMatch.Index + _startMatch.Length;
                var _endSearchLength = _combinedMatch.Index + _combinedMatch.Length - _startPatternEnd;

                var _endMatch = _endRegex.Match(_String, _startPatternEnd, _endSearchLength);
                if (!_endMatch.Success)
                    return null;

                var _endPatternStart = _endMatch.Index;
                var _contentLength = _endPatternStart - _startPatternEnd;

                if (i == _Occurence - 1)
                {
                    _start = _startPatternEnd;
                    _length = _contentLength;
                }
                
                if (_RightToLeft)
                {
                    _searchEnd = _endPatternStart;
                }
                else
                {
                    _searchStart = _startPatternEnd;
                }
            }

            return _start == -1 ? null : _String.Substring(_start, _length);
        }
        
        /// <summary>
        /// Finds the common characters at the beginning of every string in the given collection.
        /// </summary>
        /// <param name="_Strings">The collection of strings to find common characters in.</param>
        /// <returns>A string containing the common characters at the beginning of each string in the collection.</returns>
        public static string GetCommonCharacters(this IEnumerable<string> _Strings)
        {
            var _strings = _Strings.ToArray();
            var _string = _strings.FirstOrDefault() ?? string.Empty;
            
            // ReSharper disable once InconsistentNaming
            for (var i = 0; i < _string.Length; i++)
            {
                if (_strings.Any(_String => i >= _String.Length || _String[i] != _string[i]))
                {
                    return _string[..i];
                }
            }
            
            return _string;
        }

        /// <summary>
        /// Converts a hexadecimal color code to a Unity <see cref="Color"/>.
        /// </summary>
        /// <param name="_HexColor">Must be a valid Hexadecimal color and start with a <c>#</c>.</param>
        /// <returns>A <see cref="Color"/> representing the RGB values of the Hexadecimal color code.</returns>
        public static Color HexToRGB(this string _HexColor)
        {
            var _hexColor = _HexColor.AsSpan();
            var _red = int.Parse(_hexColor.Slice(1, 2), NumberStyles.HexNumber);
            var _green = int.Parse(_hexColor.Slice(3, 2), NumberStyles.HexNumber);
            var _blue = int.Parse(_hexColor.Slice(5, 2), NumberStyles.HexNumber);
            var _color = System.Drawing.Color.FromArgb(_red, _green, _blue);
            
            return new Color(_color.R, _color.G, _color.B);
        }
        
        /// <summary>
        /// Determines whether the specified string is empty or consists only of white-space characters.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to check.</param>
        /// <returns><see langword="true"/> if the string is empty or consists only of white-space characters; otherwise, <see langword="false"/>.</returns>
        public static bool IsEmptyOrWhitespace(this string _String)
        {
            return _String == string.Empty || _String.All(_Char => _Char == ' ');
        }

        /// <summary>
        /// Wraps this <see cref="string"/> in a Rich Text italic tag.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to wrap.</param>
        /// <returns>The <see cref="string"/> wrapped in an italic tag.</returns>
        public static string Italic(this string? _String)
        {
            return $"<i>{_String.OrNull()}</i>";
        }
        
        /// <summary>
        /// Finds the nth occurrence of a specified value in the given string.
        /// </summary>
        /// <param name="_String">The string to search within.</param>
        /// <param name="_Value">The value to find.</param>
        /// <param name="_Occurence">The occurrence number to find.</param>
        /// <param name="_StringComparison">The criteria to use for comparison.</param>
        /// <returns>The index of the nth occurrence of the specified value, or -1 if not found.</returns>
        public static int NthIndexOf(this string _String, string _Value, int _Occurence = 1, StringComparison _StringComparison = StringComparison.Ordinal)
        {
            var _index = -1;
            var _startIndex = 0;

            // ReSharper disable once InconsistentNaming
            for (var i = 0; i < _Occurence; i++)
            {
                _index = _String.IndexOf(_Value, _startIndex, _StringComparison);
                
                if (_index == -1)
                    return -1;

                _startIndex = _index + _Value.Length;
            }

            return _index;
        }

        /// <summary>
        /// Finds the nth occurrence of a specified value from the end of the string.
        /// </summary>
        /// <param name="_String">The string in which to search for the value.</param>
        /// <param name="_Value">The value to locate within the string.</param>
        /// <param name="_Occurrence">The occurrence number to locate from the end. Default is 1.</param>
        /// <param name="_StringComparison">The comparison type to use when searching for the value. Default is StringComparison.Ordinal.</param>
        /// <returns>The index position of the nth occurrence of the value from the end, or -1 if the value is not found.</returns>
        public static int NthIndexOfLast(this string _String, string _Value, int _Occurrence = 1, StringComparison _StringComparison = StringComparison.Ordinal)
        {
            var _index = -1;
            var _startIndex = _String.Length;

            for (var _i = 0; _i < _Occurrence; _i++)
            {
                _index = _String.LastIndexOf(_Value, _startIndex - 1, _StringComparison);
        
                if (_index == -1)
                    return -1;
                
                _startIndex = _index;
            }

            return _index;
        }

        /// <summary>
        /// Returns the <see cref="string"/> or <c>null</c> as a <see cref="string"/> if the <see cref="string"/> is <c>null</c>.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to return.</param>
        /// <returns>The <see cref="string"/> or <c>null</c> as a <see cref="string"/> if the <see cref="string"/> is <c>null</c>.</returns>
        public static string OrNull(this string? _String)
        {
            return _String ?? "null";
        }
        
        /// <summary>
        /// Removes the specified character from the input string.
        /// </summary>
        /// <param name="_String">The string from which to remove characters.</param>
        /// <param name="_CharToRemove">The character to remove from the string.</param>
        /// <returns>The modified string after the specified character has been removed.</returns>
        public static string RemoveCharacters(this string _String, char _CharToRemove)
        {
            return _String.RemoveCharacters(StringComparison.Ordinal, _CharToRemove);
        }

        /// <summary>
        /// Removes all specified characters from the given string.
        /// </summary>
        /// <param name="_String">The string from which to remove the specified characters.</param>
        /// <param name="_CharsToRemove">The characters to remove from the string.</param>
        /// <param name="_StringComparison">The comparison type to use when searching for the value. Default is StringComparison.Ordinal.</param>
        /// <returns>A new string with all specified characters removed.</returns>
        public static string RemoveCharacters(this string _String, IEnumerable<char> _CharsToRemove, StringComparison _StringComparison = StringComparison.Ordinal)
        {
            return _String.RemoveCharacters(_StringComparison, _CharsToRemove.ToArray());
        }

        /// <summary>
        /// Removes the specified characters from the given string using a specified comparison rule.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to modify.</param>
        /// <param name="_StringComparison">The <see cref="StringComparison"/> rule to use when removing characters.</param>
        /// <param name="_CharsToRemove">The array of characters to remove from the string.</param>
        /// <returns>The modified <see cref="string"/> with the specified characters removed.</returns>
        public static string RemoveCharacters(this string _String, StringComparison _StringComparison, params char[] _CharsToRemove)
        {
            return _CharsToRemove.Aggregate(_String, (_Current, _Character) => _Current.Replace(_Character.ToString(), string.Empty, _StringComparison));
        }

        /// <summary>
        /// Tries to remove the last occurrence of the specified <see cref="char"/> in this <see cref="string"/>.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to remove the <see cref="char"/> in.</param>
        /// <param name="_CharToRemove">The <see cref="char"/> to remove.</param>
        /// <returns>This <see cref="string"/> with the removed <see cref="char"/> or the original <see cref="string"/> if the <see cref="char"/> couldn't be found.</returns>
        public static string RemoveLast(this string _String, char _CharToRemove)
        {
            if (_String.LastIndexOf(_CharToRemove) is var _index && _index != -1)
            {
                return _String.Remove(_index, 1);
            }

            return _String;
        }
        
        /// <summary>
        /// Tries to remove the last occurrence of the specified <see cref="string"/> in this <see cref="string"/>.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to remove the <see cref="string"/> in.</param>
        /// <param name="_StringToRemove">The <see cref="string"/> to remove.</param>
        /// <param name="_StringComparison">The <see cref="StringComparison"/> to use when searching for the <see cref="string"/>.</param>
        /// <returns>This <see cref="string"/> with the removed <see cref="string"/> or the original <see cref="string"/> if the <see cref="string"/> couldn't be found.</returns>
        public static string RemoveLast(this string _String, string _StringToRemove, StringComparison _StringComparison = StringComparison.Ordinal)
        {
            if (_String.LastIndexOf(_StringToRemove, _StringComparison) is var _index && _index != -1)
            {
                return _String.Remove(_index, _StringToRemove.Length);
            }

            return _String;
        }
        
        /// <summary>
        /// Removes all non-digit characters from the specified string.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to process.</param>
        /// <returns>The processed <see cref="string"/> containing only digit characters.</returns>
        public static string RemoveNonDigits(this string _String)
        {
            return new string(_String.Where(char.IsDigit).ToArray());
        }

        /// <summary>
        /// Removes rich text formatting tags from the specified string.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to process.</param>
        /// <returns>The processed <see cref="string"/> without rich text formatting tags.</returns>
        public static string RemoveRichText(this string _String)
        {
            return Regex.Replace(_String, @"</?(?:b|i|u|size(?:=\d+)?|color(?:=[^>]+)?|a(?:\s+[^>]*)?)>", string.Empty, RegexOptions.Compiled);
        }

        /// <summary>
        /// Removes all ruby text elements from the specified string.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to process.</param>
        /// <returns>The processed <see cref="string"/> with all ruby text elements removed.</returns>
        public static string RemoveRubyText(this string _String)
        {
            return Regex.Replace(_String, "</?ruby.*?>|</?rb>|<rp>.*?</rp>|<rt>.*?</rt>", string.Empty, RegexOptions.Singleline);
        }

        /// <summary>
        /// Removes special characters from the specified string.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> from which to remove special characters.</param>
        /// <param name="_RemoveWhitespaces">Specifies whether to remove whitespaces as well. Default is false.</param>
        /// <returns>The <see cref="string"/> with special characters removed.</returns>
        public static string RemoveSpecialCharacters(this string _String, bool _RemoveWhitespaces = false)
        {
            var _whiteSpace = _RemoveWhitespaces ? string.Empty : "\\s";
            return Regex.Replace(_String, $"[^a-zA-Z0-9{_whiteSpace}]", string.Empty);
        }

        /// <summary>
        /// Removes the specified substring from the current instance of the string.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> instance to perform the operation on.</param>
        /// <param name="_StringsToRemove">The <see cref="string"/>s to remove from the current instance.</param>
        /// <returns>A new <see cref="string"/> with the specified substring removed.</returns>
        public static string RemoveStrings(this string _String, params string[] _StringsToRemove)
        {
            return _String.RemoveStrings(StringComparison.Ordinal, _StringsToRemove);
        }
        
        /// <summary>
        /// Removes occurrences of the specified strings from the current string.
        /// </summary>
        /// <param name="_String">The string to process.</param>
        /// <param name="_StringComparison">The type of string comparison to perform.</param>
        /// <param name="_StringsToRemove">The strings to remove from the current string.</param>
        /// <returns>The processed string with the specified strings removed.</returns>
        public static string RemoveStrings(this string _String, StringComparison _StringComparison, params string[] _StringsToRemove)
        {
            return _StringsToRemove.Aggregate(_String, (_Current, _StringToRemove) => _Current.Replace(_StringToRemove, string.Empty, _StringComparison));
        }

        /// <summary>
        /// Removes all whitespace characters from the specified string.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> from which to remove whitespace characters.</param>
        /// <returns>The resulting <see cref="string"/> with all whitespace characters removed.</returns>
        public static string RemoveWhitespaces(this string _String)
        {
            return _String.RemoveCharacters(' ');
        }

        /// <summary>
        /// Removes the given <see cref="char"/> from the start of this <see cref="string"/>.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to remove the <see cref="char"/> from.</param>
        /// <param name="_MaxAmount">Maximum amount of <see cref="char"/> that can be removed from this <see cref="string"/>.</param>
        /// <param name="_Char">The <see cref="char"/> to remove.</param>
        /// <returns>This <see cref="string"/> with the given <see cref="char"/> removed from the start.</returns>
        public static string TrimStart(this string _String, int _MaxAmount, char _Char = ' ')
        {
            var _newStartIndex = 0;
            var _maxRemovals = Mathf.Min(_MaxAmount, _String.Length);

            // ReSharper disable once InconsistentNaming
            for (var i = 0; i < _maxRemovals; i++)
            {
                if (_String[i] != _Char)
                {
                    break;
                }
                
                _newStartIndex = i + 1;
            }

            return _newStartIndex > 0 ? _String[_newStartIndex..] : _String;
        }
        
        /// <summary>
        /// Converts the specified URI string to an HTML hyperlink.
        /// </summary>
        /// <param name="_UriString">The URI string to convert.</param>
        /// <param name="_Content">The content to display inside the hyperlink. If null, the URI string is displayed.</param>
        /// <param name="_Line">The line number to include in the hyperlink as an attribute. If null, the attribute is omitted.</param>
        /// <returns>The formatted HTML hyperlink.</returns>
        public static string ToHyperlink(this string _UriString, object? _Content = null, int? _Line = null)
        {
            return $"<a href=\"{_UriString}\"{(_Line == null ? string.Empty : $" line=\"{_Line}\"")}>{_Content ?? _UriString}</a>";
        }

        /// <summary>
        /// Converts an absolute file path to a relative path (from the project root folder).
        /// </summary>
        /// <param name="_AbsolutePath">The absolute file path to convert.</param>
        /// <param name="_Folder">Optional. The specific folder to use as the root for the relative path. If not provided, the root folder will be the default.</param>
        /// <returns>The converted relative path as a string.</returns>
        public static string ToRelativePath(this string _AbsolutePath, string? _Folder = null)
        {
            var _rootFolderPath = Application.dataPath;
            var _rootFolder = Path.GetFileNameWithoutExtension(_rootFolderPath);
            
            if (!string.IsNullOrWhiteSpace(_Folder))
            {
                if (_AbsolutePath.Contains(_Folder))
                {
                    var _index = _AbsolutePath.IndexOf(_Folder, StringComparison.Ordinal);
                    return _AbsolutePath[_index..];
                }
                
                Debug.LogError($"Could not find the folder [{_Folder}] in: {_AbsolutePath}");
            }
            
            return _AbsolutePath.Replace(_rootFolderPath, _rootFolder);
        }

        /// <summary>
        /// Converts the specified string to a URL-friendly format.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to convert.</param>
        /// <returns>The converted <see cref="string"/> in URL-friendly format.</returns>
        public static string ToURLString(this string _String)
        {
            return Uri.EscapeDataString(_String.Replace(' ', '_'));
        }
        
        /// <summary>
        /// Wraps this <see cref="string"/> in a Rich Text underline tag.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to wrap.</param>
        /// <returns>The <see cref="string"/> wrapped in an underline tag.</returns>
        public static string Underline(this string? _String)
        {
            return $"<u>{_String.OrNull()}</u>";
        }
        #endregion
    }
}
