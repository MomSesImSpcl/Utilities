using MomSesImSpcl.Data;
using MomSesImSpcl.Extensions;
using UnityEditor;

namespace MomSesImSpcl.Editor.CustomEditors
{
    /// <summary>
    /// Custom editor for <see cref="RotationViewer"/>.
    /// </summary>
    [CustomEditor(typeof(RotationViewer))]
    internal sealed class RotationViewerEditor : 
#if ODIN_INSPECTOR
        Sirenix.OdinInspector.Editor.OdinEditor
#else
        UnityEditor.Editor
#endif
    {
        #region Fields
        /// <summary>
        /// <see cref="RotationViewer"/>.
        /// </summary>
        private RotationViewer rotationViewer;
        #endregion
        
        #region Methods
#if ODIN_INSPECTOR
        protected override void
#else
        private void
#endif
        OnEnable()
        {
#if ODIN_INSPECTOR
            base.OnEnable();
#endif
            this.rotationViewer = base.target.To<RotationViewer>();
            EditorApplication.update += this.RepaintInspector;
        }

#if ODIN_INSPECTOR
        protected override void
#else
        private void
#endif
        OnDisable()
        {
#if ODIN_INSPECTOR
            base.OnDisable();
#endif
            EditorApplication.update -= this.RepaintInspector;
        }

        /// <summary>
        /// Repaints the inspector in <see cref="EditorApplication"/>.<see cref="EditorApplication.update"/>.
        /// </summary>
        private void RepaintInspector() => base.Repaint();

        public override void OnInspectorGUI()
        {
            base.DrawDefaultInspector();
            
            this.rotationViewer.EulerAngles = this.rotationViewer.transform.eulerAngles;
            this.rotationViewer.SignedEulerAngles = this.rotationViewer.transform.SignedEulerAngles();
            this.rotationViewer.Rotation = this.rotationViewer.transform.rotation;
            
            this.rotationViewer.LocalEulerAngles = this.rotationViewer.transform.localEulerAngles;
            this.rotationViewer.SignedLocalEulerAngles = this.rotationViewer.transform.SignedLocalEulerAngles();
            this.rotationViewer.LocalRotation = this.rotationViewer.transform.localRotation;
        }
        #endregion
    }
}
