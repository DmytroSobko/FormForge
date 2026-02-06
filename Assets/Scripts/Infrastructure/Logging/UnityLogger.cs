using UnityEngine;

namespace FormForge.Infrastructure.Logging
{
    public sealed class UnityLogger : ILogger
    {
        private readonly string m_Tag;

        public UnityLogger(string tag = "FormForge")
        {
            m_Tag = tag;
        }

        public void Log(string message)
        {
            Debug.Log(Format(message));
        }

        public void LogWarning(string message)
        {
            Debug.LogWarning(Format(message));
        }

        public void LogError(string message)
        {
            Debug.LogError(Format(message));
        }

        public void LogException(System.Exception exception)
        {
            Debug.LogException(exception);
        }

        private string Format(string message)
        {
            return $"[{m_Tag}] {message}";
        }
    }
}