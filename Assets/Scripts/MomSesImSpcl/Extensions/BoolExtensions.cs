using System.Runtime.CompilerServices;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="bool"/>.
    /// </summary>
    public static class BoolExtensions
    {
        #region Methods
        /// <summary>
        /// Converts this <see cref="bool"/> to a <see cref="byte"/>.
        /// </summary>
        /// <param name="_Bool">The <see cref="bool"/> convert.</param>
        /// <returns><c>1</c> if the value is <c>true</c>, otherwise <c>0</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe byte AsByte(this bool _Bool)
        {
            return *(byte*)&_Bool;
        }
        
        /// <summary>
        /// Converts this <see cref="bool"/> to an <see cref="int"/>.
        /// </summary>
        /// <param name="_Bool">The <see cref="bool"/> convert.</param>
        /// <returns><c>1</c> if the value is <c>true</c>, otherwise <c>0</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int AsInt(this bool _Bool)
        {
            return *(int*)&_Bool;
        }

        /// <summary>
        /// Converts this <see cref="bool"/> to a signed <see cref="int"/>.
        /// </summary>
        /// <param name="_Bool">The <see cref="bool"/> value to be converted.</param>
        /// <returns><c>1</c> if the value is <c>true</c>, otherwise <c>-1</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int AsSignedInt(this bool _Bool)
        {
            return (_Bool.AsByte() << 1) - 1;
        }
        #endregion
    }
}
