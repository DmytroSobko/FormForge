using System.Collections.Generic;
using System.Threading.Tasks;
using FormForge.AssetManagement;
using FormForge.AssetManagement.AssetPolicy;
using FormForge.Infrastructure.Services;
using FormForge.Infrastructure.Services.MessageService.Interfaces;
using FormForge.Infrastructure.UI.LoadingOverlay.Messages;
using FormForge.Infrastructure.UI.Screens.Messages;
using FormForge.Runtime.Models.Athletes;
using FormForge.Services.AthletesService;
using FormForge.UI.Screens.Models.AthletesScreen;
using FormForge.UI.Screens.Presenters.AthletesScreen;
using UnityEngine;

namespace FormForge.UI.FrontendStateMachine.States
{
    public class AthletesScreenState : IFrontendState
    {
        public async Task EnterAsync()
        {
            var messageService = ServiceLocator.GetService<IMessageService>();
            messageService.Send(new LoadingOverlayShowMessage());
            messageService.Send(new LoadingOverlaySetProgressMessage(0.25f));

            IAthletesService athletesService = ServiceLocator.GetService<IAthletesService>();
            IReadOnlyList<Athlete> athletes = await athletesService.GetAthletes();
            
            messageService.Send(new LoadingOverlaySetProgressMessage(0.5f));

            GameObject itemPrefab = await LoadItemPrefab();
            
            messageService.Send(new LoadingOverlaySetProgressMessage(0.75f));
            messageService.Send(new OpenScreenMessage(new AthletesScreenViewModel(athletes, itemPrefab)));
            
            messageService.Send(new LoadingOverlaySetProgressMessage(1f));
            messageService.Send(new LoadingOverlayHideMessage());
        }

        private async Task<GameObject> LoadItemPrefab()
        {
            BasicAssetPolicy policy = new BasicAssetPolicy(AddressKeys.UI.AthletesScreen.AthleteItemView);
            return await ServiceLocator.GetService<IAssetManagementService>().
                LoadAsync<GameObject, UIContext>(policy);
        }
        
        public Task ExitAsync()
        {
            ServiceLocator.GetService<IMessageService>().Send(new CloseScreenMessage(typeof(AthletesScreenViewModel)));
            return Task.CompletedTask;
        }
    }
}