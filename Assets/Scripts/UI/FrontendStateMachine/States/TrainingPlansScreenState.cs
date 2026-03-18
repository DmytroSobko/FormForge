using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using FormForge.AssetManagement;
using FormForge.AssetManagement.AssetPolicy;
using FormForge.Domain.Athletes;
using FormForge.Domain.TrainingPlans;
using FormForge.Infrastructure.AssetManagementService;
using FormForge.Infrastructure.Services;
using FormForge.Infrastructure.Services.MessageService.Interfaces;
using FormForge.Infrastructure.UI.Overlays.LoadingOverlay.Messages;
using FormForge.Infrastructure.UI.Pagination;
using FormForge.Infrastructure.UI.Screens.Messages;
using FormForge.Services.AthletesService;
using FormForge.Services.TrainingPlansService;
using FormForge.Statics;
using FormForge.UI.Screens.ViewModels.AthletesScreen;
using FormForge.UI.Screens.ViewModels.TrainingPlansScreen;
using UnityEngine;

namespace FormForge.UI.FrontendStateMachine.States
{
    public class TrainingPlansScreenState : IFrontendState
    {
        public async UniTask EnterAsync()
        {
            var messageService = ServiceLocator.GetService<IMessageService>();

            messageService.Send(new LoadingOverlayShowMessage());
            messageService.Send(new LoadingOverlaySetProgressMessage(0.3f));

            var trainingPlansService = ServiceLocator.GetService<ITrainingPlansService>();

            UniTask<IReadOnlyList<TrainingPlan>> getTrainingPlansTask = 
                trainingPlansService.GetTrainingPlans();
            UniTask<GameObject> loadItemPrefabTask = LoadItemPrefab();

            var (trainingPlans, itemPrefab) =
                await UniTask.WhenAll(getTrainingPlansTask, loadItemPrefabTask);

            messageService.Send(new LoadingOverlaySetProgressMessage(0.7f));
            
            IReadOnlyList<TrainingPlanItemViewModel> trainingPlanViewModels = trainingPlans.Select(trainingPlan => 
                new TrainingPlanItemViewModel(trainingPlan)).ToList();
            
            var paginatedDataProvider = new PaginatedDataProvider<TrainingPlanItemViewModel>(trainingPlanViewModels, 
                UIStrings.TrainingPlans.NoTrainingPlansCreatedYet);
            
            var screenViewModel = new TrainingPlansScreenViewModel(paginatedDataProvider, itemPrefab);
            messageService.Send(new OpenScreenMessage(screenViewModel));
            
            messageService.Send(new LoadingOverlaySetProgressMessage(1f));
            messageService.Send(new LoadingOverlayHideMessage());
        }

        private async UniTask<GameObject> LoadItemPrefab()
        {
            var policy = 
                new BasicAssetPolicy(AddressKeys.UI.Screens.TrainingPlans.Components.TrainingPlanItemView);
            return await ServiceLocator.GetService<IAssetManagementService>().
                LoadAsync<GameObject, UIContext>(policy);
        }

        public UniTask ExitAsync()
        {
            var closeMessage = new CloseScreenMessage(typeof(TrainingPlansScreenViewModel));
            ServiceLocator.GetService<IMessageService>().Send(closeMessage);
            return UniTask.CompletedTask;
        }
    }
}