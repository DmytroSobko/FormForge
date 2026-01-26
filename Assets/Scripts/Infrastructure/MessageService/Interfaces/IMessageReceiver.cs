namespace FormForge.Messaging.Interfaces
{
    /// <summary>
    /// Classes that implement this interface will be able to be register to the MessageService to
    /// receive messages of the specified Type. A single class can implement multiple IMessageReceivers
    /// to receive multiple message types.
    /// </summary>
    /// <typeparam name="T">The type of message that this receiver will handle</typeparam>
    /// <remarks>
    /// Messages in this content are just classes. The most simple form of a Message is an empty class implementation.
    /// Messages that convey data do so by storing the data within an instance of themselves.
    /// </remarks>
    public interface IMessageReceiver<T> : IMessageReceiver where T : class
    {
        /// <summary>
        /// Handles the receiving of the specific message type this <see cref="IMessageReceiver"/> is
        /// responsible for.
        /// </summary>
        /// <param name="messageData">If the message has associated data it will be provided as an instance of itself.</param>
        void HandleMessage(T messageData = null);
    }

    public interface IMessageReceiver
    {
    }
}
