using System;

namespace FormForge.Messaging.Interfaces
{
    /// <summary>
    /// A Message Service provides the ability to register listeners to receive messages
    /// as well as a way to broadcast messages to the receivers.
    /// </summary>
    public interface IMessageService
    {
        /// <summary>
        /// Registers an instance of <see cref="IMessageReceiver{T}"/> to receive messages of the given type.
        /// </summary>
        /// <param name="messageReceiver">The object that will receive the message</param>
        /// <typeparam name="T">The type of message being sent</typeparam>
        void Register<T>(IMessageReceiver<T> messageReceiver) where T : class;
        
        /// <summary>
        /// Unregisters an instance of <see cref="IMessageReceiver{T}"/> from receiving messages of the given type.
        /// </summary>
        /// <param name="messageReceiver">The object that will no longer receive the message</param>
        /// <typeparam name="T">The type of message being sent</typeparam>
        void Unregister<T>(IMessageReceiver<T> messageReceiver) where T : class;
        
        /// <summary>
        /// Broadcasts a message to all <see cref="IMessageReceiver{T}"/>'s that are registered to receive
        /// the given message type.
        /// </summary>
        /// <param name="data">An optional data object that may be provided to Receivers when sending the message</param>
        /// <typeparam name="T">The type of message being sent</typeparam>
        void Send<T>(T data = null) where T : class;

        /// <summary>
        /// Broadcasts a message to all <see cref="IMessageReceiver{T}"/>'s that are registered to receive
        /// the given message type.
        /// </summary>
        /// <param name="messageType">The type of the Message being sent</param>
        /// <param name="data">
        /// An optional data object that may be provided to Receivers when sending the message.
        /// It must be of the same type as <see cref="messageType"/>
        /// </param>
        /// <remarks>
        /// This Method exists to support <see cref="MessageDispatcher"/>, it is <b><i>strongly</i></b> recommended
        /// that when Sending Messages via code you do so with <see cref="Send{T}"/>
        /// </remarks>
        void Send(Type messageType, object data);
    }
}
