using System;
using FormForge.Core.Services;
using FormForge.Messaging.Interfaces;
using JetBrains.Annotations;
using UnityEngine;

namespace FormForge.Messaging.Components
{
    public class MessageDispatcher : MonoBehaviour
    {
        [SerializeField] 
        private string m_message;
        [SerializeReference] 
        private SerializableMessage m_messageData;
        
        private Type m_messageType;
        
        private IMessageService m_messageService;
        private IMessageService MessageService
        {
            get
            {
                if (m_messageService == null)
                {
                    m_messageService = ServiceLocator.GetService<IMessageService>();
                }

                return m_messageService;
            }
        }

        private void Awake()
        {
            try
            {
                m_messageType = Type.GetType(m_message);
            }
            catch (Exception exception)
            {
                string errorMsg = $"Failed to find Message of type {m_message}, is it possible the Message class has been changed?";
#if PRIME_DEBUG
                throw new Exception(errorMsg, exception);
#else
                Debug.LogError($"{errorMsg} || {exception}");
#endif
            }
        }

        [UsedImplicitly]
        public void Dispatch()
        {
            if (m_messageType == null)
            {
                Debug.LogError($"Null Message for {m_message}. Message will not be sent.");
                return;
            }
            
            MessageService.Send(m_messageType, m_messageData);
        }
    }
}