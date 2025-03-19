using MomSesImSpcl.Attributes;
using UnityEditor;
using UnityEngine;

namespace MomSesImSpcl.Editor.PropertyDrawers
{
    /// <summary>
    /// A custom property drawer for displaying collection elements with a custom name in the Unity Editor.
    /// </summary>
    [CustomPropertyDrawer(typeof(ElementNameAttribute))]
    public sealed class ElementNameDrawer : PropertyDrawer
    {
        #region Methods
        public override void OnGUI(Rect _Rect, SerializedProperty _Property, GUIContent _Label)
        {
            try
            {
                var _index = int.Parse(_Property.propertyPath.Split('[', ']')[1]);
                var _elementNameAttribute = (ElementNameAttribute)base.attribute;
                var _text = $"{_elementNameAttribute.ElementName}{(_elementNameAttribute.DisplayIndex ? $" {_index}" : string.Empty)}";
                
                EditorGUI.PropertyField(_Rect, _Property, new GUIContent(_text));
            }
            catch
            {
                EditorGUI.PropertyField(_Rect, _Property, _Label);
            }
        }
        #endregion
    }
}