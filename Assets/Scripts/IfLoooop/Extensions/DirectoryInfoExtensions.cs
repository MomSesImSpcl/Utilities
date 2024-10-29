using System.IO;
using System.Linq;

namespace IfLoooop.Extensions
{
    /// <summary>
    /// Extensions methods for <see cref="DirectoryInfo"/>.
    /// </summary>
    public static class DirectoryInfoExtensions
    {
        #region Methods
        /// <summary>
        /// Copies all directories and files from this <see cref="DirectoryInfo"/> to the target <see cref="DirectoryInfo"/>.
        /// </summary>
        /// <param name="_SourceDirectory">Directory to copy from.</param>
        /// <param name="_TargetDirectory">Directory to copy to.</param>
        public static void DeepCopy(this DirectoryInfo _SourceDirectory, DirectoryInfo _TargetDirectory)
        {
            Directory.CreateDirectory(_TargetDirectory.FullName);

            // Copy each file into the new directory
            foreach (var _fileInfo in _SourceDirectory.GetFiles())
            {
                _fileInfo.CopyTo(Path.Combine(_TargetDirectory.FullName, _fileInfo.Name), true);
            }

            // Copy each subdirectory using recursion
            foreach (var _childDirectory in _SourceDirectory.GetDirectories())
            {
                _childDirectory.DeepCopy(_TargetDirectory.CreateSubdirectory(_childDirectory.Name));
            }
        }

        /// <summary>
        /// Copies all directories and files from this <see cref="DirectoryInfo"/> to the target <see cref="DirectoryInfo"/>, excluding specified paths.
        /// </summary>
        /// <param name="_SourceDirectory">Directory to copy from.</param>
        /// <param name="_TargetDirectory">Directory to copy to.</param>
        /// <param name="_ExcludePaths">Array of file and directory paths to exclude from copying.</param>
        public static void DeepCopy(this DirectoryInfo _SourceDirectory, DirectoryInfo _TargetDirectory, params string[] _ExcludePaths)
        {
            Directory.CreateDirectory(_TargetDirectory.FullName);

            // Copy each file into the new directory
            foreach (var _fileInfo in _SourceDirectory.GetFiles())
            {
                if (_ExcludePaths.Contains(_fileInfo.FullName))
                {
                    continue;
                }
                
                _fileInfo.CopyTo(Path.Combine(_TargetDirectory.FullName, _fileInfo.Name), true);
            }

            // Copy each subdirectory using recursion
            foreach (var _childDirectory in _SourceDirectory.GetDirectories())
            {
                if (_ExcludePaths.Contains(_childDirectory.FullName))
                {
                    continue;
                }
                
                _childDirectory.DeepCopy(_TargetDirectory.CreateSubdirectory(_childDirectory.Name));
            }
        }
        #endregion
    }
}