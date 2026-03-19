using Cysharp.Threading.Tasks;
using FormForge.AssetManagement;
using FormForge.AssetManagement.AssetPolicy;
using FormForge.Infrastructure.AssetManagementService;
using FormForge.Infrastructure.Services;
using FormForge.Infrastructure.Services.MessageService.Interfaces;
using FormForge.Infrastructure.UI.Overlays.LoadingOverlay.Messages;
using FormForge.Infrastructure.UI.Screens.Messages;
using FormForge.UI.Screens.ViewModels.CreateTrainingPlanScreen;
using UnityEngine;

namespace FormForge.UI.FrontendStateMachine.States
{
    public class CreateTrainingPlanScreenState : IFrontendState
    {
        public async UniTask EnterAsync()
        {
            var messageService = ServiceLocator.GetService<IMessageService>();

            messageService.Send(new LoadingOverlayShowMessage());
            messageService.Send(new LoadingOverlaySetProgressMessage(0.3f));

            var exerciseItemPrefab = await LoadExerciseItemPrefab();

            messageService.Send(new LoadingOverlaySetProgressMessage(0.7f));

            var screenViewModel = new CreateTrainingPlanScreenViewModel(exerciseItemPrefab);

            messageService.Send(new OpenScreenMessage(screenViewModel));
            messageService.Send(new LoadingOverlaySetProgressMessage(1f));
            messageService.Send(new LoadingOverlayHideMessage());
        }

        private async UniTask<GameObject> LoadExerciseItemPrefab()
        {
            BasicAssetPolicy policy = 
                new BasicAssetPolicy(AddressKeys.UI.Screens.TrainingPlans.Components.TrainingPlanItemView);
            return await ServiceLocator.GetService<IAssetManagementService>().
                LoadAsync<GameObject, UIContext>(policy);
        }

        public UniTask ExitAsync()
        {
            var messageService = ServiceLocator.GetService<IMessageService>();
            messageService.Send(new CloseScreenMessage(typeof(CreateTrainingPlanScreenViewModel)));
            return UniTask.CompletedTask;
        }
    }
}