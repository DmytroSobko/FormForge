using System.Threading.Tasks;
using FormForge.AssetManagement;
using FormForge.AssetManagement.AssetPolicy;
using FormForge.Infrastructure.Services;
using FormForge.Infrastructure.Services.MessageService.Interfaces;
using FormForge.Infrastructure.UI.Screens.Models;
using FormForge.Infrastructure.UI.Screens.Presenters;
using FormForge.UI.Screens.DataProviders;
using FormForge.UI.Screens.Models.AthletesScreen;
using FormForge.UI.Screens.Views.AthletesScreen;
using UnityEngine;

namespace FormForge.UI.Screens.Presenters.AthletesScreen
{
    public class AthletesScreenPresenter : ScreenPresenter
    {
        private const string k_NoContentMessage = "No athletes have been created yet.";

        private AthletesScreenViewModel TypedViewModel => (AthletesScreenViewModel) ViewModel;
        
        [SerializeField] private AthletesScreenView m_View;

        public override async Task Configure(IScreenViewModel viewModel)
        {
            await base.Configure(viewModel);

            AthletesDataProvider dataProvider = new AthletesDataProvider(TypedViewModel.Athletes);
            
            m_View.InitView(dataProvider, TypedViewModel.ItemPrefab, OnCreateClicked, k_NoContentMessage);
        }

        private void OnCreateClicked()
        {
            IMessageService messageService = ServiceLocator.GetService<IMessageService>();
            //messageService.Send(new OpenScreenMessage(new ));
            // TODO Go to the athlete creation screen state
        }
    }
}