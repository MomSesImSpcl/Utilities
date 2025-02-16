using System;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="Enum"/>.
    /// </summary>
    public static class EnumExtensions
    {
        #region Methods
        /// <summary>
        /// Returns the name of the given <see cref="Enum"/> value.
        /// </summary>
        /// <param name="_EnumValue">The <see cref="Enum"/> value to get the name of.</param>
        /// <typeparam name="E">Must be an <see cref="Enum"/>.</typeparam>
        /// <returns>The name of the given <see cref="Enum"/> value.</returns>
        public static string GetName<E>(this E _EnumValue) where E : Enum
        {
            return Enum.GetName(typeof(E), _EnumValue);
        }
            
        /// <summary>
        /// Converts an <see cref="Enum"/> value ot its underlying <see cref="int"/> representation.
        /// </summary>
        /// <param name="_EnumValue">The value to convert.</param>
        /// <typeparam name="E">Must be an <see cref="Enum"/>.</typeparam>
        /// <returns>The <see cref="int"/> representation of the <see cref="Enum"/> value.</returns>
        public static int ToInt<E>(this E _EnumValue) where E : Enum
        {
            return (int)(object)_EnumValue;
        }
        #endregion
    }
}
