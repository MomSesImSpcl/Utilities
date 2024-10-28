using System;
using System.Reflection;

namespace IfLoooop.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="Exception"/>.
    /// </summary>
    public static class ExceptionExtensions
    {
        #region Methods
        /// <summary>
        /// Injects a custom <c>_Message</c> into this <see cref="Exception"/>.
        /// </summary>
        /// <param name="_Exception">The <see cref="Exception"/> to inject the <c>_Message</c> into.</param>
        /// <param name="_Message">The message to inject.</param>
        /// <param name="_InsertBefore">if <c>true</c>, the <c>_Message</c> will be inserted before the existing <see cref="Exception.Message"/>.</param>
        /// <returns>This <see cref="Exception"/>.</returns>
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