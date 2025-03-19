using System.Runtime.CompilerServices;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="double"/>.
    /// </summary>
    public static class DoubleExtensions
    {
        #region Methods
        /// <summary>
        /// Determines whether the specified <see cref="double"/> value has a sign, i.e., is either positive or negative.
        /// </summary>
        /// <param name="_Double">The double value to check for a sign.</param>
        /// <returns>True if the value is negative; otherwise, false.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasSign(double _Double) => _Double < 0;
        #endregion
    }
}