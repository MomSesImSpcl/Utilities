#nullable enable
using System;
using UnityEngine;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extensions methods for <see cref="GameObject"/>.
    /// </summary>
    public static class GameObjectExtensions
    {
        #region Methods
        /// <summary>
        /// Searched for the given <see cref="Component"/> in the children of this <see cref="GameObject"/>.
        /// </summary>
        /// <param name="_GameObject">The <see cref="GameObject"/> to search under.</param>
        /// <param name="_IncludeInactive">Whether to include inactive children in the search..</param>
        /// <param name="_ExcludeSelf">Set to <c>true</c> to only search for the <see cref="Component"/> in the children and not on this <see cref="GameObject"/>.</param>
        /// <typeparam name="T">Must be of <see cref="Type"/> <see cref="Component"/>.</typeparam>
        /// <returns>The <see cref="Component"/> of the given <see cref="Type"/>, or <c>null</c> if it couldn't be found.</returns>
        public static T? GetComponentInChildren<T>(this GameObject _GameObject, bool _IncludeInactive, bool _ExcludeSelf) where T : Component
        {
            if (_ExcludeSelf)
            {
                // ReSharper disable once InconsistentNaming
                for (var i = 0; i < _GameObject.transform.childCount; i++)
                {
                    var _child = _GameObject.transform.GetChild(i);

                    if (!_IncludeInactive && !_child.gameObject.activeSelf)
                    {
                        continue;
                    }
                    
                    if (_child.GetComponent<T>() is {} _component)
                    {
                        return _component;
                    }
                }
            }
            else
            {
                return _GameObject.GetComponentInChildren<T>(_IncludeInactive);
            }

            return null;
        }
        #endregion
    }
}