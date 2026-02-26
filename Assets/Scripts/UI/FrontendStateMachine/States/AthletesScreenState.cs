using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using FormForge.AssetManagement;
using FormForge.AssetManagement.AssetPolicy;
using FormForge.Infrastructure.AssetManagementService;
using FormForge.Infrastructure.Services;
using FormForge.Infrastructure.Services.MessageService.Interfaces;
using FormForge.Infrastructure.UI.LoadingOverlay.Messages;
using FormForge.Infrastructure.UI.Screens.Messages;
using FormForge.Services.AthletesService;
using FormForge.UI.Screens.Pagination.DataProviders;
using FormForge.UI.Screens.ViewModels.AthletesScreen;
using UnityEngine;

namespace FormForge.UI.FrontendStateMachine.States
{
    public class AthletesScreenState : IFrontendState
    {
        private const string k_NoContentMessage = "No athletes have been created yet.";

        public async UniTask EnterAsync()
        {
            var messageService = ServiceLocator.GetService<IMessageService>();

            messageService.Send(new LoadingOverlayShowMessage());
            messageService.Send(new LoadingOverlaySetProgressMessage(0.3f));

            var athletesService = ServiceLocator.GetService<IAthletesService>();
            var getAthletesTask = athletesService.GetAthletes();
            var loadItemPrefabTask = LoadItemPrefab();

            var (athletes, itemPrefab) =
                await UniTask.WhenAll(getAthletesTask, loadItemPrefabTask);

            messageService.Send(new LoadingOverlaySetProgressMessage(0.7f));

            IReadOnlyList<AthleteItemViewModel> athleteViewModels = athletes.Select(a => 
                new AthleteItemViewModel(a.Type, a.DisplayName)).ToList();
            
            AthletesPaginatedDataProvider paginatedDataProvider = 
                new AthletesPaginatedDataProvider(athleteViewModels, k_NoContentMessage);
            
            var screenViewModel = new AthletesScreenViewModel(paginatedDataProvider, itemPrefab);
            messageService.Send(new OpenScreenMessage(screenViewModel));
            
            messageService.Send(new LoadingOverlaySetProgressMessage(1f));
            messageService.Send(new LoadingOverlayHideMessage());
        }

        private async UniTask<GameObject> LoadItemPrefab()
        {
            BasicAssetPolicy policy = new BasicAssetPolicy(AddressKeys.UI.Screens.Components.AthleteItemView);
            return await ServiceLocator.GetService<IAssetManagementService>().
                LoadAsync<GameObject, UIContext>(policy);
        }
        
        public UniTask ExitAsync()
        {
            ServiceLocator.GetService<IMessageService>().Send(new CloseScreenMessage(typeof(AthletesScreenViewModel)));
            return UniTask.CompletedTask;
        }
    }
}