#nullable enable
using System.Text;

namespace MomSesImSpcl.Utilities.Pooling.Wrappers
{
    /// <summary>
    /// A thread-safe wrapper for a <see cref="StringBuilder"/> object that is managed by a concurrent object pool.
    /// </summary>
    /// <remarks>
    /// This class is sealed to prevent inheritance and provides implicit conversion to a <see cref="StringBuilder"/> object.
    /// </remarks>
    public sealed class ConcurrentStringBuilderPoolWrapper : ConcurrentPoolWrapperBase<ConcurrentStringBuilderPoolWrapper>
    {
        #region Properties
        /// <summary>
        /// Gets the <see cref="StringBuilder"/> instance managed by the concurrent object pool wrapper.
        /// </summary>
        /// <remarks>
        /// This property provides access to the encapsulated <see cref="StringBuilder"/> for performing
        /// string manipulation operations.
        /// </remarks>
        public StringBuilder StringBuilder { get; } = new();
        #endregion

        #region Operators
        /// <summary>
        /// Provides an implicit conversion between <see cref="ConcurrentStringBuilderPoolWrapper"/> and <see cref="StringBuilder"/>.
        /// </summary>
        /// <param name="_ConcurrentStringBuilderPoolWrapper">The instance of <see cref="ConcurrentStringBuilderPoolWrapper"/> to convert.</param>
        /// <returns>The <see cref="StringBuilder"/> instance contained within <see cref="ConcurrentStringBuilderPoolWrapper"/>.</returns>
        public static implicit operator StringBuilder(ConcurrentStringBuilderPoolWrapper _ConcurrentStringBuilderPoolWrapper) => _ConcurrentStringBuilderPoolWrapper.StringBuilder;
        #endregion
        
        #region Methods

        /// <summary>
        /// Returns the current instance to its associated object pool, and optionally returns the string represented by the <see cref="StringBuilder"/>.
        /// </summary>
        /// <returns>A string representation of the <see cref="StringBuilder"/> if it contains any characters; otherwise, null.</returns>
        public new string? Return()
        {
            var _string = this.StringBuilder.Length == 0 ? null : this.StringBuilder.ToString();

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
        
        /// <summary>
        /// Retrieves a <see cref="System.Text.StringBuilder"/> from <see cref="ObjectPools"/>.<see cref="ObjectPools.StringBuilderPool"/> and appends the given <see cref="string"/> to it.
        /// </summary>
        /// <param name="_String1">The first <see cref="string"/> to append to the retrieved <see cref="System.Text.StringBuilder"/>.</param>
        /// <param name="_String2">The second <see cref="string"/> to append to the retrieved <see cref="System.Text.StringBuilder"/>.</param>
        /// <param name="_String3">The third <see cref="string"/> to append to the retrieved <see cref="System.Text.StringBuilder"/>.</param>
        /// <param name="_String4">The fourth <see cref="string"/> to append to the retrieved <see cref="System.Text.StringBuilder"/>.</param>
        /// <param name="_String5">The fifth <see cref="string"/> to append to the retrieved <see cref="System.Text.StringBuilder"/>.</param>
        /// <returns>The <see cref="string"/> inside the retrieved <see cref="System.Text.StringBuilder"/>.</returns>
        public static string Append(string _String1, string _String2, string _String3, string _String4, string _String5)
        {
            var _poolWrapper = ObjectPools.StringBuilderPool.Get();
            
            _poolWrapper.StringBuilder.Append(_String1);
            _poolWrapper.StringBuilder.Append(_String2);
            _poolWrapper.StringBuilder.Append(_String3);
            _poolWrapper.StringBuilder.Append(_String4);
            _poolWrapper.StringBuilder.Append(_String5);
            
            return _poolWrapper.Return();
        }
        
        /// <summary>
        /// Retrieves a <see cref="System.Text.StringBuilder"/> from <see cref="ObjectPools"/>.<see cref="ObjectPools.StringBuilderPool"/> and appends the given <see cref="string"/> to it.
        /// </summary>
        /// <param name="_String1">The first <see cref="string"/> to append to the retrieved <see cref="System.Text.StringBuilder"/>.</param>
        /// <param name="_String2">The second <see cref="string"/> to append to the retrieved <see cref="System.Text.StringBuilder"/>.</param>
        /// <param name="_String3">The third <see cref="string"/> to append to the retrieved <see cref="System.Text.StringBuilder"/>.</param>
        /// <param name="_String4">The fourth <see cref="string"/> to append to the retrieved <see cref="System.Text.StringBuilder"/>.</param>
        /// <param name="_String5">The fifth <see cref="string"/> to append to the retrieved <see cref="System.Text.StringBuilder"/>.</param>
        /// <param name="_String6">The sixth <see cref="string"/> to append to the retrieved <see cref="System.Text.StringBuilder"/>.</param>
        /// <returns>The <see cref="string"/> inside the retrieved <see cref="System.Text.StringBuilder"/>.</returns>
        public static string Append(string _String1, string _String2, string _String3, string _String4, string _String5, string _String6)
        {
            var _poolWrapper = ObjectPools.StringBuilderPool.Get();
            
            _poolWrapper.StringBuilder.Append(_String1);
            _poolWrapper.StringBuilder.Append(_String2);
            _poolWrapper.StringBuilder.Append(_String3);
            _poolWrapper.StringBuilder.Append(_String4);
            _poolWrapper.StringBuilder.Append(_String5);
            _poolWrapper.StringBuilder.Append(_String6);
            
            return _poolWrapper.Return();
        }
        
        /// <summary>
        /// Retrieves a <see cref="System.Text.StringBuilder"/> from <see cref="ObjectPools"/>.<see cref="ObjectPools.StringBuilderPool"/> and appends the given <see cref="string"/> to it.
        /// </summary>
        /// <param name="_String1">The first <see cref="string"/> to append to the retrieved <see cref="System.Text.StringBuilder"/>.</param>
        /// <param name="_String2">The second <see cref="string"/> to append to the retrieved <see cref="System.Text.StringBuilder"/>.</param>
        /// <param name="_String3">The third <see cref="string"/> to append to the retrieved <see cref="System.Text.StringBuilder"/>.</param>
        /// <param name="_String4">The fourth <see cref="string"/> to append to the retrieved <see cref="System.Text.StringBuilder"/>.</param>
        /// <param name="_String5">The fifth <see cref="string"/> to append to the retrieved <see cref="System.Text.StringBuilder"/>.</param>
        /// <param name="_String6">The sixth <see cref="string"/> to append to the retrieved <see cref="System.Text.StringBuilder"/>.</param>
        /// <param name="_String7">The seventh <see cref="string"/> to append to the retrieved <see cref="System.Text.StringBuilder"/>.</param>
        /// <returns>The <see cref="string"/> inside the retrieved <see cref="System.Text.StringBuilder"/>.</returns>
        public static string Append(string _String1, string _String2, string _String3, string _String4, string _String5, string _String6, string _String7)
        {
            var _poolWrapper = ObjectPools.StringBuilderPool.Get();
            
            _poolWrapper.StringBuilder.Append(_String1);
            _poolWrapper.StringBuilder.Append(_String2);
            _poolWrapper.StringBuilder.Append(_String3);
            _poolWrapper.StringBuilder.Append(_String4);
            _poolWrapper.StringBuilder.Append(_String5);
            _poolWrapper.StringBuilder.Append(_String6);
            _poolWrapper.StringBuilder.Append(_String7);
            
            return _poolWrapper.Return();
        }
        #endregion
    }
}
