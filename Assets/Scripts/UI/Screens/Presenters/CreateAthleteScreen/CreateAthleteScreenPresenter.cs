using Cysharp.Threading.Tasks;
using FormForge.Infrastructure.Services;
using FormForge.Infrastructure.Services.MessageService.Interfaces;
using FormForge.Infrastructure.UI.Screens.Models;
using FormForge.Infrastructure.UI.Screens.Presenters;
using FormForge.Services.ConfigsService;
using FormForge.UI.Screens.Models;
using FormForge.UI.Screens.Views.CreateAthleteScreen;
using UnityEngine;

namespace FormForge.UI.Screens.Presenters.CreateAthleteScreen
{
    public class CreateAthleteScreenPresenter : ScreenPresenter
    {
        private CreateAthleteScreenViewModel TypedViewModel => (CreateAthleteScreenViewModel) ViewModel;
        
        [SerializeField] private CreateAthleteScreenView m_View;
        
        private IMessageService m_MessageService;

        public override UniTask Initialize()
        {
            m_MessageService = ServiceLocator.GetService<IMessageService>();
            return base.Initialize();
        }

        public override async UniTask Configure(IScreenViewModel viewModel)
        {
            await base.Configure(viewModel);

            IConfigsService configsService = ServiceLocator.GetService<IConfigsService>();
            
            m_View.InitView(configsService.AthleteTypes, TypedViewModel.ItemPrefab, OnCreateClicked);
        }

        private void OnCreateClicked()
        {
            // m_MessageService.Send();
        }
    }
}