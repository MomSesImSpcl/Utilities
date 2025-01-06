using System.IO;
using System.Runtime.CompilerServices;
using MomSesImSpcl.Extensions;

namespace MomSesImSpcl.Utilities.Logging
{
    /// <summary>
    /// Represents information about the caller of a method, including file path, member name, and line number.
    /// </summary>
    public sealed class CallerInfo
    {
        #region Fields
        /// <summary>
        /// Holds the caller information as a formatted string, including file path, member name, and line number.
        /// </summary>
        private readonly string callerInfo;
        #endregion

        #region Operators
        /// <summary>
        /// Defines an implicit conversion from a <see cref="CallerInfo"/> to a string.
        /// </summary>
        /// <param name="_CallerInfo">The instance of <see cref="CallerInfo"/> to be converted.</param>
        /// <returns>A string representation of the caller information.</returns>
        public static implicit operator string(CallerInfo _CallerInfo) => _CallerInfo.callerInfo;
        #endregion
        
        #region Constructors
        /// <summary>
        /// <see cref="CallerInfo"/>.
        /// </summary>
        /// <param name="_CallerInfo"><see cref="callerInfo"/>.</param>
        private CallerInfo(string _CallerInfo)
        {
            this.callerInfo = _CallerInfo;
        }
        #endregion
        
        #region Methods
        /// <summary>
        /// Retrieves information about the caller of a method, including file path, member name, and line number.
        /// </summary>
        /// <param name="_FilePath">The file path of the source code that calls this method. This is automatically provided by the compiler.</param>
        /// <param name="_MemberName">The name of the member (method, property, etc.) that calls this method. This is automatically provided by the compiler.</param>
        /// <param name="_LineNumber">The line number in the source code at which this method is called. This is automatically provided by the compiler.</param>
        /// <returns>An instance of <see cref="CallerInfo"/> containing the caller information.</returns>
        public static CallerInfo GetCallerInfo([CallerFilePath] string _FilePath = "", [CallerMemberName] string _MemberName = "", [CallerLineNumber] int _LineNumber = 0)
        {
            return new CallerInfo(FormatCallerInfo(_FilePath, _MemberName, _LineNumber));
        }

        /// <summary>
        /// Formats caller information into a string that includes the class name, method name, and line number.
        /// Optionally, formats this string as a clickable hyperlink for use in the Unity editor.
        /// </summary>
        /// <param name="_FilePath">The source file path of the caller.</param>
        /// <param name="_MemberName">The member name of the caller.</param>
        /// <param name="_LineNumber">The line number in the source file at which the call was made.</param>
        /// <param name="_ToHyperlink">Specifies whether to format the string as a clickable hyperlink (only in the Unity editor).</param>
        /// <returns>A formatted string with the caller's class, member name, and line number. Optionally, a clickable hyperlink in the Unity editor.</returns>
        public static string FormatCallerInfo(string _FilePath, string _MemberName, int _LineNumber, bool _ToHyperlink = true)
        {
            var _class = Path.GetFileNameWithoutExtension(_FilePath);
            var _callerInfo = $"{_class}.{_MemberName}:{_LineNumber}";
            
#if UNITY_EDITOR // Returns a clickable Hyperlink to the file in editor.
            if (_ToHyperlink)
            {
                _callerInfo = _FilePath.ToHyperlink(_callerInfo, _LineNumber);
            }
#endif
            return _callerInfo;
        }

        /// <summary>
        /// Returns a string representation of the caller information, including file path, member name, and line number.
        /// </summary>
        /// <returns>A string that describes the caller information.</returns>
        public override string ToString()
        {
            return this.callerInfo;
        }
        #endregion
    }
}