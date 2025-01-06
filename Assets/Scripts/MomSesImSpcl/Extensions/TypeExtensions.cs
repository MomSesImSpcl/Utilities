using System;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="Type"/>.
    /// </summary>
    public static class TypeExtensions
    {
        #region Methods

        /// <summary>
        /// Returns the count of the enum values of the given <c>_Type</c>.
        /// </summary>
        /// <param name="_Type">The <see cref="Type"/> to get the enum count of.</param>
        /// <returns>The count of the enum values of the given <c>_Type</c>.</returns>
        public static int GetEnumCount(this Type _Type)
        {
            return _Type.GetEnumValues().Length;
        }

        /// <summary>
        /// Returns an array of the enum values of the given <c>_Type</c>.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="_Type">The <see cref="Type"/> to get the enum values of.</param>
        /// <returns>An array of the enum values of the given <c>_Type</c>.</returns>
        public static T[] GetEnumValues<T>(this Type _Type)
        {
            return (T[])_Type.GetEnumValues();
        }
        #endregion
    }
}