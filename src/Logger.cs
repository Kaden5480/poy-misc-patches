using System;

namespace MiscPatches {
    /**
     * <summary>
     * A class for helping with creating
     * more consistent and informational logging.
     * </summary>
     */
    internal class Logger {
        private Type type;

        /**
         * <summary>
         * Initializes a Logger.
         * </summary>
         * <param name="type">The type this logger is for</param>
         */
        internal Logger(Type type) {
            this.type = type;
        }

        /**
         * <summary>
         * Logs a debug message.
         * </summary>
         * <param name="message">The message to log</param>
         */
        internal void LogDebug(string message) {
            Plugin.LogDebug($"[{type}] {message}");
        }

        /**
         * <summary>
         * Logs an informational message.
         * </summary>
         * <param name="message">The message to log</param>
         */
        internal void LogInfo(string message) {
            Plugin.LogInfo($"[{type}] {message}");
        }

        /**
         * <summary>
         * Logs an error message.
         * </summary>
         * <param name="message">The message to log</param>
         */
        internal void LogError(string message) {
            Plugin.LogError($"[{type}] {message}");
        }
    }
}
