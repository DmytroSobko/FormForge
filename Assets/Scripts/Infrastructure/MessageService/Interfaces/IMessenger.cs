using System.Collections.Generic;

namespace FormForge.Messaging.Interfaces
{
    /// <summary>
    /// Used as a common base for <see cref="MessageService"/> implementations so that they can be
    /// stored in a single collection. <seealso cref="Messenger{T}"/>
    /// </summary>
    internal interface IMessenger
    {
        void Initialize();
    }
}
