using FormForge.Infrastructure.Services;
using FormForge.Infrastructure.Services.MessageService.Interfaces;
using FormForge.Infrastructure.UI.Overlays.LoadingOverlay.Messages;
using UnityEngine;
using UnityEngine.UI;

namespace FormForge.Infrastructure.UI.Overlays.LoadingOverlay
{
    public class LoadingOverlay : FadableOverlayBase, 
        IMessageReceiver<LoadingOverlayShowMessage>,
        IMessageReceiver<LoadingOverlayHideMessage>,
        IMessageReceiver<LoadingOverlaySetProgressMessage>
    {
        [SerializeField] private Image m_ProgressFill;
        
        private IMessageService m_MessageService;
        
        private void Awake()
        {
            m_MessageService = ServiceLocator.GetService<IMessageService>();
            
            AddListeners();
            HideImmediate();
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }
        
        private void AddListeners()
        {
            m_MessageService.Register<LoadingOverlayShowMessage>(this);
            m_MessageService.Register<LoadingOverlayHideMessage>(this);
            m_MessageService.Register<LoadingOverlaySetProgressMessage>(this);
        }
        
        private void RemoveListeners()
        {
            m_MessageService.Unregister<LoadingOverlayShowMessage>(this);
            m_MessageService.Unregister<LoadingOverlayHideMessage>(this);
            m_MessageService.Unregister<LoadingOverlaySetProgressMessage>(this);
        }
        
        public void HandleMessage(LoadingOverlayShowMessage messageData = null)
        {
            Show();
        }

        public void HandleMessage(LoadingOverlayHideMessage messageData = null)
        {
            Hide();
        }

        public void HandleMessage(LoadingOverlaySetProgressMessage messageData = null)
        {
            SetProgress(messageData.Progress);
        }

        private void SetProgress(float value)
        {
            m_ProgressFill.fillAmount = Mathf.Clamp01(value);
        }
    }
}