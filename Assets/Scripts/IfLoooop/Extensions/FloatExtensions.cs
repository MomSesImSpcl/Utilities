using System.Runtime.CompilerServices;

namespace IfLoooop.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="float"/>.
    /// </summary>
    // ReSharper disable once UnusedType.Global
    public static class FloatExtensions
    {
        #region Methods
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