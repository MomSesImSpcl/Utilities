using System.Runtime.CompilerServices;
using UnityEngine;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="Rect"/>.
    /// </summary>
    public static class RectExtensions
    {
        #region Methods
        /// <summary>
        /// Returns the extends of this <see cref="Rect"/>.
        /// </summary>
        /// <param name="_Rect">The <see cref="Rect"/> to get the extends of.</param>
        /// <returns>The extends of this <see cref="Rect"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Extends(this Rect _Rect)
        {
            return new Vector2(_Rect.width * .5f, _Rect.height * .5f);
        }
        #endregion
    }
}