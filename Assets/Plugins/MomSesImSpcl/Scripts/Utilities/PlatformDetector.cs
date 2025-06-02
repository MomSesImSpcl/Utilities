using System.Diagnostics.CodeAnalysis;

// ReSharper disable once RedundantUsingDirective
using UnityEngine;

namespace MomSesImSpcl.Utilities
{
    /// <summary>
    /// Contains helper methods to get information about the current device. <br/>
    /// <b>Latest platform-dependant compilation directives:</b> <br/>
    /// <i>https://docs.unity3d.com/2022.3/Documentation/Manual/PlatformDependentCompilation.html</i>
    /// </summary>
    public static class PlatformDetector
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public enum Platform
        {
            Unknown,
            Editor,
            Windows,
            MacOS,
            Linux,
            Android,
            IOS,
            WindowsHandheld,
            Browser,
            TVOS,
            PlayStation,
            Xbox,
            Switch,
            SteamDeck
        }
        
        /// <summary>
        /// Groups similar <see cref="Platform"/>s into one family.
        /// </summary>
        public enum DeviceFamily
        {
            Unknown,
            /// <summary>
            /// <b>Platforms:</b> <br/>
            /// <i>-<see cref="Platform.Editor"/>, <see cref="Platform.Windows"/>, <see cref="Platform.MacOS"/>, <see cref="Platform.Linux"/>, <see cref="Platform.Browser"/>.</i> <br/>
            /// <b>Input:</b> <br/>
            /// <i>-<see cref="InputSource.MouseAndKeyboard"/>.</i>
            /// </summary>
            Desktop,
            /// <summary>
            /// <b>Platforms:</b> <br/>
            /// <i>-<see cref="Platform.Android"/>, <see cref="Platform.IOS"/>, <see cref="Platform.WindowsHandheld"/>, <see cref="Platform.Browser"/>.</i> <br/>
            /// <b>Input:</b> <br/>
            /// <i>-<see cref="InputSource.Touch"/>.</i>
            /// </summary>
            Mobile,
            /// <summary>
            /// <b>Platforms:</b> <br/>
            /// <i>-<see cref="Platform.PlayStation"/>, <see cref="Platform.Xbox"/>.</i> <br/>
            /// <b>Input:</b> <br/>
            /// <i>-<see cref="InputSource.Controller"/>.</i>
            /// </summary>
            Console,
            /// <summary>
            /// <b>Platforms:</b> <br/>
            /// <i>-<see cref="Platform.Switch"/>, <see cref="Platform.SteamDeck"/>.</i> <br/>
            /// <b>Input:</b> <br/>
            /// <i>-<see cref="InputSource.Controller"/>.</i>
            /// </summary>
            Handheld
        }

        /// <summary>
        /// Contains the most common input sources for every <see cref="DeviceFamily"/>.
        /// </summary>
        public enum InputSource
        {
            Unknown,
            MouseAndKeyboard,
            Touch,
            Controller
        }
        
#if UNITY_WEBGL
        #region Fields
        /// <summary>
        /// JavaScript function to detect if the current device is a mobile device in a WebGL build.
        /// </summary>
        /// <returns><c>true</c> if the current device is a mobile device, otherwise <c>false</c>.</returns>
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern bool IsMobileDevice();
        #endregion
#endif
        #region Properties
        /// <summary>
        /// <see cref="GetCurrentDeviceFamily"/> override for <see cref="Platform.TVOS"/> <br/>
        /// <b>Default:</b> <see cref="DeviceFamily.Console"/>.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public static DeviceFamily TVOSDeviceFamilyOverride { get; set; } = DeviceFamily.Console;
        #endregion
        
