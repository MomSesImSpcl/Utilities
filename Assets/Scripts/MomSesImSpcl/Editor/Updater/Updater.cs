using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using MomSesImSpcl.Extensions;
using UnityEditor;
using UnityEngine;

namespace MomSesImSpcl.Editor.Updater
{
    /// <summary>
    /// Checks if an update is available for the Utilities package.
    /// </summary>
    //[CreateAssetMenu(menuName = "Updater", fileName = "Updater")]
    internal sealed class Updater : ScriptableObject
    {
        #region Constants
        /// <summary>
        /// The constant key used to store the updater status in Unity's EditorPrefs.
        /// </summary>
        private const string UPDATER_KEY = "IfLoooop.Editor.Updater";
        #endregion
        
        #region Methods
        /// <summary>
        /// Checks for updates by comparing the current version with the latest version available online.
        /// If an update is available, a log message is displayed in the Unity console.
        /// </summary>
        [InitializeOnLoadMethod]
        private static async void CheckForUpdatesAsync()
        {
            EditorApplication.wantsToQuit += OnEditorQuit;
            
            if (!EditorPrefs.GetBool(UPDATER_KEY))
            {
                EditorPrefs.SetBool(UPDATER_KEY, true);
                
                if (GetVersion(await new HttpClient().GetStringAsync("https://raw.githubusercontent.com/IfLoooop/Utilities/refs/heads/main/Assets/Scripts/IfLoooop/Version.txt"), out var _latestVersion))
                {
                    if (Directory.GetParent(GetFilePath())?.Parent?.Parent is {} _rootFolder)
                    {
                        var _versionFilePath = Path.Combine(_rootFolder.FullName, "Version.txt");
                        
                        if (File.Exists(_versionFilePath) && GetVersion(await File.ReadAllTextAsync(_versionFilePath), out var _currentVersion))
                        {
                            if (_currentVersion < _latestVersion)
                            {
                                Debug.Log($"Update available for the {"Utilities".Italic()} package. Current version: {_currentVersion.ToString("F1").Bold()} | Latest version: {_latestVersion.ToString("F1").Bold()}\n{"https://github.com/IfLoooop/Utilities/releases".ToHyperlink()}");
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Extracts the version number from the given version text.
        /// </summary>
        /// <param name="_VersionText">The version text to parse, e.g., "v1", "v2".</param>
        /// <param name="_Version">A float output parameter that will hold the parsed version number if the parse is successful.</param>
        /// <returns>Returns true if the version number is successfully parsed; otherwise, false.</returns>
        private static bool GetVersion(string _VersionText, out float _Version)
        {
            if (float.TryParse(_VersionText.Replace("v", string.Empty), out _Version))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Retrieves the file path of the source code file that contains the caller.
        /// This method uses the <paramref name="_FilePath"/> which is automatically set by the compiler to the path of the source file.
        /// </summary>
        /// <param name="_FilePath">The default file path provided by the caller. This parameter is automatically supplied by the compiler and should not be manually set.</param>
        /// <returns>Returns the file path as a string.</returns>
        private static string GetFilePath([CallerFilePath] string _FilePath = "") => _FilePath;

        /// <summary>
        /// Sets the <see cref="UPDATER_KEY"/> to false when the Unity editor is closed.
        /// </summary>
        /// <returns>Returns true, indicating that the editor can proceed with quitting.</returns>
        private static bool OnEditorQuit()
        {
            EditorPrefs.SetBool(UPDATER_KEY, false);
            
            return true;
        }
        #endregion
    }
}