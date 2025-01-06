using UnityEditor;
using UnityEngine;

namespace MomSesImSpcl
{
    /// <summary>
    /// Custom editor for the <see cref="PackageExporter"/> class.
    /// </summary>
    [CustomEditor(typeof(PackageExporter))]
    internal sealed class PackageExporterEditor : UnityEditor.Editor
    {
        #region Fields
        /// <summary>
        /// Instance of the <see cref="PackageExporter"/> class that handles the functionality for exporting assets.
        /// </summary>
        private PackageExporter packageExporter;
        #endregion

        #region Methods
        private void OnEnable()
        {
            this.packageExporter = (PackageExporter)target;
        }

        public override void OnInspectorGUI()
        {
            base.DrawDefaultInspector();

            GUILayout.Space(EditorGUIUtility.singleLineHeight);

            if (GUILayout.Button("Export Package"))
            {
                this.packageExporter.ExportPackageAsync();
            }
        }
        #endregion
    }
}