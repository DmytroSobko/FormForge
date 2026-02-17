using Cysharp.Threading.Tasks;
using FormForge.Infrastructure.Services;
using FormForge.Infrastructure.Services.MessageService.Interfaces;
using FormForge.Infrastructure.UI.Screens.Models;
using FormForge.Infrastructure.UI.Screens.Presenters;
using FormForge.UI.FrontendStateMachine;
using FormForge.UI.FrontendStateMachine.Messages;
using FormForge.UI.Screens.Models.AthletesScreen;
using FormForge.UI.Screens.Pagination.DataProviders;
using FormForge.UI.Screens.Views.AthletesScreen;
using UnityEngine;

namespace FormForge.UI.Screens.Presenters.AthletesScreen
{
    public class AthletesScreenPresenter : ScreenPresenter
    {
        private const string k_NoContentMessage = "No athletes have been created yet.";

        private AthletesScreenViewModel TypedViewModel => (AthletesScreenViewModel) ViewModel;
        
        [SerializeField] private AthletesScreenView m_View;
        private IMessageService m_MessageService;
        
        public override UniTask Initialize()
        {
            m_MessageService = ServiceLocator.GetService<IMessageService>();
            return base.Initialize();
        }

        public override async UniTask Configure(IScreenViewModel viewModel)
        {
            await base.Configure(viewModel);

            AthletesDataProvider dataProvider = new AthletesDataProvider(TypedViewModel.Athletes);
            
            m_View.InitView(dataProvider, TypedViewModel.ItemPrefab, OnCreateClicked, k_NoContentMessage);
        }

        private void OnCreateClicked()
        {
            m_MessageService.Send(new SwitchFrontendStateMessage(FrontendStates.CreateAthleteScreen));
        }
    }
}