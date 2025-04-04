using MomSesImSpcl.Data;
using MomSesImSpcl.Extensions;
using UnityEditor;
using UnityEngine;

namespace MomSesImSpcl.Editor.CustomEditors
{
    /// <summary>
    /// Custom editor for <see cref="PositionViewer"/>.
    /// </summary>
    [CustomEditor(typeof(PositionViewer))]
    public sealed class PositionViewerEditor : 
#if ODIN_INSPECTOR
        Sirenix.OdinInspector.Editor.OdinEditor
#else
        UnityEditor.Editor
#endif
    {
        #region Fields
        /// <summary>
        /// <see cref="PositionViewer"/>.
        /// </summary>
        private PositionViewer positionViewer;
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
            this.positionViewer = base.target.To<PositionViewer>();
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
            
            var _transform = this.positionViewer.transform;
            var _rectTransform = _transform.As<RectTransform>();
            
            this.positionViewer.IsRectTransform = _rectTransform;
            
            this.positionViewer.Position = _transform.position;
            this.positionViewer.Min = _rectTransform?.ToWorld(_RectTransform => _RectTransform.rect.min) ?? Vector2.zero;
            this.positionViewer.Max = _rectTransform?.ToWorld(_RectTransform => _RectTransform.rect.max) ?? Vector2.zero;
            
            this.positionViewer.LocalPosition = _transform.localPosition;
            this.positionViewer.LocalMin = _rectTransform is not null ? _rectTransform.localPosition.ToVector2() + _rectTransform.rect.min : Vector2.zero;
            this.positionViewer.LocalMax = _rectTransform is not null ? _rectTransform.localPosition.ToVector2() + _rectTransform.rect.max : Vector2.zero;
        }
        #endregion
    }
}