using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace CustomScriptTemplates.Extensions
{
    /// <summary>
    /// Contains extension methods.
    /// </summary>
    internal static class CustomScriptTemplateExtensions
    {
        #region Methods
        /// <summary>
        /// Wraps this object's <see cref="object.ToString"/> output in a Rich Text bold tag.
        /// </summary>
        /// <param name="_Object">The <see cref="object"/> to get the <see cref="object.ToString"/> output of.</param>
        /// <returns>A <see cref="string"/> containing this object's <see cref="object.ToString"/> output, wrapped in a Rich Text bold tag.</returns>
        internal static string Bold([CanBeNull] this string _Object)
        {
            return $"<b>{_Object.OrNull()}</b>";
        }
        
        /// <summary>
        /// Returns the contents of the <see cref="object.ToString()"/> method of this <see cref="string"/>, or the <see cref="string"/> <c>null</c>, if this <see cref="string"/> is <c>null</c>.
        /// </summary>
        /// <param name="_Object">The <see cref="string"/> to get the <see cref="object.ToString()"/> contents of.</param>
        /// <returns>The contents of the <see cref="object.ToString()"/> method of this <see cref="string"/>, or the <see cref="string"/> <c>null</c>, if this <see cref="string"/> is <c>null</c>.</returns>
        private static string OrNull([CanBeNull] this object _Object)
        {
            return $"{_Object?.ToString() ?? "null"}";
        }
        
        /// <summary>
        /// Counts all whitespaces in this <see cref="string"/>.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to count the whitespaces in.</param>
        /// <param name="_TabSize">The whitespace count per tab.</param>
        /// <returns>The number of whitespaces in this <see cref="string"/>.</returns>
        internal static int CountWhitespaces(this string _String, int _TabSize = 4)
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
        [CanBeNull]
        internal static string ExtractBetween(this string _String, string _StartPattern, string _EndPattern, uint _Occurence = 1, bool _RightToLeft = false)
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
        [CanBeNull]
        internal static string ExtractBetween(this string _String, string _StartPattern, string _EndPattern, bool _RightToLeft, uint _Occurence = 1)
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
        /// Determines whether the specified string is empty or consists only of white-space characters.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to check.</param>
        /// <returns><see langword="true"/> if the string is empty or consists only of white-space characters; otherwise, <see langword="false"/>.</returns>
        internal static bool IsEmptyOrWhitespace(this string _String)
        {
            return _String == string.Empty || _String.All(_Char => _Char == ' ');
        }
        
        /// <summary>
        /// Removes all whitespaces from this <see cref="string"/>.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to remove the whitespaces from.</param>
        /// <returns>This <see cref="string"/> with all whitespaces removed.</returns>
        internal static string RemoveWhitespaces(this string _String)
        {
            return _String.Replace(" ", string.Empty);
        }

        /// <summary>
        /// Removes the given <c>_PathToRemove</c> from the given <c>_Path</c>.
        /// </summary>
        /// <param name="_Path">The path to remove from.</param>
        /// <param name="_PathToRemove">The path to remove.</param>
        /// <returns>The given <c>_Path</c> with <c>_PathToRemove</c> removed from it.</returns>
        internal static string RemovePath(this string _Path, string _PathToRemove)
        {
            var _path = _Path.Replace(_PathToRemove, string.Empty);

            return _path.StartsWith(System.IO.Path.DirectorySeparatorChar.ToString()) ? _path.Substring(1) : _path;
        }
        
        /// <summary>
        /// Replaces all whitespaces in this <see cref="string"/> with <c>_</c>.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to replace the whitespaces in.</param>
        /// <returns>This <see cref="string"/> with all whitespaces replaced with <c>_</c>.</returns>
        internal static string ReplaceWhitespaces(this string _String)
        {
            return _String.Replace(' ', '_');
        }
        
        /// <summary>
        /// Converts the specified URI string to an HTML hyperlink.
        /// </summary>
        /// <param name="_UriString">The URI string to convert.</param>
        /// <param name="_Content">The content to display inside the hyperlink. If null, the URI string is displayed.</param>
        /// <param name="_Line">The line number to include in the hyperlink as an attribute. If null, the attribute is omitted.</param>
        /// <returns>The formatted HTML hyperlink.</returns>
        internal static string ToHyperlink(this string _UriString, [CanBeNull] object _Content = null, int? _Line = null)
        {
            return $"<a href=\"{_UriString}\"{(_Line == null ? string.Empty : $" line=\"{_Line}\"")}>{_Content ?? _UriString}</a>";
        }
        #endregion
    }
}