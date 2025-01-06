using MomSesImSpcl.Attributes;
using UnityEditor;
using UnityEngine;

namespace MomSesImSpcl.Editor.PropertyDrawers
{
    /// <summary>
    /// Custom property drawer for the <see cref="ElementNamesAttribute"/>.
    /// </summary>
    [CustomPropertyDrawer(typeof(ElementNamesAttribute))]
    public class ElementNamesDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect _Rect, SerializedProperty _Property, GUIContent _Label)
        {
            try
            {
                var _index = int.Parse(_Property.propertyPath.Split('[', ']')[1]);
                var _elementNameAttribute = (ElementNamesAttribute)base.attribute;
                var _text = $"{_elementNameAttribute.ElementNames[_index]}{(_elementNameAttribute.DisplayIndex ? $" {_index}" : string.Empty)}";
                
                EditorGUI.PropertyField(_Rect, _Property, new GUIContent(_text));
            }
            catch
            {
                EditorGUI.PropertyField(_Rect, _Property, _Label);
            }
        }
    }
}