using JetBrains.Annotations;

namespace CustomScriptTemplates.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="object"/>.
    /// </summary>
    public static class ObjectExtensions
    {
        #region Methods
        /// <summary>
        /// Wraps this object's <see cref="object.ToString"/> output in a Rich Text bold tag.
        /// </summary>
        /// <param name="_Object">The <see cref="object"/> to get the <see cref="object.ToString"/> output of.</param>
        /// <returns>A <see cref="string"/> containing this object's <see cref="object.ToString"/> output, wrapped in a Rich Text bold tag.</returns>
        public static string Bold([CanBeNull] this object _Object)
        {
            return $"<b>{_Object.OrNull()}</b>";
        }
        
        /// <summary>
        /// Returns the contents of the <see cref="object.ToString()"/> method of this <see cref="string"/>, or the <see cref="string"/> <c>null</c>, if this <see cref="string"/> is <c>null</c>.
        /// </summary>
        /// <param name="_Object">The <see cref="string"/> to get the <see cref="object.ToString()"/> contents of.</param>
        /// <returns>The contents of the <see cref="object.ToString()"/> method of this <see cref="string"/>, or the <see cref="string"/> <c>null</c>, if this <see cref="string"/> is <c>null</c>.</returns>
        public static string OrNull([CanBeNull] this object _Object)
        {
            return $"{_Object?.ToString() ?? "null"}";
        }
        #endregion
    }
}