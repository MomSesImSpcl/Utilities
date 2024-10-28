#nullable enable
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace IfLoooop.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="string"/>.
    /// </summary>
    public static class StringExtensions
    {
        #region Methods
        /// <summary>
        /// Converts the given <c>_String</c> to Title Case. (Except for words that are entirely in uppercase.)
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to convert to Title Case.</param>
        /// <returns>The given <see cref="string"/> with all words in Title Case.</returns>
        public static string ConvertToTitleCase(this string _String)
        {
            return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(_String.ToLower());
        }
        
        /// <summary>
        /// Replaces the rich text character: b, i, u, a with β, ι, υ, α.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to replace the <see cref="char"/>'s of.</param>
        /// <returns>The given <c>_String</c> with all rich text <see cref="char"/>'s replaced.</returns>
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
        /// Extract everything after the nth occurence of <c>_StartPattern</c>.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to extract from.</param>
        /// <param name="_StartPattern">
        /// The start pattern to search for. <br/>
        /// <i>Characters with special meaning in regular expressions must be escaped to be treated as literals.</i>
        /// </param>
        /// <param name="_Length">
        /// The number of <see cref="char"/> to extract after <c>_StartPattern</c>. <br/>
        /// <i>Leave <c>0</c> to extract all.</i>
        /// </param>
        /// <param name="_Occurence">
        /// Searches for the nth occurence depending on <c>_RightToLeft</c>. <br/>
        /// <i>Set to <c>0</c> to search for the last occurence.</i>
        /// </param>
        /// <param name="_RightToLeft"><c>true</c> to use <see cref="RegexOptions.RightToLeft"/>. <c>false</c> to use <see cref="RegexOptions.None"/>.</param>
        /// <returns>Everything after the nth occurence of <c>_StartPattern</c>, or <c>null</c> if no match could be found.</returns>
        public static string? ExtractAfter(this string _String, string _StartPattern, int _Length = 0, uint _Occurence = 1, bool _RightToLeft = false)
        {
            return _String.ExtractAfter(_StartPattern, _RightToLeft, _Length, _Occurence);
        }
        
        /// <summary>
        /// Extract everything after the nth occurence of <c>_StartPattern</c>.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to extract from.</param>
        /// <param name="_StartPattern">
        /// The start pattern to search for. <br/>
        /// <i>Characters with special meaning in regular expressions must be escaped to be treated as literals.</i>
        /// </param>
        /// <param name="_RightToLeft"><c>true</c> to use <see cref="RegexOptions.RightToLeft"/>. <c>false</c> to use <see cref="RegexOptions.None"/>.</param>
        /// <param name="_Length">
        /// The number of <see cref="char"/> to extract after <c>_StartPattern</c>. <br/>
        /// <i>Leave <c>0</c> to extract all.</i>
        /// </param>
        /// <param name="_Occurence">
        /// Searches for the nth occurence depending on <c>_RightToLeft</c>. <br/>
        /// <i>Set to <c>0</c> to search for the last occurence.</i>
        /// </param>
        /// <returns>Everything after the nth occurence of <c>_StartPattern</c>, or <c>null</c> if no match could be found.</returns>
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
        /// Extract everything before the nth occurence of <c>_StartPattern</c>.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to extract from.</param>
        /// <param name="_StartPattern">
        /// The start pattern to search for. <br/>
        /// <i>Characters with special meaning in regular expressions must be escaped to be treated as literals.</i>
        /// </param>
        /// <param name="_Length">
        /// The number of <see cref="char"/> to extract before <c>_StartPattern</c>. <br/>
        /// <i>Leave <c>0</c> to extract all.</i>
        /// </param>
        /// <param name="_Occurence">
        /// Searches for the nth occurence depending on <c>_RightToLeft</c>. <br/>
        /// <i>Set to <c>0</c> to search for the last occurence.</i>
        /// </param>
        /// <param name="_RightToLeft"><c>true</c> to use <see cref="RegexOptions.RightToLeft"/>. <c>false</c> to use <see cref="RegexOptions.None"/>.</param>
        /// <returns>Everything before the nth occurence of <c>_StartPattern</c>, or <c>null</c> if no match could be found.</returns>
        public static string? ExtractBefore(this string _String, string _StartPattern, int _Length = 0, uint _Occurence = 1, bool _RightToLeft = false)
        {
            return _String.ExtractBefore(_StartPattern, _RightToLeft, _Length, _Occurence);
        }
        
        /// <summary>
        /// Extract everything before the nth occurence of <c>_StartPattern</c>.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to extract from.</param>
        /// <param name="_StartPattern">
        /// The start pattern to search for. <br/>
        /// <i>Characters with special meaning in regular expressions must be escaped to be treated as literals.</i>
        /// </param>
        /// <param name="_RightToLeft"><c>true</c> to use <see cref="RegexOptions.RightToLeft"/>. <c>false</c> to use <see cref="RegexOptions.None"/>.</param>
        /// <param name="_Length">
        /// The number of <see cref="char"/> to extract before <c>_StartPattern</c>. <br/>
        /// <i>Leave <c>0</c> to extract all.</i>
        /// </param>
        /// <param name="_Occurence">
        /// Searches for the nth occurence depending on <c>_RightToLeft</c>. <br/>
        /// <i>Set to <c>0</c> to search for the last occurence.</i>
        /// </param>
        /// <returns>Everything before the nth occurence of <c>_StartPattern</c>, or <c>null</c> if no match could be found.</returns>
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
        /// Extract the nth occurence between <c>_StartPattern</c> and <c>_EndPattern</c> from this <see cref="string"/>.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to extract from.</param>
        /// <param name="_StartPattern">
        /// The start pattern to search for. <br/>
        /// <i>Characters with special meaning in regular expressions must be escaped to be treated as literals.</i>
        /// </param>
        /// <param name="_EndPattern">
        /// The end pattern to search for. <br/>
        /// <i>Characters with special meaning in regular expressions must be escaped to be treated as literals.</i>
        /// </param>
        /// <param name="_Occurence">
        /// Searches for the nth occurence depending on <c>_RightToLeft</c>. <br/>
        /// <i>Set to <c>0</c> to search for the last occurence.</i>
        /// </param>
        /// <param name="_RightToLeft"><c>true</c> to use <see cref="RegexOptions.RightToLeft"/>. <c>false</c> to use <see cref="RegexOptions.None"/>.</param>
        /// <returns>The nth occurence between <c>_StartPattern</c> and <c>_EndPattern</c>, or <c>null</c> if no match could be found.</returns>
        public static string? ExtractBetween(this string _String, string _StartPattern, string _EndPattern, uint _Occurence = 1, bool _RightToLeft = false)
        {
            return ExtractBetween(_String, _StartPattern, _EndPattern, _RightToLeft, _Occurence);
        }
        
        /// <summary>
        /// Extract the nth occurence between <c>_StartPattern</c> and <c>_EndPattern</c> from this <see cref="string"/>.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to extract from.</param>
        /// <param name="_StartPattern">
        /// The start pattern to search for. <br/>
        /// <i>Characters with special meaning in regular expressions must be escaped to be treated as literals.</i>
        /// </param>
        /// <param name="_EndPattern">
        /// The end pattern to search for. <br/>
        /// <i>Characters with special meaning in regular expressions must be escaped to be treated as literals.</i>
        /// </param>
        /// <param name="_RightToLeft"><c>true</c> to use <see cref="RegexOptions.RightToLeft"/>. <c>false</c> to use <see cref="RegexOptions.None"/>.</param>
        /// <param name="_Occurence">
        /// Searches for the nth occurence depending on <c>_RightToLeft</c>. <br/>
        /// <i>Set to <c>0</c> to search for the last occurence.</i>
        /// </param>
        /// <returns>The nth occurence between <c>_StartPattern</c> and <c>_EndPattern</c>, or <c>null</c> if no match could be found.</returns>
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
        /// Finds the common characters shared among all strings in the input array.
        /// </summary>
        /// <param name="_Strings">An array of strings to compare.</param>
        /// <returns>The longest common characters, shared by every <see cref="string"/> in the array.</returns>
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
        /// Returns <c>true</c> if the given <c>_String</c> is <see cref="string.Empty"/> or contains only whitespaces.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to check.</param>
        /// <returns><c>true</c> if the given <c>_String</c> is <see cref="string.Empty"/> or contains only whitespaces.</returns>
        public static bool IsEmptyOrWhitespace(this string _String)
        {
            return _String == string.Empty || _String.All(_Char => _Char == ' ');
        }
        
        /// <summary>
        /// Finds the index of the nth occurence of <c>_Value</c> in <c>_String</c>.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to search in.</param>
        /// <param name="_Value">The value to search for.</param>
        /// <param name="_Occurence">The occurence to search for.</param>
        /// <param name="_StringComparison">The <see cref="StringComparison"/> to use.</param>
        /// <returns>The index of the nth occurence of <c>_Value</c> in <c>_String</c>, or -1 if no match could be found.</returns>
        public static int NthIndexOf(this string _String, string _Value, int _Occurence = 1, StringComparison _StringComparison = StringComparison.Ordinal)
        {
            var _occurrence = -1;
            var _startIndex = 0;

            for (var _i = 0; _i < _Occurence; _i++)
            {
                _occurrence = _String.IndexOf(_Value, _startIndex, _StringComparison);
                
                if (_occurrence == -1)
                    return -1;

                _startIndex = _occurrence + _Value.Length;
            }

            return _occurrence;
        }
        
        /// <summary>
        /// Removes the given <see cref="char"/> in <c>_CharsToRemove</c> from <c>_String</c>.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to remove the character from.</param>
        /// <param name="_CharToRemove"><see cref="char"/> to remove.</param>
        /// <returns>The given <c>_String</c> with the <see cref="char"/> in <c>_CharsToRemove</c> removed.</returns>
        public static string RemoveCharacters(this string _String, char _CharToRemove)
        {
            return _String.RemoveCharacters(StringComparison.Ordinal, _CharToRemove);
        }
        
        /// <summary>
        /// Removes all characters in <c>_CharToRemove</c> from <c>_String</c>.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to remove the characters from.</param>
        /// <param name="_CharsToRemove"><see cref="char"/>s to remove.</param>
        /// <param name="_StringComparison"><see cref="StringComparison"/>.</param>
        /// <returns>The given <c>_String</c> with all characters in <c>_CharToRemove</c> removed.</returns>
        public static string RemoveCharacters(this string _String, IEnumerable<char> _CharsToRemove, StringComparison _StringComparison = StringComparison.Ordinal)
        {
            return _String.RemoveCharacters(_StringComparison, _CharsToRemove.ToArray());
        }
        
        /// <summary>
        /// Removes all characters in <c>_CharsToRemove</c> from <c>_String</c>.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to remove the characters from.</param>
        /// <param name="_StringComparison"><see cref="StringComparison"/>.</param>
        /// <param name="_CharsToRemove"><see cref="char"/>s to remove.</param>
        /// <returns>The given <c>_String</c> with all characters in <c>_CharsToRemove</c> removed.</returns>
        public static string RemoveCharacters(this string _String, StringComparison _StringComparison, params char[] _CharsToRemove)
        {
            return _CharsToRemove.Aggregate(_String, (_Current, _Character) => _Current.Replace(_Character.ToString(), string.Empty, _StringComparison));
        }
        
        /// <summary>
        /// Removes all non digit <see cref="char"/> from the given <c>_String</c>.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to remove the non digits from.</param>
        /// <returns>The given <c>_String</c> with all non digit <see cref="char"/> removed.</returns>
        public static string RemoveNonDigits(this string _String)
        {
            return new string(_String.Where(char.IsDigit).ToArray());
        }
        
        /// <summary>
        /// Removes all rich text styling from this string.
        /// </summary>
        /// <param name="_String">The string to remove all rich text styling from.</param>
        /// <returns>The given <c>_String</c> with all rich text styling removed.</returns>
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
        /// Removes all Ruby annotations from this <see cref="string"/>.
        /// </summary>
        /// <param name="_String">The string to remove all Ruby annotations from.</param>
        /// <returns>The given <c>_String</c> with all Ruby annotations removed.</returns>
        public static string RemoveRubyText(this string _String)
        {
            _String = Regex.Replace(_String, "</?ruby.*?>", string.Empty);
            _String = Regex.Replace(_String, "</?rb>", string.Empty);
            _String = Regex.Replace(_String, "<rp>.*?</rp>", string.Empty);
            _String = Regex.Replace(_String, "<rt>.*?</rt>", string.Empty);
            
            return _String;
        }
        
        /// <summary>
        /// Removes all characters that are not numbers, or digits from the given <c>_String</c>.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to remove the special characters from.</param>
        /// <param name="_RemoveWhitespaces">Also removes all whitespaces.</param>
        /// <returns>The given <c>_String</c> with all special characters removed.</returns>
        public static string RemoveSpecialCharacters(this string _String, bool _RemoveWhitespaces = false)
        {
            var _whiteSpace = _RemoveWhitespaces ? string.Empty : "\\s";
            return Regex.Replace(_String, $"[^a-zA-Z0-9{_whiteSpace}]", string.Empty);
        }
        
        /// <summary>
        /// Removes the given <see cref="string"/> in <c>_StringsToRemove</c> from <c>_String</c>.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to remove the string from.</param>
        /// <param name="_StringToRemove"><see cref="string"/> to remove.</param>
        /// <returns>The given <c>_String</c> with the <see cref="string"/> in <c>_StringToRemove</c> removed.</returns>
        public static string RemoveStrings(this string _String, string _StringToRemove)
        {
            return _String.RemoveStrings(StringComparison.Ordinal, _StringToRemove);
        }
        
        /// <summary>
        /// Removes all strings in <c>_StringsToRemove</c> from <c>_String</c>.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to remove the strings from.</param>
        /// <param name="_StringsToRemove"><see cref="string"/>s to remove.</param>
        /// <param name="_StringComparison"><see cref="StringComparison"/>.</param>
        /// <returns>The given <c>_String</c> with all strings in <c>_StringsToRemove</c> removed.</returns>
        public static string RemoveStrings(this string _String, string[] _StringsToRemove, StringComparison _StringComparison = StringComparison.Ordinal)
        {
            return _String.RemoveStrings(_StringComparison, _StringsToRemove);
        }
        
        /// <summary>
        /// Removes all strings in <c>_StringsToRemove</c> from <c>_String</c>.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to remove the strings from.</param>
        /// <param name="_StringComparison"><see cref="StringComparison"/>.</param>
        /// <param name="_StringsToRemove"><see cref="string"/>s to remove.</param>
        /// <returns>The given <c>_String</c> with all strings in <c>_StringsToRemove</c> removed.</returns>
        public static string RemoveStrings(this string _String, StringComparison _StringComparison, params string[] _StringsToRemove)
        {
            return _StringsToRemove.Aggregate(_String, (_Current, _StringToRemove) => _Current.Replace(_StringToRemove, string.Empty, _StringComparison));
        }
        
        /// <summary>
        /// Removes all whitespaces from <c>_String</c>.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to remove the whitespaces from.</param>
        /// <returns>The given <c>_String</c> with all whitespaces removed.</returns>
        public static string RemoveWhitespaces(this string _String)
        {
            return _String.RemoveCharacters(' ');
        }
        
        /// <summary>
        /// Finds all <c>_Pattern</c> in the this <c>_String</c>, and splits it by the given <c>_Separator</c>.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to search in.</param>
        /// <param name="_Pattern">The <see cref="Regex"/> pattern to search for.</param>
        /// <param name="_Separator">
        /// The <see cref="string"/> to split after. <bvr/>
        /// <i><c>_Pattern</c> must contain <c>_Separator</c>.</i> <br/>
        /// <b>This must not be a <see cref="Regex"/> pattern.</b>
        /// </param>
        /// <returns>This <see cref="string"/> split after each <c>_Separator</c>.</returns>
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
        /// Converts this <see cref="string"/> into a hyperlink.
        /// </summary>
        /// <param name="_UriString">The URL/Path.</param>
        /// <param name="_Content">The display name. (Will be the <c>_UriString</c> if not set)</param>
        /// <param name="_Line">
        /// If the link is for a file on the local machine, this is the line number where to jump to when opening the file. <br/>
        /// <i>Doesn't seem to work right now.</i>
        /// </param>
        /// <returns>This <see cref="string"/> converted into a hyperlink.</returns>
        public static string ToHyperlink(this string _UriString, object? _Content = null, int? _Line = null)
        {
            return $"<a href=\"{_UriString}\"{(_Line == null ? string.Empty : $" line=\"{_Line}\"")}>{_Content ?? _UriString}</a>";
        }
        
        /// <summary>
        /// Escapes all invalid URL characters.
        /// </summary>
        /// <param name="_String">The <see cref="string"/>to escape the characters of.</param>
        /// <returns>A <see cref="string"/> with all invalid URL characters escaped.</returns>
        public static string ToURLString(this string _String)
        {
            return Uri.EscapeDataString(_String.Replace(' ', '_'));
        }
        #endregion
    }
}