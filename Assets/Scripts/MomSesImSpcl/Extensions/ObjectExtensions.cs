#nullable enable
using System.IO;
using System.Text;
using MomSesImSpcl.Utilities.Logging;

namespace MomSesImSpcl.Extensions
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
        /// <returns>A <see cref="string"/> containing this object's <see cref="object.ToString"/>-output, wrapped in a bold tag.</returns>
        public static string Bold(this object? _Object)
        {
            return $"<b>{_Object.OrNull()}</b>";
        }

        /// <summary>
        /// Wraps this object's <see cref="object.ToString"/>-output in a Rich Text color tag.
        /// </summary>
        /// <param name="_Object">The <see cref="object"/> to get the <see cref="object.ToString"/>-output of.</param>
        /// <param name="_Color">The <see cref="RichTextColor"/> to wrap the <see cref="object.ToString"/>-output with.</param>
        /// <returns>A <see cref="string"/> containing this object's <see cref="object.ToString"/>-output, wrapped in the specified color tag.</returns>
        public static string Color(this object? _Object, RichTextColor _Color)
        {
            return $"<color={_Color}>{_Object.OrNull()}</color>";
        }

        /// <summary>
        /// Wraps this object's <see cref="object.ToString"/>-output in a Rich Text italic tag.
        /// </summary>
        /// <param name="_Object">The <see cref="object"/> to get the <see cref="object.ToString"/>-output of.</param>
        /// <returns>A <see cref="string"/> containing this object's <see cref="object.ToString"/>-output, wrapped in an italic tag.</returns>
        public static string Italic(this object? _Object)
        {
            return $"<i>{_Object.OrNull()}</i>";
        }

        /// <summary>
        /// Returns the string representation of the object, or "null" if the object is null.
        /// </summary>
        /// <param name="_Object">The object to get the string representation of.</param>
        /// <returns>A string representing the object, or "null" if the object is null.</returns>
        public static string OrNull(this object? _Object)
        {
            return _Object?.ToString() ?? "null";
        }

        /// <summary>
        /// Wraps this object's <see cref="object.ToString"/>-output in a Rich Text underline tag. <br/>
        /// <i>Doesn't work in the default console, but works with TextMeshPro.</i>
        /// </summary>
        /// <param name="_Object">The <see cref="object"/> to get the <see cref="object.ToString"/>-output of.</param>
        /// <returns>A <see cref="string"/> containing this object's <see cref="object.ToString"/>-output, wrapped in an underline tag.</returns>
        public static string Underline(this object? _Object)
        {
            return $"<u>{_Object.OrNull()}</u>";
        }
        
        /// <summary>
        /// Converts the string representation of the specified object to a MemoryStream, using the provided encoding.
        /// </summary>
        /// <param name="_Object">The object whose string representation will be converted to a MemoryStream.</param>
        /// <param name="_Encoding">The encoding to use for the conversion.</param>
        /// <returns>A MemoryStream containing the encoded string representation of the specified object.</returns>
        public static MemoryStream ToMemoryStream(this object? _Object, Encoding _Encoding)
        {
            return new MemoryStream(_Encoding.GetBytes(_Object?.ToString()?.OrNull()!));
        }
        #endregion
    }
}
