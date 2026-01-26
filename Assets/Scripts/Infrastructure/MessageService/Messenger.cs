using System;
using System.Collections.Generic;
using FormForge.Messaging.Interfaces;
using FormForge.Utils;
using UnityEngine;

namespace FormForge.Messaging
{
    /// <summary>
    /// The dedicated handler for messages of the given type. All registered <see cref="IMessageReceiver"/>s
    /// are collected within their given Messenger, which is then responsible to actually sending messages to them.
    /// </summary>
    /// <typeparam name="T">The type of message this Messenger is responsible for.</typeparam>
    internal partial class Messenger<T> : IMessenger where T : class
    {
        private HashSet<WeakReferenceWrapper> m_messageReceivers = new HashSet<WeakReferenceWrapper>();
        private HashSet<WeakReferenceWrapper> m_receiversSnapshot = new HashSet<WeakReferenceWrapper>();

        public void Initialize()
        {
            InitializeForEditor();
        }

        /// <summary>
        /// Adds an <see cref="IMessageReceiver{T}"/> to the list of receivers that will get messages of
        /// the type managed by this Messenger.
        /// </summary>
        /// <param name="messageReceiver">The receiver that will receive messages.</param>
        internal void Register(IMessageReceiver<T> messageReceiver)
        {
            var weakReferencedReceiver = new WeakReferenceWrapper(messageReceiver);
            m_messageReceivers.Add(weakReferencedReceiver);
        }

        /// <summary>
        /// Removes an <see cref="IMessageReceiver{T}"/> from the list of receivers that get messages of
        /// the type managed by this Messenger.
        /// </summary>
        /// <param name="messageReceiver">The receiver that will no longer receive messages.</param>
        internal void Unregister(IMessageReceiver<T> messageReceiver)
        {
            var weakReferencedReceiver = new WeakReferenceWrapper(messageReceiver);
            m_messageReceivers.Remove(weakReferencedReceiver);
        }

        /// <summary>
        /// Broadcasts the message to all <see cref="IMessageReceiver{T}"/>'s that are registered to receive
        /// the managed message type.
        /// </summary>
        /// <param name="messageData">An optional data object that may be passed along to Receivers when handling the message.</param>
        internal void Send(T messageData = null)
        {
            m_receiversSnapshot.UnionWith(m_messageReceivers);

            foreach (WeakReferenceWrapper weakReferenceReceiver in m_receiversSnapshot)
            {
                if (weakReferenceReceiver.TryGetTarget(out IMessageReceiver<T> messageReceiver))
                {
                    if (messageReceiver is Component messageReceiverComponent && messageReceiverComponent == null)
                    {
                        m_messageReceivers.Remove(weakReferenceReceiver);
                    }
                    else
                    {
                        LogMessage(typeof(T), messageData, messageReceiver);
                        messageReceiver.HandleMessage(messageData);
                    }
                }
                else
                {
                    m_messageReceivers.Remove(weakReferenceReceiver);
                }
            }

            m_receiversSnapshot.Clear();
        }

        /// <summary>
        /// A version of <see cref="Send" /> that doesn't require knowledge of the type T. This lets callers create non-generic delegates to improve
        /// runtime performance and avoid reflection.
        /// </summary>
        /// <remarks>
        /// This method will throw an exception if <see cref="targetType" /> doesn't match the type parameter T of the Messenger instance.
        /// </remarks>
        internal void SendGeneric(Type targetType, object value)
        {
            if (typeof(T) != targetType)
            {
#if PRIME_DEBUG
                throw new Exception("Incorrect type of object passed to SendGeneric!");
#else
                Debug.LogError("Incorrect type of object passed to SendGeneric! Aborting...");
                return;
#endif
            }

            Send((T)value);
        }
    }
}