using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace SaveLoadSystem
{
    public enum LogLevel
    {
        None,      // Only errors
        Minimal,   // Warnings and errors
        Detailed   // Everything (info, warnings, errors)
    }

    public enum MessageType
    {
        Info,
        Warning,
        Error
    }

    public static class CustomLogger
    {
        public static LogLevel CurrentLogLevel = LogLevel.Detailed;

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Log(string message, MessageType type = MessageType.Info)
        {
            // Filter based on log level
            switch (CurrentLogLevel)
            {
                case LogLevel.None:
                    if (type != MessageType.Error) return;
                    break;
                case LogLevel.Minimal:
                    if (type == MessageType.Info) return;
                    break;
                case LogLevel.Detailed:
                    break;
            }

            // Get the proper stack trace
            StackFrame frame = new StackFrame(1, true);
            string fileName = frame.GetFileName();
            string methodName = frame.GetMethod()?.Name;
            int lineNumber = frame.GetFileLineNumber();

            // Format the message with caller info
            string callerInfo = $"[{System.IO.Path.GetFileName(fileName)}:{methodName}:{lineNumber}]";
            string formattedMessage = $"{callerInfo} {message}";

            // Use Unity's debug logging system based on message type
            switch (type)
            {
                case MessageType.Info:
                    Debug.Log(formattedMessage);
                    break;
                case MessageType.Warning:
                    Debug.LogWarning(formattedMessage);
                    break;
                case MessageType.Error:
                    Debug.LogError(formattedMessage);
                    break;
            }
        }

        // For making my life easier
        public static void LogInfo(string message) => Log(message, MessageType.Info);
        public static void LogWarning(string message) => Log(message, MessageType.Warning);
        public static void LogError(string message) => Log(message, MessageType.Error);
    }
}