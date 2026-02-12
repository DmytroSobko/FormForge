using UnityEngine;

namespace FormForge.Infrastructure.Logging
{
    public sealed class UnityLogger : ILogger
    {
        private readonly string m_Tag;
        
        public UnityLogger(string tag = "FormForge")
        {
            m_Tag = "<b>" + tag + "</b>";
        }

        public void Log(string message)
        {
            Debug.unityLogger.Log(m_Tag, message);
        }

        public void LogWarning(string message)
        {
            Debug.unityLogger.LogWarning(m_Tag, message);
        }

        public void LogError(string message)
        {
            Debug.unityLogger.LogError(m_Tag, message);
        }

        public void LogException(System.Exception exception)
        {
            Debug.unityLogger.LogException(exception);
        }
    }
}