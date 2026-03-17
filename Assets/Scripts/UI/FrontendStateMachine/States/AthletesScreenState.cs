using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using FormForge.AssetManagement;
using FormForge.AssetManagement.AssetPolicy;
using FormForge.Domain.Athletes;
using FormForge.Infrastructure.AssetManagementService;
using FormForge.Infrastructure.Services;
using FormForge.Infrastructure.Services.MessageService.Interfaces;
using FormForge.Infrastructure.UI.Overlays.LoadingOverlay.Messages;
using FormForge.Infrastructure.UI.Pagination;
using FormForge.Infrastructure.UI.Screens.Messages;
using FormForge.Services.AthletesService;
using FormForge.Statics;
using FormForge.UI.Screens.ViewModels.AthletesScreen;
using UnityEngine;

namespace FormForge.UI.FrontendStateMachine.States
{
    public class AthletesScreenState : IFrontendState
    {
        public async UniTask EnterAsync()
        {
            var messageService = ServiceLocator.GetService<IMessageService>();

            messageService.Send(new LoadingOverlayShowMessage());
            messageService.Send(new LoadingOverlaySetProgressMessage(0.3f));

            var athletesService = ServiceLocator.GetService<IAthletesService>();

            UniTask<IReadOnlyList<Athlete>> getAthletesTask = athletesService.GetAthletes();
            UniTask<GameObject> loadItemPrefabTask = LoadItemPrefab();

            var (athletes, itemPrefab) =
                await UniTask.WhenAll(getAthletesTask, loadItemPrefabTask);

            messageService.Send(new LoadingOverlaySetProgressMessage(0.7f));
            
            IReadOnlyList<AthleteItemViewModel> athleteViewModels = athletes.Select(athlete => 
                new AthleteItemViewModel(athlete)).ToList();
            
            var paginatedDataProvider = new PaginatedDataProvider<AthleteItemViewModel>(athleteViewModels, 
                UIStrings.Athletes.NoAthletesCreatedYet);
            
            var screenViewModel = new AthletesScreenViewModel(paginatedDataProvider, itemPrefab);
            messageService.Send(new OpenScreenMessage(screenViewModel));
            
            messageService.Send(new LoadingOverlaySetProgressMessage(1f));
            messageService.Send(new LoadingOverlayHideMessage());
        }

        private async UniTask<GameObject> LoadItemPrefab()
        {
            var policy = new BasicAssetPolicy(AddressKeys.UI.Screens.Athletes.Components.AthleteItemView);
            return await ServiceLocator.GetService<IAssetManagementService>().
                LoadAsync<GameObject, UIContext>(policy);
        }

        public UniTask ExitAsync()
        {
            var closeMessage = new CloseScreenMessage(typeof(AthletesScreenViewModel));
            ServiceLocator.GetService<IMessageService>().Send(closeMessage);
            return UniTask.CompletedTask;
        }
    }
}