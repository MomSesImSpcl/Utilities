using System;

namespace MomSesImSpcl.Utilities
{
    /// <summary>
    /// Provides an enumerator for iterating over a sequence of numbers.
    /// </summary>
    public struct NumericEnumerator
    {
        #region Fields
        /// <summary>
        /// The end value when to stop the iteration.
        /// </summary>
        private readonly long end;
        #endregion
        
        #region Properties
        /// <summary>
        /// The current value in the iteration sequence.
        /// </summary>
        public long Current { get; private set; }
        #endregion
        
        #region Constructors
        /// <summary>
        /// <see cref="NumericEnumerator"/>.
        /// </summary>
        /// <param name="_End"><see cref="end"/>.</param>
        public NumericEnumerator(byte _End)
        {
            this.Current = 0;
            this.end = _End;
        }
        
        /// <summary>
        /// <see cref="NumericEnumerator"/>.
        /// </summary>
        /// <param name="_End"><see cref="end"/>.</param>
        public NumericEnumerator(sbyte _End)
        {
            this.Current = 0;
            this.end = _End;
        }
        
        /// <summary>
        /// <see cref="NumericEnumerator"/>.
        /// </summary>
        /// <param name="_End"><see cref="end"/>.</param>
        public NumericEnumerator(short _End)
        {
            this.Current = 0;
            this.end = _End;
        }
        
        /// <summary>
        /// <see cref="NumericEnumerator"/>.
        /// </summary>
        /// <param name="_End"><see cref="end"/>.</param>
        public NumericEnumerator(ushort _End)
        {
            this.Current = 0;
            this.end = _End;
        }
        
        /// <summary>
        /// <see cref="NumericEnumerator"/>.
        /// </summary>
        /// <param name="_End"><see cref="end"/>.</param>
        public NumericEnumerator(int _End)
        {
            this.Current = 0;
            this.end = _End;
        }
        
        /// <summary>
        /// <see cref="NumericEnumerator"/>.
        /// </summary>
        /// <param name="_End"><see cref="end"/>.</param>
        public NumericEnumerator(uint _End)
        {
            this.Current = 0;
            this.end = _End;
        }
        
        /// <summary>
        /// <see cref="NumericEnumerator"/>.
        /// </summary>
        /// <param name="_End"><see cref="end"/>.</param>
        public NumericEnumerator(long _End)
        {
            this.Current = 0;
            this.end = _End;
        }
        
        /// <summary>
        /// <see cref="NumericEnumerator"/>.
        /// </summary>
        /// <param name="_Range">The <see cref="Range"/> to iterate over.</param>
        public NumericEnumerator(Range _Range)
        {
            this.Current = _Range.Start.Value - 1;
            this.end = _Range.End.Value;
        }
        #endregion
        
        #region Methods
        /// <summary>
        /// Advances the enumerator to the next number in the sequence.
        /// </summary>
        /// <returns><c>true</c> if <see cref="Current"/> is &lt;= <see cref="end"/>, otherwise <c>false</c>.</returns>
        public bool MoveNext()
        {
            return ++this.Current <= this.end;
        }
        #endregion
    }
}