using System;

namespace MomSesImSpcl.Utilities
{
    /// <summary>
    /// Provides an enumerator for iterating over a sequence of <see cref="int"/>.
    /// </summary>
    public struct IntEnumerator
    {
        #region Fields
        /// <summary>
        /// The end value when to stop the iteration.
        /// </summary>
        private readonly int end;
        #endregion
        
        #region Properties
        /// <summary>
        /// The current value in the iteration sequence.
        /// </summary>
        public int Current { get; private set; }
        #endregion
        
        #region Constructors
        /// <summary>
        /// <see cref="IntEnumerator"/>.
        /// </summary>
        /// <param name="_End"><see cref="end"/>.</param>
        public IntEnumerator(int _End)
        {
            this.Current = -1;
            this.end = _End;
        }
        
        /// <summary>
        /// <see cref="IntEnumerator"/>.
        /// </summary>
        /// <param name="_Range">The <see cref="Range"/> to iterate over.</param>
        public IntEnumerator(Range _Range)
        {
            this.Current = _Range.Start.Value - 1;
            this.end = _Range.End.Value;
        }
        #endregion
        
        #region Methods
        /// <summary>
        /// Advances the enumerator to the next <see cref="int"/> in the sequence.
        /// </summary>
        /// <returns><c>true</c> if <see cref="Current"/> is &lt;= <see cref="end"/>, otherwise <c>false</c>.</returns>
        public bool MoveNext()
        {
            return ++this.Current <= this.end;
        }
        #endregion
    }
}