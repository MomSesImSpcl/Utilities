using System.Text;

namespace MomSesImSpcl.Utilities.Pooling.Wrappers
{
    /// <summary>
    /// A wrapper class for managing a <see cref="StringBuilder"/> instance within an object pool.
    /// </summary>
    public sealed class StringBuilderPoolWrapper : PoolWrapperBase<StringBuilderPoolWrapper>
    {
        #region Properties
        /// <summary>
        /// Gets the instance of <see cref="StringBuilder"/> managed by the wrapper.
        /// </summary>
        public StringBuilder StringBuilder { get; } = new();
        #endregion

        #region Operators
        /// <summary>
        /// Implicitly converts a <see cref="StringBuilderPoolWrapper"/> to a <see cref="StringBuilder"/>.
        /// </summary>
        /// <param name="_StringBuilderPoolWrapper">The <see cref="StringBuilderPoolWrapper"/> instance to convert.</param>
        /// <returns>The underlying <see cref="StringBuilder"/> of the provided <see cref="StringBuilderPoolWrapper"/>.</returns>
        public static implicit operator StringBuilder(StringBuilderPoolWrapper _StringBuilderPoolWrapper) => _StringBuilderPoolWrapper.StringBuilder;
        #endregion
        
        #region Methods
        /// <summary>
        /// Retrieves the current string value of the underlying <see cref="StringBuilder"/>, clears the
        /// <see cref="StringBuilder"/>, and returns this instance back to its associated object pool.
        /// </summary>
        /// <returns>The current string value of the underlying <see cref="StringBuilder"/>.</returns>
        public new string Return()
        {
            var _string = this.StringBuilder.ToString();

            this.StringBuilder.Clear();
            base.Return();
            
            return _string;
        }
        #endregion
    }
}