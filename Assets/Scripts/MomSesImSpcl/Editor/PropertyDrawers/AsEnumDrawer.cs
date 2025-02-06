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
    public class AsEnumDrawer : PropertyDrawer
    {
        #region Methods
        public override void OnGUI(Rect _Rect, SerializedProperty _Property, GUIContent _Label)
        {
            var _asEnumAttribute = (AsEnumAttribute)base.attribute;
            
#if ACTK_IS_HERE // Must enable the "ACTK_IS_HERE" checkbox under "Project Settings -> Code Stage -> Anti-Cheat Toolkit -> Third-party related" for this to work.
            if (typeof(CodeStage.AntiCheat.ObscuredTypes.IObscuredType).IsAssignableFrom(base.fieldInfo.FieldType))
            {
                _Property = _Property.FindPropertyRelative("hiddenValue");
            }
#endif
            var _enumValue = GetEnumValue(_asEnumAttribute.EnumType, _Property);
            
            EditorGUI.BeginChangeCheck();
            
            var _selectedEnumValue = EditorGUI.EnumPopup(_Rect, _Label, _enumValue);

            if (EditorGUI.EndChangeCheck())
            {
                SetPropertyValue(_Property, _selectedEnumValue);
            }
        }

        /// <summary>
        /// Gets the value of the <see cref="Enum"/> set in the <see cref="AsEnumAttribute"/> that corresponds with teh value of the given <see cref="SerializedProperty"/>.
        /// </summary>
        /// <param name="_EnumType">The <see cref="Type"/> of the <see cref="Enum"/> defined in the <see cref="AsEnumAttribute"/>.</param>
        /// <param name="_Property">Must be a non floating point numeric <see cref="Type"/>.</param>
        /// <returns>The <see cref="Enum"/> value that corresponds with teh value of the given <see cref="SerializedProperty"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When the <see cref="SerializedProperty.numericType"/> is not valid.</exception>
        private static Enum GetEnumValue(Type _EnumType, SerializedProperty _Property) => (Enum)Enum.ToObject(_EnumType, _Property.numericType switch
        {
            SerializedPropertyNumericType.Int8 => (sbyte)_Property.intValue,
            SerializedPropertyNumericType.UInt8 => (byte)_Property.uintValue,
            SerializedPropertyNumericType.Int16 => (short)_Property.intValue,
            SerializedPropertyNumericType.UInt16 => (ushort)_Property.uintValue,
            SerializedPropertyNumericType.Int32 => _Property.intValue,
            SerializedPropertyNumericType.UInt32 => _Property.uintValue,
            SerializedPropertyNumericType.Int64 => _Property.longValue,
            SerializedPropertyNumericType.UInt64 => _Property.ulongValue,
            _ => throw new ArgumentOutOfRangeException($"{_Property.numericType.Bold()} is not a valid numeric type.")
        });

        /// <summary>
        /// Sets the value of the given <see cref="SerializedProperty"/>.
        /// </summary>
        /// <param name="_Property">Must be a non floating point numeric <see cref="Type"/>.</param>
        /// <param name="_SelectedEnumValue">The currently selected <see cref="Enum"/> value in the inspector dropdown.</param>
        /// <exception cref="ArgumentOutOfRangeException">When the <see cref="SerializedProperty.numericType"/> is not valid.</exception>
        private static void SetPropertyValue(SerializedProperty _Property, Enum _SelectedEnumValue)
        {
            switch (_Property.numericType)
            {
                case SerializedPropertyNumericType.Int8:
                    _Property.intValue = Convert.ToSByte(_SelectedEnumValue);
                    break;
                case SerializedPropertyNumericType.UInt8:
                    _Property.uintValue = Convert.ToByte(_SelectedEnumValue);
                    break;
                case SerializedPropertyNumericType.Int16:
                    _Property.intValue = Convert.ToInt16(_SelectedEnumValue);
                    break;
                case SerializedPropertyNumericType.UInt16:
                    _Property.uintValue = Convert.ToUInt16(_SelectedEnumValue);
                    break;
                case SerializedPropertyNumericType.Int32:
                    _Property.intValue = Convert.ToInt32(_SelectedEnumValue);
                    break;
                case SerializedPropertyNumericType.UInt32:
                    _Property.uintValue = Convert.ToUInt32(_SelectedEnumValue);
                    break;
                case SerializedPropertyNumericType.Int64:
                    _Property.longValue = Convert.ToInt64(_SelectedEnumValue);
                    break;
                case SerializedPropertyNumericType.UInt64:
                    _Property.ulongValue = Convert.ToUInt64(_SelectedEnumValue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"{_Property.numericType.Bold()} is not a valid numeric type.");
            }
        }
        #endregion
    }
}