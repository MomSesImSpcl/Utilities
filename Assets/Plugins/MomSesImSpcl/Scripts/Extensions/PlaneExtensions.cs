using UnityEngine;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="Plane"/>.
    /// </summary>
    public static class PlaneExtensions
    {
        #region Methods
        /// <summary>
        /// Projects a point onto the plane.
        /// </summary>
        /// <param name="_Plane">The plane onto which the point is projected.</param>
        /// <param name="_Point">The point to project.</param>
        /// <returns>A point that represents the projection of the original point onto the plane.
        /// </returns>
        public static Vector3 ProjectPointOnPlane(this Plane _Plane, Vector3 _Point)
        {
            return Vector3.ProjectOnPlane(_Point, _Plane.normal) + _Plane.normal * _Plane.distance;
        }
        #endregion
    }
}