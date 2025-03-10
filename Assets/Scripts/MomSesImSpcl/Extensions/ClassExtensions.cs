#nullable enable
using MomSesImSpcl.Utilities.Logging;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <c>class</c>.
    /// </summary>
    public static class ClassExtensions
    {
        #region Methods
        /// <summary>
        /// Wraps this <see cref="object"/>'s <c>ToString()</c> output in a Rich Text bold tag.
        /// </summary>
        /// <param name="_Class">The <see cref="object"/> to wrap.</param>
        /// <returns>The <see cref="object"/>'s <c>ToString()</c> output wrapped in an bold tag.</returns>
        public static string Bold<T>(this T? _Class) where T : class
        {
            return $"<b>{_Class.OrNull()}</b>";
        }
        
        /// <summary>
        /// Wraps this <see cref="object"/>'s <c>ToString()</c> output in a Rich Text color tag.
        /// </summary>
        /// <param name="_Struct">The <see cref="object"/> to wrap.</param>
        /// <param name="_Color">The <see cref="RichTextColor"/> to display the <c>ToString()</c> output in.</param>
        /// <returns>The <see cref="object"/> wrapped in a color tag.</returns>
        public static string Color<T>(this T? _Struct, RichTextColor _Color) where T : class
        {
            return $"<color={_Color.GetName()}>{_Struct.OrNull()}</color>";
        }
        
        /// <summary>
        /// Wraps this <see cref="object"/>'s <c>ToString()</c> output in a Rich Text italic tag.
        /// </summary>
        /// <param name="_Class">The <see cref="object"/> to wrap.</param>
        /// <returns>The <see cref="object"/> wrapped in an italic tag.</returns>
        public static string Italic<T>(this T? _Class) where T : class
        {
            return $"<i>{_Class.OrNull()}</i>";
        }
        
        /// <summary>
        /// Returns the <see cref="string"/> or <c>null</c> as a <see cref="string"/> if the <see cref="object"/> is <c>null</c>.
        /// </summary>
        /// <param name="_Class">The <see cref="object"/> whose <c>ToString()</c> output to return.</param>
        /// <returns>The <see cref="object"/> or <c>null</c> as a <see cref="string"/>.</returns>
        public static string OrNull<T>(this T? _Class) where T : class
        {
            return _Class != null ? _Class.ToString() : "null";
        }
        
        /// <summary>
        /// Wraps this <see cref="object"/>'s <c>ToString()</c> output in a Rich Text underline tag.
        /// </summary>
        /// <param name="_Class">The <see cref="object"/> to wrap.</param>
        /// <returns>The <see cref="object"/> wrapped in an underline tag.</returns>
        public static string Underline<T>(this T? _Class) where T : class
        {
            return $"<u>{_Class.OrNull()}</u>";
        }
        #endregion
    }
}
