using System;
using System.Runtime.CompilerServices;
using MomSesImSpcl.Utilities;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="int"/>.
    /// </summary>
    public static class IntExtensions
    {
        #region Methods
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
        /// Converts the given <see cref="int"/> to a <see cref="bool"/>.
        /// </summary>
        /// <param name="_Int">The <see cref="int"/> to convert.</param>
        /// <returns><c>true</c> if the <see cref="int"/> can be converted to a <see cref="bool"/> representation, otherwise <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool AsBool(this int _Int)
        {
            return *(bool*)&_Int;
        }
        
        /// <summary>
        /// Creates a new <see cref="IntEnumerator"/> to iterate over a sequence of <see cref="int"/>, starting at <c>0</c> and ending at <c>_End</c>.
        /// </summary>
        /// <param name="_End"><see cref="IntEnumerator.end"/>.</param>
        /// <returns>A new <see cref="IntEnumerator"/>.</returns>
        public static IntEnumerator GetEnumerator(this int _End)
        {
            return new IntEnumerator(_End);
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
