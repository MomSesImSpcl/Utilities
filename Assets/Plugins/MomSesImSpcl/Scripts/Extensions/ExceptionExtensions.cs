using System;
using System.Reflection;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="Exception"/>.
    /// </summary>
    public static class ExceptionExtensions
    {
        #region Methods
        /// <summary>
        /// Appends or prepends a custom message to an exception's existing message.
        /// </summary>
        /// <param name="_Exception">The exception to modify.</param>
        /// <param name="_Message">The custom message to add.</param>
        /// <param name="_InsertBefore">Indicates whether to prepend the custom message.
        /// If false, the custom message will be appended.</param>
        /// <return>The modified exception with the updated message.</return>
        public static Exception CustomMessage(this Exception _Exception, string _Message, bool _InsertBefore = false)
        {
            var _field = typeof(Exception).GetField("_message", BindingFlags.Instance | BindingFlags.NonPublic);
            var _message = _field!.GetValue(_Exception) as string;

            _field.SetValue(_Exception, _InsertBefore ? $"{_Message}\n{_message}" : $"{_message}\n{_Message}");

            return _Exception;
        }
        #endregion
    }
}