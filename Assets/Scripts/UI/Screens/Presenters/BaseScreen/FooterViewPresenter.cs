using FormForge.Infrastructure.Services;
using FormForge.Infrastructure.Services.MessageService.Interfaces;
using FormForge.Infrastructure.UI;
using FormForge.UI.FrontendStateMachine;
using FormForge.UI.FrontendStateMachine.Messages;
using FormForge.UI.Screens.Views.BaseScreen;
using UnityEngine;

namespace FormForge.UI.Screens.Presenters.BaseScreen
{
    public class FooterViewPresenter : MonoBehaviour, IPresenter
    {
        [SerializeField] private FooterView m_View;
        
        private IMessageService m_MessageService;

        private void Awake()
        {
            m_MessageService = ServiceLocator.GetService<IMessageService>();

            AddListeners();
        }

        private void AddListeners()
        {
            m_View.HomeButtonClicked += OnHomeButtonClicked;
        }
        
        private void RemoveListeners()
        {
            m_View.HomeButtonClicked -= OnHomeButtonClicked;
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }
        
        private void OnHomeButtonClicked()
        {
            m_MessageService.Send(new SwitchFrontendStateMessage(FrontendStates.MainMenuScreen));
        }
    }
}