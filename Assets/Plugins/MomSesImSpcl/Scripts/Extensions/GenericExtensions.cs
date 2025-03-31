#nullable enable
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using MomSesImSpcl.Utilities;
using UnityEngine;

#if ACTK_IS_HERE
using CodeStage.AntiCheat.ObscuredTypes;
#endif

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for generic types.
    /// </summary>
    public static class GenericExtensions
    {
        #region Delegates
        /// <summary>
        /// Delegate for returning an <see cref="IntPtr"/> from the reference of an instance.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of the instance.</typeparam>
        private delegate IntPtr FieldPointerDelegate<T>(ref T _Instance);
        #endregion
        
        #region Methods
        /// <summary>
        /// Returns the pointer for the given field in this instance.
        /// </summary>
        /// <param name="_Instance">The instance in which the field is declared.</param>
        /// <param name="_FieldName">The name of the field to get the pointer of.</param>
        /// <typeparam name="T">Must be an <c>unmanaged</c> <see cref="Type"/>.</typeparam>
        /// <returns>The pointer to the field.</returns>
        public static unsafe void* GetPointer<T>(this ref T _Instance, string _FieldName) where T : unmanaged
        {
            var _offset = Marshal.OffsetOf<T>(_FieldName).ToInt32();
            
            fixed (T* _pointer = &_Instance)
            {
                return (byte*)_pointer + _offset;
            }
        }
        
        /// <summary>
        /// Returns the pointer for the given <see cref="FieldInfo"/>.
        /// </summary>
        /// <param name="_Instance">The instance in which the field is declared.</param>
        /// <param name="_FieldInfo">The <see cref="FieldInfo"/> for which to get the pointer of.</param>
        /// <typeparam name="T">Must be a <c>struct</c>.</typeparam>
        /// <returns>The pointer for the given <see cref="FieldInfo"/>.</returns>
        public static IntPtr GetPointer<T>(this ref T _Instance, FieldInfo _FieldInfo) where T : struct
        {
            var _method = new DynamicMethod(
                name: nameof(GetPointer),
                returnType: typeof(IntPtr),
                parameterTypes: new[] { typeof(T).MakeByRefType() },
                owner: typeof(T),
                skipVisibility: true);
        
            var _ilGenerator = _method.GetILGenerator();
        
            _ilGenerator.Emit(OpCodes.Ldarg_0);
        
            if (_FieldInfo.DeclaringType is {} _declaringType)
            {
                if (!_declaringType.IsValueType)
                {
                    _ilGenerator.Emit(OpCodes.Ldind_Ref);
                    
                    if (_declaringType != typeof(T))
                    {
                        _ilGenerator.Emit(OpCodes.Castclass, _declaringType);
                    }
                }
        
                _ilGenerator.Emit(OpCodes.Ldflda, _FieldInfo);
            }
            
            _ilGenerator.Emit(OpCodes.Conv_I);
            _ilGenerator.Emit(OpCodes.Ret);
        
            var _delegateType = typeof(FieldPointerDelegate<>).MakeGenericType(typeof(T));
            var _delegateInstance = _method.CreateDelegate(_delegateType);
        
            return ((FieldPointerDelegate<T>)_delegateInstance)(ref _Instance);
        }
        
        /// <summary>
        /// Returns the pointer for the given <see cref="FieldInfo"/>.
        /// </summary>
        /// <param name="_Instance">The instance in which the field is declared.</param>
        /// <param name="_FieldInfo">The <see cref="FieldInfo"/> for which to get the pointer of.</param>
        /// <typeparam name="T">Must be a <c>class</c>.</typeparam>
        /// <returns>The pointer for the given <see cref="FieldInfo"/>.</returns>
        public static IntPtr GetPointer<T>(this T _Instance, FieldInfo _FieldInfo) where T : class
        {
            var _method = new DynamicMethod
            (
                name:           nameof(GetPointer),
                returnType:     typeof(IntPtr),
                parameterTypes: new[] { typeof(T) },
                owner:          typeof(T),
                skipVisibility: true
            );

            var _iLGenerator = _method.GetILGenerator();
            
            _iLGenerator.Emit(OpCodes.Ldarg_0);
            _iLGenerator.Emit(OpCodes.Castclass, _FieldInfo.DeclaringType);
            _iLGenerator.Emit(OpCodes.Ldflda, _FieldInfo);
            _iLGenerator.Emit(OpCodes.Conv_I);
            _iLGenerator.Emit(OpCodes.Ret);
            
            var _delegate = (Func<T,IntPtr>)_method.CreateDelegate(typeof(Func<T,IntPtr>));
            
            return _delegate(_Instance);
        }

        /// <summary>
        /// Returns the <see cref="string"/> of this instance without any boxing.
        /// </summary>
        /// <param name="_Instance">The instance to convert into a <see cref="string"/>.</param>
        /// <typeparam name="T">Must be an <c>unmanaged</c> <see cref="Type"/>.</typeparam>
        /// <returns>This instance as a <see cref="string"/>.</returns>
        public static string GetStringValue<T>(this T _Instance) where T : unmanaged
        {
            var _type = typeof(T);
            
            return _type switch
            {
                _ when _type == typeof(byte)    => Unsafe.As<T, byte>(ref _Instance).ToString(),
                _ when _type == typeof(sbyte)   => Unsafe.As<T, sbyte>(ref _Instance).ToString(),
                _ when _type == typeof(short)   => Unsafe.As<T, short>(ref _Instance).ToString(),
                _ when _type == typeof(ushort)  => Unsafe.As<T, ushort>(ref _Instance).ToString(),
                _ when _type == typeof(int)     => Unsafe.As<T, int>(ref _Instance).ToString(),
                _ when _type == typeof(uint)    => Unsafe.As<T, uint>(ref _Instance).ToString(),
                _ when _type == typeof(long)    => Unsafe.As<T, long>(ref _Instance).ToString(),
                _ when _type == typeof(ulong)   => Unsafe.As<T, ulong>(ref _Instance).ToString(),
                _ when _type == typeof(nint)    => Unsafe.As<T, nint>(ref _Instance).ToString(),
                _ when _type == typeof(nuint)   => Unsafe.As<T, nuint>(ref _Instance).ToString(),
                _ when _type == typeof(float)   => Unsafe.As<T, float>(ref _Instance).ToString(CultureInfo.InvariantCulture),
                _ when _type == typeof(double)  => Unsafe.As<T, double>(ref _Instance).ToString(CultureInfo.InvariantCulture),
                _ when _type == typeof(decimal) => Unsafe.As<T, decimal>(ref _Instance).ToString(CultureInfo.InvariantCulture),
                _ when _type == typeof(char)    => Unsafe.As<T, char>(ref _Instance).ToString(),
                _ when _type == typeof(bool)    => Unsafe.As<T, bool>(ref _Instance).ToString(),
#if ACTK_IS_HERE
                _ when _type == typeof(ObscuredByte)    => Unsafe.As<T, ObscuredByte>(ref _Instance).ToString(),
                _ when _type == typeof(ObscuredSByte)   => Unsafe.As<T, ObscuredSByte>(ref _Instance).ToString(),
                _ when _type == typeof(ObscuredShort)   => Unsafe.As<T, ObscuredShort>(ref _Instance).ToString(),
                _ when _type == typeof(ObscuredUShort)  => Unsafe.As<T, ObscuredUShort>(ref _Instance).ToString(),
                _ when _type == typeof(ObscuredInt)     => Unsafe.As<T, ObscuredInt>(ref _Instance).ToString(),
                _ when _type == typeof(ObscuredUInt)    => Unsafe.As<T, ObscuredUInt>(ref _Instance).ToString(),
                _ when _type == typeof(ObscuredLong)    => Unsafe.As<T, ObscuredLong>(ref _Instance).ToString(),
                _ when _type == typeof(ObscuredULong)   => Unsafe.As<T, ObscuredULong>(ref _Instance).ToString(),
                _ when _type == typeof(ObscuredFloat)   => Unsafe.As<T, ObscuredFloat>(ref _Instance).ToString(CultureInfo.InvariantCulture),
                _ when _type == typeof(ObscuredDouble)  => Unsafe.As<T, ObscuredDouble>(ref _Instance).ToString(CultureInfo.InvariantCulture),
                _ when _type == typeof(ObscuredDecimal) => Unsafe.As<T, ObscuredDecimal>(ref _Instance).ToString(CultureInfo.InvariantCulture),
                _ when _type == typeof(ObscuredChar)    => Unsafe.As<T, ObscuredChar>(ref _Instance).ToString(),
                _ when _type == typeof(ObscuredBool)    => Unsafe.As<T, ObscuredBool>(ref _Instance).ToString(),
#endif
                _ => default(T).ToString()
            };
        }
        
        /// <summary>
        /// Sets the value of an instance Field through reflection.
        /// </summary>
        /// <param name="_Instance">The instance of the <see cref="object"/> that holds the Field.</param>
        /// <param name="_Field">The Field whose value to set.</param>
        /// <param name="_Value">The value to set to the Field.</param>
        /// <param name="_CombinedBindingFlags">Optional <see cref="CombinedBindingFlags"/> to find the Field.</param>
        /// <typeparam name="T">The <see cref="Type"/> of the <see cref="object"/> that holds the Field.</typeparam>
        /// <typeparam name="V">The <see cref="Type"/> of the Field.</typeparam>
        /// <returns><c>true</c> if the value of the member was successfully set, otherwise <c>false</c>.</returns>
        public static bool SetFieldValue<T,V>(this T _Instance, Expression<Func<T,V>> _Field, V _Value, CombinedBindingFlags _CombinedBindingFlags = CombinedBindingFlags.All) where T : notnull
        {
            var _fieldName = _Field.GetMemberName();
            
            return SetMemberValue<T,V,FieldInfo>(_fieldName, _InstanceType => _InstanceType.GetField(_fieldName, _CombinedBindingFlags.AsBindingFlags()), _FieldInfo =>
            {
                _FieldInfo.SetValue(_Instance, _Value);
                
            }, _Value, _InstanceType => _InstanceType.GetFields((CombinedBindingFlags.All.AsBindingFlags())));
        }
        
        /// <summary>
        /// Sets the value of an instance Field through reflection.
        /// </summary>
        /// <param name="_Instance">The instance of the <see cref="object"/> that holds the Field.</param>
        /// <param name="_FieldName">The name of the Field whose value to set.</param>
        /// <param name="_Value">The value to set to the Field.</param>
        /// <param name="_CombinedBindingFlags">Optional <see cref="CombinedBindingFlags"/> to find the Field.</param>
        /// <typeparam name="T">The <see cref="Type"/> of the <see cref="object"/> that holds the Field.</typeparam>
        /// <typeparam name="V">The <see cref="Type"/> of the Field.</typeparam>
        /// <returns><c>true</c> if the value of the member was successfully set, otherwise <c>false</c>.</returns>
        public static bool SetFieldValue<T,V>(this T _Instance, string _FieldName, V _Value, CombinedBindingFlags _CombinedBindingFlags = CombinedBindingFlags.All) where T : notnull
        {
            return SetMemberValue<T,V,FieldInfo>(_FieldName, _InstanceType => _InstanceType.GetField(_FieldName, _CombinedBindingFlags.AsBindingFlags()), _FieldInfo =>
            {
                _FieldInfo.SetValue(_Instance, _Value);
                
            }, _Value, _InstanceType => _InstanceType.GetFields(CombinedBindingFlags.All.AsBindingFlags()));
        }
        
        /// <summary>
        /// Sets the value of an instance Property through reflection.
        /// </summary>
        /// <param name="_Instance">The instance of the <see cref="object"/> that holds the Property.</param>
        /// <param name="_Property">The Property whose value to set.</param>
        /// <param name="_Value">The value to set to the Property.</param>
        /// <param name="_CombinedBindingFlags">Optional <see cref="CombinedBindingFlags"/> to find the Property.</param>
        /// <typeparam name="T">The <see cref="Type"/> of the <see cref="object"/> that holds the Property.</typeparam>
        /// <typeparam name="V">The <see cref="Type"/> of the Property.</typeparam>
        /// <returns><c>true</c> if the value of the member was successfully set, otherwise <c>false</c>.</returns>
        public static bool SetPropertyValue<T,V>(this T _Instance, Expression<Func<T,V>> _Property, V _Value, CombinedBindingFlags _CombinedBindingFlags = CombinedBindingFlags.All) where T : notnull
        {
            var _propertyName = _Property.GetMemberName();
            
            return SetMemberValue<T,V,PropertyInfo>(_propertyName, _InstanceType => _InstanceType.GetProperty(_propertyName, _CombinedBindingFlags.AsBindingFlags()), _PropertyInfo =>
            {
                _PropertyInfo.SetValue(_Instance, _Value);
                
            }, _Value, _InstanceType => _InstanceType.GetProperties(CombinedBindingFlags.All.AsBindingFlags()));
        }
        
        /// <summary>
        /// Sets the value of an instance Property through reflection.
        /// </summary>
        /// <param name="_Instance">The instance of the <see cref="object"/> that holds the Property.</param>
        /// <param name="_PropertyName">The name of the Property whose value to set.</param>
        /// <param name="_Value">The value to set to the Property.</param>
        /// <param name="_CombinedBindingFlags">Optional <see cref="CombinedBindingFlags"/> to find the Property.</param>
        /// <typeparam name="T">The <see cref="Type"/> of the <see cref="object"/> that holds the Property.</typeparam>
        /// <typeparam name="V">The <see cref="Type"/> of the Property.</typeparam>
        /// <returns><c>true</c> if the value of the member was successfully set, otherwise <c>false</c>.</returns>
        public static bool SetPropertyValue<T,V>(this T _Instance, string _PropertyName, V _Value, CombinedBindingFlags _CombinedBindingFlags = CombinedBindingFlags.All) where T : notnull
        {
            return SetMemberValue<T,V,PropertyInfo>(_PropertyName, _InstanceType => _InstanceType.GetProperty(_PropertyName, _CombinedBindingFlags.AsBindingFlags()), _PropertyInfo =>
            {
                _PropertyInfo.SetValue(_Instance, _Value);
                
            }, _Value, _InstanceType => _InstanceType.GetProperties(CombinedBindingFlags.All.AsBindingFlags()));
        }
        
        /// <summary>
        /// Sets the value of an instance member through reflection.
        /// </summary>
        /// <param name="_MemberName">The name of the member whose value to set.</param>
        /// <param name="_GetMethod">Should be <see cref="Type.GetField(string,BindingFlags)"/> or <see cref="Type.GetProperty(string,BindingFlags)"/>.</param>
        /// <param name="_SetMethod">Should be <see cref="FieldInfo"/>.<see cref="FieldInfo.SetValue(object,object)"/> or <see cref="PropertyInfo"/>.<see cref="PropertyInfo.SetValue(object,object)"/>.</param>
        /// <param name="_Value">The value to set to the Field.</param>
        /// <param name="_FallbackMembers">
        /// In case the Field/Property could not be found, this will print all Fields/Properties of the given <see cref="Type"/>. <br/>
        /// <i>Should be <see cref="Type.GetFields(BindingFlags)"/> or <see cref="Type.GetProperties(BindingFlags)"/>.</i>
        /// </param>
        /// <typeparam name="T">The <see cref="Type"/> of the <see cref="object"/> that holds the member.</typeparam>
        /// <typeparam name="V">The <see cref="Type"/> of the member.</typeparam>
        /// <typeparam name="I">The concrete <see cref="Type"/> of the <see cref="MemberInfo"/>.</typeparam>
        /// <exception cref="InvalidOperationException">When the member is a Property without a setter and no accessible backing field.</exception>
        /// <returns><c>true</c> if the value of the member was successfully set, otherwise <c>false</c>.</returns>
        private static bool SetMemberValue<T,V,I>(string _MemberName, Func<Type,I> _GetMethod, Action<I> _SetMethod, V _Value, Func<Type,I[]> _FallbackMembers) where I : MemberInfo
        {
            var _type = typeof(T);
                
            if (_GetMethod(_type) is {} _memberInfo)
            {
                if (_memberInfo is PropertyInfo { CanWrite: false })
                {
                    if (!_type.SetFieldValue($"<{_MemberName}>k__BackingField", _Value))
                    {
                        throw new InvalidOperationException($"The property [{_MemberName.Bold()}] has no setter and no accessible backing field was found.");
                    }
                }
                else
                {
                    _SetMethod(_memberInfo);
                }
                
                return true;
            }

            Debug.LogError(FallbackMemberMessage(_type, _MemberName, _FallbackMembers));
            return false;
        }
        
        /// <summary>
        /// Creates a message with all members of the given <see cref="Type"/> <c>I</c> inside <c>_Type</c>.
        /// </summary>
        /// <param name="_Type">The <see cref="Type"/> from which to get all members of.</param>
        /// <param name="_MemberName">The name of the member that could not be found.</param>
        /// <param name="_FallbackMembers">Every member of the given <see cref="Type"/> <c>I</c> inside <c>_Type</c>.</param>
        /// <typeparam name="I">Must be a <see cref="MemberInfo"/>.</typeparam>
        /// <returns>A message with all members of the given <see cref="Type"/> <c>I</c> inside <c>_Type</c>.</returns>
        internal static string FallbackMemberMessage<I>(Type _Type, string _MemberName, Func<Type,I[]> _FallbackMembers) where I : MemberInfo
        {
            var _members = _FallbackMembers(_Type).Select(_MemberInfo => _MemberInfo.Name);
            var _memberType = typeof(I).Name.Replace("Info", string.Empty);
            var _className = _Type.Name;
                
            return $"Could not find the {_memberType} [{_MemberName.Bold()}] in Type [{_className.Bold()}].{Environment.NewLine}Here is every {_memberType} in Type {_className}:{Environment.NewLine}-{string.Join($"{Environment.NewLine}-", _members.Select(_Member => _Member.Italic()))}\n";
        }
        
        /// <summary>
        /// Converts the string representation of the specified object to a MemoryStream, using the provided encoding.
        /// </summary>
        /// <param name="_Object">The object whose string representation will be converted to a MemoryStream.</param>
        /// <param name="_Encoding">The encoding to use for the conversion.</param>
        /// <typeparam name="T">The <see cref="Type"/> of the <see cref="object"/>.</typeparam>
        /// <returns>A MemoryStream containing the encoded string representation of the specified object.</returns>
        public static MemoryStream ToMemoryStream<T>(this T? _Object, Encoding _Encoding)
        {
            return new MemoryStream(_Encoding.GetBytes(_Object?.ToString()?.OrNull()!));
        }
        #endregion
    }
}
