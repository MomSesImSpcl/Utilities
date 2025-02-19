using System;
using System.Reflection;

namespace MomSesImSpcl.Utilities
{
    /// <summary>
    /// Combines multiple <see cref="BindingFlags"/>.
    /// </summary>
    [Flags]
    public enum CustomBindingFlags
    {
        /// <summary>
        /// <see cref="BindingFlags.Default"/>.
        /// </summary>
        Default = BindingFlags.Default,
        /// <summary>
        /// <see cref="BindingFlags.Public"/> | <see cref="BindingFlags.Instance"/>.
        /// </summary>
        PublicInstance = BindingFlags.Public | BindingFlags.Instance,
        /// <summary>
        /// <see cref="BindingFlags.NonPublic"/> | <see cref="BindingFlags.Instance"/>.
        /// </summary>
        NonPublicInstance = BindingFlags.NonPublic | BindingFlags.Instance,
        /// <summary>
        /// <see cref="BindingFlags.Public"/> | <see cref="BindingFlags.Static"/>.
        /// </summary>
        PublicStatic = BindingFlags.Public | BindingFlags.Static,
        /// <summary>
        /// <see cref="BindingFlags.NonPublic"/> | <see cref="BindingFlags.Static"/>.
        /// </summary>
        NonPublicStatic = BindingFlags.NonPublic | BindingFlags.Static,
        /// <summary>
        /// <see cref="BindingFlags.Public"/> | <see cref="BindingFlags.NonPublic"/> | <see cref="BindingFlags.Instance"/>.
        /// </summary>
        AllInstance = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
        /// <summary>
        /// <see cref="BindingFlags.Public"/> | <see cref="BindingFlags.NonPublic"/> | <see cref="BindingFlags.Static"/>.
        /// </summary>
        AllStatic = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static,
        /// <summary>
        /// <see cref="BindingFlags.Public"/> | <see cref="BindingFlags.NonPublic"/> | <see cref="BindingFlags.Instance"/> | <see cref="BindingFlags.Static"/> | <see cref="BindingFlags.FlattenHierarchy"/>.
        /// </summary>
        All = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy
    }
}