using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace CustomScriptTemplates.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="string"/>.
    /// </summary>
    public static class StringExtensions
    {
        #region Methods
        /// <summary>
        /// Counts all whitespaces in this <see cref="string"/>.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to count the whitespaces in.</param>
        /// <param name="_TabSize">The whitespace count per tab.</param>
        /// <returns>The number of whitespaces in this <see cref="string"/>.</returns>
        public static int CountWhitespaces(this string _String, int _TabSize = 4)
        {
            var _count = 0;
            
            foreach (var _character in _String)
            {
                switch (_character)
                {
                    case ' ':
                        _count++;
                        break;
                    case '\t':
                        _count += _TabSize;
                        break;
                }
            }
            
            return _count;
        }
        
        /// <summary>
        /// Extracts the nth occurence between <c>_StartPattern</c> and <c>_EndPattern</c> from this <see cref="string"/>.
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
        [CanBeNull]
        public static string ExtractBetween(this string _String, string _StartPattern, string _EndPattern, uint _Occurence = 1, bool _RightToLeft = false)
        {
            return ExtractBetween(_String, _StartPattern, _EndPattern, _RightToLeft, _Occurence);
        }
        
        /// <summary>
        /// Extracts the nth occurence between <c>_StartPattern</c> and <c>_EndPattern</c> from this <see cref="string"/>.
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
        [CanBeNull]
        public static string ExtractBetween(this string _String, string _StartPattern, string _EndPattern, bool _RightToLeft, uint _Occurence = 1)
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
                        _output = _output.Substring(0, _startIndex - _startPatternMatch.Length);
                    }
                    else
                    {
                        _output = _output.Substring(_startIndex + _length + _endPatternMatch.Length);
                    }
                }
            }
            
            return _output?.Substring(_startIndex, _length);
        }
        
        /// <summary>
        /// Checks if this <see cref="string"/> is <see cref="string.Empty"/> or only consists of whitespaces.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to check.</param>
        /// <returns><c>true</c> if this <see cref="string"/> is <see cref="string.Empty"/> or only consists of whitespaces, otherwise <c>false</c>.</returns>
        public static bool IsEmptyOrWhitespace(this string _String)
        {
            return _String == string.Empty && _String.All(_Char => _Char == ' ');
        }
        
        /// <summary>
        /// Removes all whitespaces from this <see cref="string"/>.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to remove the whitespaces from.</param>
        /// <returns>This <see cref="string"/> with all whitespaces removed.</returns>
        public static string RemoveAllWhitespaces(this string _String)
        {
            return _String.Replace(" ", string.Empty);
        }

        /// <summary>
        /// Removes the given <c>_PathToRemove</c> from the given <c>_Path</c>.
        /// </summary>
        /// <param name="_Path">The path to remove from.</param>
        /// <param name="_PathToRemove">The path to remove.</param>
        /// <returns>The given <c>_Path</c> with <c>_PathToRemove</c> removed from it.</returns>
        public static string RemovePath(this string _Path, string _PathToRemove)
        {
            var _path = _Path.Replace(_PathToRemove, string.Empty);

            return _path.StartsWith(Path.DirectorySeparatorChar.ToString()) ? _path.Substring(1) : _path;
        }
        
        /// <summary>
        /// Replaces all whitespaces in this <see cref="string"/> with <c>_</c>.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to replace the whitespaces in.</param>
        /// <returns>This <see cref="string"/> with all whitespaces replaced with <c>_</c>.</returns>
        public static string ReplaceWhitespaces(this string _String)
        {
            return _String.Replace(' ', '_');
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
        public static string ToHyperlink(this string _UriString, [CanBeNull] object _Content = null, int? _Line = null)
        {
            return $"<a href=\"{_UriString}\"{(_Line == null ? string.Empty : $" line=\"{_Line}\"")}>{_Content ?? _UriString}</a>";
        }
        #endregion
    }
}