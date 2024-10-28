#nullable enable
using System;
using UnityEngine;

namespace IfLoooop.Utilities.Singleton
{
    /// <summary>
    /// Automatically creates a singleton instance of the given <see cref="Type"/> <c>T</c> in <see cref="Awake"/>.
    /// </summary>
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        #region Fields
        /// <summary>
        /// Singleton instance of the given <see cref="Type"/> <c>T</c>.
        /// </summary>
        protected static T? Instance;
        #endregion

        #region Properties
        /// <summary>
        /// Specifies in what method the singleton instance will be initialized.
        /// </summary>
        protected abstract InitializationMethod InitializationMethod { get; }
        /// <summary>
        /// Set to <c>true</c> if the singleton instance should not be destroyed on load.
        /// </summary>
        protected new virtual bool DontDestroyOnLoad => false;
        #endregion
        
        #region Methods
        protected virtual void Awake()
        {
            if (this.InitializationMethod == InitializationMethod.Awake)
            {
                this.Init();
            }
        }

        protected virtual void OnEnable()
        {
            if (this.InitializationMethod == InitializationMethod.OnEnable)
            {
                this.Init();
            }
        }

        protected virtual void Start()
        {
            if (this.InitializationMethod == InitializationMethod.Start)
            {
                this.Init();
            }
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (this.InitializationMethod == InitializationMethod.OnValidate)
            {
                this.Init();
            }
        }
#endif
        protected virtual void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        /// <summary>
        /// Initializes <see cref="Instance"/> if it is not already set.
        /// </summary>
        private void Init()
        {
            if (Instance != null)
            {
                Destroy(base.gameObject);
                return;
            }

            Instance = this as T;

            if (this.DontDestroyOnLoad)
            {
                DontDestroyOnLoad(Instance);
            }
        }
        #endregion
    }
}