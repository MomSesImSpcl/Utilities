using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using IfLoooop.Extensions;
using IfLoooop.Utilities.Pooling;

namespace IfLoooop.Utilities
{
    /// <summary>
    /// Contains methods to escape/unescape all characters in a <see cref="string"/> that are not allowed to be in a filename.
    /// </summary>
    public static class Filename
    {
        #region Constants
        /// <summary>
        /// Every escaped character will be wrapped between this.
        /// </summary>
        private const char WRAPPER = '$';
        #endregion

        #region Properties
        /// <summary>
        /// Replaces all escaped characters to their original values.
        /// </summary>
        private static Regex FromFilenameRegex { get; } = new($@"\{WRAPPER}.*?\{WRAPPER}");
        /// <summary>
        /// ALl characters that are not allowed to be used in a filename.
        /// </summary>
        private static char[] InvalidFilenameCharacters { get; } = Path.GetInvalidFileNameChars();
        #endregion
        
        #region Methods
        /// <summary>
        /// Unescapes a <see cref="string"/> that has been escaped with <see cref="EscapeFilename"/>.
        /// </summary>
        /// <param name="_Filename">The <see cref="string"/> to unescape.</param>
        /// <returns>The given <c>_Filename</c> with all escaped characters unescaped.</returns>
        public static string UnescapeFilename(string _Filename)
        {
            return FromFilenameRegex.Replace(_Filename, _Match =>
            {
                var _hexValue = _Match.Value.ExtractBetween($@"\{WRAPPER}", $@"\{WRAPPER}");
                
                return ((char)Convert.ToInt32(_hexValue, 16)).ToString();
            });
        }
        
        /// <summary>
        /// Escapes all characters that are not valid in a filename in the given <c>_Filename</c>.
        /// </summary>
        /// <param name="_Filename">The <see cref="string"/> to escape the characters in.</param>
        /// <returns>The given <c>_Filename</c> with all invalid characters escaped to their hexadecimal representation and enclosed between <c>$</c>.</returns>
        public static string EscapeFilename(string _Filename)
        {
            const char _WRAPPER = '$';

            var _poolWrapper = ObjectPools.StringBuilderPool.Get();
            
            foreach (var _character in _Filename)
            {
                if (InvalidFilenameCharacters.Contains(_character))
                {
                    _poolWrapper.StringBuilder.Append($"{_WRAPPER}{(int)_character:X2}{_WRAPPER}");
                }
                else
                {
                    _poolWrapper.StringBuilder.Append(_character);
                }
            }
            
            return _poolWrapper.Return();
        }
        #endregion
    }
}