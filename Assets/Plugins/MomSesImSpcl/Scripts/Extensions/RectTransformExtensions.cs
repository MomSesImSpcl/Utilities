using System;
using System.Runtime.CompilerServices;
using MomSesImSpcl.Utilities;
using MomSesImSpcl.Utilities.Pooling;
using UnityEngine;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="RectTransform"/>.
    /// </summary>
    public static class RectTransformExtensions
    {
        #region Methods
        /// <summary>
        /// Returns the world <see cref="Transform.position"/> of the given <see cref="Corner"/>.
        /// </summary>
        /// <param name="_RectTransform">The <see cref="RectTransform"/> to use the <see cref="Corner"/> from.</param>
        /// <param name="_Corner">The <see cref="Corner"/> to get the <see cref="Transform.position"/> of.</param>
        /// <returns>The world <see cref="Transform.position"/> of the given <see cref="Corner"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When the given <see cref="Corner"/> is not valid.</exception>
        public static Vector3 WorldCorner(this RectTransform _RectTransform, Corner _Corner)
        {
            var _fourCornerArray = ArrayPool<Vector3>.Get(4);
            
            _RectTransform.GetWorldCorners(_fourCornerArray);
            
            var _corner = GetWorldCornerPosition(_fourCornerArray, _Corner);

            _fourCornerArray.ReturnToArrayPool();
            
            return _corner;
        }

        /// <summary>
        /// Returns a world <see cref="Transform.position"/> between <c>_From</c> and <c>_To</c> that reflects the given <c>_Percentage</c>.
        /// </summary>
        /// <param name="_RectTransform">The <see cref="RectTransform"/> to get the relative <see cref="Transform.position"/> from.</param>
        /// <param name="_From">The <see cref="Corner"/> from which to calculate the new <see cref="Transform.position"/>.</param>
        /// <param name="_To">The <see cref="Corner"/> to which to calculate the new <see cref="Transform.position"/>.</param>
        /// <param name="_Percentage">The new <see cref="Transform.position"/> will be this percentage between <c>_From</c> and <c>_To</c>.</param>
        /// <returns>A world <see cref="Transform.position"/> between <c>_From</c> and <c>_To</c> that reflects the given <c>_Percentage</c>.</returns>
        public static Vector3 WorldCornerOffset(this RectTransform _RectTransform, Corner _From, Corner _To, float _Percentage)
        {
            var _fourCornerArray = ArrayPool<Vector3>.Get(4);
            
            _RectTransform.GetWorldCorners(_fourCornerArray);
            
            var _from = GetWorldCornerPosition(_fourCornerArray, _From);
            var _to = GetWorldCornerPosition(_fourCornerArray, _To);
            
            _fourCornerArray.ReturnToArrayPool();
            
            return Vector3.Lerp(_from, _to, _Percentage);
        }

        /// <summary>
        /// Returns the world <see cref="Transform.position"/> for the given <see cref="Corner"/>.
        /// </summary>
        /// <param name="_FourCornerArray">Must already contain the 4 world corners.</param>
        /// <param name="_Corner">The <see cref="Corner"/> to get the world <see cref="Transform.position"/> for.</param>
        /// <returns>The world <see cref="Transform.position"/> for the given <see cref="Corner"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When the given <see cref="Corner"/> is not valid.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Vector3 GetWorldCornerPosition(Vector3[] _FourCornerArray, Corner _Corner)
        {
            return _Corner switch
            {
                Corner.UpperLeft    => _FourCornerArray[1],
                Corner.UpperCenter  => (_FourCornerArray[1] + _FourCornerArray[2]) * .5f,
                Corner.UpperRight   => _FourCornerArray[2],
                Corner.MiddleLeft   => (_FourCornerArray[1] + _FourCornerArray[0]) * .5f,
                Corner.MiddleCenter => (_FourCornerArray[0] + _FourCornerArray[1] + _FourCornerArray[2] + _FourCornerArray[3]) * .25f,
                Corner.MiddleRight  => (_FourCornerArray[2] + _FourCornerArray[3]) * .5f,
                Corner.LowerLeft    => _FourCornerArray[0],
                Corner.LowerCenter  => (_FourCornerArray[0] + _FourCornerArray[3]) * .5f,
                Corner.LowerRight   => _FourCornerArray[3],
                _ => throw new ArgumentOutOfRangeException(nameof(_Corner), _Corner, null)
            };
        }
        
        /// <summary>
        /// Transforms a point from this <see cref="RectTransform"/> into world coordinates.
        /// </summary>
        /// <param name="_RectTransform">The <see cref="RectTransform"/> to transform the point from.</param>
        /// <param name="_Point">A local point in this <see cref="RectTransform"/>.</param>
        /// <returns>The world coordinates of the point.</returns>
        public static Vector3 ToWorld(this RectTransform _RectTransform, Func<RectTransform, Vector3> _Point)
        {
            return _RectTransform.TransformPoint(_Point(_RectTransform));
        }
        
        /// <summary>
        /// Converts a point from Viewport coordinates to Canvas coordinates.
        /// </summary>
        /// <param name="_RectTransform">The RectTransform of the Canvas element that is receiving the position, represented as a RectTransform.</param>
        /// <param name="_ViewportPos">The position in the Viewport, represented as a Vector3, that needs to be converted to Canvas coordinates.</param>
        /// <returns>The converted position in Canvas coordinates as a Vector3.</returns>
        public static Vector3 ViewportToCanvasPosition(this RectTransform _RectTransform, Vector3 _ViewportPos)
        {
            var _rect = _RectTransform.rect;
            
            _ViewportPos.x *= _rect.size.x;
            _ViewportPos.y *= _rect.size.y;
            _ViewportPos.x -= _rect.size.x * _RectTransform.pivot.x;
            _ViewportPos.y -= _RectTransform.rect.size.y * _RectTransform.pivot.y;
            
            return _ViewportPos;
        }

        /// <summary>
        /// Converts a point from World coordinates to Canvas coordinates.
        /// </summary>
        /// <param name="_RectTransform">The RectTransform of the Canvas element that is receiving the position, represented as a RectTransform.</param>
        /// <param name="_Camera">The camera used to view the World, represented as a Camera.</param>
        /// <param name="_WorldPos">The position in the World, represented as a Vector3, that needs to be converted to Canvas coordinates.</param>
        /// <returns>The converted position in Canvas coordinates as a Vector3.</returns>
        public static Vector3 WorldToCanvasPosition(this RectTransform _RectTransform, Camera _Camera, Vector3 _WorldPos)
        {
            return ViewportToCanvasPosition(_RectTransform, _Camera.WorldToViewportPoint(_WorldPos));
        }
        #endregion
    }
}
