using UnityEngine;

namespace FormForge.AssetManagement
{
    /// <summary>
    /// Provides helper methods for logging messages, warnings, and errors.
    /// This utility allows for optional custom loggers while defaulting to Unity's Debug logger.
    /// </summary>
    public static class LoggerHelper
    {
        /// <summary>
        /// Logs an informational message using the specified logger or Unity's default logger.
        /// </summary>
        /// <typeparam name="T">The type whose name will be used as the log category.</typeparam>
        /// <param name="msg">The message to log.</param>
        /// <param name="logger">Optional custom logger. Defaults to <see cref="Debug.unityLogger"/> if null.</param>
        public static void Log<T>(string msg, ILogger logger = null)
        {
            logger ??= Debug.unityLogger;
            logger.Log("<b>" + typeof(T).Name + "</b>", msg);
        }

        /// <summary>
        /// Logs a warning message using the specified logger or Unity's default logger.
        /// </summary>
        /// <typeparam name="T">The type whose name will be used as the log category.</typeparam>
        /// <param name="msg">The warning message to log.</param>
        /// <param name="logger">Optional custom logger. Defaults to <see cref="Debug.unityLogger"/> if null.</param>
        public static void LogWarning<T>(string msg, ILogger logger = null)
        {
            logger ??= Debug.unityLogger;
            logger.LogWarning("<b>" + typeof(T).Name + "</b>", msg);
        }

        /// <summary>
        /// Logs an error message using the specified logger or Unity's default logger.
        /// </summary>
        /// <typeparam name="T">The type whose name will be used as the log category.</typeparam>
        /// <param name="msg">The error message to log.</param>
        /// <param name="logger">Optional custom logger. Defaults to <see cref="Debug.unityLogger"/> if null.</param>
        public static void LogError<T>(string msg, ILogger logger = null)
        {
            logger ??= Debug.unityLogger;
            logger.LogError("<b>" + typeof(T).Name + "</b>", msg);
        }
    }
}
