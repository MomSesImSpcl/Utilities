#nullable enable
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using MomSesImSpcl.Utilities;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="object"/>.
    /// </summary>
    public static class ObjectExtensions
    {
        #region Methods
        /// <summary>
        /// Casts this <see cref="object"/> to the given <see cref="Type"/> using the <c>as</c>-keyword.
        /// </summary>
        /// <param name="_Object">The <see cref="object"/> to cast.</param>
        /// <typeparam name="T">The <see cref="Type"/> to cast the <see cref="object"/> to.</typeparam>
        /// <returns>This <see cref="object"/> cast to the given <see cref="Type"/>, or <c>null</c> if the cast failed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T? As<T>(this object _Object) where T : class
        {
            return _Object as T;
        }
        
        /// <summary>
        /// Gets the value of an instance Field through reflection.
        /// </summary>
        /// <param name="_Instance">The instance of the <see cref="object"/> that holds the Field.</param>
        /// <param name="_FieldName">The name of the Field.</param>
        /// <param name="_CombinedBindingFlags">Optional <see cref="CombinedBindingFlags"/> to find the Field.</param>
        /// <typeparam name="V">The <see cref="Type"/> of the Field.</typeparam>
        /// <returns>The Field value.</returns>
        public static V GetFieldValue<V>(this object _Instance, string _FieldName, CombinedBindingFlags _CombinedBindingFlags = CombinedBindingFlags.All)
        {
            return _Instance.GetMemberValue<V,FieldInfo>(_FieldName, 
                _InstanceType => _InstanceType.GetField(_FieldName, (BindingFlags)_CombinedBindingFlags), 
                _InstanceType => _InstanceType.GetFields((BindingFlags)CombinedBindingFlags.All));
        }

        /// <summary>
        /// Gets the value of an instance Property through reflection.
        /// </summary>
        /// <param name="_Instance">The instance of the <see cref="object"/> that holds the Property.</param>
        /// <param name="_PropertyName">The name of the Property.</param>
        /// <param name="_CombinedBindingFlags">Optional <see cref="CombinedBindingFlags"/> to find the Property.</param>
        /// <typeparam name="V">The <see cref="Type"/> of the Property.</typeparam>
        /// <returns>The Property value.</returns>
        public static V GetPropertyValue<V>(this object _Instance, string _PropertyName, CombinedBindingFlags _CombinedBindingFlags = CombinedBindingFlags.All)
        {
            return _Instance.GetMemberValue<V,PropertyInfo>(_PropertyName, 
                _InstanceType => _InstanceType.GetProperty(_PropertyName, (BindingFlags)_CombinedBindingFlags), 
                _InstanceType => _InstanceType.GetProperties((BindingFlags)CombinedBindingFlags.All));
        }
        
        /// <summary>
        /// Gets the value of a member through reflection.
        /// </summary>
        /// <param name="_Instance">The instance of the <see cref="object"/> that holds the member.</param>
        /// <param name="_MemberName">The name of the member whose value to get.</param>
        /// <param name="_GetMember">Should be <see cref="Type.GetField(string,BindingFlags)"/> or <see cref="Type.GetProperty(string,BindingFlags)"/>.</param>
        /// <param name="_FallbackMembers">
        /// In case the Field/Property could not be found, this will print all Fields/Properties of the given <see cref="Type"/>. <br/>
        /// <i>Should be <see cref="Type.GetFields(BindingFlags)"/> or <see cref="Type.GetProperties(BindingFlags)"/>.</i>
        /// </param>
        /// <typeparam name="V">The <see cref="Type"/> of the member.</typeparam>
        /// <typeparam name="I">The concrete <see cref="Type"/> of the <see cref="MemberInfo"/>.</typeparam>
        /// <returns>The value of the member.</returns>
        /// <exception cref="NotSupportedException">When the <see cref="MemberInfo"/> is not a <see cref="FieldInfo"/> or <see cref="PropertyInfo"/>.</exception>
        /// <exception cref="InvalidCastException">When the given <see cref="Type"/> <c>V</c> does not match the <see cref="Type"/> of the member"/>.</exception>
        /// <exception cref="InvalidOperationException">When the given <see cref="Type"/> <c>T</c> does not contain a member with the given name.</exception>
        private static V GetMemberValue<V,I>(this object _Instance, string _MemberName, Func<Type,I> _GetMember, Func<Type,I[]> _FallbackMembers) where I : MemberInfo
        {
            var _type = _Instance.GetType();
            
            if (_GetMember(_type) is {} _memberInfo)
            {
                var _memberValue = _memberInfo switch
                {
                    FieldInfo _fieldInfo => _fieldInfo.GetValue(_Instance),
                    PropertyInfo _propertyInfo => _propertyInfo.GetValue(_Instance),
                    _ => throw new NotSupportedException($"The Member: [{_MemberName.Bold()}], is not a Field or Property and cannot be accessed using GetValue.")
                };
                
                if (_memberValue is V _value)
                {
                    return _value;
                }
                
                throw new InvalidCastException($"The given Type: [{typeof(V).Name.Bold()}], does not match the Type of [{_MemberName.Bold()}]: [{_memberValue.GetType().Name.Bold()}].");
            }

            throw new InvalidOperationException(GenericExtensions.FallbackMemberMessage(_type, _MemberName, _FallbackMembers));
        }
        
        /// <summary>
        /// Casts this <see cref="object"/> to the given <see cref="Type"/>.
        /// </summary>
        /// <param name="_Object">The <see cref="object"/> to cast.</param>
        /// <typeparam name="T">Must be a reference <see cref="Type"/>.</typeparam>
        /// <returns>This <see cref="object"/> cast to the given <see cref="Type"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(this object _Object) where T : class
        {
            return (T)_Object;
        }

        /// <summary>
        /// Casts this <see cref="object"/> to the given <see cref="Type"/> <c>T</c>, using <see cref="Unsafe"/>.<see cref="Unsafe.As{TFrom,TTo}"/>.
        /// </summary>
        /// <param name="_Object">The <see cref="object"/> to cast.</param>
        /// <typeparam name="T">The <see cref="Type"/> to cast the <see cref="object"/> to.</typeparam>
        /// <returns>This <see cref="object"/> cast to the given <see cref="Type"/> <c>T</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T UnsafeAs<T>(this object _Object)
        {
            return Unsafe.As<object, T>(ref _Object);
        }
        #endregion
    }
}
