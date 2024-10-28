using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CustomScriptTemplates.Extensions;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CustomScriptTemplates
{
	/// <summary>
	/// Contains methods to create custom files from templates.
	/// </summary>
	internal static class ScriptTemplates
	{
		#region Fields
		/// <summary>
		/// Icon for <c>.cs</c> files.
		/// </summary>
		private static readonly Texture2D csIcon = (Texture2D)EditorGUIUtility.IconContent("cs Script Icon").image;
		/// <summary>
		/// Icon for text asset files.
		/// </summary>
		private static readonly Texture2D textIcon = (Texture2D)EditorGUIUtility.IconContent("TextAsset Icon").image;
		#endregion
		
		#region Methods
		/// <summary>
		/// Creates a new <c>.cs</c> file from a template <c>.txt</c> file.
		/// </summary>
		/// <param name="_FileNameWithExtension">
		/// The name, the created <c>.cs</c> file will have. <br/>
		/// <b>Must include the file extension.</b>
		/// </param>
		/// <param name="_PathToTemplate">Relative file path (from the <see cref="ScriptTemplatesSettings.RootFolder"/>) to the template <c>.txt</c> file.</param>
		internal static void CreateCSharpScript(string _FileNameWithExtension, string _PathToTemplate)
		{
			ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<CSharpFile>(), _FileNameWithExtension, csIcon, _PathToTemplate);
		}
		
		/// <summary>
		/// Creates a new .txt File
		/// </summary>
		/// <param name="_FileNameWithExtension">
		/// The name, the created file will have. <br/>
		/// <b>Must include the desired file extension.</b>
		/// </param>
		internal static void CreateTextFile(string _FileNameWithExtension)
		{
			ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<TextFile>(), _FileNameWithExtension, textIcon, string.Empty);
		}
		
		/// <summary>
		/// Creates a new <c>.cs</c> file after the name for the file was chosen.
		/// </summary>
		private sealed class CSharpFile : EndNameEditAction
		{
			#region Methods
			public override void Action(int _InstanceId, string _RelativeFilePath, string _PathToTemplate)
			{
				var _csFile = CreateScript(_RelativeFilePath, _PathToTemplate);

				if (_csFile != null)
				{
					ProjectWindowUtil.ShowCreatedAsset(_csFile);	
				}
				else
				{
					throw new FileNotFoundException($"Could not create the .cs file, the template couldn't be found at:{Environment.NewLine}{_PathToTemplate.ToHyperlink()}");
				}
			}
			#endregion
		}
		
		/// <summary>
		/// Creates a new text file after the name for the file was chosen. <br/>
		/// <i>The file extension must not necessarily be <c>.txt</c>, it can be anything.</i>
		/// </summary>
		private class TextFile : EndNameEditAction
		{
			#region Methods
			public override void Action (int _InstanceId, string _RelativeFilePath, string _PathToTemplate)
			{
				ProjectWindowUtil.ShowCreatedAsset(CreateFile(_RelativeFilePath));
			}
			#endregion
		}
		
		/// <summary>
		/// Creates a new <c>.cs</c> file at the given <c>_RelativeFilePath</c>.
		/// </summary>
		/// <param name="_RelativeFilePath">Relative path (from the <see cref="ScriptTemplatesSettings.RootFolder"/>) where the file will be created.</param>
		/// <param name="_PathToTemplate">Relative file path (from the <see cref="ScriptTemplatesSettings.RootFolder"/>) to the template <c>.txt</c> file.</param>
		/// <returns>The created <c>.cs</c> file as an <see cref="Object"/>.</returns>
		[CanBeNull]
		private static Object CreateScript(string _RelativeFilePath, string _PathToTemplate)
		{
			if (File.Exists(_PathToTemplate))
			{
				var _absoluteFilePath = Path.GetFullPath(_RelativeFilePath);
				var _templateContent = File.ReadAllLines(_PathToTemplate);
				var _fileContent = WriteFile(_absoluteFilePath, _templateContent);

				File.WriteAllText(_absoluteFilePath, _fileContent);
				AssetDatabase.ImportAsset(_RelativeFilePath);
				
				return AssetDatabase.LoadAssetAtPath<Object>(_RelativeFilePath);
			}
			
			return null;
		}
		
		/// <summary>
		/// Creates a new file at the given <c>_RelativeFilePath</c>.
		/// </summary>
		/// <param name="_RelativeFilePath">Relative path (from the <see cref="ScriptTemplatesSettings.RootFolder"/>) where the file will be created.</param>
		/// <returns>The created file as an <see cref="Object"/>.</returns>
		private static Object CreateFile(string _RelativeFilePath)
		{
			File.WriteAllText(Path.GetFullPath(_RelativeFilePath), string.Empty);
			AssetDatabase.ImportAsset(_RelativeFilePath);
				
			return AssetDatabase.LoadAssetAtPath(_RelativeFilePath, typeof(TextAsset));
		}
		
		/// <summary>
		/// Writes the contents to the <c>.cs</c> file from the given <c>_TemplateContent</c>.
		/// </summary>
		/// <param name="_AbsoluteFilePath">Absolute path to the file that will be created.</param>
		/// <param name="_TemplateContent">The contents of the template file to copy.</param>
		/// <returns>The contents of the created <c>.cs</c> file.</returns>
		private static string WriteFile(string _AbsoluteFilePath, string[] _TemplateContent)
		{
			const string _NAMESPACE = "#NAMESPACE#";
			const string _SCRIPT_NAME = "#SCRIPTNAME#";
			
			var _namespace = GetNamespace(_AbsoluteFilePath);
			var _stringBuilder = new StringBuilder();
			var _padding = 0;
			
			foreach (var _line in _TemplateContent)
			{
				if (_line.Contains(_NAMESPACE))
				{
					if (_namespace != string.Empty)
					{
						_stringBuilder.Append("namespace ");
						_stringBuilder.Append(_namespace);
						_stringBuilder.Append(Environment.NewLine);
						_stringBuilder.Append('{');
						_stringBuilder.Append(Environment.NewLine);
						_padding = ScriptTemplatesSettings.IndentAmount;
					}
				}
				else
				{
					_stringBuilder.Append(string.Empty.PadLeft(_padding));

					if (_line.Contains(_SCRIPT_NAME))
					{
						var _className = Path.GetFileNameWithoutExtension(_AbsoluteFilePath).ReplaceWhitespaces();
						_stringBuilder.Append(_line.Replace(_SCRIPT_NAME, _className));
					}
					else
					{
						_stringBuilder.Append(_line);
					}

					_stringBuilder.Append(Environment.NewLine);
				}
			}

			if (_namespace != string.Empty)
			{
				_stringBuilder.Append('}');
			}

			return _stringBuilder.ToString();
		}

		/// <summary>
		/// Gets the namespace for the created file based on its location in the project.
		/// </summary>
		/// <param name="_AbsoluteFilePath">The absolute filepath of the file that is being created.</param>
		/// <returns>The namespace for the created file.</returns>
		private static string GetNamespace(string _AbsoluteFilePath)
		{
			var _folders = new List<string>();
			// ReSharper disable once PossibleNullReferenceException
			var _currentDirectory = Directory.GetParent(_AbsoluteFilePath).FullName;
			var _relativeFilePath = _currentDirectory.RemovePath(ScriptTemplatesSettings.RootParentFolderPath);
			
			do
			{
				_folders.Add(_currentDirectory.RemovePath(ScriptTemplatesSettings.RootParentFolderPath));
				
				var _rootNamespace = GetAssemblyDefinitionNamespace(_currentDirectory, out var _assemblyName);
			
				if (_assemblyName != string.Empty)
				{
					return CreateNamespacePath(_assemblyName, _rootNamespace, _folders);
				}
				
				// ReSharper disable once PossibleNullReferenceException
				_currentDirectory = Directory.GetParent(_currentDirectory).FullName;
				
			} while (_currentDirectory != ScriptTemplatesSettings.RootParentFolderPath && !string.IsNullOrWhiteSpace(_currentDirectory));

			var _assembly = GetAssembly(_relativeFilePath);
			
			return CreateNamespacePath(_assembly, EditorSettings.projectGenerationRootNamespace, _folders);
		}
		
		/// <summary>
		/// Searches for an <c>.asmdef</c> or <c>.asmref</c> file under the given <c>_AbsoluteFolderPath</c> and returns the namespace defined in it. <br/>
		/// <i>Only works with unity versions lower than <c>2020.2</c>.</i>
		/// </summary>
		/// <param name="_AbsoluteFolderPath">Absolute path to the folder to search in.</param>
		/// <param name="_AssemblyName">
		/// Will be set to the name, defined in the Assembly Definition file. <br/>
		/// <i>Not to be confused with the filename, the name defined in the Assembly Definition file can differ from the name of the <c>.asmdef</c> file.</i>
		/// </param>
		/// <returns>The namespace defined in the Assembly Definition file, or an <see cref="string.Empty"/> <see cref="string"/> if nothing could be found.</returns>
		// ReSharper disable once UnusedMember.Local
		private static string GetAssemblyDefinitionNamespace(string _AbsoluteFolderPath, out string _AssemblyName)
		{
			_AssemblyName = string.Empty;
			
			foreach (var _filePath in Directory.EnumerateFiles(_AbsoluteFolderPath, "*.asm*", SearchOption.TopDirectoryOnly))
			{
				if (_filePath.EndsWith(".asmdef"))
				{
					return GetNamespaceFromAssemblyDefinition(_filePath, out _AssemblyName);
				}
				if (_filePath.EndsWith(".asmref"))
				{
					var _guid = Regex.Match(File.ReadAllText(_filePath), "\"reference\": \".+\"").Value.ExtractBetween("\"", "\"", true);

					// When no reference is set.
					if (string.IsNullOrWhiteSpace(_guid))
					{
						return string.Empty;
					}
					
					var _assemblyDefinitionFilePath = AssetDatabase.GUIDToAssetPath(_guid);

					if (_assemblyDefinitionFilePath != string.Empty)
					{
						return GetNamespaceFromAssemblyDefinition(_assemblyDefinitionFilePath, out _AssemblyName);
					}
				}
			}
			
			return string.Empty;
		}
		
		/// <summary>
		/// Returns the namespace defined in the Assembly Definitions file at the given <c>_FilePath</c>. <br/>
		/// <i>Only works with unity versions lower than <c>2020.2</c>.</i>
		/// </summary>
		/// <param name="_FilePath">Must be a filepath to an Assembly Definition file.</param>
		/// <param name="_AssemblyName">
		/// Will be set to the name, defined in the Assembly Definition file. <br/>
		/// <i>Not to be confused with the filename, the name defined in the Assembly Definition file can differ from the name of the <c>.asmdef</c> file.</i>
		/// </param>
		/// <returns>The namespace defined in the Assembly Definitions file at the given <c>_FilePath</c>, or an <see cref="string.Empty"/> <see cref="string"/>.</returns>
		private static string GetNamespaceFromAssemblyDefinition(string _FilePath, out string _AssemblyName)
		{
			// ReSharper disable once RedundantAssignment
			var _namespace = string.Empty;
			var _assemblyDefinitionContent = File.ReadAllText(_FilePath);
			
			_AssemblyName = Regex.Match(_assemblyDefinitionContent, "\"name\": \".+\"").Value.ExtractBetween("\"", "\"", true);
			_namespace = Regex.Match(_assemblyDefinitionContent, "\"rootNamespace\": \".+\"").Value.ExtractBetween("\"", "\"", true);
			
			return string.IsNullOrWhiteSpace(_namespace) ? string.Empty : _namespace;
		}
		
		/// <summary>
		/// Gets the default assembly the created file will be in.
		/// </summary>
		/// <param name="_DirectoryPath">
		/// The folder where the file will be created.</param>
		/// <returns>The assembly name the created file will be in. <br/>
		/// <b>Must be the relative file path from the <see cref="ScriptTemplatesSettings.RootFolder"/>.</b>
		/// </returns>
		private static string GetAssembly(string _DirectoryPath)
		{
			var _firstpass = false;
			var _editorFirstpass = false;
			var _editor = false;
			
			var _directories = _DirectoryPath.Split(Path.DirectorySeparatorChar);

			foreach (var _directory in _directories)
			{
				if (string.Equals(_directory, "Standard Assets", StringComparison.OrdinalIgnoreCase) && !_editor)
				{
					_firstpass = true;
				}
				else if (string.Equals(_directory, "Pro Standard Assets", StringComparison.OrdinalIgnoreCase) && !_editor)
				{
					_firstpass = true;
				}
				else if (string.Equals(_directory, "Plugins", StringComparison.OrdinalIgnoreCase) && !_editor)
				{
					_firstpass = true;
				}
				else if (string.Equals(_directory, "Editor", StringComparison.OrdinalIgnoreCase) && _firstpass)
				{
					_editorFirstpass = true;
				}
				else if (string.Equals(_directory, "Editor", StringComparison.OrdinalIgnoreCase) && !_firstpass)
				{
					_editor = true;
				}
			}

			if (_editorFirstpass) // "_firstpass" and "_editorFirstpass" can both be "true", "_editorFirstpass" must be checked first.
			{
				return "Assembly-CSharp-Editor-firstpass";
			}
			if (_firstpass)
			{
				return "Assembly-CSharp-firstpass";
			}
			if (_editor)
			{
				return "Assembly-CSharp-Editor";
			}

			return "Assembly-CSharp";
		}
		
		/// <summary>
		/// Creates a concatenated namespace from the <c>_RootNamespace</c> + all <c>_Folders</c> that are not excluded as namespace provider.
		/// </summary>
		/// <param name="_AssemblyName">The name of the assembly under which to check for folder exclusions.</param>
		/// <param name="_RootNamespace">
		/// The root namespace of this Unity project, or any Assembly Definition file. <br/>
		/// <i>Can be <see cref="string.Empty"/>.</i>
		/// </param>
		/// <param name="_Folders">
		/// Every folder, starting from the folder where the script is being created, to either the first found Assembly Definition file or the <see cref="ScriptTemplatesSettings.RootFolder"/>. <br/>
		/// <b>The order of the <see cref="List{T}"/> elements must be: <br/> <i><c>Script Folder</c> -> <see cref="ScriptTemplatesSettings.RootFolder"/></i>.</b>
		/// </param>
		/// <returns>A concatenated namespace from the <c>_RootNamespace</c> + all <c>_Folders</c> that are not excluded as namespace provider.</returns>
		private static string CreateNamespacePath(string _AssemblyName, string _RootNamespace, List<string> _Folders)
		{
			var _excludedFolders = GetNamespaceExclusions(_AssemblyName);
			
			_Folders.RemoveAll(_Namespace => _excludedFolders.Contains(_Namespace, StringComparer.OrdinalIgnoreCase));
			_Folders.Reverse();

			// When the namespace is from an Assembly Definition file.
			if (_RootNamespace.IsEmptyOrWhitespace())
			{
				_RootNamespace = EditorSettings.projectGenerationRootNamespace;
			}
			if (!_RootNamespace.IsEmptyOrWhitespace())
			{
				_Folders.Insert(0, _RootNamespace);
			}
			
			return string.Join(".", _Folders.Select(_Folder => Path.GetFileName(_Folder).ReplaceWhitespaces()));
		}
		
		/// <summary>
		/// Returns a <see cref="List{T}"/> of all folders that should not be used as namespace provider. <br/>
		/// <b>Only works when using Rider, or ReSharper is installed.</b>
		/// </summary>
		/// <param name="_AssemblyName">The name of the assembly under which to check for folder exclusions.</param>
		/// <returns>A <see cref="List{T}"/> of relative folder paths (from the <see cref="ScriptTemplatesSettings.RootFolder"/>), or an empty <see cref="List{T}"/> if the given <c>_AssemblyName</c> could not be found or doesn't have any exclusions.</returns>
		private static List<string> GetNamespaceExclusions(string _AssemblyName)
		{
			// These folder paths will not be in the "DotSettings" file when excluded as a namespace provider.
			// They will only be in it when included, but the Boolean value will be "False".
			var _specialFolders = new Dictionary<string, bool>
			{   // Folders must be separated by: _005C
				{ ScriptTemplatesSettings.RootFolder, false },
				{ $"{ScriptTemplatesSettings.RootFolder}_005CScripts", false }
			};
			
			var _namespaceFoldersToSkip = new List<string>();
			var _dotSettingsFilePath = Path.Combine(ScriptTemplatesSettings.RootParentFolderPath, $"{_AssemblyName}.csproj.DotSettings");
			var _hexCodeRegex = new Regex(@"_(\w{4})");
			
			if (File.Exists(_dotSettingsFilePath))
			{
				foreach (var _line in File.ReadAllLines(_dotSettingsFilePath))
				{
					// ReSharper disable once InconsistentNaming
					for (var i = 0; i < _specialFolders.Count; i++)
					{
						var _folder = _specialFolders.ElementAt(i).Key;
						
						if (_line.IndexOf($"{_folder}/@EntryIndexedValue\">False</s:Boolean>", StringComparison.OrdinalIgnoreCase) > -1)
						{
							_specialFolders[_folder] = true;
						}
					}
					
					if (!_line.Contains("@EntryIndexedValue\">True</s:Boolean>"))
					{
						continue;
					}
					
					var _folderPath = _line.ExtractBetween("/=", "/@");

					if (string.IsNullOrWhiteSpace(_folderPath))
					{
						continue;
					}
					
					_namespaceFoldersToSkip.Add(Unescape(_hexCodeRegex, _folderPath));
				}

				_namespaceFoldersToSkip.AddRange(from _keyValuePair in _specialFolders where !_keyValuePair.Value select Unescape(_hexCodeRegex, _keyValuePair.Key));
			}
			else
			{
				_namespaceFoldersToSkip.AddRange(from _keyValuePair in _specialFolders where !_keyValuePair.Value select Unescape(_hexCodeRegex, _keyValuePair.Key));
			}

			return _namespaceFoldersToSkip;

			string Unescape(Regex _HexCodeRegex, string _FolderPath) => _HexCodeRegex.Replace(_FolderPath, _Match =>
			{
				// Extract the hexadecimal part (XXXX).
				var _hexValue = _Match.Groups[1].Value;
				// Convert hex value to an integer.
				var _charCode = Convert.ToInt32(_hexValue, 16);
				// Convert the integer to its corresponding character.
				var _character = (char)_charCode;

				return _character.ToString();
			});
		}
		#endregion
	}
}