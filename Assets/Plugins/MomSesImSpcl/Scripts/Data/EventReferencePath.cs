#if FMOD
using System;
using FMODUnity;

namespace MomSesImSpcl.Data
{
    /// <summary>
    /// Strongly typed <see cref="EventReference"/> <see cref="EventReference.Path"/>.
    /// </summary>
    public readonly struct EventReferencePath : IEquatable<EventReferencePath>
    {
        #region Properties
        /// <summary>
        /// <see cref="EventReference"/> <see cref="EventReference.Path"/>.
        /// </summary>
        public string Path { get; }
        #endregion
        
        #region Operators
        /// <summary>
        /// Checks if the <see cref="Path"/> of the given <see cref="EventReferencePath"/> are equal.
        /// </summary>
        /// <param name="_First"><see cref="EventReferencePath"/>.</param>
        /// <param name="_Second"><see cref="EventReferencePath"/></param>
        /// <returns><c>true</c> if equal, otherwise <c>false</c>.</returns>
        public static bool operator ==(EventReferencePath _First, EventReferencePath _Second)
        {
            return _First.Equals(_Second);
        }

        /// <summary>
        /// Checks if the <see cref="Path"/> of the given <see cref="EventReferencePath"/> are not equal.
        /// </summary>
        /// <param name="_First"><see cref="EventReferencePath"/>.</param>
        /// <param name="_Second"><see cref="EventReferencePath"/></param>
        /// <returns><c>true</c> if not equal, otherwise <c>false</c>.</returns>
        public static bool operator !=(EventReferencePath _First, EventReferencePath _Second)
        {
            return !_First.Equals(_Second);
        }
        #endregion
        
        #region Constructors
        /// <summary>
        /// <see cref="EventReferencePath"/>.
        /// </summary>
        /// <param name="_EventReference"><see cref="EventReference"/>.</param>
        public EventReferencePath(EventReference _EventReference)
        {
            this.Path = _EventReference.Path;
        }
        #endregion
        
        #region Methods
        /// <summary>
        /// Checks if the given <see cref="EventReferencePath"/> has the same <see cref="Path"/> as this one.
        /// </summary>
        /// <param name="_EventReferencePath">The <see cref="EventReferencePath"/> to check the <see cref="Path"/> of.</param>
        /// <returns><c>true</c> if equal, otherwise <c>false</c>.</returns>
        public bool Equals(EventReferencePath _EventReferencePath)
        {
            return string.Equals(this.Path, _EventReferencePath.Path);
        }

        /// <summary>
        /// Checks if the given <see cref="object"/> is an <see cref="EventReferencePath"/> and has the same <see cref="Path"/> as this one.
        /// </summary>
        /// <param name="_Object">The <see cref="object"/> to check the <see cref="Path"/> of.</param>
        /// <returns><c>true</c> if equal, otherwise <c>false</c>.</returns>
        public override bool Equals(object _Object)
        {
            return _Object is EventReferencePath _eventReferencePath && this.Equals(_eventReferencePath);
        }
        
        /// <summary>
        /// Creates a hashcode from <see cref="Path"/>.
        /// </summary>
        /// <param name="_EventReferencePath">The <see cref="EventReferencePath"/> to get the <see cref="Path"/> of.</param>
        /// <returns>The hashcode of <see cref="Path"/> or <c>0</c> if <see cref="Path"/> is <c>null</c>.</returns>
        public int GetHashCode(EventReferencePath _EventReferencePath)
        {
            return _EventReferencePath.Path is null ? 0 : _EventReferencePath.Path.GetHashCode();
        }
        
        /// <summary>
        /// Creates a hashcode from <see cref="Path"/>.
        /// </summary>
        /// <returns>The hashcode of <see cref="Path"/> or <c>0</c> if <see cref="Path"/> is <c>null</c>.</returns>
        public override int GetHashCode()
        {
            return this.GetHashCode(this);
        }
        
        /// <summary>
        /// Returns <see cref="Path"/>.
        /// </summary>
        /// <returns><see cref="Path"/>.</returns>
        public override string ToString()
        {
            return this.Path;
        }
        #endregion
    }
}
#endif
