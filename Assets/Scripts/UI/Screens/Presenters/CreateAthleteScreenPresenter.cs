using Cysharp.Threading.Tasks;
using FormForge.Infrastructure.Services;
using FormForge.Infrastructure.Services.MessageService.Interfaces;
using FormForge.Infrastructure.UI.Screens.Models;
using FormForge.Infrastructure.UI.Screens.Presenters;
using FormForge.UI.FrontendStateMachine;
using FormForge.UI.FrontendStateMachine.Messages;
using FormForge.UI.Screens.Models;
using UnityEngine;

namespace FormForge.UI.Screens.Presenters
{
    public class CreateAthleteScreenPresenter : ScreenPresenter
    {
        private CreateAthleteScreenViewModel TypedViewModel => (CreateAthleteScreenViewModel) ViewModel;
        
        [SerializeField] private CreateAthleteScreenPresenter m_View;
        private IMessageService m_MessageService;
        
        public override UniTask Initialize()
        {
            m_MessageService = ServiceLocator.GetService<IMessageService>();
            return base.Initialize();
        }

        public override async UniTask Configure(IScreenViewModel viewModel)
        {
            await base.Configure(viewModel);

        }

        private void OnCreateClicked()
        {
            m_MessageService.Send(new SwitchFrontendStateMessage(FrontendStates.CreateAthleteScreen));
        }
    }
}