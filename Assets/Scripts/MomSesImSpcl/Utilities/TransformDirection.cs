using UnityEngine;

namespace MomSesImSpcl.Utilities
{
    /// <summary>
    /// Represents directional movement relative to a <see cref="Transform"/>'s <see cref="Transform.localPosition"/>.
    /// </summary>
    public enum TransformDirection
    {
        /// <summary>
        /// Positive <see cref="Vector3.y"/>.
        /// </summary>
        Up,
        /// <summary>
        /// Negative <see cref="Vector3.y"/>.
        /// </summary>
        Down,
        /// <summary>
        /// Negative <see cref="Vector3.x"/>.
        /// </summary>
        Left,
        /// <summary>
        /// Positive <see cref="Vector3.x"/>.
        /// </summary>
        Right,
        /// <summary>
        /// Positive <see cref="Vector3.y"/> and negative <see cref="Vector3.x"/>.
        /// </summary>
        UpLeft,
        /// <summary>
        /// Positive <see cref="Vector3.y"/> and positive <see cref="Vector3.x"/>.
        /// </summary>
        UpRight,
        /// <summary>
        /// Negative <see cref="Vector3.y"/> and negative <see cref="Vector3.x"/>.
        /// </summary>
        DownLeft,
        /// <summary>
        /// Negative <see cref="Vector3.y"/> and positive <see cref="Vector3.x"/>.
        /// </summary>
        DownRight
    }
}