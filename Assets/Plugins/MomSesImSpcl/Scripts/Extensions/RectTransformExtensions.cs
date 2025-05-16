using System;
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
            
            var _bottomLeft = _fourCornerArray[0];
            var _topLeft = _fourCornerArray[1];
            var _topRight = _fourCornerArray[2];
            var _bottomRight = _fourCornerArray[3];
            var _corner = _Corner switch
            {
                Corner.UpperLeft    => _topLeft,
                Corner.UpperCenter  => (_topLeft + _topRight) * .5f,
                Corner.UpperRight   => _topRight,
                Corner.MiddleLeft   => (_topLeft + _bottomLeft) * .5f,
                Corner.MiddleCenter => (_bottomLeft + _topLeft + _topRight + _bottomRight) * .25f,
                Corner.MiddleRight  => (_topRight + _bottomRight) * .5f,
                Corner.LowerLeft    => _bottomLeft,
                Corner.LowerCenter  => (_bottomLeft + _bottomRight) * .5f,
                Corner.LowerRight   => _bottomRight,
                _ => throw new ArgumentOutOfRangeException(nameof(_Corner), _Corner, null)
            };

            _fourCornerArray.ReturnToArrayPool();
            
            return _corner;
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
