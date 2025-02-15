#if ACTK_IS_HERE
using System;
using System.Runtime.CompilerServices;
using CodeStage.AntiCheat.ObscuredTypes;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="ObscuredInt"/>.
    /// </summary>
    public static class ObscuredIntExtensions
    {
        #region Methods
        /// <summary>
        /// Casts this <see cref="ObscuredInt"/> to the given <see cref="Enum"/> <see cref="Type"/> <c>T</c>.
        /// </summary>
        /// <param name="_ObscuredInt">The <see cref="ObscuredInt"/> value to cast to its <see cref="Enum"/> representation.</param>
        /// <typeparam name="T">Must be an <see cref="Enum"/>.</typeparam>
        /// <returns>This <see cref="ObscuredInt"/> as the value inside the given <see cref="Enum"/> <see cref="Type"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ToEnum<T>(this ObscuredInt _ObscuredInt)  where T : Enum
        {
            return Unsafe.As<ObscuredInt,T>(ref _ObscuredInt);
        }
        #endregion
    }
}
#endif
