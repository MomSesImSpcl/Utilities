#if ODIN_INSPECTOR
using System;
using MomSesImSpcl.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MomSesImSpcl.Editor
{
    /// <summary>
    /// For displaying the progress of a process in the inspector.
    /// </summary>
    [Serializable]
    public sealed class ProgressBar
    {
        #region Inspector Fields
        [Tooltip("Displays the progress of a process.")]
        [ProgressBar(0, MAX)][HideLabel] // ReSharper disable once NotAccessedField.Local
        [SerializeField][ReadOnly] private ulong progressBar;
        [Tooltip("The estimated remaining time.")]
        [HideLabel][ShowIf(nameof(this.hasStarted))] // ReSharper disable once NotAccessedField.Local
        [SerializeField][ReadOnly] private string remainingTime;
        #endregion
        
        #region Constants
        /// <summary>
        /// Refactor resistant name of <see cref="max"/> for <see cref="progressBar"/>.
        /// </summary>
        private const string MAX = nameof(max);
        #endregion
        
        #region Fields
        /// <summary>
        /// The timestamp of when the <see cref="progressBar"/> was started to display a progress.
        /// </summary>
        private DateTime startTime;
        /// <summary>
        /// Will be <c>true</c> once <see cref="Start"/> has been called.
        /// </summary>
#pragma warning disable CS0414
        private bool hasStarted;
#pragma warning restore CS0414
        /// <summary>
        /// The amount when the <see cref="progressBar"/> will be completed.
        /// </summary>
        private ulong max;
        #endregion
        
        #region Properties
        /// <summary>
        /// Returns the estimated time required to fill the <see cref="progressBar"/> as a <see cref="string"/>.
        /// </summary>
        private string EstimatedTime => this.startTime.EstimateTime(this.progressBar, this.max, DateTime.Now).ToString(@"mm\:ss");
        #endregion
        
        #region Methods
        /// <summary>
        /// Starts displaying the progress in the <see cref="progressBar"/>.
        /// </summary>
        /// <param name="_Max"><see cref="max"/>.</param>
        public void Start(ulong _Max)
        {
            this.startTime = DateTime.Now;
            this.hasStarted = true;
            this.progressBar = 0;
            this.max = _Max;
        }

        /// <summary>
        /// Fills the <see cref="progressBar"/> by the given amount.
        /// </summary>
        /// <param name="_Amount">WIll be added to <see cref="progressBar"/>.</param>
        public void Add(ulong _Amount = 1)
        {
            this.progressBar += _Amount;
            this.remainingTime = this.EstimatedTime;

            if (this.progressBar == this.max)
            {
                this.hasStarted = false;
            }
        }
        
        /// <summary>
        /// Sets <see cref="progressBar"/> to the given value.
        /// </summary>
        /// <param name="_Value">The value to set <see cref="progressBar"/> to.</param>
        public void Set(ulong _Value)
        {
            this.progressBar = _Value;
            this.remainingTime = this.EstimatedTime;
            
            if (this.progressBar == this.max)
            {
                this.hasStarted = false;
            }
        }
        
        /// <summary>
        /// Resets the <see cref="progressBar"/>.
        /// </summary>
        public void Reset()
        {
            this.progressBar = 0;
            this.max = 0;
            this.hasStarted = false;
        }
        #endregion
    }
}
#endif
