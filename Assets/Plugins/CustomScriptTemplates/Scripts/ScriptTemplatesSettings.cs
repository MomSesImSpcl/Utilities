using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using CustomScriptTemplates.Extensions;
using CustomScriptTemplates.Import;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace CustomScriptTemplates
{
	/// <summary>
	/// Contains all settings.
	/// </summary>
	//[CreateAssetMenu(menuName = "Custom Script Templates Settings", fileName = ASSET_NAME)] // Uncomment this if you need to create a new "Settings.asset" file.
	internal class ScriptTemplatesSettings : ScriptableObject
	{
		#region Inspector Fields
		[Tooltip("Number of whitespaces that will be inserted before the class declaration.\n(Only when a namespace is used.)")]
		[SerializeField] private int indentAmount = 4;
		[Tooltip("Enables the creation of .txt files if true.")]
		[SerializeField] private bool enableTextFile = true;
		[Tooltip("Enables the creation of .json files if true.")] // ReSharper disable once RedundantDefaultMemberInitializer
		[SerializeField] private bool enableJsonFile = false;
		[Tooltip("Enables the creation of .xml files if true.")] // ReSharper disable once RedundantDefaultMemberInitializer
		[SerializeField] private bool enableXMLFile = false;
		[Tooltip("Add all templates you want to use to this list.")]
		[SerializeField] private List<TextAsset> templates = new List<TextAsset>();
		#endregion

		#region Constants
		/// <summary>
		/// The name of this <see cref="ScriptTemplatesSettings"/>.asset file.
		/// </summary>
		internal const string ASSET_NAME = "Settings";
		/// <summary>
		/// Region name in <see cref="ScriptTemplatesMenuItems"/> under which all menu item methods are.
		/// </summary>
		private const string REGION_NAME = "MENU_ITEMS";
		/// <summary>
		/// The root menu item for all templates.
		/// </summary>
		internal const string ROOT_ITEM = "Assets/Create/Custom Script Templates/";
		/// <summary>
		/// Name of <see cref="indentAmount"/>.
		/// </summary>
		internal const string INDENT_AMOUNT_NAME = nameof(indentAmount);
		/// <summary>
		/// Name of <see cref="enableTextFile"/>.
		/// </summary>
		internal const string ENABLE_TEXT_FILE_NAME = nameof(enableTextFile);
		/// <summary>
		/// Name of <see cref="enableJsonFile"/>.
		/// </summary>
		internal const string ENABLE_JSON_FILE_NAME = nameof(enableJsonFile);
		/// <summary>
		/// Name of <see cref="enableXMLFile"/>.
		/// </summary>
		internal const string ENABLE_XML_FILE_NAME = nameof(enableXMLFile);
		/// <summary>
		/// Name of <see cref="templates"/>
		/// </summary>
		internal const string TEMPLATES_NAME = nameof(templates);
		#endregion
		
		#region Fields
		/// <summary>
		/// Singleton of <see cref="ScriptTemplatesSettings"/>.
		/// </summary>
		[CanBeNull] private static ScriptTemplatesSettings instance;
		/// <summary>
		/// Will be <c>true</c> when <see cref="CompileMenuItems"/> has been called.
		/// </summary>
		private bool waitingForRecompile;
		#endregion
		
		#region Properties
		/// <summary>
		/// <see cref="instance"/>.
		/// </summary>
		private static ScriptTemplatesSettings Instance
		{
			get
			{
				if (instance != null)
				{
					return instance;
				}

				var _scriptTemplatesSettingsAssetPath = AssetDatabase.GUIDToAssetPath(PackageImporter.ScriptTemplatesSettingsAssets.FirstOrDefault());
				var _scriptTemplatesSettingsAsset = AssetDatabase.LoadAssetAtPath(_scriptTemplatesSettingsAssetPath, typeof(ScriptTemplatesSettings)) as ScriptTemplatesSettings;
				
				return instance = _scriptTemplatesSettingsAsset;
			}
		}
		/// <summary>
		/// Absolute path to the parent folder of <see cref="RootFolder"/>.
		/// </summary>
		// ReSharper disable once PossibleNullReferenceException
		internal static string RootParentFolderPath => Directory.GetParent(Application.dataPath).FullName;
		/// <summary>
		/// The absolute path to the plugins root folder.
		/// </summary>
		[SuppressMessage("ReSharper", "PossibleNullReferenceException")]
		internal static string AbsolutePluginRootFolderPath => Directory.GetParent(ScriptTemplateSettingsFilePath()).Parent.FullName;
		/// <summary>
		/// Relative path to the plugins root folder.
		/// <i>Starts from the <see cref="RootFolder"/>.</i>
		/// </summary>
		internal static string RelativePluginRootFolderPath => AbsolutePluginRootFolderPath.RemovePath(RootParentFolderPath);
		/// <summary>
		/// The absolute path to the folder where the templates are stored.
		/// </summary>
		internal static string TemplatesFolderPath => Path.Combine(AbsolutePluginRootFolderPath, nameof(Templates), nameof(Resources));
		/// <summary>
		/// Name of the root folder in this Unity project. <br/>
		/// <i>Should be the <c>Assets</c> folder.</i>
		/// </summary>
		internal static string RootFolder => Path.GetFileName(Application.dataPath);
		/// <summary>
		/// Name of the root folder of this plugin. <br/>
		/// <i>Should be <c>CustomScriptTemplates</c>.</i>
		/// </summary>
		internal static string PluginRootFolder => Path.GetFileName(AbsolutePluginRootFolderPath);
		/// <summary>
		/// <see cref="indentAmount"/>.
		/// </summary>
		internal static int IndentAmount => Instance.indentAmount;
		/// <summary>
		/// <see cref="templates"/>.
		/// </summary>
		internal List<TextAsset> Templates  => this.templates;
		#endregion
		
		/// <summary>
		/// Prints a message after the menu items have been written, when <see cref="CompileMenuItems"/> has been called.
		/// </summary>
		[DidReloadScripts]
		private static void OnScriptsRecompiled()
		{
			try
			{
				if (!Instance.waitingForRecompile)
				{
					return;
				}
			
				Instance.waitingForRecompile = false;
				Debug.Log("Finished recompiling.".Bold());
			}
			catch (Exception) { /* On package import, "Instance" is null for some reason, even though it shouldn't be. Everything works, but an exception is thrown. */ }
		}
		
		private void OnValidate()
		{
			this.ValidateListEntries();
		}
		
		/// <summary>
		/// Checks if all entries in <see cref="templates"/> are of the right type and removes all invalid entries.
		/// </summary>
		private void ValidateListEntries()
		{
			// ReSharper disable once InconsistentNaming
			for (var i = 0; i < this.templates.Count; i++)
			{
				// ReSharper disable once InconsistentNaming
				for (var j = i + 1; j < this.templates.Count; j++)
				{
					if (this.templates[i] == this.templates[j])
					{
						Debug.LogWarning($"The {this.templates[j].name.Bold()} template has already been added.");
						this.templates[j] = null;
					}
				}

				if (this.templates[i] != null && this.templates[i].GetType() != typeof(TextAsset))
				{
					Debug.LogWarning($"{this.templates[i].name.Bold()} is not a .txt file.");
					this.templates[i] = null;
				}
			}
		}

		/// <summary>
		/// Writes the menu items methods for each entry in <see cref="templates"/> to <see cref="ScriptTemplatesMenuItems"/> and recompiles it.
		/// </summary>
		/// <param name="_IsPackageImport">Set this to <c>true</c> when this method is called during the package import.</param>
		internal static void CompileMenuItems(bool _IsPackageImport = false)
		{
			var _scriptTemplatesMenuItemsAsset = AssetDatabase.FindAssets(nameof(ScriptTemplatesMenuItems), new[] { RootFolder });
			var _scriptTemplatesMenuItemsPath = AssetDatabase.GUIDToAssetPath(_scriptTemplatesMenuItemsAsset[0]);
			var _lines = File.ReadAllLines(_scriptTemplatesMenuItemsPath);
			
			short _regionStart = -1;
			short _regionEnd = -1;
			var _indentAmount = 0;

			var _regionRegex = new Regex($@"\s*#region\s+{REGION_NAME}\s*");
			var _whitespaceRegex = new Regex(@"^\s*");
			
			// ReSharper disable once InconsistentNaming
			for (short i = 0; i < _lines.Length; i++)
			{
				if (!_regionRegex.IsMatch(_lines[i]))
				{
					continue;
				}
				
				_regionStart = i;
				_indentAmount = _whitespaceRegex.Match(_lines[i]).Value.CountWhitespaces();

				// ReSharper disable once InconsistentNaming
				for (var j = _regionStart; j < _lines.Length; j++)
				{
					if (!_lines[j].Contains("#endregion"))
					{
						continue;
					}
							
					_regionEnd = j;
					break;
				}

				break;
			}
			
			if (_regionStart == -1)
			{
				throw new Exception("$\"Could not find the region {REGION_NAME.Bold()} in {nameof(ScriptTemplatesMenuItems).Bold()}.\"");
			}
			
			var _stringBuilder = new StringBuilder();
			
			// ReSharper disable once InconsistentNaming
			for (short i = 0; i <= _regionStart; i++) // Adds everything above the region.
			{
				_stringBuilder.Append($"{_lines[i]}{Environment.NewLine}");
			}
			
			// Adds a method for each template in the list.
			foreach (var _template in Instance.templates.Where(_Template => _Template != null))
			{
				Instance.CreateMenuItem(_stringBuilder, _template.name, ".cs", _indentAmount, _IsPackageImport);
			}
			
			Instance.AddOptionalFiles(_stringBuilder, _indentAmount, _IsPackageImport);
			
			// ReSharper disable once InconsistentNaming
			for (var i = _regionEnd; i < _lines.Length; i++) // Adds everything below the region.
			{
				_stringBuilder.Append($"{_lines[i]}{Environment.NewLine}");
			}
			
			Instance.waitingForRecompile = true;
			Debug.Log("Writing the code, please wait for Unity to finish recompiling.".Bold());
			File.WriteAllText(_scriptTemplatesMenuItemsPath, _stringBuilder.ToString().Trim());
			AssetDatabase.ImportAsset(_scriptTemplatesMenuItemsPath);
		}
		
		/// <summary>
		/// Writes the method for <see cref="ScriptTemplatesMenuItems"/>.
		/// </summary>
		/// <param name="_StringBuilder">Should be the <see cref="StringBuilder"/> that writes the content for the whole file.</param>
		/// <param name="_TemplateName">The name of the Template <c>.txt</c> file.</param>
		/// <param name="_FileExtension">The file extension the created file will have.</param>
		/// <param name="_IndentAmount">Padding between the start of the region and the border.</param>
		/// <param name="_IsPackageImport">Set this to <c>true</c> when this method is called during the package import.</param>
		/// <param name="_CreateMethod">
		/// The name of the method that is used to create the file. <br/>
		/// <i>Can be <see cref="ScriptTemplates.CreateCSharpScript"/> or <see cref="ScriptTemplates.CreateTextFile"/>.</i>
		/// </param>
		// ReSharper disable once UnusedParameter.Local
		private void CreateMenuItem(StringBuilder _StringBuilder, string _TemplateName, string _FileExtension, int _IndentAmount, bool _IsPackageImport, string _CreateMethod = nameof(ScriptTemplates.CreateCSharpScript))
		{
			var _templatePath = string.Empty;
			var _templateName = _TemplateName.RemoveAllWhitespaces();
			
#if CUSTOM_SCRIPT_TEMPLATES
			_templateName = _templateName.Replace("Template", string.Empty);
#else
			if (_IsPackageImport)
			{
				_templateName = _templateName.Replace("Template", string.Empty);
			}
#endif
			if (_CreateMethod == nameof(ScriptTemplates.CreateCSharpScript))
			{
				_templatePath = ", $\"{" + $"{nameof(ScriptTemplatesSettings)}.{nameof(TemplatesFolderPath)}" + "}/" + $"{_TemplateName}.txt\"";
			}
			
			var _summaryStart      = $"/// <summary>{Environment.NewLine}";
			var _summary           = $"/// Creates a new {_templateName}.{Environment.NewLine}";
			var _summaryEnd        = $"/// </summary>{Environment.NewLine}";
			var _attribute         = $"[{nameof(MenuItem)}({nameof(ScriptTemplatesSettings)}.{nameof(ROOT_ITEM)} + \"{_templateName}\")]{Environment.NewLine}";
			var _methodDeclaration = $"private static {typeof(void).Name.ToLower()} Create{_templateName}(){Environment.NewLine}";
			var _bodyStart    =  "{" + Environment.NewLine;
			var _body              = $"{nameof(ScriptTemplates)}.{_CreateMethod}(\"New{_templateName}{_FileExtension}\"{_templatePath});{Environment.NewLine}";
			var _bodyEnd      =  "}" + Environment.NewLine;
			
			_StringBuilder.Append($"{string.Empty.PadLeft(_IndentAmount)}{_summaryStart}");
			_StringBuilder.Append($"{string.Empty.PadLeft(_IndentAmount)}{_summary}");
			_StringBuilder.Append($"{string.Empty.PadLeft(_IndentAmount)}{_summaryEnd}");
			_StringBuilder.Append($"{string.Empty.PadLeft(_IndentAmount)}{_attribute}");
			_StringBuilder.Append($"{string.Empty.PadLeft(_IndentAmount)}{_methodDeclaration}");
			_StringBuilder.Append($"{string.Empty.PadLeft(_IndentAmount)}{_bodyStart}");
			_StringBuilder.Append($"{string.Empty.PadLeft(_IndentAmount + 4)}{_body}");
			_StringBuilder.Append($"{string.Empty.PadLeft(_IndentAmount)}{_bodyEnd}");
		}

		/// <summary>
		/// Writes the method for the optional files, if there are enabled in the inspector.
		/// </summary>
		/// <param name="_StringBuilder">Should be the <see cref="StringBuilder"/> that writes the content for the whole file.</param>
		/// <param name="_IndentAmount">Padding between the start of the region and the border.</param>
		/// <param name="_IsPackageImport">Set this to <c>true</c> when this method is called during the package import.</param>
		private void AddOptionalFiles(StringBuilder _StringBuilder, int _IndentAmount, bool _IsPackageImport)
		{
			if (this.enableTextFile)
			{
				this.CreateMenuItem(_StringBuilder, "Text", ".txt", _IndentAmount, _IsPackageImport, nameof(ScriptTemplates.CreateTextFile));
			}
			if (this.enableJsonFile)
			{
				this.CreateMenuItem(_StringBuilder, "Json", ".json", _IndentAmount, _IsPackageImport, nameof(ScriptTemplates.CreateTextFile));
			}
			if (this.enableXMLFile)
			{
				this.CreateMenuItem(_StringBuilder, "XML", ".xml", _IndentAmount, _IsPackageImport, nameof(ScriptTemplates.CreateTextFile));
			}
		}
		
		/// <summary>
		/// Returns the absolute filepath to the <see cref="ScriptTemplatesSettings"/> <c>.cs</c> file.
		/// </summary>
		/// <param name="_FilePath">
		/// Will be the path to the <see cref="ScriptTemplatesSettings"/> <c>.cs</c> file. <br/>
		/// <i>Leave empty.</i>
		/// </param>
		/// <returns>The absolute filepath to the <see cref="ScriptTemplatesSettings"/> <c>.cs</c> file.</returns>
		private static string ScriptTemplateSettingsFilePath([CallerFilePath] string _FilePath = "") => _FilePath;
	}
}