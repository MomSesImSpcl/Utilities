using System;
using System.IO;
using System.Linq;
using System.Text;
using DG.Tweening;
using MomSesImSpcl.Extensions;
using MomSesImSpcl.Interfaces;
using MomSesImSpcl.Utilities;
using MomSesImSpcl.Utilities.Logging;
using RogueDeck.Cards;
using RogueDeck.Cards.Effects;
using RogueDeck.Settings;
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
        #region Fields
        /// <summary>
        /// The <see cref="StringBuilder"/> that builds the <see cref="string"/>s for the code and the using statements.
        /// </summary>
        private readonly StringBuilder stringBuilder = new();
        #endregion
        
        #region Methods
        /// <summary>
        /// The body of this methods will be compiled and invoked in the <see cref="runtimeCodeExecutor"/>.
        /// </summary>
        /// <param name="_Contexts">
        /// Use this to cast the elements to the needed <see cref="Type"/>. <br/>
        /// As long as the correct references are set in <see cref="RuntimeCodeExecutor.contexts"/> the cast will work.
        /// </param>
        private void Main(UnityEngine.Object[] _Contexts)
        {
            
        }
        
        public string GetCode(out string _UsingStatements)
        {
            _UsingStatements = string.Empty;
            
            var _lineNumber = 1;
            var _beforeNamespace = true;
            var _mainMethodStartLine = 0;
            var _openBrackets = 0;
            
            foreach (var _line in File.ReadLines(CallerInfo.GetCallerInfo().FilePath))
            {
                if (_beforeNamespace && _line.Contains("using"))
                {
                    this.stringBuilder.Append(_line);
                    this.stringBuilder.Append(Environment.NewLine);
                }
                else if (_line.Contains($"namespace {nameof(RogueDeck)}.{nameof(Utilities)}"))
                {
                    _beforeNamespace = false;
                    _UsingStatements = this.stringBuilder.GetAndClear();
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
                        this.stringBuilder.Append(_line);
                        this.stringBuilder.Append(Environment.NewLine);
                    }
                    else if (_openBrackets == 0)
                    {
                        return this.stringBuilder.GetAndClear();
                    }
                }

                _lineNumber++;
            }

            return string.Empty;
        }
        #endregion
    }
}
