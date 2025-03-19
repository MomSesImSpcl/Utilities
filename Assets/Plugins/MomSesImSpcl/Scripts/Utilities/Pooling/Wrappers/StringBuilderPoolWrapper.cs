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

        /// <summary>
        /// Retrieves a <see cref="System.Text.StringBuilder"/> from <see cref="ObjectPools"/>.<see cref="ObjectPools.StringBuilderPool"/> and appends the given <see cref="string"/> to it.
        /// </summary>
        /// <param name="_String1">The <see cref="string"/> to append to the retrieved <see cref="System.Text.StringBuilder"/>.</param>
        /// <returns>The <see cref="string"/> inside the retrieved <see cref="System.Text.StringBuilder"/>.</returns>
        public static string Append(string _String1)
        {
            var _poolWrapper = ObjectPools.StringBuilderPool.Get();
            
            _poolWrapper.StringBuilder.Append(_String1);
            
            return _poolWrapper.Return();
        }
        
        /// <summary>
        /// Retrieves a <see cref="System.Text.StringBuilder"/> from <see cref="ObjectPools"/>.<see cref="ObjectPools.StringBuilderPool"/> and appends the given <see cref="string"/> to it.
        /// </summary>
        /// <param name="_String1">The first <see cref="string"/> to append to the retrieved <see cref="System.Text.StringBuilder"/>.</param>
        /// <param name="_String2">The second <see cref="string"/> to append to the retrieved <see cref="System.Text.StringBuilder"/>.</param>
        /// <returns>The <see cref="string"/> inside the retrieved <see cref="System.Text.StringBuilder"/>.</returns>
        public static string Append(string _String1, string _String2)
        {
            var _poolWrapper = ObjectPools.StringBuilderPool.Get();
            
            _poolWrapper.StringBuilder.Append(_String1);
            _poolWrapper.StringBuilder.Append(_String2);
            
            return _poolWrapper.Return();
        }
        
        /// <summary>
        /// Retrieves a <see cref="System.Text.StringBuilder"/> from <see cref="ObjectPools"/>.<see cref="ObjectPools.StringBuilderPool"/> and appends the given <see cref="string"/> to it.
        /// </summary>
        /// <param name="_String1">The first <see cref="string"/> to append to the retrieved <see cref="System.Text.StringBuilder"/>.</param>
        /// <param name="_String2">The second <see cref="string"/> to append to the retrieved <see cref="System.Text.StringBuilder"/>.</param>
        /// <param name="_String3">The third <see cref="string"/> to append to the retrieved <see cref="System.Text.StringBuilder"/>.</param>
        /// <returns>The <see cref="string"/> inside the retrieved <see cref="System.Text.StringBuilder"/>.</returns>
        public static string Append(string _String1, string _String2, string _String3)
        {
            var _poolWrapper = ObjectPools.StringBuilderPool.Get();
            
            _poolWrapper.StringBuilder.Append(_String1);
            _poolWrapper.StringBuilder.Append(_String2);
            _poolWrapper.StringBuilder.Append(_String3);
            
            return _poolWrapper.Return();
        }
        
        /// <summary>
        /// Retrieves a <see cref="System.Text.StringBuilder"/> from <see cref="ObjectPools"/>.<see cref="ObjectPools.StringBuilderPool"/> and appends the given <see cref="string"/> to it.
        /// </summary>
        /// <param name="_String1">The first <see cref="string"/> to append to the retrieved <see cref="System.Text.StringBuilder"/>.</param>
        /// <param name="_String2">The second <see cref="string"/> to append to the retrieved <see cref="System.Text.StringBuilder"/>.</param>
        /// <param name="_String3">The third <see cref="string"/> to append to the retrieved <see cref="System.Text.StringBuilder"/>.</param>
        /// <param name="_String4">The fourth <see cref="string"/> to append to the retrieved <see cref="System.Text.StringBuilder"/>.</param>
        /// <returns>The <see cref="string"/> inside the retrieved <see cref="System.Text.StringBuilder"/>.</returns>
        public static string Append(string _String1, string _String2, string _String3, string _String4)
        {
            var _poolWrapper = ObjectPools.StringBuilderPool.Get();
            
            _poolWrapper.StringBuilder.Append(_String1);
            _poolWrapper.StringBuilder.Append(_String2);
            _poolWrapper.StringBuilder.Append(_String3);
            _poolWrapper.StringBuilder.Append(_String4);
            
            return _poolWrapper.Return();
        }
        #endregion
    }
}