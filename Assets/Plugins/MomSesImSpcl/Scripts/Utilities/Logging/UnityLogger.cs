#if !UNITY_WEBGL
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using JetBrains.Annotations;
using MomSesImSpcl.Utilities.Pooling;
using MomSesImSpcl.Utilities.Pooling.Wrappers;
using UnityEngine;

namespace MomSesImSpcl.Utilities.Logging
{
    /// <summary>
    /// Logs debug messages in build to a .txt file.
    /// </summary>
    public static class UnityLogger
    {
        #region Constants
        /// <summary>
        /// Separator between each log entry.
        /// </summary>
        private const string SEPARATOR = "----------------------------------------------------------------------------";
        #endregion
        
        #region Fields
        /// <summary>
        /// Set to <c>true</c> to enable the <see cref="UnityLogger"/>.
        /// </summary>
        private static bool enableLogger;
        /// <summary>
        /// The <see cref="LogType"/>s to exclude from the <c>Log.txt</c> file.
        /// </summary>
        private static HashSet<LogType> excludedLogTypes;
        /// <summary>
        /// The file path of the error log .txt file.
        /// </summary>
        private static string filePath;
        /// <summary>
        /// <see cref="FileStream"/>.
        /// </summary>
        [CanBeNull] private static FileStream fileStream;
        /// <summary>
        /// <see cref="StreamWriter"/>.
        /// </summary>
        [CanBeNull] private static StreamWriter streamWriter;
        /// <summary>
        /// Prevents race conditions for the <see cref="streamWriter"/>.
        /// </summary>
        private static SemaphoreSlim semaphoreSlim;
        #endregion
        
        #region Methods
        /// <summary>
        /// Enables the <see cref="UnityLogger"/>. <br/>
        /// <b>Must be set during <see cref="RuntimeInitializeLoadType.SubsystemRegistration"/>.</b>
        /// </summary>
        /// <param name="_LogTypesToExclude"><see cref="excludedLogTypes"/>.</param>
        public static void EnableLogger(params LogType[] _LogTypesToExclude)
        {
            enableLogger = true;
            excludedLogTypes = new HashSet<LogType>(_LogTypesToExclude);
            semaphoreSlim = new SemaphoreSlim(1, 1);
        }
        
        /// <summary>
        /// Initializes the <see cref="UnityLogger"/> if <see cref="enableLogger"/> is set to <c>true</c>.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Init()
        {
            if (!enableLogger)
            {
                return;
            }
            
            try
            {
                var _directoryPath = Application.dataPath;
#if UNITY_EDITOR // Will be the "Assets"-Folder in Editor.
                _directoryPath = Directory.GetParent(_directoryPath)!.FullName;
#endif
                filePath = Path.Combine(_directoryPath, "Log.txt");
                fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                streamWriter = new StreamWriter(fileStream);

                ObjectPools.ConcurrentStringBuilderPool ??= new ConcurrentCustomPool<ConcurrentStringBuilderPoolWrapper>(1);
            }
            catch (Exception _exception)
            {
                Debug.LogException(_exception);
            }
            
            if (streamWriter == null)
            {
                return;
            }

            Application.logMessageReceivedThreaded += OnLogMessageReceived;
            Application.quitting += OnApplicationQuit;
        }

        /// <summary>
        /// Writes the message to the <c>Log.txt</c> file.
        /// </summary>
        /// <param name="_Condition">The message.</param>
        /// <param name="_Stacktrace">The stacktrace</param>
        /// <param name="_LogType"><see cref="LogType"/>.</param>
        // ReSharper disable once AsyncVoidMethod
        private static async void OnLogMessageReceived(string _Condition, string _Stacktrace, LogType _LogType)
        {
            if (excludedLogTypes.Contains(_LogType))
            {
                return;
            }
            
            try
            {
                var _poolWrapper = await ObjectPools.ConcurrentStringBuilderPool.GetAsync();
             
                _poolWrapper.StringBuilder.Append(_LogType.ToString());
                _poolWrapper.StringBuilder.Append(Environment.NewLine);
                _poolWrapper.StringBuilder.Append(_Condition);
                _poolWrapper.StringBuilder.Append(Environment.NewLine);
                _poolWrapper.StringBuilder.Append(_Stacktrace);
                _poolWrapper.StringBuilder.Append(SEPARATOR);
                _poolWrapper.StringBuilder.Append(Environment.NewLine);

                await semaphoreSlim.WaitAsync();
                
                try
                {
                    await streamWriter!.WriteAsync(_poolWrapper.Return());
                    await streamWriter!.FlushAsync(); 
                }
                finally
                {
                    semaphoreSlim.Release();
                }
            }
            catch (Exception _exception)
            {
                Debug.LogException(_exception);
            }
        }
        
        /// <summary>
        /// Closes the <see cref="streamWriter"/>.
        /// </summary>
        private static void OnApplicationQuit()
        {
            try
            {
                streamWriter?.Close();
            }
            catch (Exception _exception)
            {
                Debug.LogException(_exception);
            }
        }
        #endregion
    }
}
#endif