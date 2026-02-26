using FormForge.Infrastructure.Services;
using FormForge.Infrastructure.Services.MessageService.Interfaces;
using FormForge.Infrastructure.UI.Overlays.ErrorOverlay.Messages;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FormForge.Infrastructure.UI.Overlays.ErrorOverlay
{
    public class ErrorOverlay : FadableOverlayBase, 
        IMessageReceiver<ErrorOverlayShowMessage>,
        IMessageReceiver<ErrorOverlayHideMessage>
    {
        [SerializeField] private TextMeshProUGUI m_ErrorBodyText;
        [SerializeField] private Button m_CloseButton;

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
            m_CloseButton.onClick.AddListener(OnCloseButtonClicked);

            m_MessageService.Register<ErrorOverlayShowMessage>(this);
            m_MessageService.Register<ErrorOverlayHideMessage>(this);
        }
        
        private void RemoveListeners()
        {
            m_CloseButton.onClick.RemoveListener(OnCloseButtonClicked);

            m_MessageService.Unregister<ErrorOverlayShowMessage>(this);
            m_MessageService.Unregister<ErrorOverlayHideMessage>(this);
        }

        public void HandleMessage(ErrorOverlayShowMessage messageData = null)
        {
            m_ErrorBodyText.text = messageData.Error;
            
            Show();
        }

        public void HandleMessage(ErrorOverlayHideMessage messageData = null)
        {
            Hide();
        }

        private void OnCloseButtonClicked()
        {
            Hide();
        }
    }
}