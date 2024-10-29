using System.Runtime.CompilerServices;

namespace IfLoooop.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="bool"/>.
    /// </summary>
    public static class BoolExtensions
    {
        #region Methods
        /// <summary>
        /// Converts the specified <see cref="bool"/> to a <see cref="byte"/> representation without any branching.
        /// </summary>
        /// <param name="_Bool">The boolean value to be converted.</param>
        /// <returns>Returns 1 if the value is true, otherwise returns 0.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe byte AsByte(this bool _Bool)
        {
            return *(byte*)&_Bool;
        }
        #endregion
    }
}