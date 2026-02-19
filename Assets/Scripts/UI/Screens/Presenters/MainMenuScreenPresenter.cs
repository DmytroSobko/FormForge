using Cysharp.Threading.Tasks;
using FormForge.Infrastructure.Services;
using FormForge.Infrastructure.Services.MessageService.Interfaces;
using FormForge.Infrastructure.UI.Screens.Presenters;
using FormForge.Infrastructure.UI.Screens.ViewModels;
using FormForge.UI.FrontendStateMachine;
using FormForge.UI.FrontendStateMachine.Messages;
using FormForge.UI.Screens.ViewModels;
using FormForge.UI.Screens.Views;
using UnityEngine;

namespace FormForge.UI.Screens.Presenters
{
    public class MainMenuScreenPresenter : ScreenPresenter
    {
        [SerializeField] private MainMenuScreenView m_View;

        public override UniTask Initialize()
        {
            m_View.InitView(OnAthletesButtonPressed);

            return base.Initialize();
        }
        
        public override UniTask Configure(IScreenViewModel viewModel)
        {
            ViewModel = (MainMenuScreenViewModel) viewModel;
            
            return base.Configure(viewModel);
        }
        
        private void OnAthletesButtonPressed()
        {
            var athletesScreenMessage = new SwitchFrontendStateMessage(FrontendStates.AthletesScreen);
            ServiceLocator.GetService<IMessageService>().Send(athletesScreenMessage);
        }
    }
}