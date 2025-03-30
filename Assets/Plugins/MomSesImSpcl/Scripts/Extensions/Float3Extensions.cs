using Unity.Mathematics;
using UnityEngine;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="float3"/>.
    /// </summary>
    public static class Float3Extensions
    {
        #region Methods
        /// <summary>
        /// Returns this <see cref="float3"/> as a <see cref="Vector3"/>.
        /// </summary>
        /// <param name="_Float3">The <see cref="float3"/> to convert.</param>
        /// <returns>This <see cref="float3"/> as a <see cref="Vector3"/>.</returns>
        public static Vector3 AsVector3(this float3 _Float3)
        {
            return new Vector3(_Float3.x, _Float3.y, _Float3.z);
        }
        
        /// <summary>
        /// Normalizes this <see cref="float3"/>.
        /// </summary>
        /// <param name="_Float3">The <see cref="float3"/> to normalize.</param>
        /// <returns>This <see cref="float3"/> with normalized values.</returns>
        public static float3 Normalize(this float3 _Float3)
        {
            return math.normalize(_Float3);
        }
        #endregion
    }
}
