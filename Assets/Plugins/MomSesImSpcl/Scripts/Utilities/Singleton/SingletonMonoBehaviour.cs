#nullable enable
using UnityEngine;

namespace MomSesImSpcl.Utilities.Singleton
{
    /// <summary>
    /// A generic base class for creating singleton <see cref="MonoBehaviour"/> instances in Unity.
    /// </summary>
    /// <typeparam name="T">The type of the singleton class inheriting from this base class.</typeparam>
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        #region Fields
        /// <summary>
        /// Holds the singleton instance of the derived class.
        /// </summary>
        protected static T? Instance;
        #endregion

        #region Properties
        /// <summary>
        /// Specifies the method used to initialize the singleton instance.
        /// </summary>
        protected abstract InitializationMethod InitializationMethod { get; }
        /// <summary>
        /// Indicates whether the singleton instance should persist across scene loads.
        /// If true, the instance will not be destroyed when loading a new scene.
        /// </summary>
        // ReSharper disable once VirtualMemberNeverOverridden.Global
        protected new virtual bool DontDestroyOnLoad => false;
        /// <summary>
        /// Set this to <c>true</c> to also initialize this singleton in edit mode inside the <see cref="OnEnable"/>. <br/>
        /// <i>The <see cref="ExecuteInEditMode"/> or <see cref="ExecuteAlways"/> attribute must be added to the inheriting class for this to work.</i>
        /// </summary>
        protected virtual bool EditorInitialization => false;
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
#if UNITY_EDITOR
            if (!Application.isPlaying && this.EditorInitialization)
            {
                this.Init();
            }
#endif
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
        /// Initializes the singleton instance of the derived MonoBehaviour class.
        /// </summary>
        /// <remarks>
        /// This method ensures that only one instance of the derived class is instantiated. If an instance already exists,
        /// the current gameObject will be destroyed. If no instance exists, the current instance is assigned and, if specified,
        /// will not be destroyed on load of a new scene.
        /// </remarks>
        private void Init()
        {
            if (Instance is not null)
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    return;
                }
#endif
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