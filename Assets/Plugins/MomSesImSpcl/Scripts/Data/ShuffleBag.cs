using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MomSesImSpcl.Data
{
    /// <summary>
    /// Provides weighted random draws using a shuffled <see cref="List{T}"/> of predefined success and failure values.
    /// </summary>
    [Serializable]
    public sealed class ShuffleBag<T>
    {
        #region Inspector Fields
        /// <summary>
        /// <see cref="WeightedShuffleBag{T}"/>.
        /// </summary>
        [Tooltip("The underlying WeightedShuffleBag of this ShuffleBag.")]
        [SerializeField] private WeightedShuffleBag<T> bag;
        #endregion

        #region Properties
        /// <summary>
        /// <see cref="bag"/>.
        /// </summary>
        /// TODO: Maybe expose WeightedShuffleBag Properties/Methods instead of this.
        public WeightedShuffleBag<T> Bag => this.bag;
        #endregion
        
        #region Constructors
        /// <summary>
        /// <see cref="ShuffleBag{T}"/>.
        /// </summary>
        /// <param name="_Success">The retrieved value on a success.</param>
        /// <param name="_Failure">The retrieved value on a fail.</param>
        /// <param name="_SuccessChance">
        /// The chance of success. <br/>
        /// <i>Must be 0-1.<i/></i>
        /// </param>
        public ShuffleBag(T _Success, T _Failure, float _SuccessChance)
        {
            if (_SuccessChance is < 0f or > 1f)
            {
                throw new ArgumentOutOfRangeException(nameof(_SuccessChance), _SuccessChance, "Success chance must be between 0 and 1.");
            }
            
            const int _RESOLUTION = 100;

            var _successCount = Mathf.RoundToInt(_SuccessChance * _RESOLUTION);
            var _failureCount = _RESOLUTION - _successCount;

            var _outcomes = new List<(T outcome, int weight)>();

            if (_successCount > 0)
            {
                _outcomes.Add((_Success, _successCount));
            }

            if (_failureCount > 0)
            {
                _outcomes.Add((_Failure, _failureCount));
            }
        
            this.bag = new WeightedShuffleBag<T>(_outcomes);
        }
        
        /// <summary>
        /// Creates a <see cref="ShuffleBag{T}"/> where each outcome has the same chance of being drawn.
        /// </summary>
        /// <param name="_Outcomes">The possible outcomes.</param>
        public ShuffleBag(IEnumerable<T> _Outcomes)
        {
            if (_Outcomes == null)
            {
                throw new ArgumentNullException(nameof(_Outcomes));
            }

            var _weightedOutcomes = _Outcomes.Select(_Outcome => (_outcome: _Outcome, 1)).ToArray();

            if (_weightedOutcomes.Length == 0)
            {
                throw new ArgumentException("At least one outcome must be provided.", nameof(_Outcomes));
            }

            this.bag = new WeightedShuffleBag<T>(_weightedOutcomes);
        }

        /// <summary>
        /// Creates a <see cref="ShuffleBag{T}"/> where each outcome has the same chance of being drawn.
        /// </summary>
        /// <param name="_Outcomes">The possible outcomes.</param>
        public ShuffleBag(params T[] _Outcomes) : this((IEnumerable<T>)_Outcomes) { }
        #endregion

        #region Methods
        /// <summary>
        /// Retrieves a random value from the <see cref="bag"/>.
        /// </summary>
        /// <returns>A random value from <see cref="bag"/>.</returns>
        public T Draw() => this.bag.Draw();
        #endregion
    }

    /// <summary>
    /// Non-generic helper class for <see cref="ShuffleBag{T}"/>.
    /// </summary>
    public static class ShuffleBag
    {
        /// <summary>
        /// Creates a binary <see cref="ShuffleBag{T}"/> that returns <c>true</c> on success and <c>false</c> on failure.
        /// </summary>
        /// <param name="_SuccessChance">
        /// The chance of drawing <c>true</c>. <br/>
        /// <i>Must be between 0 and 1.</i>
        /// </param>
        /// <returns>A binary shuffle bag containing <c>true</c> and <c>false</c> outcomes.</returns>
        public static ShuffleBag<bool> BinaryDraw(float _SuccessChance) => new(true, false, _SuccessChance);
    }
}