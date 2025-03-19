namespace MomSesImSpcl.Interfaces
{
    /// <summary>
    /// Defines a contract for objects that possess a <see cref="Flag"/> identifier.
    /// </summary>
    public interface IFlag
    {
        #region Properties
        /// <summary>
        /// A <see cref="string"/> that serves as a flag identifier.
        /// </summary>
        public string Flag { get; }
        #endregion
    }
}