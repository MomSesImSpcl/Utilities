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
        /// <i>Every using statement must be separated by a new line.</i>
        /// </param>
        /// <returns>The code that will be compiled and invoked.</returns>
        public string GetCode(out string _UsingStatements);
        #endregion
    }
}