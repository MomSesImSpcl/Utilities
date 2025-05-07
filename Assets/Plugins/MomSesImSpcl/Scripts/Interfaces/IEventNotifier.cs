using System;

namespace MomSesImSpcl.Interfaces
{
    /// <summary>
    /// Interface for event notification.
    /// </summary>
    public interface IEventNotifier
    {
        #region Events
        /// <summary>
        /// Generic event notifier.
        /// </summary>
        public event Action OnEvent;
        #endregion
    }
}