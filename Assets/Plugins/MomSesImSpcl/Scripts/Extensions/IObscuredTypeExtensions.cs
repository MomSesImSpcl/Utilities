#if ACTK_IS_HERE
using System;
using CodeStage.AntiCheat.ObscuredTypes;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="IObscuredType"/>.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class IObscuredTypeExtensions
    {
        #region Methods
        /// <summary>
        /// Returns the <see cref="string"/> of this <see cref="IObscuredType"/> without any boxing.
        /// </summary>
        /// <param name="_ObscuredType">The <see cref="IObscuredType"/> to convert into a <see cref="string"/>.</param>
        /// <typeparam name="T">Must be an <see cref="IObscuredType"/> <see cref="Type"/>.</typeparam>
        /// <returns>This <see cref="IObscuredType"/> as a <see cref="string"/>.</returns>
        public static string GetObscuredStringValue<T>(this T _ObscuredType) where T : IObscuredType
        {
            return _ObscuredType.ToString();
        }
        #endregion
    }
}
#endif