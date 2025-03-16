#if UNITY_EDITOR && ODIN_INSPECTOR
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using MomSesImSpcl.Data;
using MomSesImSpcl.Extensions;
using MomSesImSpcl.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MomSesImSpcl.Utilities
{
    /// <summary>
    /// Contains an inspector button to compile and run code from a <see cref="string"/>.
    /// </summary>
    [Serializable, HideLabel]
    public sealed class RuntimeCodeExecutor
    {
        #region Inspector Fields
        [Tooltip("Set this to true, to write the code in .cs file.\nSet this to false to write the code in the inspector.")]
        [HorizontalGroup("Settings")]
        [PropertyOrder(1)][SerializeField] private bool useCodeFromFile;
        [HorizontalGroup("Settings")]
        [PropertyOrder(1)][SerializeField] private bool printGeneratedCode;
        [Tooltip("Reference to the file where the code is written in.")]
        [ShowIf(nameof(this.useCodeFromFile))]
        [PropertyOrder(2)][SerializeField] private InterfaceReference<ICodeGenerator> codeGenerator;
        [Tooltip("Every namespace that is needed to run the code.\nOne namespace per line.")]
        [HideIf(nameof(this.useCodeFromFile))]
        [PropertyOrder(3)][SerializeField][TextArea(1, 10)] private string usingStatements;
        [Tooltip("The code that will be executed.\nTreat this as a method body.")]
        [HideIf(nameof(this.useCodeFromFile))]
        [PropertyOrder(4)][SerializeField][TextArea(1, 50)] private string code;
        [Tooltip("Assign all needed references for the generated code here.")]
        [PropertyOrder(6)][SerializeField][CanBeNull] private UnityEngine.Object[] contexts;
        #endregion
        
        #region Fields
        /// <summary>
        /// Will be <c>true</c> while <see cref="CompileScriptAsync"/> is running.
        /// </summary>
#pragma warning disable CS0414
        private bool isCompiling;
#pragma warning restore CS0414
        /// <summary>
        /// Builds the contents of the generated code.
        /// </summary>
        private StringBuilder stringBuilder = new();
        /// <summary>
        /// Contains all assemblies loaded in the current application domain.
        /// </summary>
        private PortableExecutableReference[] references;
        #endregion
        
        #region Methods
        [PropertyOrder(5)][Button(ButtonStyle.FoldoutButton), DisableIf(nameof(isCompiling))] // ReSharper disable once AsyncVoidMethod
        public async void RunCode()
        {
            const string _ASSEMBLY_NAME = "MomSesImSpcl_Dynamic";
            const string _NAMESPCE_NAME = "MomSesImSpcl.Utilities";
            const string _CLASS_NAME = "RuntimeCodeExecutor";
            const string _METHOD_NAME = "Main";

            this.isCompiling = true;

            try
            {
                if (await this.CompileScriptAsync(_ASSEMBLY_NAME, _NAMESPCE_NAME, _CLASS_NAME, _METHOD_NAME) is { } _assembly)
                {
                    var _type = _assembly.GetType($"{_NAMESPCE_NAME}.{_CLASS_NAME}", true, false);
                    var _method = _type.GetMethod(_METHOD_NAME, BindingFlags.Public | BindingFlags.Instance);
                    var _instance = Activator.CreateInstance(_type);

                    _method!.Invoke(_instance, this.contexts == null ? Array.Empty<object>() : new object[] { this.contexts });
                }
            }
            catch (Exception _exception)
            {
                Debug.LogException(_exception);
            }
            finally
            {
                this.isCompiling = false;   
            }
        }
        
        /// <summary>
        /// Creates a new assembly and compiles the script.
        /// </summary>
        /// <param name="_AssemblyName">The name of the dynamically created assembly.</param>
        /// <param name="_NamespaceName">The name of the namespace in the created assembly.</param>
        /// <param name="_ClassName">The name of the class inside the namespace.</param>
        /// <param name="_MethodName">The name of the method that will be invoked.</param>
        /// <returns>The compiled <see cref="Assembly"/>.</returns>
        [ItemCanBeNull]
        private async Task<Assembly> CompileScriptAsync(string _AssemblyName, string _NamespaceName, string _ClassName, string _MethodName)
        {
            try
            {
                using var _memoryStream = new MemoryStream();
                
                await Task.Run(() =>
                {
                    const int _METHOD_BODY_PADDING = 12;
                    const int _FIELD_PADDING = 8;
                    
                    // ReSharper disable RedundantAssignment
                    var _methodBody = string.Empty;
                    var _fields = string.Empty;
                    var _usingStatements = string.Empty;
                    // ReSharper restore RedundantAssignment
                    
                    if (this.useCodeFromFile)
                    {
                        if (this.codeGenerator == null)
                        {
                            throw new NullReferenceException($"{nameof(this.codeGenerator).Bold()} must be assigned when {nameof(this.useCodeFromFile).Bold()} is set to true.");
                        }

                        this.codeGenerator.Interface.GetCode(out var _usingStatementList, out var _fieldsList).ForEach(_Line =>
                        {
                            this.stringBuilder.Append(string.Empty.PadLeft(_METHOD_BODY_PADDING));
                            this.stringBuilder.Append(_Line.TrimStart(_METHOD_BODY_PADDING).RemoveLast(Environment.NewLine));
                            this.stringBuilder.Append(Environment.NewLine);
                        });
                        
                        _methodBody = this.stringBuilder.GetAndClear().RemoveLast(Environment.NewLine);
                        
                        _fieldsList.ForEach(_Field =>
                        {
                            this.stringBuilder.Append(string.Empty.PadLeft(_FIELD_PADDING));
                            this.stringBuilder.Append(_Field.RemoveLast(Environment.NewLine));
                            this.stringBuilder.Append(Environment.NewLine);
                        });

                        _fields = this.stringBuilder.GetAndClear();
                        
                        _usingStatementList.ForEach(_UsingStatement =>
                        {
                            this.stringBuilder.Append(_UsingStatement.RemoveLast(Environment.NewLine));
                            this.stringBuilder.Append(Environment.NewLine);
                        });
                        
                        _usingStatements = this.stringBuilder.GetAndClear();
                    }
                    else
                    {
                        foreach (var _line in this.code.Split('\n'))
                        {
                            this.stringBuilder.Append(string.Empty.PadLeft(_METHOD_BODY_PADDING));
                            this.stringBuilder.Append(_line.TrimStart(_METHOD_BODY_PADDING).RemoveLast(Environment.NewLine));
                            this.stringBuilder.Append(Environment.NewLine);
                        }
                        
                        _methodBody = this.stringBuilder.GetAndClear().RemoveLast(Environment.NewLine);
                        
                        foreach (var _usingStatement in this.usingStatements.Split('\n'))
                        {
                            this.stringBuilder.Append(_usingStatement.RemoveLast(Environment.NewLine));
                            this.stringBuilder.Append(Environment.NewLine);
                        }
                        
                        _usingStatements = this.stringBuilder.GetAndClear();

                        if (string.IsNullOrWhiteSpace(this.usingStatements))
                        {
                            _usingStatements = _usingStatements.RemoveLast(Environment.NewLine);
                        }
                    }
                    
                    // ReSharper disable RedundantStringInterpolation
                    var _code = $"using {nameof(UnityEngine)};"                                            + Environment.NewLine +
                                      $"{(_usingStatements != string.Empty ? _usingStatements : string.Empty)}"  + Environment.NewLine +
                                      $"namespace {_NamespaceName}"                                              + Environment.NewLine +
                                      $"{{"                                                                      + Environment.NewLine +
                                      $"    public class {_ClassName}"                                           + Environment.NewLine +
                                      $"    {{"                                                                  + Environment.NewLine +
                                             $"{_fields}"                                                        + Environment.NewLine +
                                      $"        public async void {_MethodName}(UnityEngine.Object[] _Contexts)" + Environment.NewLine +
                                      $"        {{"                                                              + Environment.NewLine +
                                                 $"{_methodBody}"                                                + Environment.NewLine +
                                      $"        }}"                                                              + Environment.NewLine +
                                      $"    }}"                                                                  + Environment.NewLine +
                                      $"}}";
                    // ReSharper restore RedundantStringInterpolation

                    this.references ??= AppDomain.CurrentDomain.GetAssemblies()
                                        .Where(_Assembly => !_Assembly.IsDynamic && !string.IsNullOrEmpty(_Assembly.Location))
                                        .Select(_Assembly => MetadataReference.CreateFromFile(_Assembly.Location)).ToArray();
                    
                    if (this.printGeneratedCode)
                    {
                        Debug.Log(_code);
                    }
                    
                    var _cSharpCompilation = CSharpCompilation.Create(_AssemblyName, new[] { CSharpSyntaxTree.ParseText(_code) }, this.references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
                    
                    if (_cSharpCompilation.Emit(_memoryStream) is var _result && !_result.Success)
                    {
                        foreach (var _diagnostic in _result.Diagnostics.Where(_Diagnostic => _Diagnostic.IsWarningAsError || _Diagnostic.Severity == DiagnosticSeverity.Error))
                        {
                            var _error = string.Empty;
                            
                            if (_diagnostic.Location.SourceTree is {} _syntaxTree)
                            {
                                var _lineSpan = _diagnostic.Location.GetLineSpan();
                                var _startLinePosition = _lineSpan.StartLinePosition.Line;
                                var _endLinePosition = _lineSpan.EndLinePosition.Line;

                                var _errorLines = _syntaxTree.GetText().Lines.Skip(_startLinePosition).Take(_endLinePosition - _startLinePosition + 1);
                                var _line = string.Join(Environment.NewLine, _errorLines.Select(_Line => _Line.ToString()));
                                var _lineNumbers = _endLinePosition > _startLinePosition ? $"s [{(_startLinePosition + "-" + _endLinePosition).Bold()}]" : $" [{_startLinePosition.Bold()}]";

                                _error = $"Error in line{_lineNumbers}: {_line.TrimStart().Italic()}{Environment.NewLine}";
                            }
                                
                            Debug.LogError($"{_error}Diagnostic Id [{_diagnostic.Id.Bold()}]: {_diagnostic.GetMessage()}");
                        }
                            
                        return null;
                    }
                        
                    _memoryStream.Seek(0, SeekOrigin.Begin);

                    return Task.CompletedTask;
                });
                
                return Assembly.Load(_memoryStream.ToArray());
            }
            catch (TaskCanceledException){ /* Errors will be printed through the Debug.LogError's above, no need to log the TaskCanceledException. */ }
            catch (Exception _exception)
            {
                Debug.LogException(_exception);
            }
            finally
            {
                this.isCompiling = false;
            }

            return null;
        }
        #endregion
    }
}
#endif
