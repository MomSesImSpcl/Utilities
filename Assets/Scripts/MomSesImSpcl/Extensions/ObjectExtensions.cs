#nullable enable
using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using MomSesImSpcl.Utilities;
using UnityEngine;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="object"/>.
    /// </summary>
    public static class ObjectExtensions
    {
        #region Methods
        /// <summary>
        /// Casts this <see cref="object"/> to the given <see cref="Type"/>.
        /// </summary>
        /// <param name="_Object">The <see cref="object"/> to cast.</param>
        /// <typeparam name="T">The <see cref="Type"/> to cast the <see cref="object"/> to.</typeparam>
        /// <returns>This <see cref="object"/> cast to the given <see cref="Type"/>.</returns>
        public static T As<T>(this object _Object)
        {
            return (T)_Object;
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

            throw new InvalidOperationException(FallbackMemberMessage(_type, _MemberName, _FallbackMembers));
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
            
            return SetMemberValue<T,V,FieldInfo>(_fieldName, _InstanceType => _InstanceType.GetField(_fieldName, (BindingFlags)_CombinedBindingFlags), _FieldInfo =>
            {
                _FieldInfo.SetValue(_Instance, _Value);
                
            }, _Value, _InstanceType => _InstanceType.GetFields((BindingFlags)CombinedBindingFlags.All));
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
            return SetMemberValue<T,V,FieldInfo>(_FieldName, _InstanceType => _InstanceType.GetField(_FieldName, (BindingFlags)_CombinedBindingFlags), _FieldInfo =>
            {
                _FieldInfo.SetValue(_Instance, _Value);
                
            }, _Value, _InstanceType => _InstanceType.GetFields((BindingFlags)CombinedBindingFlags.All));
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
            
            return SetMemberValue<T,V,PropertyInfo>(_propertyName, _InstanceType => _InstanceType.GetProperty(_propertyName, (BindingFlags)_CombinedBindingFlags), _PropertyInfo =>
            {
                _PropertyInfo.SetValue(_Instance, _Value);
                
            }, _Value, _InstanceType => _InstanceType.GetProperties((BindingFlags)CombinedBindingFlags.All));
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
            return SetMemberValue<T,V,PropertyInfo>(_PropertyName, _InstanceType => _InstanceType.GetProperty(_PropertyName, (BindingFlags)_CombinedBindingFlags), _PropertyInfo =>
            {
                _PropertyInfo.SetValue(_Instance, _Value);
                
            }, _Value, _InstanceType => _InstanceType.GetProperties((BindingFlags)CombinedBindingFlags.All));
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
