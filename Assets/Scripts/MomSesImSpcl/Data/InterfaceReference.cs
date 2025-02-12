using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MomSesImSpcl.Data
{
    /// <summary>
    /// Wrapper to serialize an interface in the inspector.
    /// </summary>
    /// <typeparam name="T">Should be an interface.</typeparam>
    [Serializable]
    public class InterfaceReference<T> : ISerializationCallbackReceiver where T : class
    {
        #region Inspector Fields
        [Tooltip("The class that implements the interface.")]
        [SerializeField] private Object target;
        #endregion
        
        #region Properites
        /// <summary>
        /// Returns <see cref="target"/> as the interface <c>T</c>.
        /// </summary>
        public T Interface => this.target as T;
        #endregion
        
        #region Operators
        /// <summary>
        /// Implicitly checks if <see cref="target"/> is not <c>null</c>.
        /// </summary>
        /// <param name="_InterfaceReference">The <see cref="InterfaceReference{T}"/> to check the <see cref="target"/> of.</param>
        /// <returns><c>true</c> if <see cref="target"/> is not <c>null</c>, otherwise <c>false</c>.</returns>
        public static implicit operator bool(InterfaceReference<T> _InterfaceReference) => _InterfaceReference.target is not null;
        #endregion
        
        #region Methods
        private void OnValidate()
        {
            if (this.target is T || this.target is not GameObject _gameObject)
            {
                return;
            }
                
            this.target = null;
                    
            foreach (var _component in _gameObject.GetComponents<Component>())
            {
                if (_component is not T)
                {
                    continue;
                }
                        
                this.target = _component;
                break;
            }

            if (this.target is null)
            {
                Debug.LogWarning($"The given GameObject doesn't implement the interface: {typeof(T).Name}");
            }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize() => this.OnValidate();
        void ISerializationCallbackReceiver.OnAfterDeserialize() { }
        #endregion
    }
}
