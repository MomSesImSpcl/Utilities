using System;
using System.Globalization;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension method for primitive numeric types.
    /// </summary>
    public static class NumericExtensions
    {
        #region Methods
        /// <summary>
        /// Trims the trailing zeros from a <see cref="float"/>, <see cref="double"/> or <see cref="decimal"/>.
        /// </summary>
        /// <param name="_Number">The number to trim the zero from.</param>
        /// <param name="_Format">The <see cref="System.ValueType.ToString"/> formatting.</param>
        /// <typeparam name="T">Can only be a numeric <see cref="Type"/>, has no effect on integers, only works on floating point numbers.</typeparam>
        /// <returns>This number as a <see cref="string"/> with all trailing zeros removed.</returns>
        public static string TrimTrailingZero<T>(this T _Number, string _Format = "F29") where T : unmanaged, IFormattable
        {
            return _Number.ToString(_Format, CultureInfo.InvariantCulture).AsSpan().TrimEnd('0').TrimEnd('.').ToString();
        }
        
        /// <summary>
        /// Trims the trailing zeros from a <see cref="float"/>, <see cref="double"/> or <see cref="decimal"/>.
        /// </summary>
        /// <param name="_Number">The number to trim the zero from.</param>
        /// <param name="_CultureInfo">The <see cref="CultureInfo"/> to format the <see cref="string"/> with.</param>
        /// <param name="_Format">The <see cref="System.ValueType.ToString"/> formatting.</param>
        /// <typeparam name="T">Can only be a numeric <see cref="Type"/>, has no effect on integers, only works on floating point numbers.</typeparam>
        /// <returns>This number as a <see cref="string"/> with all trailing zeros removed.</returns>
        public static string TrimTrailingZero<T>(this T _Number, CultureInfo _CultureInfo, string _Format = "F29") where T : unmanaged, IFormattable
        {
            return _Number.ToString(_Format, _CultureInfo).AsSpan().TrimEnd('0').TrimEnd('.').ToString();
        }
        #endregion
    }
}