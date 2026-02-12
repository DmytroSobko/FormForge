using System.Threading.Tasks;
using FormForge.Core.Services;
using FormForge.Infrastructure.UI.Screens.Model;
using FormForge.Infrastructure.UI.Screens.View;
using FormForge.Messaging.Interfaces;
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

        public override Task Initialize()
        {
            m_View.InitView(OnAthletesButtonPressed);

            return base.Initialize();
        }
        
        public override Task Configure(IScreenViewModel viewModel)
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