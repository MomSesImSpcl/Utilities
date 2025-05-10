#nullable enable
using System;
using System.Runtime.CompilerServices;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="Uri"/>.
    /// </summary>
    public static class UriExtensions
    {
        #region Methods
        /// <summary>
        /// Converts the specified URI to an HTML hyperlink.
        /// </summary>
        /// <param name="_URL">The URI to convert.</param>
        /// <param name="_Content">The content to display inside the hyperlink. If null, the URI string is displayed.</param>
        /// <param name="_Line">The line number to include in the hyperlink as an attribute. If null, the attribute is omitted.</param>
        /// <returns>The formatted HTML hyperlink.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToHyperLink(this Uri _URL, object? _Content = null, int? _Line = null)
        {
            return _URL.OriginalString.ToHyperlink(_Content, _Line);
        }
        #endregion
    }
}
