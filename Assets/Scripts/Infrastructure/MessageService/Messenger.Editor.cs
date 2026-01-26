using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using FormForge.Core.Services;
using FormForge.Messaging.Interfaces;
using UnityEngine;

namespace FormForge.Messaging
{
    internal partial class Messenger<T> : IMessageReceiver<ClearLogsMessage>
    {
        private SortedSet<MessageLog> MessageLogs { get; set; }

        public void HandleMessage(ClearLogsMessage messageData)
        {
            if (messageData.ClearAllLogs)
            {
                MessageLogs.Clear();
                return;
            }

            if (messageData.MessageType == typeof(T))
            {
                MessageLogs.Clear();
            }
        }

        [Conditional("UNITY_EDITOR")]
        private void InitializeForEditor()
        {
            // Used by Message Tracking only while playing
            if (Application.isPlaying)
            {
                ServiceLocator.GetService<IMessageService>().Register<ClearLogsMessage>(this);
            }
        }

        [Conditional("UNITY_EDITOR")]
        private void LogMessage(Type messageType, T messageData, IMessageReceiver<T> messageReceiver)
        {
            if (MessageLogs == null)
            {
                MessageLogs = new SortedSet<MessageLog>();
            }

            StackTrace trace = new StackTrace(true);
            StackFrame invokingFrame = null;
            for (int i = 0; i < trace.FrameCount; i++)
            {
                var frame = trace.GetFrame(i);
                if (MessageLogHelper.IsValidInvokingFrame(frame))
                {
                    invokingFrame = frame;
                    break;
                }
            }

            MessageLogs.Add(new MessageLog(messageType, messageReceiver, MessageLogHelper.BuildMessagePayloadLog(messageData), invokingFrame));
        }
    }
    
    internal class ClearLogsMessage
    {
        public Type MessageType;
        public bool ClearAllLogs;

        public ClearLogsMessage(Type messageType, bool clearAllLogs = false)
        {
            MessageType = messageType;
            ClearAllLogs = clearAllLogs;
        }
    }

    internal static class MessageLogHelper
    {
        private static StringBuilder m_payloadBuilder;

        private static StringBuilder PayloadBuilder
        {
            get
            {
                if (m_payloadBuilder == null)
                {
                    m_payloadBuilder = new StringBuilder(512);
                }

                return m_payloadBuilder;
            }
        }

        internal static string BuildMessagePayloadLog<T>(T messageData)
        {
            const string NoPayloadDetails = "No Payload data found, its possible there is no data associated with this message.\nOnly fields and properties will be detected.";
            if (messageData == null)
            {
                return NoPayloadDetails;
            }

            PayloadBuilder.Clear();
            Type type = typeof(T);

            foreach (FieldInfo field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                object value = field.GetValue(messageData);
                PayloadBuilder.AppendLine($"{field.FieldType.Name} {field.Name} = {value}");
            }

            foreach (PropertyInfo property in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (property.CanRead)
                {
                    object value = property.GetValue(messageData);
                    PayloadBuilder.AppendLine($"{property.PropertyType.Name} {property.Name} = {value}");
                }
            }

            return PayloadBuilder.ToString();
        }

        internal static bool IsValidInvokingFrame(StackFrame invokingFrame)
        {
            Type declaringType = invokingFrame.GetMethod().DeclaringType;
            if (declaringType != null)
            {
                if (!string.IsNullOrEmpty(declaringType.Namespace) &&
                    !declaringType.Namespace.Contains("Prime.Core.Messaging"))
                {
                    return true;
                }

                if (typeof(Component).IsAssignableFrom(declaringType))
                {
                    return true;
                }
            }

            return false;
        }
    }
}

