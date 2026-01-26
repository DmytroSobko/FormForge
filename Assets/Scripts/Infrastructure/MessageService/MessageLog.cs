using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using FormForge.Messaging.Interfaces;
using UnityEngine;

namespace FormForge.Messaging
{
    internal struct MessageLog : IComparable<MessageLog>
    {
        public static readonly MessageLog NO_LOG;
        
        private static uint CreationIndexCount = 0;
        
        private uint m_creationIndex;
        
        private readonly int m_frame;
        private readonly float m_time;
        private readonly Type m_messageType;
        private readonly string m_payloadDetails;
        private readonly string m_filePath;
        private readonly int m_lineNumber;
        private readonly IMessageReceiver m_messageReceiver;
        
        private string m_message;

        internal uint LogID => m_creationIndex;
        internal string PayloadDetails => m_payloadDetails;
        internal string FilePath => m_filePath;
        internal int LineNumber => m_lineNumber;
        internal IMessageReceiver MessageReceiver => m_messageReceiver;
        
        internal string Message
        {
            get
            {
                if (string.IsNullOrEmpty(m_message))
                {
                    StringBuilder messageBuilder = new StringBuilder($"[{FormatTime(m_time)}] {m_messageType.Name} sent (from {Path.GetFileName(m_filePath)}:{m_lineNumber}) to {m_messageReceiver.GetType().Name}");
                    if (m_messageReceiver is Component receiverComponent)
                    {
                        messageBuilder.Append($" on {receiverComponent.gameObject.name}");
                    }

                    m_message = messageBuilder.ToString();
                }

                return m_message;
            }
        }
        
        public MessageLog(Type messageType, IMessageReceiver messageReceiver, string payloadDetails, StackFrame invokingFrame)
        {
            m_frame = Time.frameCount;
            m_time = Time.time;
            m_creationIndex = ++CreationIndexCount;
            m_messageType = messageType;
            m_payloadDetails = payloadDetails;
            m_messageReceiver = messageReceiver;

            if (invokingFrame != null)
            {
                m_filePath = invokingFrame.GetFileName();
                m_lineNumber = invokingFrame.GetFileLineNumber();
            }
            else
            {
                m_filePath = "Unknown";
                m_lineNumber = 0;
            }

            m_message = string.Empty;
        }

        static MessageLog()
        {
            NO_LOG = new MessageLog(typeof(MessageLog), null, string.Empty, null);
            NO_LOG.m_creationIndex = 0;
        }

        public int CompareTo(MessageLog other)
        {
            int frameComparison = m_frame.CompareTo(other.m_frame);
            if (frameComparison == 0)
            {
                int timeComparison = m_time.CompareTo(other.m_time);
                if (timeComparison == 0)
                {
                    return m_creationIndex.CompareTo(other.m_creationIndex);
                }
            }
            return frameComparison;
        }

        private string FormatTime(float time)
        {
            int minutes = (int)(time / 60);
            int seconds = (int)(time % 60);
            int milliseconds = (int)((time * 1000) % 1000);

            return $"{minutes}:{seconds:00}:{milliseconds:000}";
        }
    }
}