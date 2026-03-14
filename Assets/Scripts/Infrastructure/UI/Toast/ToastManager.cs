using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FormForge.Infrastructure.Services;
using FormForge.Infrastructure.Services.MessageService.Interfaces;

namespace FormForge.Infrastructure.UI.Toast
{
    public class ToastManager : IMessageReceiver<ToastShowMessage>
    {
        private ToastView m_ToastView;
        
        private readonly Queue<ToastShowMessage> m_Queue = new Queue<ToastShowMessage>();
        private bool m_Showing;

        public ToastManager(ToastView toastView)
        {
            m_ToastView = toastView;
            ServiceLocator.GetService<IMessageService>().Register(this);
        }

        public void Dispose()
        {
            ServiceLocator.GetService<IMessageService>().Unregister(this);
        }

        private void Show(ToastShowMessage message)
        {
            if (message.Type == EToastType.Error)
            {
                m_Queue.Clear(); // error overrides queue
            }

            m_Queue.Enqueue(message);

            if (!m_Showing)
            {
                ProcessQueue().Forget();
            }
        }

        private async UniTaskVoid ProcessQueue()
        {
            m_Showing = true;

            while (m_Queue.Count > 0)
            {
                var msg = m_Queue.Dequeue();
                
                bool completed = false;
                m_ToastView.Show(msg, () =>
                {
                    completed = true;
                });

                while (!completed)
                {
                    await UniTask.Yield();
                }
            }

            m_Showing = false;
        }

        public void HandleMessage(ToastShowMessage messageData = null)
        {
            if (messageData == null)
            {
                return;
            }

            Show(messageData);
        }
    }
}