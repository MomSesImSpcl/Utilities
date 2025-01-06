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
        #endregion
    }
}