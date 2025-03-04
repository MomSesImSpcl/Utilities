#nullable enable
using System;
using System.Collections.Generic;
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
        /// Searches for the given <see cref="Component"/> in the children of this <see cref="MonoBehaviour"/>.
        /// </summary>
        /// <param name="_MonoBehaviour">The <see cref="MonoBehaviour"/> to search under.</param>
        /// <param name="_IncludeInactive">Whether to include inactive children in the search.</param>
        /// <param name="_ExcludeSelf">Set to <c>true</c> to only search for the <see cref="Component"/> in the children and not on this <see cref="MonoBehaviour"/>.</param>
        /// <typeparam name="T">Must be of <see cref="Type"/> <see cref="Component"/>.</typeparam>
        /// <returns>The <see cref="Component"/> of the given <see cref="Type"/>, or <c>null</c> if it couldn't be found.</returns>
        public static T? GetComponentInChildren<T>(this MonoBehaviour _MonoBehaviour, bool _IncludeInactive, bool _ExcludeSelf) where T : Component
        {
            return _MonoBehaviour.gameObject.GetComponentInChildren<T>(_IncludeInactive, _ExcludeSelf);
        }
        
        /// <summary>
        /// Searches for the given <see cref="Component"/>s in the children of this <see cref="MonoBehaviour"/>.
        /// </summary>
        /// <param name="_MonoBehaviour">The <see cref="MonoBehaviour"/> to search under.</param>
        /// <param name="_IncludeInactive">Whether to include inactive children in the search.</param>
        /// <param name="_ExcludeSelf">Set to <c>true</c> to only search for the <see cref="Component"/> in the children and not on this <see cref="MonoBehaviour"/>.</param>
        /// <typeparam name="T">Must be of <see cref="Type"/> <see cref="Component"/>.</typeparam>
        /// <returns>The <see cref="Component"/>s of the given <see cref="Type"/>, or <c>null</c> if none could be found.</returns>
        public static IEnumerable<T> GetComponentsInChildren<T>(this MonoBehaviour _MonoBehaviour, bool _IncludeInactive, bool _ExcludeSelf) where T : Component
        {
            return _MonoBehaviour.gameObject.GetComponentsInChildren<T>(_IncludeInactive, _ExcludeSelf);
        }
        #endregion
    }
}
