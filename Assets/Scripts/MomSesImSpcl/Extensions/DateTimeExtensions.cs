using System;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="DateTime"/>.
    /// </summary>
    public static class DateTimeExtensions
    {
        #region Methods
        /// <summary>
        /// Estimates the remaining time required to complete a process based on the current progress.
        /// </summary>
        /// <param name="_StartTime">The starting time of the process.</param>
        /// <param name="_CurrentStep">The current progress step number.</param>
        /// <param name="_TotalSteps">The total number of steps required to complete the process.</param>
        /// <param name="_LastProgressUpdate">The time of the last progress update.</param>
        /// <returns>A <see cref="TimeSpan"/> representing the estimated remaining time to complete the process.</returns>
        public static TimeSpan EstimateTime(this DateTime _StartTime, long _CurrentStep, long _TotalSteps, DateTime _LastProgressUpdate)
        {
            var _elapsedTime = DateTime.Now - _StartTime;
            var _timeSinceLastUpdate = DateTime.Now - _LastProgressUpdate;

            if (_CurrentStep != _TotalSteps)
            {
                var _adjustedElapsedTime = _elapsedTime.Add(_timeSinceLastUpdate);
                var _averageTimePerStep = _adjustedElapsedTime.TotalSeconds / Math.Max(_CurrentStep + 1, 1);
                var _remainingSteps = _TotalSteps - _CurrentStep;

                return TimeSpan.FromSeconds(_averageTimePerStep * _remainingSteps);
            }

            return TimeSpan.Zero;
        }
        #endregion
    }
}