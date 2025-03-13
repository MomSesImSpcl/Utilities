using System.Collections.Generic;
using MomSesImSpcl.Utilities;

namespace MomSesImSpcl.Interfaces
{
    /// <summary>
    /// Defines the contract for the <see cref="RuntimeCodeExecutor.codeGenerator"/> in <see cref="RuntimeCodeExecutor"/>.
    /// </summary>
    public interface ICodeGenerator
    {
        #region Methods
        /// <summary>
        /// Should define the logic how to get the code and the using statements.
        /// </summary>
        /// <param name="_UsingStatements">
        /// The using statements in the file. <br/>
        /// <i>Each element represents one line.</i>
        /// </param>
        /// <returns>
        /// The code that will be compiled and invoked. <br/>
        /// <i>Each element represents one line.</i></returns>
        public List<string> GetCode(out List<string> _UsingStatements);
        #endregion
    }
}
