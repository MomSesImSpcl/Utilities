using System;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace MomSesImSpcl.Data
{
    /// <summary>
    /// Wrapper to serialize an interface in the inspector.
    /// </summary>
    /// <typeparam name="T">Should be an interface.</typeparam>
    [Serializable]
#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.HideLabel]
#endif
    public sealed class InterfaceReference<T>
#if UNITY_EDITOR
        : ISerializationCallbackReceiver where T : class
#endif
    {
        #region Inspector Fields
        [Tooltip("Reference to the Object that implements the given interface of type T.")]
        [SerializeField] private Object interfaceReference;
        #endregion
        
        #region Properites
        /// <summary>
        /// Returns <see cref="interfaceReference"/> as the interface <c>T</c>.
        /// </summary>
        public T Interface => this.interfaceReference as T;
        #endregion
        
        #region Operators
        /// <summary>
        /// Implicitly checks if <see cref="interfaceReference"/> is not <c>null</c>.
        /// </summary>
        /// <param name="_InterfaceReference">The <see cref="InterfaceReference{T}"/> to check the <see cref="interfaceReference"/> of.</param>
        /// <returns><c>true</c> if <see cref="interfaceReference"/> is not <c>null</c>, otherwise <c>false</c>.</returns>
        public static implicit operator bool(InterfaceReference<T> _InterfaceReference) => _InterfaceReference.interfaceReference is not null;
        #endregion
        
#if UNITY_EDITOR
        #region Methods
        private void OnValidate()
        {
            if (this.interfaceReference is T || this.interfaceReference is not GameObject _gameObject)
            {
                return;
            }
                
            this.interfaceReference = null;
                    
            foreach (var _component in _gameObject.GetComponents<Component>())
            {
                if (_component is not T)
                {
                    continue;
                }
                        
                this.interfaceReference = _component;
                break;
            }

            if (this.interfaceReference is null)
            {
                Debug.LogWarning($"The given GameObject doesn't implement the interface: {typeof(T).Name}");
            }
        }
        
        void ISerializationCallbackReceiver.OnBeforeSerialize() => this.OnValidate();
        void ISerializationCallbackReceiver.OnAfterDeserialize() { }
        #endregion
#endif
    }
}
