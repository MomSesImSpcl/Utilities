using System;
using MomSesImSpcl.Attributes;
using MomSesImSpcl.Extensions;
using UnityEditor;
using UnityEngine;

namespace MomSesImSpcl.Editor.PropertyDrawers
{
    /// <summary>
    /// Draws a <see cref="SerializedProperty.numericType"/> as the corresponding <see cref="Enum"/> value in the inspector.
    /// </summary>
    [CustomPropertyDrawer(typeof(AsEnumAttribute))]
    public sealed class AsEnumDrawer : PropertyDrawer
    {
        #region Methods
        public override void OnGUI(Rect _Rect, SerializedProperty _Property, GUIContent _Label)
        {
            var _asEnumAttribute = (AsEnumAttribute)base.attribute;
            
            var _propertyValue = GetBoxedValue(_Property);
            var _enumValue = (Enum)Enum.ToObject(_asEnumAttribute.EnumType, _propertyValue);
            
            EditorGUI.BeginChangeCheck();
            
            var _selectedEnumValue = EditorGUI.EnumPopup(_Rect, _Label, _enumValue);
            
            if (EditorGUI.EndChangeCheck())
            {
                _Property.boxedValue = ConvertToBoxedValue(_Property, _selectedEnumValue);
            }
        }
        
        /// <summary>
        /// Get the <see cref="SerializedProperty.boxedValue"/> from the given <see cref="SerializedProperty"/> as the correct <see cref="Type"/>.
        /// </summary>
        /// <param name="_Property">Must be a non floating points numeric <see cref="Type"/>.</param>
        /// <returns>A numeric value that matches the <see cref="Type"/> of the given <see cref="SerializedProperty"/>.</returns>
        private static dynamic GetBoxedValue(SerializedProperty _Property)
        {
            switch (_Property.boxedValue)
            {
                case byte _byte:
                    return _byte;
                case sbyte _sbyte:
                    return _sbyte;
                case short _short:
                    return _short;
                case ushort _ushort:
                    return _ushort;
                case int _int:
                    return _int;
                case uint _uint:
                    return _uint;
                case long _long:
                    return _long;
                case ulong _ulong:
                    return _ulong;
#if ACTK_IS_HERE // Must enable the "ACTK_IS_HERE" checkbox under "Project Settings -> Code Stage -> Anti-Cheat Toolkit -> Third-party related" for this to work.
                case CodeStage.AntiCheat.ObscuredTypes.ObscuredByte _obscuredByte:
                    return _obscuredByte;
                case CodeStage.AntiCheat.ObscuredTypes.ObscuredSByte _obscuredSByte:
                    return _obscuredSByte;
                case CodeStage.AntiCheat.ObscuredTypes.ObscuredShort _obscuredShort:
                    return _obscuredShort;
                case CodeStage.AntiCheat.ObscuredTypes.ObscuredUShort _obscuredUShort:
                    return _obscuredUShort;
                case CodeStage.AntiCheat.ObscuredTypes.ObscuredInt _obscuredInt:
                    return _obscuredInt;
                case CodeStage.AntiCheat.ObscuredTypes.ObscuredUInt _obscuredUInt:
                    return _obscuredUInt;
                case CodeStage.AntiCheat.ObscuredTypes.ObscuredLong _obscuredLong:
                    return _obscuredLong;
                case CodeStage.AntiCheat.ObscuredTypes.ObscuredULong _obscuredULong:
                    return _obscuredULong;
#endif
                default:
                    throw new NotSupportedException($"The type of the property: [{_Property.type.Bold()}] is not supported.");
            }
        }
        
        /// <summary>
        /// Converts the given <see cref="Enum"/> value to the correct <see cref="Type"/> based on the <see cref="SerializedProperty.boxedValue"/> of the given <see cref="SerializedProperty"/>.
        /// </summary>
        /// <param name="_Property">Must be a non floating points numeric <see cref="Type"/>.</param>
        /// <param name="_EnumValue">The <see cref="Enum"/> value to convert to its numeric counterpart.</param>
        /// <returns>A numeric value that matches the <see cref="Type"/> of the given <see cref="SerializedProperty"/>.</returns>
        private static dynamic ConvertToBoxedValue(SerializedProperty _Property, Enum _EnumValue)
        {
            switch (_Property.boxedValue)
            {
                case byte:
                    return Convert.ToByte(_EnumValue);
                case sbyte:
                    return Convert.ToSByte(_EnumValue);
                case short:
                    return Convert.ToInt16(_EnumValue);
                case ushort:
                    return Convert.ToUInt16(_EnumValue);
                case int:
                    return Convert.ToInt32(_EnumValue);
                case uint:
                    return Convert.ToUInt32(_EnumValue);
                case long:
                    return Convert.ToInt64(_EnumValue);
                case ulong:
                    return Convert.ToUInt64(_EnumValue);
#if ACTK_IS_HERE // Must enable the "ACTK_IS_HERE" checkbox under "Project Settings -> Code Stage -> Anti-Cheat Toolkit -> Third-party related" for this to work.
                case CodeStage.AntiCheat.ObscuredTypes.ObscuredByte:
                    return (CodeStage.AntiCheat.ObscuredTypes.ObscuredByte)Convert.ToByte(_EnumValue);
                case CodeStage.AntiCheat.ObscuredTypes.ObscuredSByte:
                    return (CodeStage.AntiCheat.ObscuredTypes.ObscuredSByte)Convert.ToSByte(_EnumValue);
                case CodeStage.AntiCheat.ObscuredTypes.ObscuredShort:
                    return (CodeStage.AntiCheat.ObscuredTypes.ObscuredShort)Convert.ToInt16(_EnumValue);
                case CodeStage.AntiCheat.ObscuredTypes.ObscuredUShort:
                    return (CodeStage.AntiCheat.ObscuredTypes.ObscuredUShort)Convert.ToUInt16(_EnumValue);
                case CodeStage.AntiCheat.ObscuredTypes.ObscuredInt:
                    return (CodeStage.AntiCheat.ObscuredTypes.ObscuredInt)Convert.ToInt32(_EnumValue);
                case CodeStage.AntiCheat.ObscuredTypes.ObscuredUInt:
                    return (CodeStage.AntiCheat.ObscuredTypes.ObscuredUInt)Convert.ToUInt32(_EnumValue);
                case CodeStage.AntiCheat.ObscuredTypes.ObscuredLong:
                    return (CodeStage.AntiCheat.ObscuredTypes.ObscuredLong)Convert.ToInt64(_EnumValue);
                case CodeStage.AntiCheat.ObscuredTypes.ObscuredULong:
                    return (CodeStage.AntiCheat.ObscuredTypes.ObscuredULong)Convert.ToUInt64(_EnumValue);
#endif
                default:
                    throw new NotSupportedException($"The type of the property: [{_Property.type.Bold()}] is not supported.");
            }
        }
        #endregion
    }
}
