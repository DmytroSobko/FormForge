using FormForge.Infrastructure.Services;
using FormForge.Infrastructure.Services.MessageService.Interfaces;
using FormForge.Infrastructure.UI.Overlays.ProcessingOverlay.Messages;
using TMPro;
using UnityEngine;

namespace FormForge.Infrastructure.UI.Overlays.ProcessingOverlay
{
    public class ProcessingOverlay : FadableOverlayBase, 
        IMessageReceiver<ProcessingOverlayShowMessage>,
        IMessageReceiver<ProcessingOverlayHideMessage>
    {
        [SerializeField] private TextMeshProUGUI m_ProcessText;

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
            m_MessageService.Register<ProcessingOverlayShowMessage>(this);
            m_MessageService.Register<ProcessingOverlayHideMessage>(this);
        }
        
        private void RemoveListeners()
        {
            m_MessageService.Unregister<ProcessingOverlayShowMessage>(this);
            m_MessageService.Unregister<ProcessingOverlayHideMessage>(this);
        }
        
        public void HandleMessage(ProcessingOverlayShowMessage messageData = null)
        {
            m_ProcessText.text = messageData.Process;
            
            Show();
        }

        public void HandleMessage(ProcessingOverlayHideMessage messageData = null)
        {
            Hide();
        }
    }
}