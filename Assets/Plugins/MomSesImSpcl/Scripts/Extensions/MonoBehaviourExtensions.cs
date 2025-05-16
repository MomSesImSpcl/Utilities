#nullable enable
using System.Runtime.CompilerServices;
using UnityEngine;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="MonoBehaviour"/>.
    /// </summary>
    public static class MonoBehaviourExtensions
    {
        #region Methods
        /// <summary>
        /// Gets the <see cref="RectTransform"/> from this <see cref="MonoBehaviour"/>.
        /// </summary>
        /// <param name="_MonoBehaviour">The <see cref="MonoBehaviour"/> to get the <see cref="RectTransform"/> from.</param>
        /// <typeparam name="T"><see cref="MonoBehaviour"/>.</typeparam>
        /// <returns>The <see cref="RectTransform"/> of this <see cref="MonoBehaviour"/>, or <c>null</c> if the <see cref="Transform"/> <see cref="Component"/> is not a <see cref="RectTransform"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectTransform? RectTransform<T>(this T _MonoBehaviour) where T : MonoBehaviour
        {
            return _MonoBehaviour.transform.AsRectTransform();
        }
        #endregion
    }
}