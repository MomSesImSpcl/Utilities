using System;
using System.IO;
using System.Runtime.CompilerServices;
using CustomScriptTemplates.Extensions;
using UnityEditor;
using UnityEngine;

namespace CustomScriptTemplates.Import
{
    /// <summary>
    /// Creates the <see cref="ScriptTemplatesSettings"/> <c>.asset</c> file and copies all default templates to the template folder.
    /// </summary>
    [InitializeOnLoad]
    internal static class PackageImporter
    {
        #region Properties
        /// <summary>
        /// Returns all assets of <see cref="Type"/> <see cref="ScriptTemplatesSettings"/> under <see cref="ScriptTemplatesSettings.RelativePluginRootFolderPath"/>.
        /// </summary>
        internal static string[] ScriptTemplatesSettingsAssets => AssetDatabase.FindAssets($"t:{typeof(ScriptTemplatesSettings)}", new[] { ScriptTemplatesSettings.RelativePluginRootFolderPath });
        #endregion
        
        #region Constructors
        /// <summary>
        /// <see cref="PackageImporter"/>.
        /// </summary>
        static PackageImporter()
        {
            AssetDatabase.importPackageCompleted += OnImportPackageCompleted;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Is called after a Package has successfully been imported.
        /// </summary>
        /// <param name="_PackageName">The name of the imported Package.</param>
        private static void OnImportPackageCompleted(string _PackageName)
        {
            if (_PackageName != ScriptTemplatesSettings.PluginRootFolder)
            {
                return;
            }
            
            CreateSettingsAsset();
        }

        /// <summary>
        /// Creates the <see cref="ScriptTemplatesSettings"/>.asset file and copies all default templates to the template folder. <br/>
        /// <i>Only does this when no <see cref="ScriptTemplatesSettings"/>.asset file exists.</i>
        /// </summary>
        private static void CreateSettingsAsset()
        {
            if (ScriptTemplatesSettingsAssets.Length == 0)
            {
                var _scriptTemplateSettings = ScriptableObject.CreateInstance<ScriptTemplatesSettings>();

                if (!Directory.Exists(ScriptTemplatesSettings.TemplatesFolderPath))
                {
                    Directory.CreateDirectory(ScriptTemplatesSettings.TemplatesFolderPath);
                }
                
                CopyDefaultTemplates(_scriptTemplateSettings);
                
                AssetDatabase.CreateAsset(_scriptTemplateSettings, Path.Combine(ScriptTemplatesSettings.RelativePluginRootFolderPath, $"{ScriptTemplatesSettings.ASSET_NAME}.asset"));
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                ScriptTemplatesSettings.CompileMenuItems(true);
            }
        }

        /// <summary>
        /// Copies all default templates into the template folder.
        /// </summary>
        /// <param name="_ScriptTemplateSettings">The created <see cref="ScriptTemplatesSettings"/> <c>.asset</c>.</param>
        private static void CopyDefaultTemplates(ScriptTemplatesSettings _ScriptTemplateSettings)
        {
            // ReSharper disable once PossibleNullReferenceException
            var _importDirectory = Directory.GetParent(PackageImporterFilePath()).FullName;
            var _defaultTemplates = Directory.EnumerateFiles(Path.Combine(_importDirectory, "DefaultTemplates"), "*.txt", SearchOption.TopDirectoryOnly);
                
            foreach (var _defaultTemplateFilePath in _defaultTemplates)
            {
                var _templateFileName = Path.GetFileName(_defaultTemplateFilePath);
                var _templateFilePath = Path.Combine(ScriptTemplatesSettings.TemplatesFolderPath, _templateFileName);
                var _templateContent = File.ReadAllText(_defaultTemplateFilePath);
                    
                File.WriteAllText(_templateFilePath, _templateContent);
                    
                var _relativeTemplateFilePath = _templateFilePath.RemovePath(ScriptTemplatesSettings.RootParentFolderPath);
                    
                AssetDatabase.ImportAsset(_relativeTemplateFilePath);
                    
                var _loadedTextAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(_relativeTemplateFilePath);
                    
                if (_loadedTextAsset != null)
                {
                    _ScriptTemplateSettings.Templates.Add(_loadedTextAsset);
                }
                else
                {
                    Debug.LogWarning($"Failed to load the TextAsset at path: {_relativeTemplateFilePath}");
                }
            }
        }
        
        /// <summary>
        /// Returns the absolute filepath to the <see cref="PackageImporter"/> <c>.cs</c> file.
        /// </summary>
        /// <param name="_FilePath">
        /// Will be the path to the <see cref="PackageImporter"/> <c>.cs</c> file. <br/>
        /// <i>Leave empty.</i>
        /// </param>
        /// <returns>The absolute filepath to the <see cref="PackageImporter"/> <c>.cs</c> file.</returns>
        private static string PackageImporterFilePath([CallerFilePath] string _FilePath = "") => _FilePath;
        #endregion
    }
}