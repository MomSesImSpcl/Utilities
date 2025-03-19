using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using MomSesImSpcl.Interfaces;
using MomSesImSpcl.Utilities.Logging;
using UnityEngine;

// ReSharper disable RedundantUsingDirective
using MomSesImSpcl;
using MomSesImSpcl.Utilities;
using Object = UnityEngine.Object;
// ReSharper restore RedundantUsingDirective

#pragma warning disable CS1998

namespace MomSesImSpcl.Utilities
{
    /// <summary>
    /// Helper class for the <see cref="RuntimeCodeExecutor"/> to generate code from a <c>.cs</c>-file instead of from the inspector.
    /// </summary>
    internal sealed class CodeGenerator : MonoBehaviour, ICodeGenerator
    {
#if UNITY_EDITOR && ODIN_INSPECTOR
        #region Inspector Fields
        [Tooltip("Contains fields to dynamically compile and execute code during runtime.")]
        [SerializeField] private RuntimeCodeExecutor runtimeCodeExecutor;
        #endregion
#endif
#pragma warning disable CS0414
        // ReSharper disable once EmptyRegion
        #region Fields
        #endregion
#pragma warning restore CS0414
        
        #region Methods
        /// <summary>
        /// The body of this method will be compiled and invoked in the <see cref="runtimeCodeExecutor"/>.
        /// </summary>
        /// <param name="_Contexts">
        /// Use this to cast the elements to the needed <see cref="Type"/>. <br/>
        /// <i>Use the <c>Get()</c>-extension to get the correct <see cref="Type"/>.</i>.
        /// </param>
        [SuppressMessage("ReSharper", "ArrangeStaticMemberQualifier")]
        // ReSharper disable once UnusedParameter.Local
        // ReSharper disable once AsyncVoidMethod
        private async void Main(Object[] _Contexts)
        {
            
        }
        
        public List<string> GetCode(out List<string> _UsingStatements, out List<string> _Fields)
        {
            var _lineNumber = 1;
            var _beforeNamespace = true;
            var _fields = false;
            var _mainMethodStartLine = 0;
            var _openBrackets = 0;

            _UsingStatements = new List<string>();
            _Fields = new List<string>();
            var _methodBody = new List<string>();
            
            foreach (var _line in File.ReadLines(CallerInfo.GetCallerInfo().FilePath))
            {
                if (_beforeNamespace && _line.Contains("using "))
                {
                    _UsingStatements.Add(_line + Environment.NewLine);
                }
                else if (_line.Contains($"namespace {nameof(MomSesImSpcl)}.{nameof(Utilities)}"))
                {
                    _beforeNamespace = false;
                }
                else if (_line.Contains("#region Fields"))
                {
                    _fields = true;
                }
                else if (_fields)
                {
                    if (_line.Contains("#endregion"))
                    {
                        _fields = false;
                    }
                    else
                    {
                        _Fields.Add(_line + Environment.NewLine);
                    }
                }
                else if (_line.Contains($"void {nameof(Main)}({nameof(Object)}[] _Contexts)"))
                {
                    _mainMethodStartLine = _lineNumber;
                }
                else if (_mainMethodStartLine > 0)
                {
                    _openBrackets += _line.Count(_Char => _Char == '{');
                    _openBrackets -= _line.Count(_Char => _Char == '}');

                    if (_lineNumber > _mainMethodStartLine + 1 && _openBrackets != 0)
                    {
                        _methodBody.Add(_line + Environment.NewLine);
                    }
                    else if (_openBrackets == 0)
                    {
                        return _methodBody;
                    }
                }

                _lineNumber++;
            }

            return _methodBody;
        }
        #endregion
    }
}