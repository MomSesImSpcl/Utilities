using System;
using System.Runtime.CompilerServices;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="int"/>.
    /// </summary>
    public static class IntExtensions
    {
        #region Methods
        /// <summary>
        /// Determines whether the specified integer has a negative sign.
        /// </summary>
        /// <param name="_Int">The integer to check for a negative sign.</param>
        /// <returns>
        /// true if the specified integer is negative; otherwise, false.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasSign(this int _Int) => !(_Int >= 0f);

        /// <summary>
        /// Casts this <see cref="int"/> to the given <see cref="Enum"/> <see cref="Type"/> <c>T</c>.
        /// </summary>
        /// <param name="_Int">The <see cref="int"/> value to cast to its <see cref="Enum"/> representation.</param>
        /// <typeparam name="T">Must be an <see cref="Enum"/>.</typeparam>
        /// <returns>This <see cref="int"/> as the value inside the given <see cref="Enum"/> <see cref="Type"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T AsEnum<T>(this int _Int) where T : Enum
        {
            return Unsafe.As<int,T>(ref _Int);
        }
        #endregion
    }
}
