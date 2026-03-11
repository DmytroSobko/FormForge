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
using FormForge.Infrastructure.UI.Screens.Messages;
using FormForge.ScriptableObjects.Athletes;
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
            
            UniTask<IReadOnlyList<Athlete>> getAthletesTask = athletesService.GetAthletes();
            UniTask<GameObject> loadItemPrefabTask = LoadItemPrefab();
            UniTask<AthleteTypeVisualsDatabase> athleteTypeVisualsDatabaseTask = LoadAthleteTypeVisualsDatabase();

            var (athletes, itemPrefab, athleteTypeVisualsDatabase) =
                await UniTask.WhenAll(getAthletesTask, loadItemPrefabTask, athleteTypeVisualsDatabaseTask);

            messageService.Send(new LoadingOverlaySetProgressMessage(0.7f));
            
            IReadOnlyList<AthleteItemViewModel> athleteViewModels = athletes.Select(athlete => 
                new AthleteItemViewModel(athlete, athleteTypeVisualsDatabase.Get(athlete.Type).Icon)).ToList();
            
            var paginatedDataProvider = 
                new AthletesPaginatedDataProvider(athleteViewModels, k_NoContentMessage);
            
            var screenViewModel = new AthletesScreenViewModel(paginatedDataProvider, itemPrefab);
            messageService.Send(new OpenScreenMessage(screenViewModel));
            
            messageService.Send(new LoadingOverlaySetProgressMessage(1f));
            messageService.Send(new LoadingOverlayHideMessage());
        }

        private async UniTask<GameObject> LoadItemPrefab()
        {
            var policy = new BasicAssetPolicy(AddressKeys.UI.Screens.Components.AthleteItemView);
            return await ServiceLocator.GetService<IAssetManagementService>().
                LoadAsync<GameObject, UIContext>(policy);
        }
        
        private async UniTask<AthleteTypeVisualsDatabase> LoadAthleteTypeVisualsDatabase()
        {
            string dbAddress = AddressKeys.ScriptableObjects.VisualDatabases.AthleteTypeVisualsDatabase;
            var policy = new BasicAssetPolicy(dbAddress);
            return await ServiceLocator.GetService<IAssetManagementService>().
                LoadAsync<AthleteTypeVisualsDatabase, UIContext>(policy);
        }
        
        public UniTask ExitAsync()
        {
            var closeMessage = new CloseScreenMessage(typeof(AthletesScreenViewModel));
            ServiceLocator.GetService<IMessageService>().Send(closeMessage);
            return UniTask.CompletedTask;
        }
    }
}