using System;
using System.Diagnostics;
using MomSesImSpcl.Extensions;
using Debug = UnityEngine.Debug;

namespace MomSesImSpcl.Utilities
{
    /// <summary>
    /// Call <see cref="MeasureTime"/>.<see cref="MeasureTime.GetTimeStamp"/> and dispose it with a <c>using</c>/block to print the elapsed time.
    /// </summary>
    public readonly struct MeasureTime : IDisposable
    {
        #region Fields
        /// <summary>
        /// The starting timestamp.
        /// </summary>
        private readonly long timeStamp;
        #endregion
        
        #region Properties
        /// <summary>
        /// Sets the starting timestamp from where to measure.
        /// </summary>
        public static MeasureTime GetTimeStamp => new(true);
        #endregion
        
        #region Constructors
        /// <summary>
        /// Assigns the starting timestamp to <see cref="timeStamp"/>.
        /// </summary>
        /// <param name="_">Not needed.</param>
        // ReSharper disable once UnusedParameter.Local
        private MeasureTime(bool _)
        {
            this.timeStamp = Stopwatch.GetTimestamp();
        }
        #endregion
        
        #region Methods
        public void Dispose()
        {
            Debug.Log((Stopwatch.GetTimestamp() - this.timeStamp).ToString().Bold());
        }
        #endregion
    }
}