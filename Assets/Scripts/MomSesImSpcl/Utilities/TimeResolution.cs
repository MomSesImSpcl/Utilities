namespace MomSesImSpcl.Utilities
{
    /// <summary>
    /// Specifies a unit of time measurement.
    /// </summary>
    public enum TimeResolution
    {
        /// <summary>
        /// Automatically chooses the most appropriate time resolution.
        /// </summary>
        Auto = 0,
        /// <summary>
        /// Represents time in seconds (s).
        /// </summary>
        Seconds,
        /// <summary>
        /// Represents time in milliseconds (1s = 1.000ms).
        /// </summary>
        MilliSeconds,
        /// <summary>
        /// Represents time in microseconds (1s = 1.000.000µs).
        /// </summary>
        MicroSeconds,
        /// <summary>
        /// Represents time in microseconds (1s = 1.000.000.000ns).
        /// </summary>
        NanoSeconds,
        /// <summary>
        /// Represents time in ticks (1 tick = 100ns).
        /// </summary>
        Ticks
    }
}