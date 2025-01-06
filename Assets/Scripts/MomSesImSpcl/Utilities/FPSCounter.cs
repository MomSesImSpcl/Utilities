using MomSesImSpcl.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace MomSesImSpcl.Utilities
{
    /// <summary>
    /// The FPSCounter class provides functionality to measure and display the frames per second (FPS) in a Unity application.
    /// </summary>
    /// <remarks>
    /// This class requires a Text component to be attached to the same GameObject.
    /// </remarks>
    [RequireComponent(typeof(Text))]
    public class FPSCounter : MonoBehaviour
    {
        #region Inspector Fields
        [Tooltip("Time in seconds between the measurements")]
        [SerializeField] private float measurePeriod = .5f;
        #endregion

        #region Fields
        /// <summary>
        /// The Text component used to display the FPS on the UI.
        /// </summary>
        private Text text;
        /// <summary>
        /// Stores the number of frames rendered within the measurement period.
        /// </summary>
        private int counter;
        /// <summary>
        /// The time, in seconds, at which the next FPS measurement will be taken.
        /// </summary>
        private float nextMeasurement;
        /// <summary>
        /// Stores the current frames per second (FPS) calculated by the FPSCounter.
        /// </summary>
        private int currentFPS;
        #endregion
        
        #region Methods
        private void Awake()
        {
            this.text = base.GetComponent<Text>();
        }
        
        private void Start()
        {
            this.nextMeasurement = Time.realtimeSinceStartup + measurePeriod;
        }
        
        private void Update()
        {
            this.CalculateFPS();
        }

        /// <summary>
        /// Calculates the frames per second (FPS) by measuring the number of frames rendered within
        /// a specified measurement period. Updates the display Text component with the calculated FPS.
        /// </summary>
        private void CalculateFPS()
        {
            this.counter++;

            if (Time.realtimeSinceStartup > this.nextMeasurement)
            {
                this.currentFPS = (int)(this.counter / this.measurePeriod);
                this.counter = 0;
                this.nextMeasurement += this.measurePeriod;

                if (this.text is not null)
                {
                    this.text.text = $"FPS: {this.currentFPS.ToString()}";   
                }
            }
        }
        #endregion
    }
}