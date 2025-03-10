using MomSesImSpcl.Utilities.Logging;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <c>struct</c>.
    /// </summary>
    public static class StructExtensions
    {
        #region Methods
        /// <summary>
        /// Wraps this <see cref="object"/>'s <c>ToString()</c> output in a Rich Text bold tag.
        /// </summary>
        /// <param name="_Struct">The <see cref="object"/> to wrap.</param>
        /// <returns>The <see cref="object"/>'s <c>ToString()</c> output wrapped in an bold tag.</returns>
        public static string Bold<T>(this T _Struct) where T : struct
        {
            return $"<b>{_Struct.ToString()}</b>";
        }
        
        /// <summary>
        /// Wraps this <see cref="object"/>'s <c>ToString()</c> output in a Rich Text color tag.
        /// </summary>
        /// <param name="_Struct">The <see cref="object"/> to wrap.</param>
        /// <param name="_Color">The <see cref="RichTextColor"/> to display the <c>ToString()</c> output in.</param>
        /// <returns>The <see cref="object"/> wrapped in a color tag.</returns>
        public static string Color<T>(this T _Struct, RichTextColor _Color) where T : struct
        {
            return $"<color={_Color.GetName()}>{_Struct.ToString()}</color>";
        }
        
        /// <summary>
        /// Wraps this <see cref="object"/>'s <c>ToString()</c> output in a Rich Text italic tag.
        /// </summary>
        /// <param name="_Struct">The <see cref="object"/> to wrap.</param>
        /// <returns>The <see cref="object"/> wrapped in an italic tag.</returns>
        public static string Italic<T>(this T _Struct) where T : struct
        {
            return $"<i>{_Struct.ToString()}</i>";
        }
        
        /// <summary>
        /// Wraps this <see cref="object"/>'s <c>ToString()</c> output in a Rich Text underline tag.
        /// </summary>
        /// <param name="_Struct">The <see cref="object"/> to wrap.</param>
        /// <returns>The <see cref="object"/> wrapped in an underline tag.</returns>
        public static string Underline<T>(this T _Struct) where T : struct
        {
            return $"<u>{_Struct.ToString()}</u>";
        }
        #endregion
    }
}