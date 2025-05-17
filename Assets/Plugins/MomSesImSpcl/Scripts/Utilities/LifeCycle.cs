namespace MomSesImSpcl.Utilities
{
    /// <summary>
    /// Represents the unity lifecycle methods.
    /// </summary>
    public enum LifeCycle
    {
        Awake,
        OnEnable,
        Start,
#if UNITY_EDITOR
        /// <summary>
        /// Editor only.
        /// </summary>
        OnValidate,
#endif
        OnDisable,
        OnDestroy
    }
}
