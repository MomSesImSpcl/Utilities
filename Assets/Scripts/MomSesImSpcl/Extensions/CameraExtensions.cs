using UnityEngine;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="Camera"/>
    /// </summary>
    public static class CameraExtensions
    {
        #region Methods
        /// <summary>
        /// Converts a position from Canvas local coordinates to world coordinates using the provided Camera.
        /// </summary>
        /// <param name="_Camera">The Camera to perform the conversion.</param>
        /// <param name="_Canvas">The RectTransform of the Canvas containing the local position.</param>
        /// <param name="_CanvasLocalPos">The local position within the Canvas to convert.</param>
        /// <returns>The world coordinates corresponding to the Canvas local position.</returns>
        public static Vector3 CanvasToWorldPosition(this Camera _Camera, RectTransform _Canvas, Vector2 _CanvasLocalPos)
        {
            var _viewportPoint = _CanvasLocalPos + _Canvas.rect.size * _Canvas.pivot;
            _viewportPoint /= _Canvas.rect.size;
            var _worldPosition = _Camera.ViewportToWorldPoint(_viewportPoint, Camera.MonoOrStereoscopicEye.Mono);
            return _worldPosition;
        }

        /// <summary>
        /// Calculates the points of the camera frustum at a given distance from the camera.
        /// </summary>
        /// <param name="_Camera">The Camera from which the frustum points are calculated.</param>
        /// <param name="_DistanceFromCamera">The distance from the camera at which to calculate the frustum points.</param>
        /// <param name="_ReturnMidpoints">Set to <c>true</c> to return the midpoints instead of the corners.</param>
        /// <returns>An array containing the positions of the four points that form the corners of the frustum at the given distance.</returns>
        public static Vector3[] CalculateFrustumPoints(this Camera _Camera, float _DistanceFromCamera, bool _ReturnMidpoints = false)
        {
            var _frustumCorners = new Vector3[4];
            
            if (_Camera.orthographic)
            {
                var _halfHeight = _Camera.orthographicSize;
                var _halfWidth = _halfHeight * _Camera.aspect;
                
                _frustumCorners[0] = new Vector3(-_halfWidth, -_halfHeight, _DistanceFromCamera); // Bottom-left
                _frustumCorners[1] = new Vector3(-_halfWidth, _halfHeight, _DistanceFromCamera);  // Top-left
                _frustumCorners[2] = new Vector3(_halfWidth, _halfHeight, _DistanceFromCamera);   // Top-right
                _frustumCorners[3] = new Vector3(_halfWidth, -_halfHeight, _DistanceFromCamera);  // Bottom-right
                
                // ReSharper disable once InconsistentNaming
                for (var i = 0; i < _frustumCorners.Length; i++)
                {
                    _frustumCorners[i] = _Camera.transform.TransformPoint(_frustumCorners[i]);
                }
            }
            else
            {
                var _normalizedViewportCoordinates = new Rect(0, 0, 1, 1);
                
                _Camera.CalculateFrustumCorners(_normalizedViewportCoordinates, _DistanceFromCamera, Camera.MonoOrStereoscopicEye.Mono, _frustumCorners);
                
                // ReSharper disable once InconsistentNaming
                for (var i = 0; i < _frustumCorners.Length; i++)
                {
                    _frustumCorners[i] = _Camera.transform.TransformPoint(_frustumCorners[i]);
                }
            }
            
            if (_ReturnMidpoints)
            {
                // ReSharper disable once InconsistentNaming
                for (var i = 0; i < _frustumCorners.Length; i++)
                {
                    _frustumCorners[i] = (_frustumCorners[i] + _frustumCorners[(i + 1) % 4]) / 2;
                }
            }
            
            return _frustumCorners;
        }

        /// <summary>
        /// Calculates the angle between the mouse cursor and a given world position from the perspective of the camera.
        /// </summary>
        /// <param name="_Camera">The Camera used to calculate the angle.</param>
        /// <param name="_WorldPosition">The world position to which the angle is calculated.</param>
        /// <returns>The angle between the mouse cursor and the given world position in degrees.</returns>
        public static float CalculateMouseAngle(this Camera _Camera, Vector3 _WorldPosition)
        {
            var _screenPosition = _Camera.WorldToScreenPoint(_WorldPosition);
            var _direction = Input.mousePosition - _screenPosition;
            var _mouseAngle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;

            return _mouseAngle;
        }

        /// <summary>
        /// Converts a world position to a local position in a given RectTransform using the provided Camera.
        /// </summary>
        /// <param name="_Camera">The Camera to use for the conversion.</param>
        /// <param name="_Canvas">The Canvas containing the RectTransform for which the local position is calculated.</param>
        /// <param name="_WorldPosition">The world position to convert to a local position.</param>
        /// <returns>The local position within the specified RectTransform corresponding to the world position.</returns>
        public static Vector2 WorldPointToLocalPointInRectangle(this Camera _Camera, Canvas _Canvas, Vector3 _WorldPosition)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_Canvas.transform as RectTransform, _Camera.WorldToScreenPoint(_WorldPosition), _Canvas.worldCamera, out var _localPoint);
            return _localPoint;
        }
        #endregion
    }
}
