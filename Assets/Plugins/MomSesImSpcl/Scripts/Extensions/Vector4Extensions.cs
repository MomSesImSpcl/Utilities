using System.Runtime.CompilerServices;
using UnityEngine;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="Vector4"/>.
    /// </summary>
    public static class Vector4Extensions
    {
        #region Methods
        /// <summary>
        /// Creates a new <see cref="Quaternion"/> from this <see cref="Vector4"/>.
        /// </summary>
        /// <param name="_Vector4">The <see cref="Vector4"/> to create a <see cref="Quaternion"/> from.</param>
        /// <returns>A new <see cref="Quaternion"/> with the value of this <see cref="Vector4"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion ToQuaternion(this Vector4 _Vector4)
        {
            return new Quaternion(_Vector4.x, _Vector4.y, _Vector4.z, _Vector4.w);
        }
        #endregion
    }
}