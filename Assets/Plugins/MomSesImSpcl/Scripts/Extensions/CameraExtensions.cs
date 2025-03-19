using System;
using System.Globalization;
using MomSesImSpcl.Utilities;
using MomSesImSpcl.Utilities.Pooling;
using UnityEngine;
using math = Unity.Mathematics.math;

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
        /// <param name="_DistanceFrom">The world <see cref="Transform.position"/> to calculate the distance to the <see cref="Camera"/> from.</param>
        /// <param name="_ReturnMidpoints">Set to <c>true</c> to return the midpoints instead of the corners.</param>
        /// <param name="_Visualize">Set to <c>true</c> visualize the calculated points in the scene.</param>
        /// <returns>
        /// An array containing the positions of the four points that form the corners of the frustum at the given distance. <br/>
        /// <b>[0]:</b> Bottom-Left. <br/>
        /// <b>[1]:</b> Top-Left. <br/>
        /// <b>[2]:</b> Top-Right. <br/>
        /// <b>[3]:</b> Bottom-Right.
        /// </returns>
        public static Vector3[] CalculateFrustumPoints(this Camera _Camera, Vector3 _DistanceFrom, bool _ReturnMidpoints = false, bool _Visualize = false)
        {
            var _distance = _Camera.transform.position.Distance(_DistanceFrom);
            return _Camera.CalculateFrustumPoints(_distance, _ReturnMidpoints, _Visualize);
        }
        
        /// <summary>
        /// Calculates the points of the camera frustum at a given distance from the camera.
        /// </summary>
        /// <param name="_Camera">The Camera from which the frustum points are calculated.</param>
        /// <param name="_DistanceFromCamera">The distance from the camera at which to calculate the frustum points.</param>
        /// <param name="_ReturnMidpoints">Set to <c>true</c> to return the midpoints instead of the corners.</param>
        /// <param name="_Visualize">Set to <c>true</c> visualize the calculated points in the scene.</param>
        /// <returns>
        /// An array containing the positions of the four points that form the corners of the frustum at the given distance. <br/>
        /// <b>[0]:</b> Bottom-Left. <br/>
        /// <b>[1]:</b> Top-Left. <br/>
        /// <b>[2]:</b> Top-Right. <br/>
        /// <b>[3]:</b> Bottom-Right.
        /// </returns>
        public static Vector3[] CalculateFrustumPoints(this Camera _Camera, float _DistanceFromCamera, bool _ReturnMidpoints = false, bool _Visualize = false)
        {
            var _frustumCorners = ArrayPool<Vector3>.Get(4);
            
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
            
#if UNITY_EDITOR
            if (_Visualize)
            {
                foreach (var _frustumCorner in _frustumCorners)
                {
                    Draw.Sphere(_frustumCorner, 1, Color.red);
                }
            }
#endif
            return _frustumCorners;
        }
        
        /// <summary>
        /// Calculates the points where the frustum corners of this <see cref="Camera"/> intersect with the given <c>_TargetHeight</c>.
        /// </summary>
        /// <param name="_Camera">The <see cref="Camera"/> to get the frustum of.</param>
        /// <param name="_TargetHeight"><see cref="Vector3.y"/>-<see cref="Transform.position"/> in world space coordinates.</param>
        /// <param name="_ReturnMidpoints">Set to <c>true</c> to return the midpoints instead of the corners.</param>
        /// <param name="_Visualize">Set to <c>true</c> visualize the calculated points in the scene.</param>
        /// <returns>A <see cref="Vector3"/> <see cref="Array"/> that contains the points where the frustum corners of this <see cref="Camera"/> intersect with the given <c>_TargetHeight</c>.</returns>
        public static Vector3[] CalculateFrustumPointsAtHeight(this Camera _Camera, float _TargetHeight, bool _ReturnMidpoints = false, bool _Visualize = false)
        {
            var _cameraTransform = _Camera.transform;
            var _distance = _cameraTransform.GetDistanceToHeight(_TargetHeight);
            var _cameraPosition = _cameraTransform.position;
            var _frustumCorners = _Camera.CalculateFrustumPoints(_distance, _ReturnMidpoints);
            var _targetPlane = new Plane(Vector3.up, new Vector3(0, _TargetHeight, 0));
            
            // ReSharper disable once InconsistentNaming
            for (var i = 0; i < _frustumCorners.Length; i++)
            {
                var _rayDirection = _frustumCorners[i] - _cameraPosition;
                var _ray = new Ray(_cameraPosition, _rayDirection);
        
                if (_targetPlane.Raycast(_ray, out var _enter))
                {
                    _frustumCorners[i] = _ray.GetPoint(_enter);
                }
                else
                {
                    _frustumCorners[i] = _cameraPosition;
#pragma warning disable CS8509
                    var _corner = i switch
#pragma warning restore CS8509
                    {
                        0 => "Bottom-Left",
                        1 => "Top-Left",
                        2 => "Top-Right",
                        3 => "Bottom-Right"
                    };
                    
                    Debug.LogWarning($"Frustum corner {_corner.Bold()} did not intersect at height {_TargetHeight.ToString(CultureInfo.InvariantCulture).Bold()}.");
                }
            }
            
#if UNITY_EDITOR
            if (_Visualize)
            {
                foreach (var _frustumCorner in _frustumCorners)
                {
                    Draw.Sphere(_frustumCorner, 1, Color.red);
                }
            }
#endif
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
            var _mouseAngle = math.atan2(_direction.y, _direction.x) * math.TODEGREES;

            return _mouseAngle;
        }

        /// <summary>
        /// Calculates the <see cref="Screen.width"/> and <see cref="Screen.height"/> of the <see cref="Screen"/> at the given distance from the <see cref="Camera"/>.
        /// </summary>
        /// <param name="_Camera">The <see cref="Camera"/> to calculate the dimensions of.</param>
        /// <param name="_DistanceFromCamera">The distance from the <see cref="Camera"/> at which to calculate the <see cref="Screen.width"/> and <see cref="Screen.height"/>.</param>
        /// <param name="_Visualize">Set to <c>true</c> visualize the frustum corner at the given distance in the scene.</param>
        /// <returns>A <see cref="Vector2"/> where <see cref="Vector2.x"/> is the <see cref="Screen.width"/> and <see cref="Vector2.y"/> is the <see cref="Screen.height"/>.</returns>
        public static Vector2 CalculateSize(this Camera _Camera, float _DistanceFromCamera, bool _Visualize = false)
        {
            var _frustumCorners = _Camera.CalculateFrustumPoints(_DistanceFromCamera);
            var _width = math.distance(_frustumCorners[1], _frustumCorners[2]);
            var _height = math.distance(_frustumCorners[0], _frustumCorners[1]);

#if UNITY_EDITOR
            if (_Visualize)
            {
                foreach (var _frustumCorner in _frustumCorners)
                {
                    Draw.Sphere(_frustumCorner, 1, Color.red);
                }
            }
#endif
            _frustumCorners.ReturnToArrayPool();
            
            return new Vector2(_width, _height);
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
