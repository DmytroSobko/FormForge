using System;
using System.Collections.Generic;
using System.Reflection;
using FormForge.Core.Services;
using FormForge.Messaging;
using FormForge.Messaging.Interfaces;
using UnityEngine;
using UnityEngine.Scripting;

namespace FormForge.Core.Messaging
{
    /// <inheritdoc/>
    internal class MessageService : IMessageService
    {
        /// <summary>
        /// A delegate type matching the signature of <see cref="Messenger{T}.SendGeneric" />, used with <see cref="MethodInfo.CreateDelegate" /> to
        /// avoid having to use <see cref="MethodInfo.Invoke" /> when sending messages.
        /// </summary>
        private delegate void SendMessageGeneric(Type targetType, object msg);
        
        private Dictionary<Type, IMessenger> m_messengers = new Dictionary<Type, IMessenger>();
        private Dictionary<Type, SendMessageGeneric> m_sendGenericDelegates = 
            new Dictionary<Type, SendMessageGeneric>();

        [Preserve]
        public MessageService()
        {
            
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void RegisterSelf()
        {
            ServiceLocator.RegisterService<IMessageService, MessageService>(ServiceLifespan.LazySingleton);
        }
        
        public void Register<T>(IMessageReceiver<T> messageReceiver) where T : class
        {
            Type messageType = typeof(T);
            if (m_messengers.TryAdd(messageType, new Messenger<T>()))
            {
                var delegateMethod = (SendMessageGeneric) typeof(Messenger<>).MakeGenericType(messageType)
                    .GetMethod("SendGeneric", BindingFlags.Instance | BindingFlags.NonPublic)
                    .CreateDelegate(typeof(SendMessageGeneric), m_messengers[messageType]);
                m_sendGenericDelegates[messageType] = delegateMethod;
                m_messengers[messageType].Initialize();
            }

            Messenger<T> messenger = (Messenger<T>)m_messengers[messageType];
            messenger.Register(messageReceiver);
        }

        public void Unregister<T>(IMessageReceiver<T> messageReceiver) where T : class
        {
            Type messageType = typeof(T);
            if (m_messengers.TryGetValue(messageType, out IMessenger messenger))
            {
                Messenger<T> typedMessenger = (Messenger<T>)messenger;
                typedMessenger.Unregister(messageReceiver);
            }
        }

        public void Send<T>(T data = null) where T : class
        {
            Type messageType = typeof(T);
            if (m_messengers.TryGetValue(messageType, out IMessenger messenger))
            {
                Messenger<T> typedMessenger = (Messenger<T>)messenger;
                typedMessenger.Send(data);
            }
        }

        public void Send(Type messageType, object data)
        {
            if (m_messengers.TryGetValue(messageType, out IMessenger messenger))
            {
                var method = m_sendGenericDelegates[messageType];
                if (method != null)
                {
                    method(messageType, data);
                }
                else
                {
#if PRIME_DEBUG
                    throw new Exception($"Send method delegate not found for Messenger<{messageType}>");
#else
                    Debug.LogError($"Send method delegate not found for Messenger<{messageType}> Your message will not be sent.");
#endif
                }
            }
            else
            {
                Debug.LogWarning($"Messenger not found for type {messageType}. Your message will not be sent.");
            }
        }
    }
}
