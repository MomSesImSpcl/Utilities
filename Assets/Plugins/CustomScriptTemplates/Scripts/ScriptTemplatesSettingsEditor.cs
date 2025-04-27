using System.Globalization;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace CustomScriptTemplates
{
    /// <summary>
    /// Custom Editor for <see cref="ScriptTemplatesSettings"/>.
    /// </summary>
    [CustomEditor(typeof(ScriptTemplatesSettings))]
    internal sealed class ScriptTemplatesSettingsEditor : Editor
    {
        #region Inspector Fields
        /// <summary>
        /// <see cref="ScriptTemplatesSettings.indentAmount"/>.
        /// </summary>
        private SerializedProperty indentAmount;
        /// <summary>
        /// <see cref="ScriptTemplatesSettings.enableTextFile"/>.
        /// </summary>
        private SerializedProperty enableTextFile;
        /// <summary>
        /// <see cref="ScriptTemplatesSettings.enableJsonFile"/>.
        /// </summary>
        private SerializedProperty enableJsonFile;
        /// <summary>
        /// <see cref="ScriptTemplatesSettings.enableXMLFile"/>.
        /// </summary>
        private SerializedProperty enableXMLFile;
        /// <summary>
        /// <see cref="ScriptTemplatesSettings.templates"/>.
        /// </summary>
        private ReorderableList templates;
        #endregion
        
        #region Methods
        private void OnEnable()
        {
            this.indentAmount = base.serializedObject.FindProperty(ScriptTemplatesSettings.INDENT_AMOUNT_NAME);
            this.enableTextFile = base.serializedObject.FindProperty(ScriptTemplatesSettings.ENABLE_TEXT_FILE_NAME);
            this.enableJsonFile = base.serializedObject.FindProperty(ScriptTemplatesSettings.ENABLE_JSON_FILE_NAME);
            this.enableXMLFile = base.serializedObject.FindProperty(ScriptTemplatesSettings.ENABLE_XML_FILE_NAME);
            this.templates = new ReorderableList(base.serializedObject, base.serializedObject.FindProperty(ScriptTemplatesSettings.TEMPLATES_NAME), true, true, true, true)
            {
                drawHeaderCallback = _Rect => { EditorGUI.LabelField(_Rect, CultureInfo.CurrentCulture.TextInfo.ToTitleCase(nameof(this.templates))); }
            };
            this.templates.drawElementCallback = (_Rect, _Index, _IsActive, _IsFocused) =>
            {
                EditorGUI.PropertyField(new Rect(_Rect.x, _Rect.y + 2.5f, Screen.width - 50, EditorGUIUtility.singleLineHeight), this.templates.serializedProperty.GetArrayElementAtIndex(_Index), GUIContent.none);
            };
        }
        
        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
            
            EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(this.indentAmount);
            GUILayout.Space(EditorGUIUtility.singleLineHeight);
            
            EditorGUILayout.LabelField("Optional Files", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Menu items must be recompiled after enabling/disabling a file.", MessageType.Info);
            EditorGUILayout.PropertyField(this.enableTextFile);
            EditorGUILayout.PropertyField(this.enableJsonFile);
            EditorGUILayout.PropertyField(this.enableXMLFile);
            GUILayout.Space(EditorGUIUtility.singleLineHeight);
            
            this.templates.DoLayoutList();
            base.serializedObject.ApplyModifiedProperties();
            
            if (GUILayout.Button("Compile Menu Items"))
            {
                ScriptTemplatesSettings.CompileMenuItems();
            }
        }
        #endregion
    }
}