using IfLoooop.Data;
using UnityEditor;
using UnityEngine;

namespace IfLoooop.Editor.PropertyDrawers
{
#if ODIN_INSPECTOR
    /// <summary>
    /// A property drawer for the <see cref="ReadonlySelectableEntry{V}"/> struct,
    /// which provides a custom UI to display a key-value pair in the Unity Inspector,
    /// where the value is read-only.
    /// </summary>
    [CustomPropertyDrawer(typeof(ReadonlySelectableEntry<>))]
    public sealed class ReadonlySelectableEntryDrawer : PropertyDrawer
    {
        #region Methods
        public override void OnGUI(Rect _Position, SerializedProperty _Property, GUIContent _Label)
        {
            var _entryProperty = _Property.FindPropertyRelative(ReadonlySelectableEntry<object>.ENTRY);
            var _keyProperty = _entryProperty.FindPropertyRelative(SerializedKeyValuePair<object, object>.KEY);
            var _valueProperty = _entryProperty.FindPropertyRelative(SerializedKeyValuePair<object, object>.VALUE);
            
            var _keyRect = new Rect(_Position.x, _Position.y, _Position.width * 0.5f, _Position.height);
            var _valueRect = new Rect(_Position.x + _Position.width * 0.5f, _Position.y, _Position.width * 0.5f, _Position.height);
            
            EditorGUI.PropertyField(_keyRect, _keyProperty, GUIContent.none);
            
            GUI.enabled = false;
            EditorGUI.PropertyField(_valueRect, _valueProperty, GUIContent.none);
            GUI.enabled = true;
        }
        #endregion
    } 
#endif
}