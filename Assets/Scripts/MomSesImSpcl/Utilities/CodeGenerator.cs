using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MomSesImSpcl.Interfaces;
using MomSesImSpcl.Utilities;
using MomSesImSpcl.Utilities.Logging;
using UnityEngine;

// ReSharper disable RedundantUsingDirective
using MomSesImSpcl;
using MomSesImSpcl.Utilities;
// ReSharper restore RedundantUsingDirective

namespace MomSesImSpcl.Utilities
{
    /// <summary>
    /// Helper class for the <see cref="RuntimeCodeExecutor"/> to generate code from a <c>.cs</c>-file instead of from the inspector.
    /// </summary>
    public sealed class CodeGenerator : MonoBehaviour, ICodeGenerator
    {
#if UNITY_EDITOR && ODIN_INSPECTOR
        #region Inspector Fields
        [Tooltip("Contains fields to dynamically compile and execute code during runtime.")]
        [SerializeField] private RuntimeCodeExecutor runtimeCodeExecutor;
        #endregion
#endif
        #region Methods
        /// <summary>
        /// The body of this method will be compiled and invoked in the <see cref="runtimeCodeExecutor"/>.
        /// </summary>
        /// <param name="_Contexts">
        /// Use this to cast the elements to the needed <see cref="Type"/>. <br/>
        /// As long as the correct references are set in <see cref="RuntimeCodeExecutor.contexts"/> the cast will work.
        /// </param>
        // ReSharper disable once UnusedParameter.Local
        private void Main(UnityEngine.Object[] _Contexts)
        {
            
        }
        
        public List<string> GetCode(out List<string> _UsingStatements)
        {
            var _lineNumber = 1;
            var _beforeNamespace = true;
            var _mainMethodStartLine = 0;
            var _openBrackets = 0;

            _UsingStatements = new List<string>();
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
                else if (_line.Contains($"private void {nameof(Main)}"))
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
