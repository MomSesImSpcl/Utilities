using System;

namespace IfLoooop.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="DateTime"/>.
    /// </summary>
    public static class DateTimeExtensions
    {
        #region Methods
        /// <summary>
        /// Estimates the remaining time to complete a process.
        /// </summary>
        /// <param name="_StartTime">Start time of the process.</param>
        /// <param name="_CurrentStep">The number of steps completed so far.</param>
        /// <param name="_TotalSteps">The total number of steps in the process.</param>
        /// <param name="_LastProgressUpdate">The last time the process was updated.</param>
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