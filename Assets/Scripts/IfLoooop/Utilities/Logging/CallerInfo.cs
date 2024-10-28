using System.IO;
using System.Runtime.CompilerServices;
using IfLoooop.Extensions;

namespace IfLoooop.Utilities.Logging
{
    /// <summary>
    /// Used to retrieve the class, member name and the line number from where the <see cref="GetCallerInfo"/> method is called.
    /// </summary>
    public sealed class CallerInfo
    {
        #region Fields
        /// <summary>
        /// Contains the class and member name + the line number, from where the <see cref="GetCallerInfo"/> method was called. <br/>
        /// <i>CLASS_NAME.MEMBER_NAME:LINE_NUMBER</i>
        /// </summary>
        private readonly string callerInfo;
        #endregion

        #region Operators
        /// <summary>
        /// Implicitly returns <see cref="callerInfo"/> from a <see cref="CallerInfo"/> object.
        /// </summary>
        /// <param name="_CallerInfo">A <see cref="CallerInfo"/> object.</param>
        /// <returns>The value of <see cref="callerInfo"/>.</returns>
        public static implicit operator string(CallerInfo _CallerInfo) => _CallerInfo.callerInfo;
        #endregion
        
        #region Constructors
        /// <summary>
        /// Create a new <see cref="CallerInfo"/>.
        /// </summary>
        /// <param name="_CallerInfo">This will be the value of <see cref="callerInfo"/>.</param>
        private CallerInfo(string _CallerInfo)
        {
            this.callerInfo = _CallerInfo;
        }
        #endregion
        
        #region Methods
        /// <summary>
        /// Creates a new <see cref="CallerInfo"/> with information about the calling member.
        /// </summary>
        /// <param name="_FilePath">Path to the file from where this method is called. (Leave empty)</param>
        /// <param name="_MemberName">Name of the member from which this method is called. (Leave empty)</param>
        /// <param name="_LineNumber">Line number in the file, from where this method is called. (Leave 0)</param>
        /// <returns>A new <see cref="CallerInfo"/>.</returns>
        public static CallerInfo GetCallerInfo([CallerFilePath] string _FilePath = "", [CallerMemberName] string _MemberName = "", [CallerLineNumber] int _LineNumber = 0)
        {
            return new CallerInfo(FormatCallerInfo(_FilePath, _MemberName, _LineNumber));
        }

        /// <summary>
        /// Formats caller information into a readable <see cref="string"/> containing the class name, member name, and line number. <br/>
        /// <i>Returns a clickable hyperlink to the file, when output in the console.</i>
        /// </summary>
        /// <param name="_FilePath">Path to the file from where this method is called.</param>
        /// <param name="_MemberName">Name of the member from which this method is called.</param>
        /// <param name="_LineNumber">Line number in the file, from where this method is called.</param>
        /// <param name="_ToHyperlink">Set to <c>fasle</c>, to not create a hyperlink when output to the console (Editor only).</param>
        /// <returns>A formatted <see cref="string"/> that includes the class name, member name, and line number.</returns>
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
        /// Returns the value of <see cref="callerInfo"/>
        /// </summary>
        /// <returns>The value of <see cref="callerInfo"/></returns>
        public override string ToString()
        {
            return this.callerInfo;
        }
        #endregion
    }
}