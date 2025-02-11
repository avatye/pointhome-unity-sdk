using System;
using UnityEngine;

namespace Avatye.Pointhome.Util
{
    internal static class LogTracer
    {
        private const string TAG = "PHUnity";
        private const string CONSOLE_TAG = "ph_unity_log";

        private static bool isDebugMode = true;


        internal static void SetLogEnabled(bool isLogEnable)
        {
            isDebugMode = isLogEnable;
        }

        // Info
        internal static void I(string message, string tag = TAG)
        {
            if (isDebugMode)
            {
                MakeLog(message, LogType.Log, tag);
            }
        }

        // Warning
        internal static void W(string message, string tag = TAG)
        {
            if (isDebugMode)
            {
                MakeLog(message, LogType.Warning, tag);
            }
        }

        // Error
        internal static void E(string message, string tag = TAG)
        {
            if (isDebugMode)
            {
                MakeLog(message, LogType.Error, tag);
            }
        }

        // Exception
        internal static void Exception(Exception exception, string tag = TAG)
        {
            if (isDebugMode)
            {
                string message = exception.Message;
                message += $"\n{exception.StackTrace}";
                MakeLog(message, LogType.Exception, tag);
            }
        }


        private static void MakeLog(string message, LogType logType, string tag)
        {
            string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{tag}] {message}";

            // Unity Editor: Unity 에디터 환경에서 실행.
            // Development Build: 빌드된 개발자 버전에서 실행.

            switch (logType)
            {
                case LogType.Warning:
                    Debug.LogWarning(logMessage);
                    break;
                case LogType.Error:
                case LogType.Exception:
                    Debug.LogError(logMessage);
                    break;
                default:
                    Debug.Log(logMessage);
                    break;
            }
        }


        // 콘솔에서 특정 명령어 입력 시 로그 활성화
        [RuntimeInitializeOnLoadMethod]
        private static void InitializeLogCommand()
        {
            Application.logMessageReceived += HandleConsoleInput;
        }
        private static void HandleConsoleInput(string condition, string stackTrace, LogType type)
        {
            if (condition.Contains(CONSOLE_TAG))
            {
                SetLogEnabled(true);
            }
        }
    }
}
