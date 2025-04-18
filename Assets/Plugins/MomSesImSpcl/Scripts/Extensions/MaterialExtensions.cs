using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="Material"/>.
    /// </summary>
    public static class MaterialExtensions
    {
        #region Methods
        /// <summary>
        /// Tries to get the <see cref="int"/> value of the property with the given id from this <see cref="Material"/>.
        /// </summary>
        /// <param name="_Material">The <see cref="Material"/> to search the property in.</param>
        /// <param name="_PropertyId">The id of the property to search for.</param>
        /// <param name="_Int">Will contain the value of the property if it exists, default otherwise.</param>
        /// <param name="_PrintIfNotFound">Set to <c>true</c> to print a warning to the console of the property couldn't be found.</param>
        /// <returns><c>true</c> if the <see cref="Material"/> contains a property with the given identifier, otherwise <c>false</c></returns>
        public static bool TryGetInt(this Material _Material, int _PropertyId, out int _Int, bool _PrintIfNotFound = false)
        {
            return _Material.TryGet(_PropertyId, out _Int, _PrintIfNotFound);
        }
        
        /// <summary>
        /// Tries to get the <see cref="int"/> value of the property with the given name from this <see cref="Material"/>.
        /// </summary>
        /// <param name="_Material">The <see cref="Material"/> to search the property in.</param>
        /// <param name="_PropertyName">The name of the property to search for.</param>
        /// <param name="_Int">Will contain the value of the property if it exists, default otherwise.</param>
        /// <param name="_PrintIfNotFound">Set to <c>true</c> to print a warning to the console of the property couldn't be found.</param>
        /// <returns><c>true</c> if the <see cref="Material"/> contains a property with the given identifier, otherwise <c>false</c></returns>
        public static bool TryGetInt(this Material _Material, string _PropertyName, out int _Int, bool _PrintIfNotFound = false)
        {
            return _Material.TryGet(_PropertyName, out _Int, _PrintIfNotFound);
        }
        
        /// <summary>
        /// Tries to get the <see cref="float"/> value of the property with the given id from this <see cref="Material"/>.
        /// </summary>
        /// <param name="_Material">The <see cref="Material"/> to search the property in.</param>
        /// <param name="_PropertyId">The id of the property to search for.</param>
        /// <param name="_Float">Will contain the value of the property if it exists, default otherwise.</param>
        /// <param name="_PrintIfNotFound">Set to <c>true</c> to print a warning to the console of the property couldn't be found.</param>
        /// <returns><c>true</c> if the <see cref="Material"/> contains a property with the given identifier, otherwise <c>false</c></returns>
        public static bool TryGetFloat(this Material _Material, int _PropertyId, out float _Float, bool _PrintIfNotFound = false)
        {
            return _Material.TryGet(_PropertyId, out _Float, _PrintIfNotFound);
        }
        
        /// <summary>
        /// Tries to get the <see cref="float"/> value of the property with the given name from this <see cref="Material"/>.
        /// </summary>
        /// <param name="_Material">The <see cref="Material"/> to search the property in.</param>
        /// <param name="_PropertyName">The name of the property to search for.</param>
        /// <param name="_Float">Will contain the value of the property if it exists, default otherwise.</param>
        /// <param name="_PrintIfNotFound">Set to <c>true</c> to print a warning to the console of the property couldn't be found.</param>
        /// <returns><c>true</c> if the <see cref="Material"/> contains a property with the given identifier, otherwise <c>false</c></returns>
        public static bool TryGetFloat(this Material _Material, string _PropertyName, out float _Float, bool _PrintIfNotFound = false)
        {
            return _Material.TryGet(_PropertyName, out _Float, _PrintIfNotFound);
        }

        /// <summary>
        /// Tries to get the <see cref="Color"/> value of the property with the given id from this <see cref="Material"/>.
        /// </summary>
        /// <param name="_Material">The <see cref="Material"/> to search the property in.</param>
        /// <param name="_PropertyId">The id of the property to search for.</param>
        /// <param name="_Color">Will contain the value of the property if it exists, default otherwise.</param>
        /// <param name="_PrintIfNotFound">Set to <c>true</c> to print a warning to the console of the property couldn't be found.</param>
        /// <returns><c>true</c> if the <see cref="Material"/> contains a property with the given identifier, otherwise <c>false</c></returns>
        public static bool TryGetColor(this Material _Material, int _PropertyId, out Color _Color, bool _PrintIfNotFound = false)
        {
            return _Material.TryGet(_PropertyId, out _Color, _PrintIfNotFound);
        }
        
        /// <summary>
        /// Tries to get the <see cref="Color"/> value of the property with the given name from this <see cref="Material"/>.
        /// </summary>
        /// <param name="_Material">The <see cref="Material"/> to search the property in.</param>
        /// <param name="_PropertyName">The name of the property to search for.</param>
        /// <param name="_Color">Will contain the value of the property if it exists, default otherwise.</param>
        /// <param name="_PrintIfNotFound">Set to <c>true</c> to print a warning to the console of the property couldn't be found.</param>
        /// <returns><c>true</c> if the <see cref="Material"/> contains a property with the given identifier, otherwise <c>false</c></returns>
        public static bool TryGetColor(this Material _Material, string _PropertyName, out Color _Color, bool _PrintIfNotFound = false)
        {
            return _Material.TryGet(_PropertyName, out _Color, _PrintIfNotFound);
        }
        
        /// <summary>
        /// Tries to get the value of the property with the given identifier from this <see cref="Material"/>.
        /// </summary>
        /// <param name="_Material">The <see cref="Material"/> to search the property in.</param>
        /// <param name="_PropertyIdentifier">Must be the name or id of a property.</param>
        /// <param name="_Value">The value of the property if it exists, default otherwise.</param>
        /// <param name="_PrintIfNotFound">Set to <c>true</c> to print a warning to the console of the property couldn't be found.</param>
        /// <typeparam name="I">Must be an <see cref="int"/> or <see cref="string"/>.</typeparam>
        /// <typeparam name="V">The <see cref="Type"/> of the property to get the value of.</typeparam>
        /// <returns><c>true</c> if the <see cref="Material"/> contains a property with the given identifier, otherwise <c>false</c></returns>
        private static bool TryGet<I,V>(this Material _Material, I _PropertyIdentifier, out V _Value, bool _PrintIfNotFound)
        {
            if (HasProperty(_Material, _PropertyIdentifier, out var _propertyIdentifierTypeCode))
            {
                switch (_propertyIdentifierTypeCode)
                {
                    case TypeCode.Int32:
                        var _propertyId = Unsafe.As<I, int>(ref _PropertyIdentifier);
                        _Value = _Material.GetValue<V>(_propertyId);
                        return true;
                    case TypeCode.String:
                        var _propertyName = Unsafe.As<I, string>(ref _PropertyIdentifier);
                        _Value = _Material.GetValue<V>(_propertyName);
                        return true;
                    default:
                        _Value = default;
                        break;
                }
            }
            
#if UNITY_EDITOR
            if (_PrintIfNotFound)
            {
                switch (_propertyIdentifierTypeCode)
                {
                    case TypeCode.Int32:
                        Debug.LogWarning($"The property with id [{_PropertyIdentifier.ToString().Bold()}] couldn't be found in [{_Material.name.Bold()}].");
                        break;
                    case TypeCode.String:
                        Debug.LogWarning($"The property [{_PropertyIdentifier.ToString().Bold()}] couldn't be found in [{_Material.name.Bold()}].");
                        break;
                }
            }
#endif
            _Value = default;
            return false;
        }

        /// <summary>
        /// Checks if the given <see cref="Material"/> has a property with the given identifier.
        /// </summary>
        /// <param name="_Material">The <see cref="Material"/> to search the property in.</param>
        /// <param name="_PropertyIdentifier">Must be the name or id of a property.</param>
        /// <param name="_PropertyIdentifierTypeCode">The <see cref="TypeCode"/> of the given identifier.</param>
        /// <typeparam name="I">Must be an <see cref="int"/> or <see cref="string"/>.</typeparam>
        /// <returns><c>true</c> if the <see cref="Material"/> contains a property with the given identifier, otherwise <c>false</c>.</returns>
        private static bool HasProperty<I>(Material _Material, I _PropertyIdentifier, out TypeCode _PropertyIdentifierTypeCode)
        {
            _PropertyIdentifierTypeCode = Type.GetTypeCode(typeof(I));
            
            switch (_PropertyIdentifierTypeCode)
            {
                case TypeCode.Int32:
                    var _propertyId = Unsafe.As<I, int>(ref _PropertyIdentifier);
                    return _Material.HasProperty(_propertyId);
                case TypeCode.String:
                    var _propertyName = Unsafe.As<I, string>(ref _PropertyIdentifier);
                    return _Material.HasProperty(_propertyName);
                default:
                    return false;
            }
        }
        
        /// <summary>
        /// Gets the value of the property with the given name in this <see cref="Material"/>.
        /// </summary>
        /// <param name="_Material">The <see cref="Material"/> to get the property value from.</param>
        /// <param name="_PropertyName">The name of the property to get the value of.</param>
        /// <typeparam name="V">The <see cref="Type"/> of the property to get the value of.</typeparam>
        /// <returns>The value of the property, or the default value of the given <see cref="Type"/>, if the <see cref="Type"/> is not covered by this method.</returns>
        private static V GetValue<V>(this Material _Material, string _PropertyName)
        {
            switch (Type.GetTypeCode(typeof(V)))
            {
                case TypeCode.Int32:
                    var _intValue = _Material.GetInteger(_PropertyName);
                    return Unsafe.As<int, V>(ref _intValue);
                case TypeCode.Single:
                    var _floatValue = _Material.GetFloat(_PropertyName);
                    return Unsafe.As<float, V>(ref _floatValue);
                default:
                {
                    if (typeof(V) == typeof(Color))
                    {
                        var _colorValue = _Material.GetColor(_PropertyName);
                        return Unsafe.As<Color, V>(ref _colorValue);
                    }

                    Debug.LogError($"{typeof(V).Name} is currently not supported.");
                    return default;
                }
            }
        }
        
        /// <summary>
        /// Gets the value of the property with the given id in this <see cref="Material"/>.
        /// </summary>
        /// <param name="_Material">The <see cref="Material"/> to get the property value from.</param>
        /// <param name="_PropertyId">The id of the property to get the value of.</param>
        /// <typeparam name="V">The <see cref="Type"/> of the property to get the value of.</typeparam>
        /// <returns>The value of the property, or the default value of the given <see cref="Type"/>, if the <see cref="Type"/> is not covered by this method.</returns>
        private static V GetValue<V>(this Material _Material, int _PropertyId)
        {
            switch (Type.GetTypeCode(typeof(V)))
            {
                case TypeCode.Int32:
                    var _intValue = _Material.GetInteger(_PropertyId);
                    return Unsafe.As<int, V>(ref _intValue);
                case TypeCode.Single:
                    var _floatValue = _Material.GetFloat(_PropertyId);
                    return Unsafe.As<float, V>(ref _floatValue);
                default:
                {
                    if (typeof(V) == typeof(Color))
                    {
                        var _colorValue = _Material.GetColor(_PropertyId);
                        return Unsafe.As<Color, V>(ref _colorValue);
                    }

                    Debug.LogError($"{typeof(V).Name} is currently not supported.");
                    return default;
                }
            }
        }
        #endregion
    }
}