using System;
using System.Diagnostics;
using System.Globalization;
using MomSesImSpcl.Extensions;
using Debug = UnityEngine.Debug;

namespace MomSesImSpcl.Utilities
{
    /// <summary>
    /// Call one of the properties with/inside a <c>using</c>-statement/block and the elapsed time will be printed to the console on <see cref="Dispose"/>.
    /// </summary>
    public readonly struct MeasureTime : IDisposable
    {
        #region Fields
        /// <summary>
        /// The <see cref="TimeResolution"/> to measure in.
        /// </summary>
        private readonly TimeResolution timeResolution;
        /// <summary>
        /// The starting timestamp.
        /// </summary>
        private readonly decimal timeStamp;
        #endregion
        
        #region Properties
        /// <summary>
        /// Sets the starting timestamp from where to measure in <see cref="TimeResolution.Auto"/>.
        /// </summary>
        public static MeasureTime GetTime => new(TimeResolution.Auto);
        /// <summary>
        /// Sets the starting timestamp from where to measure in <see cref="TimeResolution.Seconds"/>.
        /// </summary>
        public static MeasureTime GetSeconds => new(TimeResolution.Seconds);
        /// <summary>
        /// Sets the starting timestamp from where to measure in <see cref="TimeResolution.MilliSeconds"/>.
        /// </summary>
        public static MeasureTime GetMilliSeconds => new(TimeResolution.MilliSeconds);
        /// <summary>
        /// Sets the starting timestamp from where to measure in <see cref="TimeResolution.MicroSeconds"/>.
        /// </summary>
        public static MeasureTime GetMicroSeconds => new(TimeResolution.MicroSeconds);
        /// <summary>
        /// Sets the starting timestamp from where to measure in <see cref="TimeResolution.NanoSeconds"/>.
        /// </summary>
        public static MeasureTime GetNanoSeconds => new(TimeResolution.NanoSeconds);
        /// <summary>
        /// Sets the starting timestamp from where to measure in <see cref="TimeResolution.Ticks"/>.
        /// </summary>
        public static MeasureTime GetTicks => new(TimeResolution.Ticks);
        #endregion
        
        #region Constructors
        /// <summary>
        /// Assigns the starting timestamp to <see cref="timeStamp"/>.
        /// </summary>
        /// <param name="_TimeResolution"><see cref="timeResolution"/>.</param>
        private MeasureTime(TimeResolution _TimeResolution)
        {
            this.timeResolution = _TimeResolution;
            this.timeStamp = Stopwatch.GetTimestamp();
        }
        #endregion
        
        #region Methods
        public void Dispose()
        {
            var _timeStamp = (decimal)Stopwatch.GetTimestamp();
            var _elapsedTicks = _timeStamp - this.timeStamp;
            var _frequency = (decimal)Stopwatch.Frequency;
            var _timeResolution = this.timeResolution;
                
#pragma warning disable CS8524
            var _elapsedTime = this.timeResolution switch
#pragma warning restore CS8524
            {
                TimeResolution.Auto => GetAppropriateTimeResolution(_elapsedTicks, _frequency, out _timeResolution),
                TimeResolution.Seconds => CalculateElapsedTime(TimeResolution.Seconds, _elapsedTicks, _frequency),
                TimeResolution.MilliSeconds => CalculateElapsedTime(TimeResolution.MilliSeconds, _elapsedTicks, _frequency),
                TimeResolution.MicroSeconds => CalculateElapsedTime(TimeResolution.MicroSeconds, _elapsedTicks, _frequency),
                TimeResolution.NanoSeconds => CalculateElapsedTime(TimeResolution.NanoSeconds, _elapsedTicks, _frequency),
                TimeResolution.Ticks => _elapsedTicks,
            };  
            
            var _elapsedTimeString = _elapsedTime.ToString("F29", CultureInfo.InvariantCulture).TrimEnd('0').TrimEnd('.');
            Debug.Log($"Elapsed Time: {_elapsedTimeString.Bold()} {_timeResolution.GetName()}.");
        }

        /// <summary>
        /// Calculates the appropriate <see cref="TimeResolution"/> based on the given <c>_ElapsedTicks</c>.
        /// </summary>
        /// <param name="_ElapsedTicks">The ticks to calculate the <see cref="TimeResolution"/> for.</param>
        /// <param name="_Frequency"><see cref="Stopwatch.Frequency"/>.</param>
        /// <param name="_TimeResolution">The <see cref="TimeResolution"/> that was calculated with.</param>
        /// <returns><c>_ElapsedTicks</c> converted to the appropriate <see cref="TimeResolution"/>.</returns>
        private static decimal GetAppropriateTimeResolution(decimal _ElapsedTicks, decimal _Frequency, out TimeResolution _TimeResolution)
        {
            decimal _elapsedTime;
            
            switch (_ElapsedTicks)
            {
                case >= 100_000:
                    _elapsedTime = CalculateElapsedTime(TimeResolution.Seconds, _ElapsedTicks, _Frequency);
                    _TimeResolution = TimeResolution.Seconds;
                    break;
                case >= 10_000:
                    _elapsedTime = CalculateElapsedTime(TimeResolution.MilliSeconds, _ElapsedTicks, _Frequency);
                    _TimeResolution = TimeResolution.MilliSeconds;
                    break;
                case >= 1_000:
                    _elapsedTime = CalculateElapsedTime(TimeResolution.MicroSeconds, _ElapsedTicks, _Frequency);
                    _TimeResolution = TimeResolution.MicroSeconds;
                    break;
                case >= 100:
                    _elapsedTime = CalculateElapsedTime(TimeResolution.NanoSeconds, _ElapsedTicks, _Frequency);
                    _TimeResolution = TimeResolution.NanoSeconds;
                    break;
                default:
                    _elapsedTime = _ElapsedTicks;
                    _TimeResolution = TimeResolution.Ticks;
                    break;
            }

            return _elapsedTime;
        }

        /// <summary>
        /// Calculates the elapsed time based on the given <see cref="TimeResolution"/>.
        /// </summary>
        /// <param name="_TimeResolution">The <see cref="TimeResolution"/> to calculate with.</param>
        /// <param name="_ElapsedTicks">The value to convert to the given <see cref="TimeResolution"/>.</param>
        /// <param name="_Frequency"><see cref="Stopwatch.Frequency"/>.</param>
        /// <returns><c>_ElapsedTicks</c> converted to the given <see cref="TimeResolution"/>.</returns>
        private static decimal CalculateElapsedTime(TimeResolution _TimeResolution, decimal _ElapsedTicks, decimal _Frequency)
        {
            return _TimeResolution switch
            {
                TimeResolution.Seconds => _ElapsedTicks / _Frequency,
                TimeResolution.MilliSeconds => _ElapsedTicks * (1_000m / _Frequency),
                TimeResolution.MicroSeconds => _ElapsedTicks * (1_000_000m / _Frequency),
                TimeResolution.NanoSeconds => _ElapsedTicks * (1_000_000_000m / _Frequency),
                _ => _ElapsedTicks
            };
        }
        #endregion
    }
}
