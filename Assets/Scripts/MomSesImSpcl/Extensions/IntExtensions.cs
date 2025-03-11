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
        public static bool AsBool(this int _Int)
        {
            return _Int != 0;
        }
        
        /// <summary>
        /// Creates a new <see cref="NumericEnumerator"/> to iterate over a sequence of <see cref="int"/>, starting at <c>0</c> and ending at <c>_End</c>.
        /// </summary>
        /// <param name="_End"><see cref="NumericEnumerator.end"/>.</param>
        /// <returns>A new <see cref="NumericEnumerator"/>.</returns>
        public static NumericEnumerator GetEnumerator(this int _End)
        {
            return new NumericEnumerator(_End);
        }

        /// <summary>
        /// Determines whether this <see cref="int"/> has a sign or not.
        /// </summary>
        /// <param name="_Int">The <see cref="int"/> to check the sign  of.</param>
        /// <returns><c>true</c> if this <see cref="int"/> is negative, <c>false</c> if it is positive.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasSign(this int _Int)
        {
            return (_Int >> 31).AsBool();
        }
        #endregion
    }
}
