#nullable enable
using UnityEngine;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="Object"/>.
    /// </summary>
    public static class UnityEngineObjectExtensions
    {
        #region Methods
        /// <summary>
        /// Tries to cast this <see cref="Object"/> to the given <see cref="System.Type"/> <c>T</c> or get the <see cref="Component"/> from this <see cref="Object"/>.
        /// </summary>
        /// <param name="_Object">The <see cref="Object"/> to cast.</param>
        /// <typeparam name="T">Must be of <see cref="System.Type"/> <see cref="Object"/>.</typeparam>
        /// <returns>This <see cref="Object"/> as the given <see cref="System.Type"/> <c>T</c> or <c>null</c>.</returns>
        public static T? Get<T>(this Object _Object) where T : Object
        {
            if (_Object is T _object)
            {
                return _object;
            }
            if (_Object is Component _component)
            {
                return _component.GetComponent<T>();
            }
            if (_Object is GameObject _gameObject)
            {
                return _gameObject.GetComponent<T>();
            }
            if (_Object is ScriptableObject _scriptableObject)
            {
                return _scriptableObject as T;
            }

            return _Object as T;
        }
        #endregion
    }
}