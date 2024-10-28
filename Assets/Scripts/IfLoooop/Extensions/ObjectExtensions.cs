#nullable enable
using System.IO;
using System.Text;
using IfLoooop.Utilities;
using IfLoooop.Utilities.Logging;

namespace IfLoooop.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="object"/>.
    /// </summary>
    public static class ObjectExtensions
    {
        #region Methods
        /// <summary>
        /// Wraps this object's <see cref="object.ToString"/>-output in a Rich Text bold tag.
        /// </summary>
        /// <param name="_Object">The <see cref="object"/> to get the <see cref="object.ToString"/>-output of.</param>
        /// <returns>A <see cref="string"/> containing this object's <see cref="object.ToString"/>-output, wrapped in a Rich Text bold tag.</returns>
        public static string Bold(this object? _Object)
        {
            return $"<b>{_Object.OrNull()}</b>";
        }
        
        /// <summary>
        /// Wraps this object's <see cref="object.ToString"/>-output in a Rich Text color tag.
        /// </summary>
        /// <param name="_Object">The <see cref="object"/> to get the <see cref="object.ToString"/>-output of.</param>
        /// <param name="_Color">The <see cref="RichTextColor"/> to apply to the output.</param>
        /// <returns>A <see cref="string"/> containing this object's <see cref="object.ToString"/>-output, wrapped in the given <see cref="RichTextColor"/>.</returns>
        public static string Color(this object? _Object, RichTextColor _Color)
        {
            return $"<color={_Color}>{_Object.OrNull()}</color>";
        }
        
        /// <summary>
        /// Wraps this object's <see cref="object.ToString"/>-output in a Rich Text italic tag.
        /// </summary>
        /// <param name="_Object">The <see cref="object"/> to get the <see cref="object.ToString"/>-output of.</param>
        /// <returns>A <see cref="string"/> containing this object's <see cref="object.ToString"/>-output, wrapped in a Rich Text italic tag.</returns>
        public static string Italic(this object? _Object)
        {
            return $"<i>{_Object.OrNull()}</i>";
        }
        
        /// <summary>
        /// Returns this object's <see cref="object.ToString"/>-output, or the <see cref="string"/> "null" if this <see cref="object"/> is <c>null</c>.
        /// </summary>
        /// <param name="_Object">The object to get the <see cref="object.ToString"/>-output of.</param>
        /// <returns>This object's <see cref="object.ToString"/>-output, or the <see cref="string"/> "null" if this <see cref="object"/> is <c>null</c>.</returns>
        public static string OrNull(this object? _Object)
        {
            return _Object?.ToString() ?? "null";
        }

        /// <summary>
        /// Converts this object's <see cref="object.ToString"/>-output into a <see cref="MemoryStream"/>.
        /// </summary>
        /// <param name="_Object">The <see cref="object"/> to get the <see cref="object.ToString"/>-output of.</param>
        /// <param name="_Encoding">The <see cref="Encoding"/> to use when converting into a <see cref="MemoryStream"/>.</param>
        /// <returns>This object's <see cref="object.ToString"/>-output as a <see cref="MemoryStream"/>.</returns>
        public static MemoryStream ToMemoryStream(this object? _Object, Encoding _Encoding)
        {
            return new MemoryStream(_Encoding.GetBytes(_Object?.ToString()?.OrNull()!));
        }
        #endregion
    }
}