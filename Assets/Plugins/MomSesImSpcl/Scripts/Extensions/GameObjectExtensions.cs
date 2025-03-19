#nullable enable
using System;
using System.Collections.Generic;
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
        /// Searches for the given <see cref="Component"/> in the children of this <see cref="GameObject"/>.
        /// </summary>
        /// <param name="_GameObject">The <see cref="GameObject"/> to search under.</param>
        /// <param name="_IncludeInactive">Whether to include inactive children in the search.</param>
        /// <param name="_ExcludeSelf">Set to <c>true</c> to only search for the <see cref="Component"/> in the children and not on this <see cref="GameObject"/>.</param>
        /// <typeparam name="T">Must be of <see cref="Type"/> <see cref="Component"/>.</typeparam>
        /// <returns>The <see cref="Component"/> of the given <see cref="Type"/>, or <c>null</c> if it couldn't be found.</returns>
        public static T? GetComponentInChildren<T>(this GameObject _GameObject, bool _IncludeInactive, bool _ExcludeSelf) where T : Component
        {
            if (_ExcludeSelf)
            {
                var _transform = _GameObject.transform;
                
                // ReSharper disable once InconsistentNaming
                for (var i = 0; i < _transform.childCount; i++)
                {
                    var _child = _transform.GetChild(i);

                    if (!_IncludeInactive && !_child.gameObject.activeSelf)
                    {
                        continue;
                    }

                    foreach (var _component in _child.GetComponentsInChildren<T>(_IncludeInactive))
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
        
        /// <summary>
        /// Searches for the given <see cref="Component"/>s in the children of this <see cref="GameObject"/>.
        /// </summary>
        /// <param name="_GameObject">The <see cref="GameObject"/> to search under.</param>
        /// <param name="_IncludeInactive">Whether to include inactive children in the search.</param>
        /// <param name="_ExcludeSelf">Set to <c>true</c> to only search for the <see cref="Component"/> in the children and not on this <see cref="GameObject"/>.</param>
        /// <typeparam name="T">Must be of <see cref="Type"/> <see cref="Component"/>.</typeparam>
        /// <returns>The <see cref="Component"/>s of the given <see cref="Type"/>, or <c>null</c> if none could be found.</returns>
        public static IEnumerable<T> GetComponentsInChildren<T>(this GameObject _GameObject, bool _IncludeInactive, bool _ExcludeSelf) where T : Component
        {
            if (!_ExcludeSelf)
            {
                foreach (var _component in _GameObject.GetComponents<T>())
                {
                    yield return _component;
                }
            }

            var _transform = _GameObject.transform;

            // ReSharper disable once InconsistentNaming
            for (var i = 0; i < _transform.childCount; i++)
            {
                var _child = _transform.GetChild(i);

                if (!_IncludeInactive && !_child.gameObject.activeSelf)
                {
                    continue;
                }

                foreach (var _component in _child.GetComponentsInChildren<T>(_IncludeInactive))
                {
                    yield return _component;
                }
            }
        }
        #endregion
    }
}