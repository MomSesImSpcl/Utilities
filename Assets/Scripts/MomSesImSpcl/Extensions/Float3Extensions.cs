using Unity.Mathematics;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="float3"/>.
    /// </summary>
    public static class Float3Extensions
    {
        #region Methods
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