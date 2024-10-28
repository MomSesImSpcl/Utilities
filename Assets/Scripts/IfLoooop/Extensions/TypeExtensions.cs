using System;

namespace IfLoooop.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="Type"/>.
    /// </summary>
    public static class TypeExtensions
    {
        #region Methods
        /// <summary>
        /// Returns the number of enum values of the given <c>_Type</c>.
        /// </summary>
        /// <param name="_Type">The <see cref="Type"/> of the enum.</param>
        /// <returns>The number of enum values of the given <c>_Type</c>.</returns>
        public static int GetEnumCount(this Type _Type)
        {
            return _Type.GetEnumValues().Length;
        }
        
        /// <summary>
        /// Returns the enum values of the given <c>_Type</c> as <c>T[]</c>.
        /// </summary>
        /// <param name="_Type">The <see cref="Type"/> to get the enum values of.</param>
        /// <typeparam name="T">Must be the <see cref="Type"/> of the enum.</typeparam>
        /// <returns>The enum values of the given <c>_Type</c> as <c>T[]</c>.</returns>
        public static T[] GetEnumValues<T>(this Type _Type)
        {
            return (T[])_Type.GetEnumValues();
        }
        #endregion
    }
}