using MomSesImSpcl.Extensions;
using Unity.Mathematics;
using UnityEngine;

namespace MomSesImSpcl.Utilities
{
    /// <summary>
    /// Provides utility methods for drawing shapes using Unity's Debug class.
    /// </summary>
    public static class Draw
    {
        #region Methods
        /// <summary>
        /// Draws a visual representation of an angle from the given <c>_Center</c> <see cref="Transform.position"/>.
        /// </summary>
        /// <param name="_Center">The center <see cref="Transform.position"/> of the circle.</param>
        /// <param name="_Radius">The radius of the circle.</param>
        /// <param name="_Angle">The angle (in degrees) at which to place the point on the circle's circumference.</param>
        /// <param name="_Duration">The duration (in seconds) for which the visualization remains visible.</param>
        public static void Angle(Vector3 _Center, float _Radius, float _Angle, float _Duration = 1f)
        {
            var _point = _Center.ToVector2().GetPointAround(_Radius, _Angle);
            
            Circle(_Center, _Radius, Color.red, _Duration);
            Sphere(_point, .5f, Color.green, _Duration);
            Debug.DrawLine(_Center, _point, Color.blue, _Duration);
        }
        
        /// <summary>
        /// Draws a 2D circle in the scene.
        /// </summary>
        /// <param name="_Center">The center <see cref="Transform.position"/> of the circle.</param>
        /// <param name="_Radius">The radius of the circle.</param>
        /// <param name="_Color">The color of the sphere lines.</param>
        /// <param name="_Duration">The duration that the sphere will be visible. Default is 1 second.</param>
        public static void Circle(Vector3 _Center, float _Radius, Color _Color, float _Duration = 1f)
        {
            // Defines how smooth the circle will be, smaller value = smoother.
            const float _ANGLE_STEP = 1f; // DON'T SET TO 0!
            var _previousPoint = _Center + Vector3.zero.WithX(_Radius);
            
            for (var _angle = _ANGLE_STEP; _angle <= 360f; _angle += _ANGLE_STEP)
            {
                var _rad = _angle * math.TORADIANS;
                var _x = math.cos(_rad) * _Radius;
                var _y = math.sin(_rad) * _Radius;
                var _nextPoint = _Center + Vector3.zero.WithXY(_x, _y);
                
                Debug.DrawLine(_previousPoint, _nextPoint, _Color, _Duration);
                
                _previousPoint = _nextPoint;
            }
        }
        
        /// <summary>
        /// Draws a wireframe sphere at the given position with the specified radius, color, and duration.
        /// </summary>
        /// <param name="_Position">The center position of the sphere in world coordinates.</param>
        /// <param name="_Radius">The radius of the sphere.</param>
        /// <param name="_Color">The color of the sphere lines.</param>
        /// <param name="_Duration">The duration that the sphere will be visible. Default is 1 second.</param>
        public static void Sphere(Vector3 _Position, float _Radius, Color _Color, float _Duration = 1)
        {
            const float _ANGLE_STEP = 10f;
            
            for (float _theta = 0; _theta < 360; _theta += _ANGLE_STEP)
            {
                for (float _phi = -90; _phi <= 90; _phi += _ANGLE_STEP)
                {
                    var _point1 = _Position + Quaternion.Euler(_phi, _theta, 0) * Vector3.forward * _Radius;
                    var _point2 = _Position + Quaternion.Euler(_phi, _theta + _ANGLE_STEP, 0) * Vector3.forward * _Radius;
                    var _point3 = _Position + Quaternion.Euler(_phi + _ANGLE_STEP, _theta, 0) * Vector3.forward * _Radius;
                    var _point4 = _Position + Quaternion.Euler(_phi + _ANGLE_STEP, _theta + _ANGLE_STEP, 0) * Vector3.forward * _Radius;
                    
                    Debug.DrawLine(_point1, _point2, _Color, _Duration, false);
                    Debug.DrawLine(_point1, _point3, _Color, _Duration, false);
                    Debug.DrawLine(_point2, _point4, _Color, _Duration, false);
                    Debug.DrawLine(_point3, _point4, _Color, _Duration, false);
                }
            }
        }

        /// <summary>
        /// Draws a wireframe square at the given positions with the specified color, and duration.
        /// </summary>
        /// <param name="_TopLeft">Position of the top left corner.</param>
        /// <param name="_TopRight">Position of the top right corner.</param>
        /// <param name="_BottomRight">Position of the bottom right corner.</param>
        /// <param name="_BottomLeft">Position of the bottom left corner.</param>
        /// <param name="_Color">The color of the square lines.</param>
        /// <param name="_Duration">The duration that the square will be visible. Default is 1 second.</param>
        public static void Square(Vector2 _TopLeft, Vector2 _TopRight, Vector2 _BottomRight, Vector2 _BottomLeft, Color _Color, float _Duration = 1)
        {
            Debug.DrawLine(_TopLeft, _TopRight, _Color, _Duration);
            Debug.DrawLine(_TopRight, _BottomRight, _Color, _Duration);
            Debug.DrawLine(_BottomRight, _BottomLeft, _Color, _Duration);
            Debug.DrawLine(_BottomLeft, _TopLeft, _Color, _Duration);
        }
        #endregion
    }
}
