using System.Text;

namespace IfLoooop.Utilities.Pooling.Wrappers
{
    /// <summary>
    /// <see cref="System.Text.StringBuilder"/> wrapper for <see cref="ObjectPool{T}"/>.
    /// </summary>
    public sealed class StringBuilderPoolWrapper : PoolWrapperBase<StringBuilderPoolWrapper>
    {
        #region Properties
        /// <summary>
        /// <see cref="System.Text.StringBuilder"/>.
        /// </summary>
        internal StringBuilder StringBuilder { get; } = new();
        #endregion

        #region Operators
        /// <summary>
        /// Implicitly returns the <see cref="StringBuilder"/> from a <see cref="StringBuilderPoolWrapper"/> object.
        /// </summary>
        /// <param name="_StringBuilderPoolWrapper">The <see cref="StringBuilderPoolWrapper"/> object.</param>
        /// <returns>The <see cref="StringBuilder"/> in the <see cref="StringBuilderPoolWrapper"/> object.</returns>
        public static implicit operator StringBuilder(StringBuilderPoolWrapper _StringBuilderPoolWrapper) => _StringBuilderPoolWrapper.StringBuilder;
        #endregion
        
        #region Methods
        /// <summary>
        /// Returns the contents of <see cref="StringBuilder"/>, clears it and returns this <see cref="StringBuilderPoolWrapper"/> back to its <see cref="PoolWrapperBase{T}.ObjectPool"/>.
        /// </summary>
        /// <returns>The contents of <see cref="StringBuilder"/>.</returns>
        internal new string Return()
        {
            var _string = this.StringBuilder.ToString();

            this.StringBuilder.Clear();
            base.Return();
            
            return _string;
        }
        #endregion
    }
}