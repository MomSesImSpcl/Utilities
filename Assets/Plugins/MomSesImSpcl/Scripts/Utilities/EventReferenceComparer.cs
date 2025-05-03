#if FMOD
using System.Collections.Generic;
using FMODUnity;

namespace MomSesImSpcl.Utilities
{
    /// <summary>
    /// <see cref="IEqualityComparer{T}"/> for <see cref="EventReference"/>.
    /// </summary>
    public sealed class EventReferenceComparer : IEqualityComparer<EventReference>
    {
        #region Methods
        /// <summary>
        /// Singleton of <see cref="EventReferenceComparer"/>.
        /// </summary>
        public static EventReferenceComparer Instance { get; } = new();
        #endregion
        
        #region Methods
        public bool Equals(EventReference _First, EventReference _Second)
        {
            return _First.Guid.Equals(_Second.Guid);
        }

        public int GetHashCode(EventReference _Object)
        {
            return _Object.Guid.GetHashCode();
        }
        #endregion
    }
}
#endif