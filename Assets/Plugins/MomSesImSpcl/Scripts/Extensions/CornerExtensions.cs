using System.Runtime.CompilerServices;
using MomSesImSpcl.Utilities;
using UnityEngine;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="Corner"/>.
    /// </summary>
    public static class CornerExtensions
    {
        #region Methods
        /// <summary>
        /// Returns a 2D direction vector corresponding to the given <see cref="Corner"/>. <br/>
        /// The direction points outward from the specified corner of a box.
        /// </summary>
        /// <param name="_Corner">The <see cref="Corner"/> to get the direction for.</param>
        /// <returns>A <see cref="Vector2"/> representing the direction from the specified <see cref="Corner"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 GetDirection(this Corner _Corner) => _Corner switch
        {
            Corner.UpperLeft    => new Vector2(-1,  1),
            Corner.UpperCenter  => new Vector2( 0,  1),
            Corner.UpperRight   => new Vector2( 1,  1),
            Corner.MiddleLeft   => new Vector2(-1,  0),
            Corner.MiddleCenter => Vector2.zero,
            Corner.MiddleRight  => new Vector2( 1,  0),
            Corner.LowerLeft    => new Vector2(-1, -1),
            Corner.LowerCenter  => new Vector2( 0, -1),
            Corner.LowerRight   => new Vector2( 1, -1),
            _                   => Vector2.zero
        };
        
        /// <summary>
        /// Returns the <see cref="RectTransform.pivot"/> for the given <see cref="Corner"/>.
        /// </summary>
        /// <param name="_Corner">The <see cref="Corner"/> to get the <see cref="RectTransform.pivot"/> for.</param>
        /// <returns>The <see cref="RectTransform.pivot"/> for the given <see cref="Corner"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 GetPivot(this Corner _Corner) => _Corner switch
        {
            Corner.UpperLeft    => new Vector2(0, 1),
            Corner.UpperCenter  => new Vector2(.5f, 1),
            Corner.UpperRight   => new Vector2(1, 1),
            Corner.MiddleLeft   => new Vector2(0, .5f),
            Corner.MiddleCenter => new Vector2(.5f, .5f),
            Corner.MiddleRight  => new Vector2(1, .5f),
            Corner.LowerLeft    => new Vector2(0, 0),
            Corner.LowerCenter  => new Vector2(.5f, 0),
            Corner.LowerRight   => new Vector2(1, 0),
            _                   => Vector2.zero
        };
        
        /// <summary>
        /// Returns a <see cref="Transform.rotation"/> in Euler angles (degrees) on the <see cref="Vector3.z"/> <see cref="Axis"/> for the given <see cref="Corner"/>. <br/>
        /// <i><see cref="Vector3.zero"/> = <see cref="Corner.UpperCenter"/>.</i>
        /// </summary>
        /// <param name="_Corner">The <see cref="Corner"/> to get the rotation for.</param>
        /// <returns>A <see cref="Vector3"/> representing the rotation in Euler angles for this <see cref="Corner"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 GetRotation(this Corner _Corner) => _Corner switch
        {
            Corner.UpperLeft    => new Vector3(0, 0, 45),
            Corner.UpperCenter  => new Vector3(0, 0, 0),
            Corner.UpperRight   => new Vector3(0, 0, -45),
            Corner.MiddleLeft   => new Vector3(0, 0, 90),
            Corner.MiddleCenter => Vector3.zero,
            Corner.MiddleRight  => new Vector3(0, 0, -90),
            Corner.LowerLeft    => new Vector3(0, 0, 45),
            Corner.LowerCenter  => new Vector3(0, 0, 0),
            Corner.LowerRight   => new Vector3(0, 0, -45),
            _                   => Vector2.zero
        };

        /// <summary>
        /// Checker whether this <see cref="Corner"/> is diagonally orientated.
        /// </summary>
        /// <param name="_Corner">The <see cref="Corner"/> to check.</param>
        /// <returns><c>true</c> if this <see cref="Corner"/> is diagonally orientated, otherwise <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsDiagonal(this Corner _Corner) => _Corner switch
        {
            Corner.UpperLeft    => true,
            Corner.UpperCenter  => false,
            Corner.UpperRight   => true,
            Corner.MiddleLeft   => false,
            Corner.MiddleCenter => false,
            Corner.MiddleRight  => false,
            Corner.LowerLeft    => true,
            Corner.LowerCenter  => false,
            Corner.LowerRight   => true,
            _                   => false
        };
        #endregion
    }
}
