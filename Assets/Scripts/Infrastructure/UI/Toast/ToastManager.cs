using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FormForge.AssetManagement;
using FormForge.AssetManagement.AssetPolicy;
using FormForge.Infrastructure.AssetManagementService;
using FormForge.Infrastructure.Collections;
using FormForge.Infrastructure.Services;
using FormForge.Infrastructure.Services.MessageService.Interfaces;
using UnityEngine;

namespace FormForge.Infrastructure.UI.Toast
{
    public class ToastManager : IMessageReceiver<ToastShowMessage>
    {
        private const int k_ToastPoolSize = 5;

        private GameObject m_ToastPrefab;
        private Transform m_Container;

        private Pool<PoolableObject> m_ToastPool;
        private readonly Queue<ToastShowMessage> m_Queue = new Queue<ToastShowMessage>();
        private bool m_Showing;

        public async void Init(Transform container)
        {
            await LoadPrefab();
            
            m_Container = container;
            m_ToastPool = new Pool<PoolableObject>(k_ToastPoolSize, m_ToastPrefab);
            
            ServiceLocator.GetService<IMessageService>().Register(this);
        }
        
        ~ToastManager()
        {
            ServiceLocator.GetService<IMessageService>().Unregister(this);
        }

        private async UniTask LoadPrefab()
        {
            var policy = new BasicAssetPolicy(AddressKeys.UI.Toast.ToastView);
            m_ToastPrefab = await ServiceLocator.GetService<IAssetManagementService>().
                LoadAsync<GameObject, UIContext>(policy);
        }
        
        private void Show(ToastShowMessage message)
        {
            if (message.Type == ToastType.Error)
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
                var toast = m_ToastPool.Acquire();
                toast.transform.SetParent(m_Container, false);
                var toastView = toast.GetComponent<ToastView>();
                
                bool completed = false;
                toastView.Show(msg, () =>
                {
                    completed = true;
                    toast.Recycle();
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
            if (m_ToastPool == null || messageData == null)
            {
                return;
            }

            Show(messageData);
        }
    }
}