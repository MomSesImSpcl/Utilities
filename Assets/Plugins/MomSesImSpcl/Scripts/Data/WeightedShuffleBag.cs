using System;
using System.Collections.Generic;
using MomSesImSpcl.Extensions;
using UnityEngine;

namespace MomSesImSpcl.Data
{
    /// <summary>
    /// Provides weighted random draws using a shuffled bag of predefined outcomes. <br/>
    /// Each outcome is guaranteed to be drawn according to its relative weight before the bag is repopulated and reshuffled.
    /// </summary>
    [Serializable]
    public sealed class WeightedShuffleBag<T> : ISerializationCallbackReceiver
    {
        #region Inspector Fields
        /// <summary>
        /// The configured outcomes and their relative weights.
        /// </summary>
        [Tooltip("All possible outcomes and their relative weights.")]
        [SerializeField] private List<SerializedKeyValuePair<T, int>> outcomes = new();
        /// <summary>
        /// Contains the outcomes currently available to be drawn.
        /// </summary>
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ReadOnly]
#endif
        [Tooltip("Contains the outcomes currently available to be drawn.")]
        [SerializeField] private List<T> bag = new();
        #endregion

        #region Fields
        /// <summary>
        /// The default <see cref="EqualityComparer{T}"/> for <typeparamref name="T"/>.
        /// </summary>
        private static readonly EqualityComparer<T> tComparer = EqualityComparer<T>.Default;
        #endregion
        
        #region Properties
        /// <summary>
        /// The total number of <see cref="outcomes"/> in this <see cref="bag"/>.
        /// </summary>
        public int Outcomes => this.outcomes.Count;
        /// <summary>
        /// The number of outcomes remaining in this the <see cref="bag"/>.
        /// </summary>
        public int Remaining => this.bag.Count;
        /// <summary>
        /// Indicates whether the <see cref="bag"/> is empty or not.
        /// </summary>
        public bool IsEmpty => this.bag.Count == 0;
        #endregion
        
        #region Constructors
        /// <summary>
        /// Creates a <see cref="WeightedShuffleBag{T}"/> with the specified outcomes.
        /// </summary>
        /// <param name="_Outcomes">The outcomes and their relative weights.</param>
        public WeightedShuffleBag(IEnumerable<(T outcome, int weight)> _Outcomes)
        {
            if (_Outcomes == null)
            {
                throw new ArgumentNullException(nameof(_Outcomes));
            }
            
            foreach (var (_outcome, _weight) in _Outcomes)
            {
                this.Add(_outcome, _weight);
            }

            if (this.outcomes.Count == 0)
            {
                throw new ArgumentException("At least one outcome must be provided.", nameof(_Outcomes));
            }
            
            this.PopulateBag();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adds an outcome to <see cref="outcomes"/>. <br/>
        /// <i>Will only be added to the <see cref="bag"/>, once it is empty, or <c>_Refill</c> is set to <c>true</c>.</i>
        /// </summary>
        /// <param name="_Outcome">The outcome.</param>
        /// <param name="_Weight">The relative weight of the outcome.</param>
        /// <param name="_Refill">Set to <c>true</c>, to immediately add the given outcome to the <see cref="bag"/> and reshuffle it.</param>
        /// TODO: Maybe check for duplicate entries and merge/combine them, or don't allow them at all.
        public void Add(T _Outcome, int _Weight, bool _Refill = false)
        {
            if (_Weight <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(_Weight), _Weight, "Weight must be greater than zero.");
            }

            this.outcomes.Add(new SerializedKeyValuePair<T, int>(_Outcome, _Weight));

            if (_Refill)
            {
                this.PopulateBag();
            }
        }

        /// <summary>
        /// Removes an outcome from <see cref="outcomes"/>.
        /// </summary>
        /// <param name="_OutcomeToRemove">The outcome to remove from <see cref="Outcomes"/>.</param>
        /// <param name="_Refill">Set to <c>true</c>, to immediately remove the given outcome from the <see cref="bag"/> and reshuffle it.</param>
        /// <returns><c>true</c> if the outcome was removed, otherwise <c>false</c>.</returns>
        public bool Remove(T _OutcomeToRemove, bool _Refill = false)
        {
            var _index = this.outcomes.FindIndex
                (
                    _OutcomeToRemove, (_Kvp, _Outcome) => tComparer.Equals(_Kvp.Key, _Outcome)
                );

            if (_index < 0)
            {
                return false;
            }

            this.outcomes.RemoveAt(_index);
            
            if (_Refill)
            {
                this.PopulateBag();
            }
            
            return true;
        }

        /// <summary>
        /// Clears <see cref="outcomes"/> and <see cref="bag"/>.
        /// </summary>
        public void Clear()
        {
            this.outcomes.Clear();
            this.bag.Clear();
        }
        
        /// <summary>
        /// Draws an outcome from the <see cref="bag"/>.
        /// </summary>
        public T Draw()
        {
            if (this.outcomes.Count == 0)
            {
                throw new InvalidOperationException("Cannot draw from an empty ShuffleBag.");
            }

            if (this.bag.Count == 0)
            {
                this.PopulateBag();
            }

            return this.bag.Pop();
        }

        /// <summary>
        /// Repopulates and shuffles <see cref="bag"/>.
        /// </summary>
        private void PopulateBag()
        {
            this.bag.Clear();

            if (this.outcomes.Count == 0)
            {
                return;
            }
            
            var _gcd = this.GetGreatestCommonDivisor();

            foreach (var (_value, _weight) in this.outcomes)
            {
                var _count = _weight / _gcd;

                for (var _i = 0; _i < _count; _i++)
                {
                    this.bag.Add(_value);
                }
            }

            this.bag.Shuffle();
        }

        /// <summary>
        /// Gets the greatest common divisor shared by all outcome weights.
        /// </summary>
        private int GetGreatestCommonDivisor()
        {
            var _gcd = this.outcomes[0].Value;

            // ReSharper disable once InconsistentNaming
            for (var i = 1; i < this.outcomes.Count; i++)
            {
                _gcd = Utilities.Math.GreatestCommonDivisor(_gcd, this.outcomes[i].Value);
            }

            return _gcd;
        }

        /// <summary>
        /// Discards the remaining outcomes and creates a new shuffled <see cref="bag"/>.
        /// </summary>
        public void Reset() => this.PopulateBag();
        #endregion

        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize()
        {
            this.outcomes ??= new List<SerializedKeyValuePair<T, int>>();
            this.bag ??= new List<T>();

            // ReSharper disable once InconsistentNaming
            for (var i = this.outcomes.Count - 1; i >= 0; i--)
            {
                // Remove outcomes with invalid weights.
                if (this.outcomes[i].Value <= 0)
                {
                    this.outcomes.RemoveAt(i);
                }
            }
            
            if (this.outcomes.Count == 0)
            {
                this.bag.Clear();
            }
        }
    }
}