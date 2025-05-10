using System;

namespace MomSesImSpcl.Interfaces
{
    /// <summary>
    /// Contains an <c>Init</c> method.
    /// </summary>
    /// <typeparam name="P">The <see cref="Type"/> of the initialization parameter.</typeparam>
    public interface IInit<in P>
    {
        #region Methods
        /// <summary>
        /// Optional initialization.
        /// </summary>
        /// <param name="_Parameter">The parameter needed for the initialization.</param>
        public void Init(P _Parameter);
        #endregion
    }
}