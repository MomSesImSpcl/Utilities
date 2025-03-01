using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="int"/>.
    /// </summary>
    public static class IntExtensions
    {
        #region Methods
        /// <summary>
        /// Compares two <see cref="int"/>s and returns <c>true</c> if they are similar.
        /// </summary>
        /// <param name="_Int1">The <see cref="int"/> to compare.</param>
        /// <param name="_Int2">The <see cref="int"/> to compare with.</param>
        /// <returns><c>true</c> if the two <see cref="int"/>s are similar, otherwise, <c>false</c>.</returns>
        public static bool Approximately(this int _Int1, int _Int2)
        {
            return Mathf.Approximately(_Int1, _Int2);
        }
        
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
        
        /// <summary>
        /// Determines whether the specified integer has a negative sign.
        /// </summary>
        /// <param name="_Int">The integer to check for a negative sign.</param>
        /// <returns>
        /// true if the specified integer is negative; otherwise, false.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasSign(this int _Int) => !(_Int >= 0f);
        #endregion
    }
}
