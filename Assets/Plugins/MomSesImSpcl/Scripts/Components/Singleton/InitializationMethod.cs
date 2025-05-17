namespace MomSesImSpcl.Components.Singleton
{
    /// <summary>
    /// Specifies the method in which the singleton instance should be initialized.
    /// </summary>
    public enum InitializationMethod
    {
        /// <summary>
        /// Will be initialized in the Awake method.
        /// </summary>
        Awake,
        /// <summary>
        /// Will be initialized in the OnEnable method.
        /// </summary>
        OnEnable,
        /// <summary>
        /// Will be initialized in the Start method.
        /// </summary>
        Start,
        /// <summary>
        /// When the singleton should be initialized manually.
        /// </summary>
        Manual,
        /// <summary>
        /// Will be initialized in the OnValidate method.
        /// </summary>
        OnValidate
    }
}
