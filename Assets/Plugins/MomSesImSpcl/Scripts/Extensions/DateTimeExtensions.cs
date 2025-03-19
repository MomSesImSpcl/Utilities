using System;
using Unity.Mathematics;

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
        public static TimeSpan EstimateTime(this DateTime _StartTime, ulong _CurrentStep, ulong _TotalSteps, DateTime _LastProgressUpdate)
        {
            var _elapsedTime = DateTime.Now - _StartTime;
            var _timeSinceLastUpdate = DateTime.Now - _LastProgressUpdate;

            if (_CurrentStep != _TotalSteps)
            {
                var _adjustedElapsedTime = _elapsedTime.Add(_timeSinceLastUpdate);
                var _averageTimePerStep = _adjustedElapsedTime.TotalSeconds / math.max(_CurrentStep + 1, 1);
                var _remainingSteps = _TotalSteps - _CurrentStep;
                var _totalSeconds = _averageTimePerStep * _remainingSteps;
                
                return _totalSeconds > TimeSpan.MaxValue.TotalSeconds ? TimeSpan.MaxValue : TimeSpan.FromSeconds(_totalSeconds);
            }

            return TimeSpan.Zero;
        }
        #endregion
    }
}