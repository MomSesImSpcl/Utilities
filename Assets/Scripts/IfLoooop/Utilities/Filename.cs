using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using IfLoooop.Extensions;
using IfLoooop.Utilities.Pooling;

namespace IfLoooop.Utilities
{
    /// <summary>
    /// Provides utility methods for working with filenames, including escaping and unescaping
    /// characters that are not valid in filenames.
    /// </summary>
    public static class Filename
    {
        #region Constants
        /// <summary>
        /// A constant character used as a wrapper to encapsulate invalid filename characters
        /// in their escaped hexadecimal representation.
        /// </summary>
        private const char WRAPPER = '$';
        #endregion

        #region Properties
        /// <summary>
        /// Regular expression used to identify escaped invalid filename characters in their wrapped hexadecimal representation.
        /// </summary>
        private static Regex FromFilenameRegex { get; } = new($@"\{WRAPPER}.*?\{WRAPPER}");
        /// <summary>
        /// An array containing characters that are not allowed in filenames.
        /// These characters will be used to identify and escape invalid filename characters.
        /// </summary>
        private static char[] InvalidFilenameCharacters { get; } = Path.GetInvalidFileNameChars();
        #endregion
        
        #region Methods
        /// <summary>
        /// Unescapes all characters that were previously escaped in a filename string and returns the original characters.
        /// </summary>
        /// <param name="_Filename">The <see cref="string"/> with escaped characters to unescape.</param>
        /// <returns>The <c>_Filename</c> with all escaped characters converted back to their original form.</returns>
        public static string UnescapeFilename(string _Filename)
        {
            return FromFilenameRegex.Replace(_Filename, _Match =>
            {
                var _hexValue = _Match.Value.ExtractBetween($@"\{WRAPPER}", $@"\{WRAPPER}");
                
                return ((char)Convert.ToInt32(_hexValue, 16)).ToString();
            });
        }

        /// <summary>
        /// Escapes all characters in a filename that are not valid in filenames,
        /// converting them to a hexadecimal representation wrapped in a special character.
        /// </summary>
        /// <param name="_Filename">The <see cref="string"/> containing characters to escape.</param>
        /// <returns>The <c>_Filename</c> with all invalid characters converted to their hexadecimal representations.</returns>
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