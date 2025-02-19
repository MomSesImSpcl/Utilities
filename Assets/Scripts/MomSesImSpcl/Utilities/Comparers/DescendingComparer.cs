using System;
using System.Collections.Generic;

namespace MomSesImSpcl.Utilities.Comparers
{
    /// <summary>
    /// A comparer that sorts elements in descending order based on a key extracted from the elements.
    /// </summary>
    /// <typeparam name="T">The type of the elements to compare.</typeparam>
    /// <typeparam name="C">The type of the key used for comparison. This must implement <see cref="IComparable{C}"/>.</typeparam>
    public readonly struct DescendingComparer<T,C> : IComparer<T> where C : IComparable<C>
    {
        #region Fields
        /// <summary>
        /// A function that extracts the comparison key from the element of <see cref="Type"/> <c>T</c>.
        /// </summary>
        private readonly Func<T,C> comparerKey;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DescendingComparer{T,C}"/> struct.
        /// </summary>
        /// <param name="_ComparerKey"><see cref="comparerKey"/>.</param>
        public DescendingComparer(Func<T,C> _ComparerKey)
        {
            this.comparerKey = _ComparerKey;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Compares two elements of <see cref="Type"/> <c>T</c> in descending order based on the <see cref="comparerKey"/>.
        /// </summary>
        /// <param name="_First">The first element to compare.</param>
        /// <param name="_Second">The second element to compare.</param>
        /// <returns>
        /// A value less than 0 if the key of <c>_Second</c> is greater than the key of <c>_First</c>, <br/>
        /// 0 if the keys are equal, or a value greater than 0 if the key of <c>_Second</c> is less than the key of <c>_First</c>.
        /// </returns>
        public int Compare(T _First, T _Second)
        {
            return comparerKey(_Second).CompareTo(comparerKey(_First));
        }
        #endregion
    }
}