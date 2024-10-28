#nullable enable
using System;

namespace IfLoooop.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="Uri"/>.
    /// </summary>
    public static class UriExtensions
    {
        #region Methods
        /// <summary>
        /// Converts this <see cref="Uri"/> into a hyperlink.
        /// </summary>
        /// <param name="_URL">The URL/Path.</param>
        /// <param name="_Content">The display name. (WIll be the <c>_Link</c> if not set)</param>
        /// <param name="_Line">
        /// If the link is for a file on the local machine, this is the line number where to jump to when opening the file. <br/>
        /// <i>Doesn't seem to work right now.</i>
        /// </param>
        /// <returns>The <see cref="Uri"/> converted into a hyperlink.</returns>
        public static string ToHyperLink(this Uri _URL, object? _Content = null, int? _Line = null)
        {
            return _URL.OriginalString.ToHyperlink(_Content, _Line);
        }
        #endregion
    }
}