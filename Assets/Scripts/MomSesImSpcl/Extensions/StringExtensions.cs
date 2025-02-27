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
        /// <param name="_Color">The <see cref="RichTextColor"/> to wrap the <see cref="string"/> with.</param>
        /// <returns>The <see cref="string"/> wrapped in an color tag.</returns>
        public static string Color(this string? _String, RichTextColor _Color)
        {
            return $"<color={_Color}>{_String.OrNull()}</color>";
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
            _String = Regex.Replace(_String, "(</?)b(>)", _Match => $"{_Match.Groups[1].Value}β{_Match.Groups[2].Value}");
            _String = Regex.Replace(_String, "(</?)i(>)", _Match => $"{_Match.Groups[1].Value}ι{_Match.Groups[2].Value}");
            _String = Regex.Replace(_String, "(</?)u(>)", _Match => $"{_Match.Groups[1].Value}υ{_Match.Groups[2].Value}");
            _String = Regex.Replace(_String, "(</?)a(\\s+[^>]*?>|>)", _Match => $"{_Match.Groups[1].Value}α{_Match.Groups[2].Value}");
            _String = Regex.Replace(_String, "(<)a href(.*)", _Match => $"{_Match.Groups[1].Value}α href{_Match.Groups[2].Value}");
            
            return _String;
        }

        /// <summary>
        /// Extracts a substring from the specified string that occurs after the specified pattern.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to search within.</param>
        /// <param name="_StartPattern">The <see cref="string"/> pattern to search for.</param>
        /// <param name="_Length">The length of the substring to extract. If 0, extracts until the end of the string.</param>
        /// <param name="_Occurence">The occurrence of the start pattern to extract after.</param>
        /// <param name="_RightToLeft">Determines the search direction. If true, searches from right to left.</param>
        /// <returns>The extracted substring, or null if the start pattern is not found.</returns>
        public static string? ExtractAfter(this string _String, string _StartPattern, int _Length = 0, uint _Occurence = 1, bool _RightToLeft = false)
        {
            return _String.ExtractAfter(_StartPattern, _RightToLeft, _Length, _Occurence);
        }

        /// <summary>
        /// Extracts a substring from the given string that occurs after the specified pattern.
        /// </summary>
        /// <param name="_String">The string to search within.</param>
        /// <param name="_StartPattern">The pattern to search for.</param>
        /// <param name="_RightToLeft">If true, searches from right to left.</param>
        /// <param name="_Length">The length of the substring to extract. If 0, extracts to the end of the string.</param>
        /// <param name="_Occurence">The occurrence of the pattern to consider. Default is 1.</param>
        /// <returns>The extracted substring, or null if the pattern is not found.</returns>
        public static string? ExtractAfter(this string _String, string _StartPattern, bool _RightToLeft, int _Length = 0, uint _Occurence = 1)
        {
            var _regexOptions = _RightToLeft ? RegexOptions.RightToLeft : RegexOptions.None;
            string? _output = null;
            
            if (_Occurence < 1)
            {
                // Reverse the search order when the last occurence is requested.
                _RightToLeft = !_RightToLeft;
                _regexOptions = _RightToLeft ? RegexOptions.RightToLeft : RegexOptions.None;
                _Occurence = 1;
            }
            
            // ReSharper disable once InconsistentNaming
            for (var i = 1; i <= _Occurence; i++)
            {
                int _startIndex;
                var _match = Regex.Match(_String, _StartPattern, _regexOptions).Value;

                if (_match == string.Empty)
                {
                    break;
                }
                
                if (_RightToLeft)
                {
                    _startIndex = _String.LastIndexOf(_match, StringComparison.Ordinal) + _match.Length;
                    
                    if (i == _Occurence)
                    {
                        _output = _String[_startIndex..] + _output;
                    }
                    else
                    {
                        _output = _String[(_startIndex - _match.Length)..] + _output;
                    }
                    
                    _String = _String[..(_startIndex - _match.Length - 1)];
                }
                else
                {
                    _startIndex = _String.IndexOf(_match, StringComparison.Ordinal) + _match.Length;
                    
                    _output = _String = _String[_startIndex..];
                }
            }

            if (_Length > 0)
            {
                _output = _output?[.._Length];
            }
            
            return _output;
        }

        /// <summary>
        /// Extracts a substring from the specified string that occurs before the first instance of the given start pattern.
        /// </summary>
        /// <param name="_String">The string to extract from.</param>
        /// <param name="_StartPattern">The start pattern that identifies where the substring begins.</param>
        /// <param name="_Length">The number of characters to extract. If zero, extracts up to the start pattern.</param>
        /// <param name="_Occurence">The occurrence of the start pattern to consider. Default is 1.</param>
        /// <param name="_RightToLeft">Specifies whether to search from right to left. Default is false.</param>
        /// <returns>The extracted substring or null if the start pattern is not found.</returns>
        public static string? ExtractBefore(this string _String, string _StartPattern, int _Length = 0, uint _Occurence = 1, bool _RightToLeft = false)
        {
            return _String.ExtractBefore(_StartPattern, _RightToLeft, _Length, _Occurence);
        }

        /// <summary>
        /// Extracts a substring from the specified string that occurs before the given start pattern.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to extract from.</param>
        /// <param name="_StartPattern">The start pattern to look for.</param>
        /// <param name="_RightToLeft">Specifies whether to search right to left. Defaults to false.</param>
        /// <param name="_Length">The length of the substring to be extracted. Defaults to 0.</param>
        /// <param name="_Occurence">The occurrence of the start pattern to target. Defaults to 1.</param>
        /// <returns>The extracted <see cref="string"/> or null if the start pattern is not found.</returns>
        public static string? ExtractBefore(this string _String, string _StartPattern, bool _RightToLeft, int _Length = 0, uint _Occurence = 1)
        {
            var _regexOptions = _RightToLeft ? RegexOptions.RightToLeft : RegexOptions.None;
            string? _output = null;
            
            if (_Occurence < 1)
            {
                // Reverse the search order when the last occurence is requested.
                _RightToLeft = !_RightToLeft;
                _regexOptions = _RightToLeft ? RegexOptions.RightToLeft : RegexOptions.None;
                _Occurence = 1;
            }
            
            // ReSharper disable once InconsistentNaming
            for (var i = 1; i <= _Occurence; i++)
            {
                int _startIndex;
                var _match = Regex.Match(_String, _StartPattern, _regexOptions).Value;
                
                if (_match == string.Empty)
                {
                    break;
                }
                
                if (_RightToLeft)
                {
                    _startIndex = _String.LastIndexOf(_match, StringComparison.Ordinal);

                    _output = _String = _String[.._startIndex];
                }
                else
                {
                    _startIndex = _String.IndexOf(_match, StringComparison.Ordinal) + _match.Length;
                    
                    if (i == _Occurence)
                    {
                        _output += _String[..(_startIndex - _match.Length)];
                    }
                    else
                    {
                        _output += _String[.._startIndex];
                    }
                    
                    _String = _String[_startIndex..];
                }
            }

            if (_Length > 0)
            {
                _output = _output?[^_Length..];
            }
            
            return _output;
        }

        /// <summary>
        /// Extracts a substring from the specified string that is found between the given start and end patterns.
        /// </summary>
        /// <param name="_String">The string to search within.</param>
        /// <param name="_StartPattern">The start pattern to find.</param>
        /// <param name="_EndPattern">The end pattern to find.</param>
        /// <param name="_Occurence">The occurrence number of the substring to extract.</param>
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
        /// <param name="_Occurence">The occurrence of the match to extract.</param>
        /// <returns>The extracted substring, or null if no match is found.</returns>
        public static string? ExtractBetween(this string _String, string _StartPattern, string _EndPattern, bool _RightToLeft, uint _Occurence = 1)
        {
            var _output = _String;
            var _regexOptions = _RightToLeft ? RegexOptions.RightToLeft : RegexOptions.None;
            // ReSharper disable RedundantAssignment
            var _startPatternMatch = string.Empty;
            var _endPatternMatch = string.Empty;
            // ReSharper restore RedundantAssignment
            var _startIndex = -1;
            var _length = 0;
            
            if (_Occurence < 1)
            {
                // Reverse the search order when the last occurence is requested.
                _RightToLeft = !_RightToLeft;
                _regexOptions = _RightToLeft ? RegexOptions.RightToLeft : RegexOptions.None;
                _Occurence = 1;
            }
            
            // ReSharper disable once InconsistentNaming
            for (var i = 1; i <= _Occurence; i++)
            {
                var _match = Regex.Match(_output, $"{_StartPattern}.*?{_EndPattern}", _regexOptions).Value;

                if (_match == string.Empty)
                {
                    _output = null;
                    break;
                }
                
                _startPatternMatch = Regex.Match(_match, _StartPattern).Value;
                _endPatternMatch = Regex.Match(_match, _EndPattern).Value;
                _length = _match.Length - _startPatternMatch.Length - _endPatternMatch.Length;
                
                if (_RightToLeft)
                {
                    _startIndex = _output.LastIndexOf(_match, StringComparison.Ordinal) + _startPatternMatch.Length;
                }
                else
                {
                    _startIndex = _output.IndexOf(_match, StringComparison.Ordinal) + _startPatternMatch.Length;
                }
                
                if (i != _Occurence)
                {
                    // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
                    if (_RightToLeft)
                    {
                        _output = _output[..(_startIndex - _startPatternMatch.Length)];
                    }
                    else
                    {
                        _output = _output[(_startIndex + _length + _endPatternMatch.Length)..];
                    }
                }
            }
            
            return _output?.Substring(_startIndex, _length);
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
        /// <param name="_HexColor">The hexadecimal color code as a <see cref="string"/>.</param>
        /// <returns>A <see cref="Color"/> representing the RGB values of the hexadecimal color code.</returns>
        public static Color HexToRGB(this string _HexColor)
        {
            _HexColor = _HexColor.Replace("#", string.Empty);
        
            var _red = int.Parse(_HexColor.Substring(0, 2), NumberStyles.HexNumber);
            var _green = int.Parse(_HexColor.Substring(2, 2), NumberStyles.HexNumber);
            var _blue = int.Parse(_HexColor.Substring(4, 2), NumberStyles.HexNumber);

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
            _String = Regex.Replace(_String, "</?b>", string.Empty);
            _String = Regex.Replace(_String, "</?i>", string.Empty);
            _String = Regex.Replace(_String, "</?u>", string.Empty);
            _String = Regex.Replace(_String, "</?size(?:=\\d+)?>", string.Empty);
            _String = Regex.Replace(_String, "</?color(?:=.*?)?>", string.Empty);
            _String = Regex.Replace(_String, "</?a(?:\\s+[^>]*?>|>)", string.Empty);
            
            return _String;
        }

        /// <summary>
        /// Removes all ruby text elements from the specified string.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to process.</param>
        /// <returns>The processed <see cref="string"/> with all ruby text elements removed.</returns>
        public static string RemoveRubyText(this string _String)
        {
            _String = Regex.Replace(_String, "</?ruby.*?>", string.Empty);
            _String = Regex.Replace(_String, "</?rb>", string.Empty);
            _String = Regex.Replace(_String, "<rp>.*?</rp>", string.Empty);
            _String = Regex.Replace(_String, "<rt>.*?</rt>", string.Empty);
            
            return _String;
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
        /// <param name="_StringToRemove">The <see cref="string"/> to remove from the current instance.</param>
        /// <returns>A new <see cref="string"/> with the specified substring removed.</returns>
        public static string RemoveStrings(this string _String, string _StringToRemove)
        {
            return _String.RemoveStrings(StringComparison.Ordinal, _StringToRemove);
        }

        /// <summary>
        /// Removes the specified strings from the given string.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> from which the strings will be removed.</param>
        /// <param name="_StringsToRemove">An array of <see cref="string"/> containing the substrings to remove.</param>
        /// <param name="_StringComparison">A <see cref="StringComparison"/> enumeration value that specifies the rules for the string comparison.</param>
        /// <returns>The modified <see cref="string"/> with the specified strings removed.</returns>
        public static string RemoveStrings(this string _String, string[] _StringsToRemove, StringComparison _StringComparison = StringComparison.Ordinal)
        {
            return _String.RemoveStrings(_StringComparison, _StringsToRemove);
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
        /// Splits the specified string using the provided pattern and optional separator.
        /// </summary>
        /// <param name="_String">The string to split.</param>
        /// <param name="_Pattern">The pattern to split the string by.</param>
        /// <param name="_Separator">An optional separator to use within the split pattern. Default is an empty string.</param>
        /// <returns>An array of strings that have been split by the specified pattern and separator.</returns>
        public static string[] Split(this string _String, string _Pattern, string _Separator = "")
        {
            var _matches = Regex.Matches(_String, _Pattern);
            var _splits = new string[_matches.Count + 1];

            var _currentIndex = 0;

            // ReSharper disable once InconsistentNaming
            for (var i = 0; i < _matches.Count; i++)
            {
                var _value = _matches[i].Value;
                var _patternIndex = _String.IndexOf(_value, _currentIndex, StringComparison.Ordinal);
                var _separatorIndex = _value.IndexOf(_Separator, StringComparison.Ordinal);
                var _length = _separatorIndex == -1 ? _value.Length : _separatorIndex;
        
                _splits[i] = _String.Substring(_currentIndex, _patternIndex + _length - _currentIndex);
                _currentIndex = _patternIndex + _length;
            }
            
            _splits[^1] = _String[_currentIndex..];

            return _splits;
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
