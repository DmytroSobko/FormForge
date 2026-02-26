using Cysharp.Threading.Tasks;
using FormForge.AssetManagement;
using FormForge.AssetManagement.AssetPolicy;
using FormForge.Infrastructure.AssetManagementService;
using FormForge.Infrastructure.Services;
using FormForge.Infrastructure.Services.MessageService.Interfaces;
using FormForge.Infrastructure.UI.Overlays.LoadingOverlay.Messages;
using FormForge.Infrastructure.UI.Screens.Messages;
using FormForge.ScriptableObjects.Athletes;
using FormForge.Services.ConfigsService;
using FormForge.UI.Screens.ViewModels.CreateAthleteScreen;
using UnityEngine;

namespace FormForge.UI.FrontendStateMachine.States
{
    public class CreateAthleteScreenState : IFrontendState
    {
        public async UniTask EnterAsync()
        {
            var messageService = ServiceLocator.GetService<IMessageService>();
            messageService.Send(new LoadingOverlayShowMessage());
            messageService.Send(new LoadingOverlaySetProgressMessage(0.3f));

            var loadItemPrefabTask = LoadItemPrefab();
            var athleteTypeVisualsDatabaseTask = LoadAthleteTypeVisualsDatabase();
            var (itemPrefab, athleteTypeVisualsDatabase) = 
                await UniTask.WhenAll(loadItemPrefabTask, athleteTypeVisualsDatabaseTask);

            messageService.Send(new LoadingOverlaySetProgressMessage(0.7f));

            var configsService = ServiceLocator.GetService<IConfigsService>();
            var screenViewModel = new CreateAthleteScreenViewModel(itemPrefab,
                configsService.AthleteTypes, athleteTypeVisualsDatabase);

            messageService.Send(new OpenScreenMessage(screenViewModel));
            messageService.Send(new LoadingOverlaySetProgressMessage(1f));
            messageService.Send(new LoadingOverlayHideMessage());
        }

        private async UniTask<GameObject> LoadItemPrefab()
        {
            BasicAssetPolicy policy = 
                new BasicAssetPolicy(AddressKeys.UI.Screens.Components.AthleteTypeItemView);
            return await ServiceLocator.GetService<IAssetManagementService>().
                LoadAsync<GameObject, UIContext>(policy);
        }
        
        private async UniTask<AthleteTypeVisualsDatabase> LoadAthleteTypeVisualsDatabase()
        {
            BasicAssetPolicy policy = 
                new BasicAssetPolicy(AddressKeys.ScriptableObjects.VisualDatabases.AthleteTypeVisualsDatabase);
            return await ServiceLocator.GetService<IAssetManagementService>().
                LoadAsync<AthleteTypeVisualsDatabase, UIContext>(policy);
        }

        public UniTask ExitAsync()
        {
            var messageService = ServiceLocator.GetService<IMessageService>();
            messageService.Send(new CloseScreenMessage(typeof(CreateAthleteScreenViewModel)));
            return UniTask.CompletedTask;
        }
    }
}