#if FMOD
using System;
using FMOD;
using FMODUnity;

namespace MomSesImSpcl.Data
{
    /// <summary>
    /// Strongly typed <see cref="EventReference"/>.<see cref="EventReference.Guid"/>. <br/>
    /// <i>Also implements a more efficient equality check then <see cref="EventReference"/>.</i>
    /// </summary>
    public readonly struct EventReferenceGuid : IEquatable<EventReferenceGuid>
    {
        #region Properties
        /// <summary>
        /// The <see cref="GUID"/> of an <see cref="EventReference"/>.
        /// </summary>
        public GUID Guid { get; }
        #endregion
        
        #region Operators
        /// <summary>
        /// Checks if the <see cref="Guid"/> of the given <see cref="EventReferenceGuid"/> are equal.
        /// </summary>
        /// <param name="_First"><see cref="EventReferenceGuid"/>.</param>
        /// <param name="_Second"><see cref="EventReferenceGuid"/></param>
        /// <returns><c>true</c> if equal, otherwise <c>false</c>.</returns>
        public static bool operator ==(EventReferenceGuid _First, EventReferenceGuid _Second)
        {
            return _First.Equals(_Second);
        }

        /// <summary>
        /// Checks if the <see cref="Guid"/> of the given <see cref="EventReferenceGuid"/> are not equal.
        /// </summary>
        /// <param name="_First"><see cref="EventReferenceGuid"/>.</param>
        /// <param name="_Second"><see cref="EventReferenceGuid"/></param>
        /// <returns><c>true</c> if not equal, otherwise <c>false</c>.</returns>
        public static bool operator !=(EventReferenceGuid _First, EventReferenceGuid _Second)
        {
            return !_First.Equals(_Second);
        }
        #endregion
        
        #region Constructors
        /// <summary>
        /// <see cref="EventReferenceGuid"/>.
        /// </summary>
        /// <param name="_EventReference"><see cref="EventReference"/>.</param>
        public EventReferenceGuid(EventReference _EventReference)
        {
            this.Guid = _EventReference.Guid;
        }
        #endregion
        
        #region Methods
        /// <summary>
        /// Checks if the given <see cref="EventReferenceGuid"/> has the same <see cref="Guid"/> as this one.
        /// </summary>
        /// <param name="_EventReferenceGuid">The <see cref="EventReferenceGuid"/> to check the <see cref="Guid"/> of.</param>
        /// <returns><c>true</c> if equal, otherwise <c>false</c>.</returns>
        public bool Equals(EventReferenceGuid _EventReferenceGuid)
        {
            return this.Guid.Equals(_EventReferenceGuid.Guid);
        }

        /// <summary>
        /// Checks if the given <see cref="object"/> is an <see cref="EventReferenceGuid"/> and has the same <see cref="Guid"/> as this one.
        /// </summary>
        /// <param name="_Object">The <see cref="object"/> to check the <see cref="Guid"/> of.</param>
        /// <returns><c>true</c> if equal, otherwise <c>false</c>.</returns>
        public override bool Equals(object _Object)
        {
            return _Object is EventReferenceGuid _object && this.Equals(_object);
        }
        
        /// <summary>
        /// Creates a hashcode from <see cref="Guid"/>.
        /// </summary>
        /// <param name="_EventReferenceGuid">The <see cref="EventReferenceGuid"/> to get the <see cref="Guid"/> of.</param>
        /// <returns>The hashcode of <see cref="Guid"/>.</returns>
        public int GetHashCode(EventReferenceGuid _EventReferenceGuid)
        {
            return _EventReferenceGuid.Guid.GetHashCode();
        }
        
        /// <summary>
        /// Creates a hashcode from <see cref="Guid"/>.
        /// </summary>
        /// <returns>The hashcode of <see cref="Guid"/>.</returns>
        public override int GetHashCode()
        {
            return this.Guid.GetHashCode();
        }
        
        /// <summary>
        /// Returns <see cref="Guid"/>.
        /// </summary>
        /// <returns><see cref="Guid"/>.</returns>
        public override string ToString()
        {
            return this.Guid.ToString();
        }
        #endregion
    }
}
#endif