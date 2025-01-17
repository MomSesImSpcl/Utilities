#if ODIN_INSPECTOR
#nullable enable
using MomSesImSpcl.Utilities.Singleton;
using Sirenix.OdinInspector;

namespace MomSesImSpcl.Utilities.Singleton
{
    /// <summary>
    /// A generic base class for creating singleton <see cref="SerializedMonoBehaviour"/> instances in Unity.
    /// </summary>
    /// <typeparam name="T">The type of the singleton class inheriting from this base class.</typeparam>
    public abstract class SerializedSingletonMonoBehaviour<T> : SerializedMonoBehaviour where T : SerializedSingletonMonoBehaviour<T>
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
        /// Initializes the singleton instance of the derived MonoBehaviour class.
        /// </summary>
        /// <remarks>
        /// This method ensures that only one instance of the derived class is instantiated. If an instance already exists,
        /// the current gameObject will be destroyed. If no instance exists, the current instance is assigned and, if specified,
        /// will not be destroyed on load of a new scene.
        /// </remarks>
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
#endif