        #region Methods
        /// <summary>
        /// Returns the <see cref="Platform"/> the game is currently running on.
        /// </summary>
        /// <returns>The <see cref="Platform"/> the game is currently running on.</returns>
        public static Platform GetCurrentPlatform()
        {
#if UNITY_STANDALONE_WIN
            return Platform.Windows;
#elif UNITY_STANDALONE_OSX
            return Platform.MacOS;
#elif UNITY_STANDALONE_LINUX
#if STEAMWORKS_NET
            if (Steamworks.SteamUtils.IsSteamRunningOnSteamDeck()) // TODO: Check if this is the correct method.
            {
                return Platform.SteamDeck;
            }
#endif
            return Platform.Linux;
#elif UNITY_WSA
            if (Application.platform is RuntimePlatform.XboxOne or RuntimePlatform.GameCoreXboxSeries or RuntimePlatform.GameCoreXboxOne)
            {
                return Platform.Xbox;
            }
            
#if ENABLE_WINMD_SUPPORT // Will never be enabled in the Unity Editor, only after the game has been build for UWP.
            return Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily switch
            {
                "Windows.Desktop" => Platform.Windows,
                "Windows.Mobile" => Platform.WindowsHandheld,
                "Windows.Xbox" => Platform.Xbox,
                _ => Platform.Unknown,
            };
#elif UNITY_EDITOR // Prevents compiler errors in editor.
            return Platform.Editor;
#endif
#elif UNITY_ANDROID
            return Platform.Android;
#elif UNITY_IOS
            return Platform.IOS;
#elif UNITY_WEBGL
            return Platform.Browser;
#elif UNITY_TVOS
            return Platform.TVOS;
#elif UNITY_EDITOR
            return Platform.Editor;
#else // TODO: Check if Playstation and Switch have preprocessor directives.
            return Application.platform switch
            {
                RuntimePlatform.PS4 or RuntimePlatform.PS5 => Platform.PlayStation,
                RuntimePlatform.Switch => Platform.Switch,
                _ => Platform.Unknown
            };
#endif
        }
        
        /// <summary>
        /// Returns the <see cref="DeviceFamily"/> for the current <see cref="Platform"/>.
        /// </summary>
        /// <returns>The <see cref="DeviceFamily"/> for the current <see cref="Platform"/>.</returns>
        public static DeviceFamily GetCurrentDeviceFamily()
        {
            return GetCurrentPlatform() switch
            {
                Platform.Editor => DeviceFamily.Desktop,
                Platform.Windows => DeviceFamily.Desktop,
                Platform.MacOS => DeviceFamily.Desktop,
                Platform.Linux => DeviceFamily.Desktop,
                Platform.Android => DeviceFamily.Mobile,
                Platform.IOS => DeviceFamily.Mobile,
                Platform.WindowsHandheld => DeviceFamily.Mobile,
#if UNITY_WEBGL
                Platform.Browser => SystemInfo.deviceType == DeviceType.Handheld || IsMobileDevice() ? DeviceFamily.Mobile : DeviceFamily.Desktop,
#endif
                Platform.TVOS => TVOSDeviceFamilyOverride,
                Platform.PlayStation => DeviceFamily.Console,
                Platform.Xbox => DeviceFamily.Console,
                Platform.Switch => DeviceFamily.Handheld,
                Platform.SteamDeck => DeviceFamily.Handheld,
                _ => DeviceFamily.Unknown,
            };
        }

        /// <summary>
        /// Returns the default <see cref="InputSource"/> for the current <see cref="DeviceFamily"/>.
        /// </summary>
        /// <param name="_DeviceFamily">
        /// If <c>null</c>, returns the <see cref="InputSource"/> for the current <see cref="DeviceFamily"/>, <br/>
        /// otherwise returns the <see cref="InputSource"/> for the given <see cref="DeviceFamily"/>.
        /// </param>
        /// <returns>The default <see cref="InputSource"/> for the current <see cref="DeviceFamily"/>.</returns>
        public static InputSource GetInputSource(DeviceFamily? _DeviceFamily = null)
        {
            return (_DeviceFamily ?? GetCurrentDeviceFamily()) switch
            {
                DeviceFamily.Desktop => InputSource.MouseAndKeyboard,
                DeviceFamily.Mobile => InputSource.Touch,
                DeviceFamily.Console => InputSource.Controller,
                DeviceFamily.Handheld => InputSource.Controller,
                _ => InputSource.Unknown
            };
        }
        #endregion
    }
}
