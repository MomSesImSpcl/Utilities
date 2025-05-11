#nullable enable
using System;
using System.Linq;
using System.Reflection;
using MomSesImSpcl.Utilities;
using UnityEngine;
using ZLinq;

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
        
        /// <summary>
        /// Gets the value of a static Field through reflection.
        /// </summary>
        /// <param name="_Type">The <see cref="Type"/> in which the Field is located.</param>
        /// <param name="_FieldName">The name of the Field.</param>
        /// <param name="_CombinedBindingFlags">Optional <see cref="CombinedBindingFlags"/> to find the Field.</param>
        /// <typeparam name="V">The <see cref="Type"/> of the Field.</typeparam>
        /// <returns>The Field value.</returns>
        public static V GetFieldValue<V>(this Type _Type, string _FieldName, CombinedBindingFlags _CombinedBindingFlags = CombinedBindingFlags.All)
        {
            // ReSharper disable VariableHidesOuterVariable
            return _Type.GetMemberValue<V,FieldInfo>(null, _FieldName, 
                _Type => _Type.GetField(_FieldName, _CombinedBindingFlags.AsBindingFlags()), 
                _Type => _Type.GetFields(CombinedBindingFlags.All.AsBindingFlags()));
            // ReSharper restore VariableHidesOuterVariable
        }
        
        /// <summary>
        /// Gets the value of a Field through reflection.
        /// </summary>
        /// <param name="_Type">The <see cref="Type"/> in which the Field is located.</param>
        /// <param name="_Instance">The instance of the <see cref="object"/> that holds the Field.</param>
        /// <param name="_FieldName">The name of the Field.</param>
        /// <param name="_CombinedBindingFlags">Optional <see cref="CombinedBindingFlags"/> to find the Field.</param>
        /// <typeparam name="V">The <see cref="Type"/> of the Field.</typeparam>
        /// <returns>The Field value.</returns>
        public static V GetFieldValue<V>(this Type _Type, object _Instance, string _FieldName, CombinedBindingFlags _CombinedBindingFlags = CombinedBindingFlags.All)
        {
            // ReSharper disable VariableHidesOuterVariable
            return _Type.GetMemberValue<V,FieldInfo>(_Instance, _FieldName, 
                _Type => _Type.GetField(_FieldName, _CombinedBindingFlags.AsBindingFlags()), 
                _Type => _Type.GetFields(CombinedBindingFlags.All.AsBindingFlags()));
            // ReSharper restore VariableHidesOuterVariable
        }
        
        /// <summary>
        /// Gets the value of a static Property through reflection.
        /// </summary>
        /// <param name="_Type">The <see cref="Type"/> in which the Property is located.</param>
        /// <param name="_PropertyName">The name of the Property.</param>
        /// <param name="_CombinedBindingFlags">Optional <see cref="CombinedBindingFlags"/> to find the Property.</param>
        /// <typeparam name="V">The <see cref="Type"/> of the Property.</typeparam>
        /// <returns>The Property value.</returns>
        public static V GetPropertyValue<V>(this Type _Type, string _PropertyName, CombinedBindingFlags _CombinedBindingFlags = CombinedBindingFlags.All)
        {
            // ReSharper disable VariableHidesOuterVariable
            return _Type.GetMemberValue<V,PropertyInfo>(null, _PropertyName, 
                _Type => _Type.GetProperty(_PropertyName, _CombinedBindingFlags.AsBindingFlags()), 
                _Type => _Type.GetProperties(CombinedBindingFlags.All.AsBindingFlags()));
            // ReSharper restore VariableHidesOuterVariable
        }
        
        /// <summary>
        /// Gets the value of a Property through reflection.
        /// </summary>
        /// <param name="_Type">The <see cref="Type"/> in which the Property is located.</param>
        /// <param name="_Instance">The instance of the <see cref="object"/> that holds the Property.</param>
        /// <param name="_PropertyName">The name of the Property.</param>
        /// <param name="_CombinedBindingFlags">Optional <see cref="CombinedBindingFlags"/> to find the Property.</param>
        /// <typeparam name="V">The <see cref="Type"/> of the Property.</typeparam>
        /// <returns>The Property value.</returns>
        public static V GetPropertyValue<V>(this Type _Type, object _Instance, string _PropertyName, CombinedBindingFlags _CombinedBindingFlags = CombinedBindingFlags.All)
        {
            // ReSharper disable VariableHidesOuterVariable
            return _Type.GetMemberValue<V,PropertyInfo>(_Instance, _PropertyName, 
                _Type => _Type.GetProperty(_PropertyName, _CombinedBindingFlags.AsBindingFlags()), 
                _Type => _Type.GetProperties(CombinedBindingFlags.All.AsBindingFlags()));
            // ReSharper restore VariableHidesOuterVariable
        }
        
        /// <summary>
        /// Sets the value of a static member through reflection.
        /// </summary>
        /// <param name="_Type">The <see cref="Type"/> in which the member is located.</param>
        /// <param name="_Instance">The instance of the <see cref="object"/> that holds the member, or <c>null</c> if the member is static.</param>
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
        private static V GetMemberValue<V,I>(this Type _Type, object? _Instance, string _MemberName, Func<Type,I> _GetMember, Func<Type,I[]> _FallbackMembers) where I : MemberInfo
        {
            if (_GetMember(_Type) is {} _memberInfo)
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

            throw new InvalidOperationException(GenericExtensions.FallbackMemberMessage(_Type, _MemberName, _FallbackMembers));
        }

        /// <summary>
        /// Checks if this <see cref="Type"/> implements the given <c>_InterfaceType</c>.
        /// </summary>
        /// <param name="_Type">The <see cref="Type"/> to check for the interface implementation.</param>
        /// <param name="_InterfaceType">The <see cref="Type"/> of the interface to check for.</param>
        /// <returns><c>true</c> if this <see cref="Type"/> implements the given <c>_InterfaceType</c>, otherwise <c>flase</c>.</returns>
        public static bool ImplementsInterface(this Type _Type, Type _InterfaceType)
        {
            return _InterfaceType.IsGenericTypeDefinition switch
            {
                false => _InterfaceType.IsAssignableFrom(_Type),
                true => _Type.GetInterfaces().AsValueEnumerable().Any(_Interface => _Interface.IsGenericType && _Interface.GetGenericTypeDefinition() == _InterfaceType)
            };
        }
        
        /// <summary>
        /// Checks if this <see cref="Type"/> implements the given <c>_InterfaceType</c>.
        /// </summary>
        /// <param name="_Type">The <see cref="Type"/> to check for the interface implementation.</param>
        /// <param name="_InterfaceType">The <see cref="Type"/> of the interface to check for.</param>
        /// <param name="_ParameterTypes">
        /// Optional parameter <see cref="Type"/>s if the interface has generic parameters. <br/>
        /// <i>If the interface has generic parameters and this is left empty, the method will still return <c>true</c> if this <see cref="Type"/> implements the given <c>_InterfaceType</c>.</i> <br/>
        /// <i>If this is not empty, this method will only return <c>true</c> if the given <see cref="Type"/>s exactly match the parameter <see cref="Type"/>s of the interface.</i>
        /// </param>
        /// <returns><c>true</c> if this <see cref="Type"/> implements the given <c>_InterfaceType</c>, otherwise <c>flase</c>.</returns>
        public static bool ImplementsInterface(this Type _Type, Type _InterfaceType, params Type[] _ParameterTypes)
        {
            if (_ParameterTypes.Length == 0)
            {
                throw new ArgumentException($"{nameof(_ParameterTypes)} is empty.");
            }

            foreach (var _interface in _Type.GetInterfaces())
            {
                if (!_interface.IsGenericType || _interface.GetGenericTypeDefinition() != _InterfaceType)
                {
                    continue;
                }

                var _arguments = _interface.GetGenericArguments();

                if (_arguments.Length == _ParameterTypes.Length && _arguments.SequenceEqual(_ParameterTypes))
                {
                    return true;
                }
            }
                
            return false;
        }
        
        /// <summary>
        /// Sets the value of a static Field through reflection.
        /// </summary>
        /// <param name="_Type">The <see cref="Type"/> in which the Field is located.</param>
        /// <param name="_FieldName">The name of the Field whose value to set.</param>
        /// <param name="_Value">The value to set to the Field.</param>
        /// <param name="_CombinedBindingFlags">Optional <see cref="CombinedBindingFlags"/> to find the Field.</param>
        /// <typeparam name="V">The <see cref="Type"/> of the Field.</typeparam>
        /// <returns><c>true</c> if the value of the Field was successfully set, otherwise <c>false</c>.</returns>
        public static bool SetFieldValue<V>(this Type _Type, string _FieldName, V _Value, CombinedBindingFlags _CombinedBindingFlags = CombinedBindingFlags.All)
        {
            // ReSharper disable VariableHidesOuterVariable
            return SetMemberValue(_Type, _FieldName, _Type => _Type.GetField(_FieldName, _CombinedBindingFlags.AsBindingFlags()), _FieldInfo =>
            {
                _FieldInfo.SetValue(null, _Value);
                
            }, _Value, _Type => _Type.GetFields(CombinedBindingFlags.All.AsBindingFlags()));
            // ReSharper restore VariableHidesOuterVariable
        }
        
        /// <summary>
        /// Sets the value of a static Property through reflection.
        /// </summary>
        /// <param name="_Type">The <see cref="Type"/> in which the Property is located.</param>
        /// <param name="_PropertyName">The name of the Property whose value to set.</param>
        /// <param name="_Value">The value to set to the Property.</param>
        /// <param name="_CombinedBindingFlags">Optional <see cref="CombinedBindingFlags"/> to find the Property.</param>
        /// <typeparam name="V">The <see cref="Type"/> of the Property.</typeparam>
        /// <returns><c>true</c> if the value of the Property was successfully set, otherwise <c>false</c>.</returns>
        public static bool SetPropertyValue<V>(this Type _Type, string _PropertyName, V _Value, CombinedBindingFlags _CombinedBindingFlags = CombinedBindingFlags.All)
        {
            // ReSharper disable VariableHidesOuterVariable
            return SetMemberValue(_Type, _PropertyName, _Type => _Type.GetProperty(_PropertyName, _CombinedBindingFlags.AsBindingFlags()), _PropertyInfo =>
            {
                _PropertyInfo.SetValue(null, _Value);
                
            }, _Value, _Type => _Type.GetProperties(CombinedBindingFlags.All.AsBindingFlags()));
            // ReSharper restore VariableHidesOuterVariable
        }

        /// <summary>
        /// Sets the value of a static member through reflection.
        /// </summary>
        /// <param name="_Type">The <see cref="Type"/> in which the member is located.</param>
        /// <param name="_MemberName">The name of the member whose value to set.</param>
        /// <param name="_GetMethod">Should be <see cref="Type.GetField(string,BindingFlags)"/> or <see cref="Type.GetProperty(string,BindingFlags)"/>.</param>
        /// <param name="_SetMethod">Should be <see cref="FieldInfo"/>.<see cref="FieldInfo.SetValue(object,object)"/> or <see cref="PropertyInfo"/>.<see cref="PropertyInfo.SetValue(object,object)"/>.</param>
        /// <param name="_Value">The value to set to the Field.</param>
        /// <param name="_FallbackMembers">
        /// In case the Field/Property could not be found, this will print all Fields/Properties of the given <see cref="Type"/>. <br/>
        /// <i>Should be <see cref="Type.GetFields(BindingFlags)"/> or <see cref="Type.GetProperties(BindingFlags)"/>.</i>
        /// </param>
        /// <typeparam name="V">The <see cref="Type"/> of the Field.</typeparam>
        /// <typeparam name="I">The concrete <see cref="Type"/> of the <see cref="MemberInfo"/>.</typeparam>
        /// <exception cref="InvalidOperationException">When the member is a Property without a setter and no accessible backing field.</exception>
        /// <returns><c>true</c> if the value of the member was successfully set, otherwise <c>false</c>.</returns>
        private static bool SetMemberValue<V,I>(Type _Type, string _MemberName, Func<Type,I> _GetMethod, Action<I> _SetMethod, V _Value, Func<Type,I[]> _FallbackMembers) where I : MemberInfo
        {
            if (_GetMethod(_Type) is {} _memberInfo)
            {
                if (_memberInfo is PropertyInfo { CanWrite: false })
                {
                    if (!_Type.SetFieldValue($"<{_MemberName}>k__BackingField", _Value))
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

            Debug.LogError(GenericExtensions.FallbackMemberMessage(_Type, _MemberName, _FallbackMembers));
            return false;
        }
        #endregion
    }
}
