using System.Runtime.CompilerServices;
using UnityEngine;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="float"/>.
    /// </summary>
    public static class FloatExtensions
    {
        #region Methods
        /// <summary>
        /// Compares two <see cref="float"/>s and returns <c>true</c> if they are similar.
        /// </summary>
        /// <param name="_Float1">The <see cref="float"/> to compare.</param>
        /// <param name="_Float2">The <see cref="float"/> to compare with.</param>
        /// <returns><c>true</c> if the two <see cref="float"/>s are similar, otherwise, <c>false</c>.</returns>
        public static bool Approximately(this float _Float1, float _Float2)
        {
            return Mathf.Approximately(_Float1, _Float2);
        }
        
        /// <summary>
        /// Converts the given <see cref="float"/> to a <see cref="bool"/>.
        /// </summary>
        /// <param name="_Float">The <see cref="float"/> to convert.</param>
        /// <returns><c>true</c> if the <see cref="float"/> can be converted to a <see cref="bool"/> representation, otherwise <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool AsBool(this float _Float)
        {
            var _int = (int)_Float;
            return *(bool*)&_int;
        }

        /// <summary>
        /// Determines whether the given <see cref="float"/> has a sign (is negative).
        /// </summary>
        /// <param name="_Float">The <see cref="float"/> to check.</param>
        /// <returns><c>true</c> if the <see cref="float"/> is negative; otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasSign(float _Float) => !(_Float >= 0f);
        #endregion
    }
}
