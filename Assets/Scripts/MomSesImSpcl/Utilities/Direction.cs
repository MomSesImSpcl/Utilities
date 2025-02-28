using UnityEngine;

namespace MomSesImSpcl.Utilities
{
    /// <summary>
    /// Directions for a <see cref="Vector3"/>/<see cref="Vector2"/>.
    /// </summary>
    public enum Direction
    {
        /// <summary>
        /// <see cref="Vector3"/>.<see cref="Vector3.up"/>.
        /// </summary>
        Up,
        /// <summary>
        /// <see cref="Vector3"/>.<see cref="Vector3.down"/>.
        /// </summary>
        Down,
        /// <summary>
        /// <see cref="Vector3"/>.<see cref="Vector3.left"/>.
        /// </summary>
        Left,
        /// <summary>
        /// <see cref="Vector3"/>.<see cref="Vector3.right"/>.
        /// </summary>
        Right,
        /// <summary>
        /// <see cref="Vector3"/>.<see cref="Vector3.forward"/>.
        /// </summary>
        Forward,
        /// <summary>
        /// <see cref="Vector3"/>.<see cref="Vector3.back"/>.
        /// </summary>
        Back
    }
}