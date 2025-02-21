using MomSesImSpcl.Data;
using UnityEditor;
using UnityEditor.EventSystems;

namespace MomSesImSpcl.Editor.CustomEditors
{
    /// <summary>
    /// <see cref="CustomEditor"/> for <see cref="CustomEventTrigger"/>.
    /// </summary>
    [CustomEditor(typeof(CustomEventTrigger))]
    public sealed class CustomEventTriggerEditor : EventTriggerEditor
    {
        #region Fields
        /// <summary>
        /// <see cref="CustomEventTrigger.comment"/>.
        /// </summary>
        private SerializedProperty inspectorCommentField;
        /// <summary>
        /// <see cref="CustomEventTrigger.Flag"/>.
        /// </summary>
        private SerializedProperty flagField;
        #endregion
        
        #region Methods
        protected override void OnEnable()
        {
            base.OnEnable();
            
            this.inspectorCommentField = base.serializedObject.FindProperty(CustomEventTrigger.COMMENT);
            this.flagField = base.serializedObject.FindProperty(CustomEventTrigger.FLAG);
        }

        public override void OnInspectorGUI()
        {
            base.serializedObject.Update();
            
            EditorGUILayout.PropertyField(this.inspectorCommentField);
            EditorGUILayout.PropertyField(this.flagField);
            
            base.serializedObject.ApplyModifiedProperties();
            
            base.OnInspectorGUI();
        }
        #endregion
    }
}
