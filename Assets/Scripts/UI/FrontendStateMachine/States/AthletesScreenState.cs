using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using FormForge.AssetManagement;
using FormForge.AssetManagement.AssetPolicy;
using FormForge.Domain.Athletes;
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
            var athletesService = ServiceLocator.GetService<IAthletesService>();

            messageService.Send(new LoadingOverlayShowMessage());

            var getAthletesTask = athletesService.GetAthletes();
            var loadItemPrefabTask = LoadItemPrefab();

            messageService.Send(new LoadingOverlaySetProgressMessage(0.3f));

            await UniTask.WhenAll(loadItemPrefabTask, getAthletesTask);

            messageService.Send(new LoadingOverlaySetProgressMessage(0.6f));

            IReadOnlyList<Athlete> athletes = await getAthletesTask;
            GameObject itemPrefab = await loadItemPrefabTask;

            IReadOnlyList<AthleteItemViewModel> athleteViewModels = athletes.Select(a => 
                new AthleteItemViewModel(a.Type, a.DisplayName)).ToList();
            
            AthletesPaginatedDataProvider paginatedDataProvider = 
                new AthletesPaginatedDataProvider(athleteViewModels, k_NoContentMessage);

            messageService.Send(new LoadingOverlaySetProgressMessage(0.8f));
            
            var screenViewModel = new AthletesScreenViewModel(paginatedDataProvider, itemPrefab);
            messageService.Send(new OpenScreenMessage(screenViewModel));
            
            messageService.Send(new LoadingOverlaySetProgressMessage(1f));
            messageService.Send(new LoadingOverlayHideMessage());
        }

        private async UniTask<GameObject> LoadItemPrefab()
        {
            BasicAssetPolicy policy = new BasicAssetPolicy(AddressKeys.UI.AthletesScreen.AthleteItemView);
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