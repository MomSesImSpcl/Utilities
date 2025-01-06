using System.Collections.Generic;
using System.IO;
using System.Linq;
using MomSesImSpcl.Extensions;
using UnityEditor;
using UnityEngine;

namespace MomSesImSpcl
{
    /// <summary>
    /// Handles the export of assets into a unitypackage file.
    /// </summary>
    internal sealed class PackageExporter : MonoBehaviour
    {
        #region Inspector Fields
        [Tooltip("Version.txt file that contains the current version of the Utilities package.")]
        [SerializeField] private TextAsset versionFile;
        [Tooltip("A collection of assets to be exported with the package.")]
        [SerializeField] private List<Object> assets = new();
        [Tooltip("Won't check if the version has changed, when exporting the plugin.")]
        [SerializeField] private bool skipVersionCheck;
        #endregion
        
        #region Methods
        /// <summary>
        /// Exports the specified assets into a .unitypackage file.
        /// The export process is initiated by opening a file save dialog where the user specifies the destination path.
        /// If the user cancels the dialog or no valid path is provided, the export process will be aborted.
        /// </summary>
        internal async void ExportPackageAsync()
        {
            var _exportPath = EditorUtility.SaveFilePanel("Export Package", "Downloads", "Utilities", "unitypackage");

            if (string.IsNullOrWhiteSpace(_exportPath))
            {
                return;
            }

            var _versionFilePath = AssetDatabase.GetAssetPath(this.versionFile);

            if (File.Exists(_versionFilePath))
            {
                var _version = await File.ReadAllTextAsync(_versionFilePath);

                if (_version == PlayerSettings.bundleVersion && !this.skipVersionCheck)
                {
                    Debug.LogWarning($"Version has not changed [{PlayerSettings.bundleVersion}]. If you still want to export the package, check {nameof(this.skipVersionCheck).Italic()} in the inspector.");
                    return;
                }
                
                await File.WriteAllTextAsync(_versionFilePath, PlayerSettings.bundleVersion);
            }
            else
            {
                throw new FileNotFoundException($"Could not find: {_versionFilePath}");
            }
            
            var _assetPathNames = (from _asset in this.assets where _asset != null select AssetDatabase.GetAssetPath(_asset)).ToArray();

            AssetDatabase.ExportPackage(_assetPathNames, _exportPath, ExportPackageOptions.Recurse);

            this.skipVersionCheck = false;
        }
        #endregion
    }   
}