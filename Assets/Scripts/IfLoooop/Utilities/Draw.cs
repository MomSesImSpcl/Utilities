using UnityEngine;

namespace IfLoooop.Utilities
{
    /// <summary>
    /// Provides utility methods for drawing shapes using Unity's Debug class.
    /// </summary>
    public static class Draw
    {
        #region Methods
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
        #endregion
    }
}