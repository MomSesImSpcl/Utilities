using System.Text;
using MomSesImSpcl.Utilities.Logging;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="StringBuilder"/>.
    /// </summary>
    public static class StringBuilderExtensions
    {
        #region Methods
        /// <summary>
        /// Wraps the content of this <see cref="StringBuilder"/> around a <c>&lt;b&gt;&lt;/b&gt;</c> tag.
        /// </summary>
        /// <param name="_StringBuilder">The <see cref="StringBuilder"/> containing the <see cref="string"/> to wrap.</param>
        /// <param name="_Index">The index in the <see cref="string"/> at which to insert the opening tag.</param>
        /// <returns>This <see cref="StringBuilder"/>.</returns>
        public static StringBuilder Bold(this StringBuilder _StringBuilder, int _Index = 0)
        {
            _StringBuilder.Insert(_Index, "<b>");
            _StringBuilder.Append("</b>");

            return _StringBuilder;
        }

        /// <summary>
        /// Clears this <see cref="StringBuilder"/> and returns the underlying <see cref="string"/>.
        /// </summary>
        /// <param name="_StringBuilder">The <see cref="StringBuilder"/> to return the <see cref="string"/> of.</param>
        /// <returns>The underlying <see cref="string"/> of this <see cref="StringBuilder"/>.</returns>
        public static string GetAndClear(this StringBuilder _StringBuilder)
        {
            var _string = _StringBuilder.ToString();
            _StringBuilder.Clear();
            return _string;
        }
        
        /// <summary>
        /// Wraps the content of this <see cref="StringBuilder"/> around a <c>&lt;color&gt;&lt;/color&gt;</c> tag.
        /// </summary>
        /// <param name="_StringBuilder">The <see cref="StringBuilder"/> containing the <see cref="string"/> to wrap.</param>
        /// <param name="_Color">The <see cref="RichTextColor"/> value.</param>
        /// <param name="_Index">The index in the <see cref="string"/> at which to insert the opening tag.</param>
        /// <returns>This <see cref="StringBuilder"/>.</returns>
        public static StringBuilder Color(this StringBuilder _StringBuilder, RichTextColor _Color, int _Index = 0)
        {
            const string _OPENING_TAG = "<color=";
            var _color = _Color.GetName();
            const string _CLOSING_TAG = ">";
            
            _StringBuilder.Insert(_Index, _OPENING_TAG);
            _StringBuilder.Insert(_Index + _OPENING_TAG.Length, _color);
            _StringBuilder.Insert(_Index + _OPENING_TAG.Length + _color.Length, _CLOSING_TAG);
            _StringBuilder.Append("</color>");

            return _StringBuilder;
        }
        
        /// <summary>
        /// Wraps the content of this <see cref="StringBuilder"/> around a <c>&lt;i&gt;&lt;/i&gt;</c> tag.
        /// </summary>
        /// <param name="_StringBuilder">The <see cref="StringBuilder"/> containing the <see cref="string"/> to wrap.</param>
        /// <param name="_Index">The index in the <see cref="string"/> at which to insert the opening tag.</param>
        /// <returns>This <see cref="StringBuilder"/>.</returns>
        public static StringBuilder Italic(this StringBuilder _StringBuilder, int _Index = 0)
        {
            _StringBuilder.Insert(_Index, "<i>");
            _StringBuilder.Append("</i>");

            return _StringBuilder;
        }
        
        /// <summary>
        /// Wraps the content of this <see cref="StringBuilder"/> around a <c>&lt;u&gt;&lt;/u&gt;</c> tag.
        /// </summary>
        /// <param name="_StringBuilder">The <see cref="StringBuilder"/> containing the <see cref="string"/> to wrap.</param>
        /// <param name="_Index">The index in the <see cref="string"/> at which to insert the opening tag.</param>
        /// <returns>This <see cref="StringBuilder"/>.</returns>
        public static StringBuilder Underline(this StringBuilder _StringBuilder, int _Index = 0)
        {
            _StringBuilder.Insert(_Index, "<u>");
            _StringBuilder.Append("</u>");

            return _StringBuilder;
        }
        #endregion
    }
}
