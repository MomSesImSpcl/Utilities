using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using MomSesImSpcl.Utilities;
using Sirenix.Utilities;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="Enum"/>.
    /// </summary>
    public static class EnumExtensions
    {
        #region Methods
        /// <summary>
        /// Casts this <see cref="CombinedBindingFlags"/> to a <see cref="BindingFlags"/> <see cref="object"/>.
        /// </summary>
        /// <param name="_CombinedBindingFlags">The <see cref="CombinedBindingFlags"/> to cast to <see cref="BindingFlags"/>.</param>
        /// <returns>This <see cref="CombinedBindingFlags"/> as a <see cref="CombinedBindingFlags"/> <see cref="object"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BindingFlags AsBindingFlags(this CombinedBindingFlags _CombinedBindingFlags)
        {
            return (BindingFlags)_CombinedBindingFlags;
        }
        
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
        /// Converts an <see cref="Enum"/> value ot its underlying <see cref="sbyte"/> representation.
        /// </summary>
        /// <param name="_EnumValue">The value to convert.</param>
        /// <typeparam name="E">Must be an <see cref="Enum"/>.</typeparam>
        /// <returns>The <see cref="sbyte"/> representation of the <see cref="Enum"/> value.</returns>
        public static sbyte ToSByte<E>(this E _EnumValue) where E : Enum
        {
#if UNITY_EDITOR
            EnumTypeCheck<E,sbyte>();
#endif
            return Unsafe.As<E,sbyte>(ref _EnumValue);
        }
        
        /// <summary>
        /// Converts an <see cref="Enum"/> value ot its underlying <see cref="byte"/> representation.
        /// </summary>
        /// <param name="_EnumValue">The value to convert.</param>
        /// <typeparam name="E">Must be an <see cref="Enum"/>.</typeparam>
        /// <returns>The <see cref="byte"/> representation of the <see cref="Enum"/> value.</returns>
        public static byte ToByte<E>(this E _EnumValue) where E : Enum
        {
#if UNITY_EDITOR
            EnumTypeCheck<E,byte>();
#endif
            return Unsafe.As<E,byte>(ref _EnumValue);
        }
        
        /// <summary>
        /// Converts an <see cref="Enum"/> value ot its underlying <see cref="short"/> representation.
        /// </summary>
        /// <param name="_EnumValue">The value to convert.</param>
        /// <typeparam name="E">Must be an <see cref="Enum"/>.</typeparam>
        /// <returns>The <see cref="short"/> representation of the <see cref="Enum"/> value.</returns>
        public static short ToShort<E>(this E _EnumValue) where E : Enum
        {
#if UNITY_EDITOR
            EnumTypeCheck<E,short>();
#endif
            return Unsafe.As<E,short>(ref _EnumValue);
        }
        
        /// <summary>
        /// Converts an <see cref="Enum"/> value ot its underlying <see cref="ushort"/> representation.
        /// </summary>
        /// <param name="_EnumValue">The value to convert.</param>
        /// <typeparam name="E">Must be an <see cref="Enum"/>.</typeparam>
        /// <returns>The <see cref="ushort"/> representation of the <see cref="Enum"/> value.</returns>
        public static ushort ToUShort<E>(this E _EnumValue) where E : Enum
        {
#if UNITY_EDITOR
            EnumTypeCheck<E,ushort>();
#endif
            return Unsafe.As<E,ushort>(ref _EnumValue);
        }
        
        /// <summary>
        /// Converts an <see cref="Enum"/> value ot its underlying <see cref="int"/> representation.
        /// </summary>
        /// <param name="_EnumValue">The value to convert.</param>
        /// <typeparam name="E">Must be an <see cref="Enum"/>.</typeparam>
        /// <returns>The <see cref="int"/> representation of the <see cref="Enum"/> value.</returns>
        public static int ToInt<E>(this E _EnumValue) where E : Enum
        {
#if UNITY_EDITOR
            EnumTypeCheck<E,int>();
#endif
            return Unsafe.As<E,int>(ref _EnumValue);
        }
        
        /// <summary>
        /// Converts an <see cref="Enum"/> value ot its underlying <see cref="uint"/> representation.
        /// </summary>
        /// <param name="_EnumValue">The value to convert.</param>
        /// <typeparam name="E">Must be an <see cref="Enum"/>.</typeparam>
        /// <returns>The <see cref="uint"/> representation of the <see cref="Enum"/> value.</returns>
        public static uint ToUInt<E>(this E _EnumValue) where E : Enum
        {
#if UNITY_EDITOR
            EnumTypeCheck<E,uint>();
#endif
            return Unsafe.As<E,uint>(ref _EnumValue);
        }
        
        /// <summary>
        /// Converts an <see cref="Enum"/> value ot its underlying <see cref="long"/> representation.
        /// </summary>
        /// <param name="_EnumValue">The value to convert.</param>
        /// <typeparam name="E">Must be an <see cref="Enum"/>.</typeparam>
        /// <returns>The <see cref="long"/> representation of the <see cref="Enum"/> value.</returns>
        public static long ToLong<E>(this E _EnumValue) where E : Enum
        {
#if UNITY_EDITOR
            EnumTypeCheck<E,long>();
#endif
            return Unsafe.As<E,long>(ref _EnumValue);
        }
        
        /// <summary>
        /// Converts an <see cref="Enum"/> value ot its underlying <see cref="ulong"/> representation.
        /// </summary>
        /// <param name="_EnumValue">The value to convert.</param>
        /// <typeparam name="E">Must be an <see cref="Enum"/>.</typeparam>
        /// <returns>The <see cref="ulong"/> representation of the <see cref="Enum"/> value.</returns>
        public static ulong ToULong<E>(this E _EnumValue) where E : Enum
        {
#if UNITY_EDITOR
            EnumTypeCheck<E,ulong>();
#endif
            return Unsafe.As<E,ulong>(ref _EnumValue);
        }
        
#if UNITY_EDITOR
        /// <summary>
        /// Checks if the <see cref="Enum"/> <see cref="Type"/> <c>E</c> can be converted into the given <see cref="Type"/> <c>T</c>.
        /// </summary>
        /// <typeparam name="E">The <see cref="Type"/> of the <see cref="Enum"/>.</typeparam>
        /// <typeparam name="T">The target <see cref="Type"/> to convert the <see cref="Enum"/> value into.</typeparam>
        /// <exception cref="ArgumentException">When the underlying <see cref="Type"/> of the <see cref="Enum"/> doesn't match the target <see cref="Type"/> <c>T</c>.</exception>
        private static void EnumTypeCheck<E,T>() where E : Enum where T : unmanaged
        {
            if (Unsafe.SizeOf<E>() != Unsafe.SizeOf<T>())
            {
#if ODIN_INSPECTOR
                var _enumType = Enum.GetUnderlyingType(typeof(E)).GetNiceName().Bold();
                var _targetType = typeof(T).GetNiceName().Bold();
#else
                var _enumType = Enum.GetUnderlyingType(typeof(E)).Name.Bold();
                var _targetType = typeof(T).Name.Bold();
#endif
                throw new ArgumentException($"The Enum: [{typeof(E).Name.Bold()}] uses an [{_enumType}] as its underlying Type and can't be converted into a [{_targetType}].");
            }
        }
#endif
        #endregion
    }
}